using FluentBlog.Infrastructure;
using FluentBlog.Models;
using System.Collections.Generic;
using System.Linq;

namespace FluentBlog.DataRepositories
{
    public class SqlSettingRepository : ISettingRepository
    {
        private readonly AppDbContext _context;

        public SqlSettingRepository(AppDbContext context)
        {
            _context = context;
        }

        // 改
        public Setting Update(Setting updateSetting)
        {
            var setting = _context.Settings.Attach(updateSetting);
            setting.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateSetting;
        }

        public Dictionary<string,string> GetSettings()
        {
            return _context.Settings.ToDictionary(s=>s.Name,s=>s.Value);
        }
    }
}