using EnsekWebAPI.Controllers;
using EnsekWebAPI.Database;
using EnsekWebAPI.Database.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace EnsekWebAPI
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddDbContext<SqliteContext>(options => options.UseSqlite(Configuration.GetConnectionString("SQLite")));

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "ENSEK API", Version = "v1" });
      });

      services.AddControllers();

      services.AddScoped<IDatabaseFactory, DatabaseFactory>();
      services.AddScoped<ITextToDataConverter, CsvToDataConverter>();
      services.AddScoped<AccountsRepository>();
      services.AddScoped<MeterReadingsRepository>();
      services.AddScoped<IMeterReadingUploadTask, MeterReadingUploadTask>();

      services.AddCors(p => p.AddPolicy("corsapp", builder =>
      {
        builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
      }));
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseCors("corsapp");
      app.UseHttpsRedirection();
      app.UseRouting();
      app.UseAuthorization();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();
      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ENSEK API V1");
      });
    }
  }
}
