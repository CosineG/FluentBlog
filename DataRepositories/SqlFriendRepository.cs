using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Infrastructure;
using FluentBlog.Models;

namespace FluentBlog.DataRepositories
{
    public class SqlFriendRepository : IFriendRepository
    {
        private readonly AppDbContext _context;

        public SqlFriendRepository(AppDbContext context)
        {
            _context = context;
        }
        // 增
        public Friend Insert(Friend friend)
        {
            _context.Friends.Add(friend);
            _context.SaveChanges();
            return friend;
        }
        // 删
        public bool Delete(int fid)
        {
            Friend friend = _context.Friends.Find(fid);
            if (friend == null) return false;
            _context.Friends.Remove(friend);
            int state = _context.SaveChanges();
            return state > 0;
        }
        // 改
        public Friend Update(Friend updateFriend)
        {
            var friend = _context.Friends.Attach(updateFriend);
            friend.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updateFriend;
        }
        // 查询友链总数
        public int GetFriendsCount()
        {
            return _context.Friends.Count();
        }
        // 获得所有友链
        public List<Friend> GetAllFriends()
        {
            return _context.Friends.OrderBy(f => f.Fid).ToList();
        }
    }
}
