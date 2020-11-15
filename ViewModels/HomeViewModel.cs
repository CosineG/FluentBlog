using FluentBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.ViewModels
{
    public class HomeViewModel
    {
        public List<Archive> Archives { get; set; }
        public List<User> Authors { get; set; }
        public int PageNum { get; set; }
        public int PageCount { get; set; }
    }
}
