using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.Models
{
    // 文章
    public class Feed
    {
        // 编号
        [Key]
        public int Fid { get; set; }
        // 内容
        [DefaultValue(null)]
        public string Text { get; set; }
        // 创建时间
        [Required]
        public DateTime Created { get; set; }
        // 作者ID
        public string Uid { get; set; }
        // 点赞数
        [DefaultValue(0)]
        public int Likes { get; set; }
    }
}
