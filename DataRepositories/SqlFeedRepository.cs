using System;
using FluentBlog.Infrastructure;
using FluentBlog.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Hosting;

namespace FluentBlog.DataRepositories
{
    public class SqlFeedRepository : IFeedRepository
    {
        private readonly AppDbContext _context;

        public SqlFeedRepository(AppDbContext context)
        {
            _context = context;
        }

        // 增
        public Feed Insert(Feed feed)
        {
            _context.Feeds.Add(feed);
            _context.SaveChanges();
            return feed;
        }

        // 删
        public bool Delete(int fid)
        {
            Feed feed = _context.Feeds.Find(fid);
            if (feed == null) return false;
            _context.Feeds.Remove(feed);
            int state = _context.SaveChanges();
            return state > 0;
        }

        // 改
        public Feed Update(Feed updateFeed)
        {
            var feed = _context.Feeds.Attach(updateFeed);
            feed.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateFeed;
        }

        // 用动态id查动态
        public Feed GetFeedById(int fid)
        {
            return _context.Feeds.Find(fid);
        }

        // 查最新动态
        public Feed GetLastFeed()
        {
            return _context.Feeds.OrderByDescending(f => f.Created).FirstOrDefault();
        }

        // 查所有动态
        public List<Feed> GetAllFeeds()
        {
            return _context.Feeds.ToList();
        }

        // 查询动态总数
        public int GetFeedsCount()
        {
            return _context.Feeds.Count();
        }

        // 增加点赞数
        public Feed AddLikesCount(int fid)
        {
            Console.WriteLine(fid);
            Feed feed = GetFeedById(fid);
            feed.Likes += 1;
            Update(feed);
            return feed;
        }
    }
}