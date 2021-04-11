using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.CustomMiddlewares;

namespace FluentBlog.Models
{
    // 分类和标签
    public class Meta
    {
        // 分类编号
        [Key] 
        public int Mid { get; set; }

        // 显示名称
        [Required(ErrorMessage = "必须输入名称")]
        public string Name { get; set; }

        // 缩略名（链接中使用）
        [Required(ErrorMessage = "必须输入缩略名")]
        [RegularExpression(@"^[A-Za-z0-9]+$", ErrorMessage = "不得包含特殊字符")]
        public string Slug { get; set; }

        // 类型
        [Required] 
        public string Type { get; set; }

        // 描述
        public string Description { get; set; }

        // 默认
        public bool Default { get; set; }
    }
}