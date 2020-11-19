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
        // 用slug查询类别
        Meta GetMetaBySlug(string slug);
        // 查询类别下的文章数目
        int GetArchiveOfMetaCount(int mid);
        // 获得类别下第几页的文章
        public List<Archive> GetArchivesByMetaAndPage(int mid, int page, int archivesCountPerPage);
    }
}
