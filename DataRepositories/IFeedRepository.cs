using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Models;

namespace FluentBlog.DataRepositories
{
    public interface IFeedRepository
    {
        // 增
        Feed Insert(Feed feed);
        // 删
        bool Delete(int fid);
        // 改
        Feed Update(Feed feed);
        // 用动态id查动态
        Feed GetFeedById(int fid);
        // 查最新动态
        Feed GetLastFeed();
        // 查所有动态
        List<Feed> GetAllFeeds();
        // 查询动态总数
        int GetFeedsCount();
        // 增加浏览次数
        Feed AddLikesCount(int fid);
    }
}
