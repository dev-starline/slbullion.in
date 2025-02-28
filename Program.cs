using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using SL_Bullion.Constant;
using SL_Bullion.DAL;
using StackExchange.Redis;
using System.Configuration;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options=>{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});
builder.Services.AddDbContext<BullionDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession(options=>
{
    options.IdleTimeout=TimeSpan.FromMinutes(120);
}
);
builder.Services.AddControllersWithViews().AddNToastNotifyNoty();
builder.Services.AddTransient<ApplicationConstant>();
builder.Services.AddTransient<ResponseMessage>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
        policy =>
        {
            var allowedOrigins = builder.Configuration.GetSection("allowedOrigins").Get<string[]>();
            policy.WithOrigins(allowedOrigins).AllowAnyHeader().AllowAnyMethod();
        });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowOrigin");
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.UseNToastNotify();
app.MapControllerRoute(name: "default",
               pattern: "{controller=Symbol}/{action=List}/{id?}");

app.Use(async (context, next) =>
{
    string? session = context.Session.GetString("role");
    if (context.Request.RouteValues["controller"]?.ToString()?.ToLower() == "master")
    {
        if (context.Request.RouteValues["action"]?.ToString()?.ToLower() != "login")
        {
            if (string.IsNullOrEmpty(session))
            {
                var path = $"/Master/Login";
                context.Response.Redirect(path);
                return;
            }
        }
    } else if (context.Request.RouteValues["controller"]?.ToString()?.ToLower() == "bullion") 
    {
       
    }
    else if (!context.Request.Path.Value.Contains("/Login"))
    {
        if (string.IsNullOrEmpty(session))
        {
            var path = $"/Login";
            context.Response.Redirect(path);
            return;
        }
    }
    await next();
});

app.Run();
