
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Timesheets.Api.Services.CsvService;
using Timesheets.Domain;

namespace Timesheets
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
            builder.Services.AddSwaggerGen(config =>
            {
                config.CustomSchemaIds(x => x.FullName.Replace("+", "."));
            });

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("TimesheetDb")));

            builder.Services.AddScoped<ICsvService, CsvService>();

            builder.Services.AddMediatR(x =>
            {
                x.RegisterServicesFromAssembly(typeof(Program).Assembly);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.Run();
        }
    }
}
