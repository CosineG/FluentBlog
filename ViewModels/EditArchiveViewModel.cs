using FluentBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.ViewModels
{
    public class EditArchiveViewModel : ArchiveViewModel
    {
        public new Dictionary<int, bool> Categories { get; set; }
        public new Dictionary<int, bool> Tags { get; set; }
    }
}