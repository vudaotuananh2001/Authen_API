using JWT_API.Data;
using JWT_API.Repository.Authen;
using JWT_API.Services.AuthenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));

builder.Services.AddScoped<IAuthen, AuthencationImpl>();
builder.Services.AddScoped<IAuthenService, AuthenServiceImpl>();

// authen
var secretKey = builder.Configuration["AppSettings:SecretKey"];
var key = Encoding.UTF8.GetBytes(secretKey);

// cấu hình JWT 
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(options =>
   {
       options.RequireHttpsMetadata = true;
       options.SaveToken = true;
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuerSigningKey = true,
           IssuerSigningKey = new SymmetricSecurityKey(key),
           ValidateIssuer = false, // Có thể thay đổi thành true nếu bạn sử dụng Issuer
           ValidateAudience = false, // Có thể thay đổi thành true nếu bạn sử dụng Audience
           ValidateLifetime = true,
           ClockSkew = TimeSpan.Zero // Tùy chọn: bỏ qua thời gian trễ mặc định của JWT
       };
   });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // Xác thực
app.UseAuthorization(); // phân quyền

app.MapControllers();

app.Run();
