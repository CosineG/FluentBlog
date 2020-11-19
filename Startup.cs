using System;
using System.Collections.Generic;
using System.Linq;
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
            services.AddDbContextPool<AppDbContext>(options =>
                options.UseMySql(_configuration.GetConnectionString("DBConnection")));
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
                .AddUserManager<CustomUserManager>()
                .AddErrorDescriber<CustomIdentityErrorDescriberZhHans>()
                .AddEntityFrameworkStores<AppDbContext>();
            services.AddControllersWithViews(a => a.EnableEndpointRouting = false)
                .AddXmlSerializerFormatters();
            // 依赖注入仓储
            services.AddScoped<IMetaRepository, SqlMetaRepository>();
            services.AddScoped<IArchiveRepository, SqlArchiveRepository>();
            services.AddScoped<IRelationshipRepository, SqlRelationshipRepository>();
            services.AddScoped<ISettingRepository, SqlSettingRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();
        }
    }
}