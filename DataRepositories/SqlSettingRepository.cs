using FluentBlog.Infrastructure;
using FluentBlog.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FluentBlog.DataRepositories
{
    public class SqlSettingRepository : ISettingRepository
    {
        private readonly AppDbContext _context;

        public SqlSettingRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        public Setting Update(Setting updateSetting)
        {
            var setting = _context.Settings.Attach(updateSetting);
            setting.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateSetting;
        }

        /// <summary>
        /// 更新所有设置
        /// </summary>
        public Dictionary<string, string> UpdateMultipleSettings(Dictionary<string, string> settings)
        {
            var multipleSetting = settings.Select(s => new Setting {Name = s.Key, Value = s.Value});
            foreach (var updateSetting in multipleSetting)
            {
                var setting = _context.Settings.Attach(updateSetting);
                setting.State = EntityState.Modified;
            }

            _context.SaveChanges();
            return settings;
        }

        public Dictionary<string, string> GetSettings()
        {
            return _context.Settings.AsNoTracking().ToDictionary(s => s.Name, s => s.Value);
        }
    }
}