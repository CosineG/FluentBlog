using System;
using FluentBlog.Infrastructure;
using FluentBlog.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;

namespace FluentBlog.DataRepositories
{
    public class SqlArchiveRepository : IArchiveRepository
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SqlArchiveRepository(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // 增
        public Archive Insert(Archive archive)
        {
            _context.Archives.Add(archive);
            _context.SaveChanges();
            return archive;
        }

        // 删
        public Archive Delete(int id)
        {
            Archive archive = _context.Archives.Find(id);

            if (archive == null) return null;
            _context.Archives.Remove(archive);
            _context.SaveChanges();

            return archive;
        }

        // 改
        public Archive Update(Archive updateArchive)
        {
            var archive = _context.Archives.Attach(updateArchive);
            archive.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateArchive;
        }

        // 用文章id查文章
        public Archive GetArchiveById(int aid)
        {
            return _context.Archives.Find(aid);
        }
        // 查询文章总数
        public int GetArchivesNum()
        {
            return _context.Archives.Count();
        }
        //
        public List<Archive> GetArchivesByPage(int page, int archivesPerPage)
        {
            int skipNum = (page - 1) * archivesPerPage;
            int archivesNum = GetArchivesNum();
            if (archivesNum < archivesPerPage)
            {
                return _context.Archives.OrderByDescending(a => a.Aid).ToList();
            }
            else
            {
                return _context.Archives.OrderByDescending(a => a.Aid).Skip(skipNum).Take(archivesPerPage).ToList();
            }
        }

        public string GetDefaultTitleImage()
        {
            string defaultTitleImageFolderPath = Path.Combine(_environment.WebRootPath, "images", "defaultTitleImages");
            DirectoryInfo defaultTitleImageFolder = new DirectoryInfo(defaultTitleImageFolderPath);
            FileInfo[] images = defaultTitleImageFolder.GetFiles();
            Random random = new Random();
            return Path.Combine("images", "defaultTitleImages", images[random.Next(0, images.Length)].Name);
        }

        public string MarkdownToPlainText(string content)
        {
            List<string> patterns = new List<string>()
            {
                //全局匹配内粗体
                @"(\*\*|__)(.*?)(\*\*|__)", 
                //全局匹配图片
                @"\!\[[\s\S]*?\]\([\s\S]*?\)", 
                //全局匹配连接
                @"\[[\s\S]*?\]\([\s\S]*?\)", 
                //全局匹配内html标签
                @"<\/?.+?\/?>", 
                //全局匹配内联代码块
                @"(\*)(.*?)(\*)", 
                //全局匹配内联代码块
                @"`{1,2}[^`](.*?)`{1,2}", 
                //全局匹配代码块
                @"```([\s\S]*?)```[\s]*", 
                //全局匹配删除线
                @"\~\~(.*?)\~\~", 
                //全局匹配无序列表
                @"[\s]*[-\*\+]+(.*)", 
                //全局匹配有序列表
                @"[\s]*[0-9]+\.(.*)", 
                //全局匹配标题
                @"(#+)(.*)", 
                //全局匹配摘要
                @"(>+)(.*)", 
                //全局匹配换行
                @"\r\n/g", 
                //全局匹配换行
                @"\n/g", 
                //全局匹配空字符
                @"\s/g"
            };
            return patterns.Aggregate(content, (current, pattern) => Regex.Replace(current, pattern, ""));
        }
    }
}