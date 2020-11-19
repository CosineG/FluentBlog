using FluentBlog.Infrastructure;
using FluentBlog.Models;
using System.Collections.Generic;
using System.Linq;

namespace FluentBlog.DataRepositories
{
    public class SqlMetaRepository : IMetaRepository
    {
        private readonly AppDbContext _context;
        private readonly IRelationshipRepository _relationshipRepository;

        public SqlMetaRepository(AppDbContext context, IRelationshipRepository relationshipRepository)
        {
            _context = context;
            _relationshipRepository = relationshipRepository;
        }

        // 增
        public Meta Insert(Meta meta)
        {
            _context.Metas.Add(meta);
            _context.SaveChanges();
            return meta;
        }

        // 删
        public Meta Delete(int mid)
        {
            Meta meta = _context.Metas.Find(mid);

            if (meta != null)
            {
                _context.Metas.Remove(meta);
                _context.SaveChanges();
            }

            return meta;
        }

        // 改
        public Meta Update(Meta updateMeta)
        {
            var meta = _context.Metas.Attach(updateMeta);
            meta.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateMeta;
        }

        // 用slug查询类别
        public Meta GetMetaBySlug(string slug)
        {
            return _context.Metas.FirstOrDefault(m => m.Slug.Equals(slug));
        }

        // 查询类别下的文章数目
        public int GetArchiveOfMetaCount(int mid)
        {
            return _context.Relationships.Count(r => r.Mid.Equals(mid));
        }


        // 根据页码取得类别下的文章
        public List<Archive> GetArchivesByMetaAndPage(int mid, int page, int archivesCountPerPage)
        {
            int skipNum = (page - 1) * archivesCountPerPage;
            int archivesCount = GetArchiveOfMetaCount(mid);
            var archives = _relationshipRepository.GetArchivesByMetaId(mid);
            if (archivesCount < archivesCountPerPage)
            {
                return archives.OrderByDescending(a => a.Aid).ToList();
            }

            return archives.OrderByDescending(a => a.Aid).Skip(skipNum).Take(archivesCountPerPage)
                .ToList();
        }
    }
}