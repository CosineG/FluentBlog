using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Enum;
using FluentBlog.Models;

namespace FluentBlog.DataRepositories
{
    public interface IRelationshipRepository
    {
        // 增
        Relationship Insert(Relationship meta);
        // 改
        List<Relationship> Update(int aid, List<int> mids);
        // 删
        bool Delete(int? mid = null, int? aid = null);
        // 查询分类/标签下的所有文章
        List<Archive> GetArchivesByMetaId(int mid);
        // 查询文章所属的分类/标签
        List<Meta> GetMetasByArchiveId(int aid, MetaType type);
    }
}
