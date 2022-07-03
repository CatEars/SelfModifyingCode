using SelfModifyingCode.Server.Directory;
using SelfModifyingCode.Server.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerDocument();
builder.Services.AddSingleton<IProgramPathRepository, FakeProgramPathRepository>();
builder.Services.AddSingleton<IProgramRepository, FakeProgramRepository>();

var app = builder.Build();

app.MapControllers();

app.UseOpenApi();
app.UseSwaggerUi3();
app.Run();