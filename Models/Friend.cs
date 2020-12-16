using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.Models
{
    // 友链站点
    public class Friend
    {
        [Key]
        public int Fid { get; set; }
        // 名称
        public string Name { get; set; }
        // 链接
        public string Url { get; set; }
        // 图标
        public string Avatar { get; set; }
        // 简介
        public string Introduce { get; set; }
    }
}
