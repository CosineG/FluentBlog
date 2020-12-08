using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Models;
using Microsoft.AspNetCore.Identity;

namespace FluentBlog.Infrastructure
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meta>().HasData(
                new Meta
                {
                    Mid = 1,
                    Name = "未归类",
                    Slug = "uncategorized",
                    Type = "category",
                    Default = true
                }
            );
            modelBuilder.Entity<Archive>().HasData(
                new Archive
                {
                    Aid = 1,
                    Title = "欢迎使用Fluent Blog",
                    Text = "欢迎使用Fluent Blog。这里是使用了asp.net core构建的博客框架。",
                    Created = DateTime.Parse("2020-10-28"),
                    Uid = "d89af503-9855-465b-a575-fffa345f97e6"
                }
            );
            modelBuilder.Entity<Setting>().HasData(
                new Setting
                {
                    Name = "BlogName",
                    Value = "Fluent Blog"
                },
                new Setting
                {
                    Name = "BlogIntro",
                    Value = "现在向第一缕阳光宣誓，走出尘埃与那茫然彷徨。"
                },
                new Setting
                {
                    Name = "Notice",
                    Value = "欢迎使用Fluent Blog，一个基于ASP.NET Core 3.1的MVC博客框架。"
                },
                new Setting
                {
                    Name = "ArchivesCountPerPage",
                    Value = "5"
                },
                new Setting
                {
                    Name = "FeedSwitch",
                    Value = "on"
                },
                new Setting
                {
                    Name = "DonateSwitch",
                    Value = "on"
                },
                new Setting
                {
                    Name = "DonateAlipay",
                    Value = ""
                },
                new Setting
                {
                    Name = "DonateWechat",
                    Value = ""
                }
            );
        }
    }
}