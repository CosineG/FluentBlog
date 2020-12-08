using System;
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

        // 查询分类/标签数量
        public int GetMetaCount(int? type)
        {
            return type switch
            {
                0 => _context.Metas.Count(m => m.Type == "category"),
                1 => _context.Metas.Count(m => m.Type == "tag"),
                _ => _context.Metas.Count()
            };
        }

        // 获得所有分类/标签以及其下的文章数目
        public Tuple<List<Meta>, List<int>> GetMetasAndCountIncluded(int? type)
        {
            List<Meta> metas = type switch
            {
                0 => _context.Metas.Where(m => m.Type == "category").ToList(),
                1 => _context.Metas.Where(m => m.Type == "tag").ToList(),
                _ => _context.Metas.ToList()
            };
            List<int> counts = metas.Select(meta => GetArchiveOfMetaCount(meta.Mid)).ToList();
            return new Tuple<List<Meta>, List<int>>(metas, counts);
        }

        // 用slug查询分类/标签
        public Meta GetMetaBySlug(string slug)
        {
            return _context.Metas.FirstOrDefault(m => m.Slug.Equals(slug));
        }

        // 查询分类/标签下的文章数目
        public int GetArchiveOfMetaCount(int mid)
        {
            return _context.Relationships.Count(r => r.Mid.Equals(mid));
        }


        // 根据页码取得分类/标签下的文章
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