
using Business.Service;
using Data.Repository;

namespace Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddAutoMapper(typeof(Program));
        
        // Services
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IBorrowerService, BorrowerService>();
        
        //Repositories
        builder.Services.AddSingleton<IBookRepository, BookRepository>();
        builder.Services.AddSingleton<IBorrowerRepository, BorrowerRepository>();
        
        builder.Services.AddControllers();
        
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}