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
    }
}