using Microsoft.EntityFrameworkCore;
using QuizApiBackend.Models;

namespace QuizApiBackend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            

            builder.Services.AddDbContext<QuizDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("QuizDbConnection")));
            var useJson = builder.Configuration.GetValue<bool>("UseJsonRepository");

            if (useJson)
            {
                builder.Services.AddScoped<IRepository<Participant>, JsonParticipantRepository>();
                builder.Services.AddScoped<IQuestionRepository, JsonQuestionRepository>();
            }
            else
            {
                builder.Services.AddScoped<IRepository<Participant>, ParticipantRepository>();
                builder.Services.AddScoped<IRepository<Question>, QuestionRepository>();
            }
            var origindetails = "allowallorigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: origindetails, policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Enable CORS using the defined policy
            app.UseCors(origindetails);
            // Configure the HTTP request pipeline.
            
           
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Quiz API V1");
                    c.RoutePrefix = string.Empty;
                   
                });
            

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
