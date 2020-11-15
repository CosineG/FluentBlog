using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.Models
{
    // 文件
    public class File
    {
        // 编号
        [Key]
        public int Fid { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
    }
}
