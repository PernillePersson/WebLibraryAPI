
using Business.Service;
using Data;
using Data.Repository;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

namespace Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddDbContext<LibraryContext>(opts =>
            opts.UseSqlServer("Server=EASV-DB4;Database=LibAPI;User Id=CSe2022t_t_1;Password=CSe2022tT1#;TrustServerCertificate=True;"));
            //opts.UseSqlServer("Server=tcp:magnuslavermad.database.windows.net,1433;Initial Catalog=libwebapi;Persist Security Info=False;User ID=pernille;Password=Pkp12345;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
            //opts.UseSqlite("Data Source=./MyLibrary.db"));
        builder.Services.AddAutoMapper(typeof(Program));
        builder.Services.AddLogging();
        builder.Services.AddCors();
        
        // Services
        builder.Services.AddScoped<IBookService, BookService>();
        builder.Services.AddScoped<IBorrowerService, BorrowerService>();
        
        //Repositories
        builder.Services.AddScoped<IBookRepository, BookRepository>();
        builder.Services.AddScoped<IBorrowerRepository, BorrowerRepository>();
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

        app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());
        

        app.UseRouting();
        //app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}


