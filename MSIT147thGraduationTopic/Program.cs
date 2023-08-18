using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.BackGroundServiecs;
using MSIT147thGraduationTopic.Models.Infra.Utility;
using MSIT147thGraduationTopic.Models.Interfaces;
using MSIT147thGraduationTopic.Models.Services;
using System.Configuration;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using static MSIT147thGraduationTopic.Models.Infra.Utility.MailSetting;
using MSIT147thGraduationTopic.Models.Dtos.Paypal;
using MSIT147thGraduationTopic.Models.Paypal;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<OptionSettings>(builder.Configuration.GetSection("OptionSettings"));
builder.Services.AddSingleton(HtmlEncoder.Create(UnicodeRanges.All));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddScoped<IMailService, SendMailService>();
builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
builder.Services.AddScoped<IUrlHelper>(x => {
    var actionContext = x.GetRequiredService<IActionContextAccessor>().ActionContext;
    var factory = x.GetRequiredService<IUrlHelperFactory>();
    return factory.GetUrlHelper(actionContext);
});


/*** Timed execute backgroud task ***/

builder.Services.AddScoped<RecommendService>();
// Register as singleton first so it can be injected through Dependency Injection
builder.Services.AddSingleton<AutoPopularityCalculationService>();
// Add as hosted service using the instance registered as singleton before
builder.Services.AddHostedService(
    provider => provider.GetRequiredService<AutoPopularityCalculationService>());

// paypal client configuration
builder.Services.AddSingleton(x =>
    new PaypalClient(
        builder.Configuration["PayPalOptions:ClientId"],
        builder.Configuration["PayPalOptions:ClientSecret"],
        builder.Configuration["PayPalOptions:Mode"]
    )
);



// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GraduationTopicContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString
    ("GraduationTopicConnection")));

//AspNetCore.Authentication 用戶驗証操作機制註冊 DI  (在 Controller 範圍外使用方式)
builder.Services.AddHttpContextAccessor();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//==== AspNetCore.Authentication 全域範圍的驗証機制組態設置 ===== (全環境 cookie 套用)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
    {
        //未登入時會自動移轉到此網址。
        options.LoginPath = new PathString("/Member/NoLogin");
        //未授權角色時會自動移轉到此網址。
        options.AccessDeniedPath = new PathString("/Member/NoRole");
        //登入1hr後會失效
        options.ExpireTimeSpan = TimeSpan.FromHours(1);
    });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

//==== AspNetCore.Authentication 用戶登入驗証操作機制使用 ====
//執行順序不能顛倒不然驗証功能會無法正常工作。
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
