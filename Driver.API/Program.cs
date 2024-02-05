using Driver.DAL.DataContext;
using Driver.DAL.Repositories;
using Driver.DAL.Repositories.IRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var DataContext = DriverDataContext.GetInstance(builder.Configuration.GetConnectionString("Default"));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IDriverRepository>(DR => new DriverRepository(DataContext));

var app = builder.Build();
DriverDataContext.GetInstance(builder.Configuration.GetConnectionString("Default"));
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
