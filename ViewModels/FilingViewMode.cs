using FluentBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.ViewModels
{
    public class FilingViewModel
    {
        public List<List<List<Archive>>> Archives;
        public Tuple<int, int, int> Count { get; set; }
        public Tuple<List<Meta>, List<int>> Categories { get; set; }
        public Tuple<List<Meta>, List<int>> Tags { get; set; }

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