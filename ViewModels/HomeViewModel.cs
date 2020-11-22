using FluentBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.ViewModels
{
    public class HomeViewModel
    {
        public Meta CurrentMeta { get; set; }
        public List<Archive> Archives { get; set; }
        public List<User> Authors { get; set; }
        public List<List<Meta>> Categories { get; set; }
        public int PageNum { get; set; }
        public int PageCount { get; set; }

        public readonly List<string> CategoryColors = new List<string>()
        {
            "btn-outline-primary",
            "btn-outline-success",
            "btn-outline-danger",
            "btn-outline-warning",
            "btn-outline-info"
        };
    }
}