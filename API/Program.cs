using API.Infra;
using API.Infra.Data;
using API.Services;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



#region [Database]
builder.Services.AddDbContext<DataContext>(
        options => options.UseNpgsql(builder.Configuration.GetSection("DatabaseSettings:ConnectionString").Value));

builder.Services.AddTransient<DataContext>();
#endregion

#region [Healthcheck]
builder.Services.AddHealthChecks()
             .AddNpgSql(builder.Configuration.GetSection("DatabaseSettings:ConnectionString").Value,
                    name: "postgreSQL", tags: new string[] { "db", "data" });


builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(5);
    options.MaximumHistoryEntriesPerEndpoint(10);
    options.AddHealthCheckEndpoint("API com Health Checks", "/health");
}).AddInMemoryStorage();

#endregion




#region [DI]
builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<NewsService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

#region [Healthcheck]
app.UseHealthChecks("/health", new HealthCheckOptions()
{
    Predicate = _ => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Ativa o dashboard para a visualização da situação de cada Health Check
app.UseHealthChecksUI(options =>
{
    options.UIPath = "/monitor";
});

#endregion

app.UseAuthorization();

app.MapControllers();

app.Run();
