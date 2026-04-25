using pingword.src.Configuration;
using pingword.src.Errors;
using Serilog;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

var formtLog = "{Timestamp:dd-MM-yyy HH:mm:ss} [{Level:u3}] - {Message:lj}{NewLine}{Exception}";

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(outputTemplate: formtLog)
    .Enrich.FromLogContext()
    .CreateLogger();


builder.Host.UseSerilog();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jsonConfig = Environment.GetEnvironmentVariable("FIREBASE_CONFIG_JSON");

if (string.IsNullOrEmpty(jsonConfig)) {
    throw new Exception("Variável de ambiente FIREBASE_CONFIG_JSON não encontrada.");
}

FirebaseApp.Create(new AppOptions()
{
    // Usa FromJson para ler a string diretamente
    Credential = GoogleCredential.FromJson(jsonConfig)
});

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddExceptionHandler<ExceptionHandlerError>();
builder.Services.AddProblemDetails();


builder.Services.AddInfrastructure(builder.Configuration);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler();

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
