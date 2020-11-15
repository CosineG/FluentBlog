using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.DataRepositories;
using FluentBlog.Models;
using FluentBlog.ViewModels;


namespace FluentBlog.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArchiveRepository _archiveRepository;
        private readonly CustomUserManager _customUserManager;
        private readonly ISettingRepository _settingRepository;

        public HomeController(IArchiveRepository archiveRepository, CustomUserManager customUserManager,
            ISettingRepository settingRepository)
        {
            _archiveRepository = archiveRepository;
            _customUserManager = customUserManager;
            _settingRepository = settingRepository;
        }

        // 首页
        [Route("")]
        // [Route("PageNum/{page?}")]
        public ViewResult Index(int? page)
        {
            int archivesPerPage = 5;
            List<Archive> archives = _archiveRepository.GetArchivesByPage(page ?? 1, archivesPerPage);
            List<User> authors = new List<User>();
            foreach (var archive in archives)
            {
                archive.Text = _archiveRepository.MarkdownToPlainText(archive.Text);
                archive.TitleImage ??= _archiveRepository.GetDefaultTitleImage();
                authors.Add(_customUserManager.GetUserById(archive.Uid));
            }

            int pageNum = _archiveRepository.GetArchivesNum() / archivesPerPage + 1;
            HomeViewModel homeViewModel = new HomeViewModel()
            {
                Archives = archives,
                Authors = authors,
                PageNum = page ?? 1,
                PageCount = pageNum
            };
            var settings = _settingRepository.GetSettings();
            if (page == null || page == 1)
            {
                ViewBag.Title = settings["BlogName"];
            }
            else
            {
                ViewBag.Title = "第" + page + "页" + " - " + settings["BlogName"];
            }

            return View(homeViewModel);
        }
    }
}