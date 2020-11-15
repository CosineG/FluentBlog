using FluentBlog.Infrastructure;
using FluentBlog.Models;
using System.Collections.Generic;
using System.Linq;

namespace FluentBlog.DataRepositories
{
    public class SqlRelationshipRepository : IRelationshipRepository
    {
        private readonly AppDbContext _context;

        public SqlRelationshipRepository(AppDbContext context)
        {
            _context = context;
        }

        // 增
        public Relationship Insert(Relationship relationship)
        {
            _context.Relationships.Add(relationship);
            _context.SaveChanges();
            return relationship;
        }

        // 删
        public Relationship Delete(int mid, int aid)
        {
            Relationship relationship = _context.Relationships.FirstOrDefault(r => r.Mid == mid & r.Aid == aid);
            if (relationship == null) return null;
            _context.Relationships.Remove(relationship);
            _context.SaveChanges();
            return relationship;
        }

        // 用分类id查文章
        public List<Archive> GetArchivesByMetaId(int mid)
        {
            var archiveIdList = _context.Relationships.Where(r => r.Mid.Equals(mid)).Select(r => r.Aid).ToList();
            return archiveIdList.Select(aid => _context.Archives.Find(aid)).ToList();
        }

        // 用文章id查分类
        public List<Meta> GetMetasByArchiveId(int aid)
        {
            var metaIdList = _context.Relationships.Where(r => r.Aid.Equals(aid)).Select(r => r.Mid).ToList();
            return metaIdList.Select(mid => _context.Metas.Find(mid)).ToList();
        }
    }
}