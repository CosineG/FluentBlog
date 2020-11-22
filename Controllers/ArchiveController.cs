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
    public class ArchiveController : BaseController
    {
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IArchiveRepository _archiveRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CustomUserManager _customUserManager;
        private readonly ISettingRepository _settingRepository;

        public ArchiveController(IRelationshipRepository relationshipRepository, IArchiveRepository archiveRepository,
            CustomUserManager customUserManager, IHttpContextAccessor httpContextAccessor,
            ISettingRepository settingRepository) : base(settingRepository)
        {
            _relationshipRepository = relationshipRepository;
            _archiveRepository = archiveRepository;
            _customUserManager = customUserManager;
            _httpContextAccessor = httpContextAccessor;
            _settingRepository = settingRepository;
        }

        [Route("/archive/{aid}")]
        public ViewResult Index(int aid)
        {
            Archive archive = _archiveRepository.GetArchiveById(aid);
            //判断文章是否存在
            if (archive == null)
            {
                Response.StatusCode = 404;
                return View("ArchiveNotFound", aid);
            }
            archive = _archiveRepository.AddViewsCount(archive);
            // 查询作者
            User author = _customUserManager.GetUserById(archive.Uid);
            List<Meta> categories = _relationshipRepository.GetMetasByArchiveId(archive.Aid, "category");
            List<Meta> tags = _relationshipRepository.GetMetasByArchiveId(archive.Aid, "tag");
            
            ArchiveViewModel archiveViewModel = new ArchiveViewModel
            {
                Archive = archive,
                Author = author,
                DefaultTitleImage = _archiveRepository.GetDefaultTitleImage(),
                Url = _httpContextAccessor.HttpContext?.Request?.GetDisplayUrl(),
                Categories = categories,
                Tags = tags
            };
            var settings = _settingRepository.GetSettings();
            ViewBag.Title = archive.Title + " - " + settings["BlogName"];
            //将ViewModel对象传递给View()方法
            return View(archiveViewModel);
        }
    }
}