using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.DataRepositories;
using FluentBlog.Enum;
using FluentBlog.Models;
using FluentBlog.ViewModels;

namespace FluentBlog.ViewComponents
{
    public class ManageModalViewComponent : ViewComponent
    {
        /// <summary>
        /// 编辑窗口
        /// </summary>
        public IViewComponentResult Invoke(ModalViewModel modalViewModel)
        {
            return View(modalViewModel);
        }
    }
}
