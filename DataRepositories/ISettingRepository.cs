using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Models;

namespace FluentBlog.DataRepositories
{
    public interface ISettingRepository
    {
        // 改
        Setting Update(Setting updateSetting);
        Dictionary<string,string> GetSettings();

        /// <summary>
        /// 更新所有设置
        /// </summary>
        Dictionary<string, string> UpdateMultipleSettings(Dictionary<string, string> settings);
    }
}
