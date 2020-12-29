using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.DataRepositories;

namespace FluentBlog.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController(ISettingRepository settingRepository) : base(settingRepository)
        {
        }

        [Route("/Error/404")]
        public ViewResult PageNotFound()
        {
            ViewBag.Title = "页面未找到" + " - ";
            return View();
        }
    }
}