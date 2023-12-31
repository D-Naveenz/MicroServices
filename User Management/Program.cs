using webapi.Services;
using User_Management.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// loading services
ConfigManager configManager = new(builder);

// connecting to the database
configManager.ConfigureDBConnection("DefaultConnection");

var app = configManager.GetApp();

// Configure the HTTP request pipeline.
configManager.AllowSwaggerUI();

app.UseHttpsRedirection();

// add CORS middleware
app.UseCors(); // this is for the default policy

app.UseAuthorization();

// app.UseSession();

app.MapControllers();

app.MapUserEndpoints();

app.MapAuthEndpoints();

app.Run();
