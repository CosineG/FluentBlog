using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Models;
using FluentBlog.DataRepositories;
using FluentBlog.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;

namespace FluentBlog.Controllers
{
    public class ArchiveController : Controller
    {
        private readonly IMetaRepository _metaRepository;
        private readonly IArchiveRepository _archiveRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CustomUserManager _customUserManager;
        private readonly ISettingRepository _settingRepository;

        public ArchiveController(IMetaRepository metaRepository, IArchiveRepository archiveRepository,
            CustomUserManager customUserManager, IHttpContextAccessor httpContextAccessor,
            ISettingRepository settingRepository)
        {
            _metaRepository = metaRepository;
            _archiveRepository = archiveRepository;
            _customUserManager = customUserManager;
            _httpContextAccessor = httpContextAccessor;
            _settingRepository = settingRepository;
        }

        [Route("Archive")]
        public ViewResult Detail(int aid)
        {
            Archive archive = _archiveRepository.GetArchiveById(aid);
            //判断学生信息是否存在
            if (archive == null)
            {
                Response.StatusCode = 404;
                return View("ArchiveNotFound", aid);
            }

            // 查询作者
            User author = _customUserManager.GetUserById(archive.Uid);
            ArchiveViewModel archiveViewModel = new ArchiveViewModel()
            {
                Archive = archive,
                Author = author,
                DefaultTitleImage = _archiveRepository.GetDefaultTitleImage(),
                Url = _httpContextAccessor.HttpContext?.Request?.GetDisplayUrl()
            };
            var settings = _settingRepository.GetSettings();
            ViewBag.Title = archive.Title + " - " + settings["BlogName"];
            //将ViewModel对象传递给View()方法
            return View(archiveViewModel);
        }
    }
}