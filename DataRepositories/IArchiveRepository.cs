using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Models;
using FluentBlog.Enum;

namespace FluentBlog.DataRepositories
{
    public interface IArchiveRepository
    {
        // 增
        Archive Insert(Archive archive);
        // 删
        bool Delete(int aid);
        // 改
        Archive Update(Archive updateArchive);
        // 查
        Archive GetArchiveById(int aid);
        // 查询文章总数
        int GetArchivesCount();
        // 获得所有文章
        List<Archive> GetAllArchives();
        // 获得热门文章
        List<Archive> GetTopArchives(TopArchiveRule rule);
        // 获得第几页的文章
        List<Archive> GetArchivesByPage(int page, int archivesCountPerPage);
        // 没有标题图时随机调用默认标题图
        string GetDefaultTitleImage();
        // markdown转纯文本（用于显示文章摘要）
        string MarkdownToPlainText(string content);
        // 增加浏览次数
        Archive AddViewsCount(Archive archive);
        // 获得最小的可用ID
        int GetMinAvailableId();
    }
}
