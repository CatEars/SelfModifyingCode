using SelfModifyingCode.Server.Directory;
using SelfModifyingCode.Server.Infrastructure;
using SelfModifyingCode.Server.Infrastructure.LocalFiles;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSwaggerDocument();
var pathRepository = new InMemoryPathRepository();
var programRepository = new InMemoryProgramRepository();
builder.Services.AddSingleton<IProgramPathRepository, InMemoryPathRepository>(_ => pathRepository);
builder.Services.AddSingleton<IProgramRepository, InMemoryProgramRepository>(_ => programRepository);
builder.Services.AddSingleton(_ => pathRepository);
builder.Services.AddSingleton(_ => programRepository);
builder.Services.AddSingleton<WatchDirectory>(_ => new("bundle-watcher"));
builder.Services.AddHostedService<DirectoryWatcherRegistrator>();

var app = builder.Build();

app.MapControllers();

app.UseOpenApi();
app.UseSwaggerUi3();
app.Run();