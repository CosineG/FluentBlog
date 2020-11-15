using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.Models
{
    // 分类和标签
    public class Meta
    {
        // 分类编号
        [Key]
        public int Mid { get; set; }
        // 显示名称
        [Required]
        public string Name { get; set; }
        // 缩略名（链接中使用）
        [Required]
        public string Slug { get; set; }
        // 类型
        [Required]
        public string Type { get; set; }
        // 描述
        public string Description { get; set; }
    }
}
