var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddSwaggerDocument();
var app = builder.Build();

app.MapControllers();
        
app.UseOpenApi();
app.UseSwaggerUi3();
app.Run();