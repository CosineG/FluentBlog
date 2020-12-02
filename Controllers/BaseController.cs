using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.DataRepositories;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FluentBlog.Controllers
{
    // BaseController，提取博客设置供其他Controller使用
    public abstract class BaseController : Controller
    {
        private readonly ISettingRepository _settingRepository;

        protected BaseController(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var settings = _settingRepository.GetSettings();
            ViewBag.Settings = settings;
            base.OnActionExecuting(context);
        }
    }
}
