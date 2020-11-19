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
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IWebHostEnvironment _environment;

        public SqlArchiveRepository(AppDbContext context, IRelationshipRepository relationshipRepository,
            IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
            _relationshipRepository = relationshipRepository;
        }

        // 增
        public Archive Insert(Archive archive)
        {
            _context.Archives.Add(archive);
            _context.SaveChanges();
            return archive;
        }

        // 删
        public bool Delete(int aid)
        {
            Archive archive = _context.Archives.Find(aid);
            if (archive == null) return false;
            _context.Archives.Remove(archive);
            int state = _context.SaveChanges();
            _relationshipRepository.Delete(aid: aid);
            return state > 0;
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
        public int GetArchivesCount()
        {
            return _context.Archives.Count();
        }

        // 根据页码取得文章
        public List<Archive> GetArchivesByPage(int page, int archivesCountPerPage)
        {
            int skipNum = (page - 1) * archivesCountPerPage;
            int archivesNum = GetArchivesCount();
            if (archivesNum < archivesCountPerPage)
            {
                return _context.Archives.OrderByDescending(a => a.Aid).ToList();
            }

            return _context.Archives.OrderByDescending(a => a.Aid).Skip(skipNum).Take(archivesCountPerPage)
                .ToList();
        }

        // 取得网站自带的默认标题头图
        public string GetDefaultTitleImage()
        {
            string defaultTitleImageFolderPath = Path.Combine(_environment.WebRootPath, "images", "defaultTitleImages");
            DirectoryInfo defaultTitleImageFolder = new DirectoryInfo(defaultTitleImageFolderPath);
            FileInfo[] images = defaultTitleImageFolder.GetFiles();
            Random random = new Random();
            return Path.Combine("images", "defaultTitleImages", images[random.Next(0, images.Length)].Name);
        }

        // 删除markdown语法标记，用作文章预览内容
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