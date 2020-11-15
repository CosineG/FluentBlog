using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Models;

namespace FluentBlog.DataRepositories
{
    public interface IArchiveRepository
    {
        // 增
        Archive Insert(Archive archive);
        // 删
        Archive Delete(int id);
        // 改
        Archive Update(Archive updateArchive);
        // 查
        Archive GetArchiveById(int aid);
        // 查询文章总数
        int GetArchivesNum();
        // 获得第几页的文章
        List<Archive> GetArchivesByPage(int page, int archivesPerPage);
        // 没有标题图时随机调用默认标题图
        string GetDefaultTitleImage();
        // markdown转纯文本（用于显示文章摘要）
        string MarkdownToPlainText(string content);
    }
}
