using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Models;

namespace FluentBlog.DataRepositories
{
    public interface IRelationshipRepository
    {
        // 增
        Relationship Insert(Relationship meta);
        // 删
        bool Delete(int? mid = null, int? aid = null);
        // 用文章id查分类id
        List<Archive> GetArchivesByMetaId(int mid);
        // 用分类id查文章id
        List<Meta> GetMetasByArchiveId(int aid, string type ="");
    }
}
