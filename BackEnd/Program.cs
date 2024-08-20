using BackEnd.Data;
using BackEnd.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddDbContext<CrdpCurriculumMsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
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
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
       };
   });
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy =>
//    policy.RequireRole("Admin"));
//    options.AddPolicy("UserPolicy", policy =>
//    policy.RequireRole("User"));
//});
builder.Services.AddCors(options =>
{
//    options.AddPolicy("AllowOrigin",
//        builder => builder.AllowAnyOrigin()
//                         .AllowAnyHeader()
//                         .AllowAnyMethod());
    options.AddPolicy("AllowOrigin",
        builder => builder.WithOrigins("https://localhost:4200", "https://127.0.0.1:4200", "https://10.0.0.14:4200", "https://52.169.111.157:4200")
           .AllowAnyHeader()
           .AllowAnyMethod());

});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder.WithOrigins("https://localhost:4200", "https://127.0.0.1:4200", "https://10.0.0.14:4200", "https://52.169.111.157:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials());
});

//builder.Services.AddControllers();//me//

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowSpecificOrigin");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowOrigin");
app.MapControllers();

app.Run();
