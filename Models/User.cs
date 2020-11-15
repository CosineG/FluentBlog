using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FluentBlog.Models
{
    // 用户
    public class User: IdentityUser
    {
        // 昵称（显示名称）
        public string DisplayName { get; set; }
    }
}
