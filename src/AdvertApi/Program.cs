using AdvertApi.HealthChecks;
using AdvertApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(AdvertProfile));
builder.Services.AddTransient<IAdvertStorageService, DynamoDBAdvertStorage>();


builder.Services.AddHealthChecks(); 

/*
 //Custom health check is not working.
builder.Services.AddTransient<StorageHealthCheck>();
builder.Services.AddHealthChecks().AddCheck<StorageHealthCheck>("Storage");*/

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllOrigin", policy => policy.WithOrigins("*").AllowAnyHeader());
});


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


app.UseHealthChecks("/health");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
