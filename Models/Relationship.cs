using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.Models
{
    // 分类和标签
    public class Relationship
    {
        // 编号
        [Key]
        public int Rid { get; set; }
        // 文章编号
        public int Aid { get; set; }
        // 分类编号
        public int Mid { get; set; }
    }
}
