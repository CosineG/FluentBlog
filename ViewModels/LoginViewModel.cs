using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FluentBlog.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "必须填写邮箱")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "必须填写密码")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        [DefaultValue(false)]
        public bool LastLoginFailed { get; set; }
    }
}
