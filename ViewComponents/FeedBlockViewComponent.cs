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
            var lastFeed = _feedRepository.GetLastFeed();
            if (lastFeed == null)
            {
                return Content("");
            }
            feeds.Add(lastFeed);
            authors.Add(_customUserManager.GetUserById(lastFeed.Uid));
            FeedViewModel homeViewModel = new FeedViewModel
            {
                Feeds = feeds,
                Authors = authors
            };
            return View(homeViewModel);
        }
    }
}