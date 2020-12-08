using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Models;

namespace FluentBlog.DataRepositories
{
    public interface IMetaRepository
    {
        // 增
        Meta Insert(Meta meta);
        // 删
        Meta Delete(int id);
        // 改
        Meta Update(Meta updateMeta);
        // 查询分类/标签数量
        int GetMetaCount(int? type);
        // 获得所有分类/标签以及其下的文章数目
        Tuple<List<Meta>, List<int>> GetMetasAndCountIncluded(int? type);
        // 用slug查询分类/标签
        Meta GetMetaBySlug(string slug);
        // 查询分类/标签下的文章数目
        int GetArchiveOfMetaCount(int mid);
        // 获得分类/标签下第几页的文章
        public List<Archive> GetArchivesByMetaAndPage(int mid, int page, int archivesCountPerPage);
    }
}
