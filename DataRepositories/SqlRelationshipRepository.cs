using FluentBlog.Infrastructure;
using FluentBlog.Models;
using System.Collections.Generic;
using System.Linq;
using FluentBlog.Enum;

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
        public bool Delete(int? mid = null, int? aid = null)
        {
            switch (mid)
            {
                case null when aid == null:
                    return false;
                case null:
                {
                    IQueryable<Relationship> relationship = _context.Relationships.Where(r => r.Aid.Equals(aid));
                    _context.Relationships.RemoveRange(relationship);
                    int state = _context.SaveChanges();
                    return state > 0;
                }
                default:
                {
                    if (aid == null)
                    {
                        IQueryable<Relationship> relationship = _context.Relationships.Where(r => r.Aid.Equals(mid));
                        _context.Relationships.RemoveRange(relationship);
                        int state = _context.SaveChanges();
                        return state > 0;
                    }
                    else
                    {
                        Relationship relationship =
                            _context.Relationships.FirstOrDefault(r => r.Mid == mid & r.Aid == aid);
                        if (relationship == null) return false;
                        _context.Relationships.Remove(relationship);
                        int state = _context.SaveChanges();
                        return state > 0;
                    }
                }
            }
        }

        // 查询分类/标签下的所有文章
        public List<Archive> GetArchivesByMetaId(int mid)
        {
            var archiveIdList = _context.Relationships.Where(r => r.Mid.Equals(mid)).Select(r => r.Aid).ToList();
            return archiveIdList.Select(aid => _context.Archives.Find(aid)).ToList();
        }

        // 查询文章所属的分类/标签
        public List<Meta> GetMetasByArchiveId(int aid, MetaType type)
        {
            List<int> metaIdList = _context.Relationships.Where(r => r.Aid.Equals(aid)).Select(r => r.Mid).ToList();
            return type switch
            {
                // 分类
                MetaType.Category => metaIdList.Select(mid => _context.Metas.Find(mid))
                    .Where(m => m.Type.Equals("category")).ToList(),
                // 标签
                MetaType.Tag => metaIdList.Select(mid => _context.Metas.Find(mid)).Where(m => m.Type.Equals("tag")).ToList(),
                // 全部
                _ => metaIdList.Select(mid => _context.Metas.Find(mid)).ToList()
            };
        }
    }
}