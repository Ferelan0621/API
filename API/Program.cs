using API.Data;
using API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);
// IMPORTANTE: Tienen que ser AddSingleton, NO AddScoped ni AddTransient
builder.Services.AddSingleton<API.Services.NotificadorLaboratorios>();
builder.Services.AddSingleton<API.Services.NotificadorPrestamos>();
var app = builder.Build();

app.MapControllers();

app.Run();
