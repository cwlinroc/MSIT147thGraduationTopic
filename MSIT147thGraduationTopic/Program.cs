using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MSIT147thGraduationTopic.EFModels;
using MSIT147thGraduationTopic.Models.Services;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<GraduationTopicContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString
    ("GraduationTopicConnection")));

//AspNetCore.Authentication �Τ�����ާ@������U DI  (�b Controller �d��~�ϥΤ覡)
builder.Services.AddHttpContextAccessor();

//�ۭq�Τ�n�J��T�ާ@���U DI
builder.Services.AddScoped<UserInfoService>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

//==== AspNetCore.Authentication ����d���������պA�]�m ===== (������ cookie �M��)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)    
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,options => 
        {
            //���n�J�ɷ|�۰ʲ���즹���}�C
            options.LoginPath = new PathString("/MemberFront/NoLogin");
            //�����v����ɷ|�۰ʲ���즹���}�C
            options.AccessDeniedPath = new PathString("/MemberFront/NoRole");
            //�n�J10����|����
            options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
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

//==== AspNetCore.Authentication �Τ�n�J����ާ@����ϥ� ====
//���涶�Ǥ����A�ˤ��M����\��|�L�k���`�u�@�C
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Merchandises}/{action=Index}/{id?}");

app.Run();
