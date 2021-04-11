using System.Collections.Generic;
using FluentBlog.Enum;
using FluentBlog.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace FluentBlog.ViewModels
{
    public class ManageViewModel
    {
        public ManageItemType ManageItemType { get; set; }
        public List<Archive> Archives { get; set; }
        public List<List<Meta>> ArchivesCategories { get; set; }
        public List<Meta> Metas { get; set; }
        public List<int> MetasCounts { get; set; }
        public List<Feed> Feeds { get; set; }
        public List<Friend> Friends { get; set; }
        public List<User> Authors { get; set; }


        public string Alert { get; set; }
    }
}