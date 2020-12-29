using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.DataRepositories;
using FluentBlog.Models;
using Microsoft.AspNetCore.Http;
using FluentBlog.ViewModels;

namespace FluentBlog.ViewComponents
{
    public class FeedBlockViewComponent : ViewComponent
    {
        private readonly IFeedRepository _feedRepository;
        private readonly CustomUserManager _customUserManager;

        public FeedBlockViewComponent(IFeedRepository feedRepository, CustomUserManager customUserManager)
        {
            _feedRepository = feedRepository;
            _customUserManager = customUserManager;
        }

        public IViewComponentResult Invoke()
        {
            List<Feed> feeds = new List<Feed>();
            List<User> authors = new List<User>();
            List<bool> likedList = new List<bool>();
            var lastFeed = _feedRepository.GetLastFeed();
            if (lastFeed == null)
            {
                return Content("");
            }

            bool liked;
            try
            {
                liked = Convert.ToBoolean(HttpContext.Session.GetString("LikeFeed" + lastFeed.Fid));
            }
            catch (FormatException)
            {
                liked = false;
            }
            likedList.Add(liked);
            feeds.Add(lastFeed);
            authors.Add(_customUserManager.GetUserById(lastFeed.Uid));
            FeedViewModel feedViewModel = new FeedViewModel
            {
                LikedList = likedList,
                Feeds = feeds,
                Authors = authors
            };
            return View(feedViewModel);
        }
    }
}