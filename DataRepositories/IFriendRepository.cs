using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Models;

namespace FluentBlog.DataRepositories
{
    public interface IFriendRepository
    {
        // 增
        Friend Insert(Friend friend);
        // 删
        bool Delete(int fid);
        // 改
        Friend Update(Friend updateFriend);
        // 查询友链总数
        int GetFriendsCount();
        // 根据id查询友链
        Friend GetFriendById(int fid);
        // 获得所有友链
        List<Friend> GetAllFriends();
    }
}
