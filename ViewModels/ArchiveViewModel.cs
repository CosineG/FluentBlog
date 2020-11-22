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
        public List<Meta> Categories { get; set; }
        public List<Meta> Tags { get; set; }
        public string DefaultTitleImage { get; set; }
        public string Url { get; set; }

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