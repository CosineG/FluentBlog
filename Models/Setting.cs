using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.Models
{
    // 设置
    public class Setting
    {
        // 配置项名称
        [Key]
        [Required]
        public string Name { get; set; }
        // 配置项值
        public string Value { get; set; }
    }
}
