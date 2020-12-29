using FluentBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.ViewModels
{
    public class FeedViewModel
    {
        public List<Feed> Feeds { get; set; }
        public List<User> Authors { get; set; }
        public int FeedsCount { get; set; }
        public List<bool> LikedList { get; set; }
    }
}