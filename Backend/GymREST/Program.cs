using GymREST.Data;
using GymREST.Data.Repos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

var dataSourceBuilder = new NpgsqlDataSourceBuilder(builder.Configuration.GetConnectionString("Default"));
var dataSource = dataSourceBuilder.Build();

builder.Services
    .AddDbContext<DataContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")))
    .AddScoped<PlansRepo>()
    .AddScoped<InitialDatasRepo>()
    .AddScoped<UserChosenPlanRepo>()
    .AddScoped<ProfileRepo>()
    .AddScoped<ExercisesRepo>()
    .AddScoped<EventsRepo>()
    .AddScoped<CalendarExerciseRepo>()
    .AddScoped<WorkoutEventRepo>();
    

builder.Services.AddControllers();
builder.Services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
    {
        builder
        .SetIsOriginAllowed(_ => true)
        // .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost") las see olla korra
        .AllowCredentials()
        .AllowAnyMethod()
        .AllowAnyHeader();
    }));



var app = builder.Build();


using (var scope = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
using (var context = scope.ServiceProvider.GetService<DataContext>())
{
    context?.Database.EnsureCreated();
    
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ExampleApi v1"));
}

app.UseCors("MyPolicy");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

app.Run();
