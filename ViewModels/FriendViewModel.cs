using FluentBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.ViewModels
{
    public class FriendViewModel
    {
        public List<Friend> Friends { get; set; }
        public int FriendsCount { get; set; }

        public readonly List<string> Colors = new List<string>()
        {
            "blue",
            "dark-blue",
            "orchid",
            "purple",
            "coral",
            "salmon",
            "orange",
            "cyan",
            "turquoise",
            "green"
        };
    }
}