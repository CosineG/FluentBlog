using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.Models
{
    // 文章
    public class Archive
    {
        // 文章编号
        [Key]
        public int Aid { get; set; }
        // 标题
        [Required]
        public string Title { get; set; }
        // 创建时间
        [Required]
        public DateTime Created { get; set; }
        // 内容
        [DefaultValue(null)]
        public string Text { get; set; }
        // 作者ID
        public string Uid { get; set; }
        // 查看数
        [DefaultValue(0)]
        public int Views { get; set; }
        // 评论数
        [DefaultValue(0)]
        public int CommentsNum { get; set; }
        // 头图
        public string TitleImage { get; set; }
    }
}
