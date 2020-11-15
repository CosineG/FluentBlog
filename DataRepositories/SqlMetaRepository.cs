using FluentBlog.Infrastructure;
using FluentBlog.Models;
using System.Collections.Generic;

namespace FluentBlog.DataRepositories
{
    public class SqlMetaRepository : IMetaRepository
    {
        private readonly AppDbContext _context;

        public SqlMetaRepository(AppDbContext context)
        {
            _context = context;
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
    }
}