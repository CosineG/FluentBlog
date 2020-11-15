using FluentBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.ViewModels
{
    public class ArchiveViewModel
    {
        public Archive Archive { get; set; }
        public User Author { get; set; }
        public string DefaultTitleImage { get; set; }
        public string Url { get; set; }
    }
}
