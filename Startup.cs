using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FluentBlog.DataRepositories;
using FluentBlog.Infrastructure;
using FluentBlog.CustomMiddlewares;
using FluentBlog.Models;
using Microsoft.AspNetCore.HttpOverrides;

namespace FluentBlog
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // 连接Mysql数据库
            string connectionString = _configuration.GetConnectionString("DBConnection");
            services.AddDbContextPool<AppDbContext>(options =>
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            // 注册用户时的密码要求
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            });
            // 用户验证服务
            services.AddIdentity<User, IdentityRole>()
                .AddUserManager<UserManager<User>>()
                .AddErrorDescriber<CustomIdentityErrorDescriberZhHans>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/admin/login";
            });
            services.AddControllersWithViews(a => a.EnableEndpointRouting = false)
                .AddXmlSerializerFormatters()
                .AddJsonOptions(option => { });
                // Razor页面实时编译，方便调试
                //.AddRazorRuntimeCompilation();
            // 依赖注入仓储
            services.AddScoped<IMetaRepository, SqlMetaRepository>();
            services.AddScoped<IArchiveRepository, SqlArchiveRepository>();
            services.AddScoped<IFeedRepository, SqlFeedRepository>();
            services.AddScoped<IFriendRepository, SqlFriendRepository>();
            services.AddScoped<IRelationshipRepository, SqlRelationshipRepository>();
            services.AddScoped<ISettingRepository, SqlSettingRepository>();
            services.AddDistributedMemoryCache();
            services.AddSession();
            services.AddHttpClient("leanCloudClient");
            services.AddRouting(options => options.LowercaseUrls = true);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
            }
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            app.UseMvcWithDefaultRoute();
        }
    }
}