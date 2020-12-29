using FluentBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.ViewModels
{
    public class TopArchivesViewModel
    {
        public List<Archive> ArchivesByView { get; set; }
        public List<Archive> ArchivesByComment { get; set; }
    }
}