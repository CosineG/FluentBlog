using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentBlog.DataRepositories;
using FluentBlog.Models;
using FluentBlog.ViewModels;

namespace FluentBlog.Controllers
{
    public class MetaController : Controller
    {
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IMetaRepository _metaRepository;
        private readonly IArchiveRepository _archiveRepository;
        private readonly CustomUserManager _customUserManager;
        private readonly ISettingRepository _settingRepository;

        public MetaController(IRelationshipRepository relationshipRepository, IMetaRepository metaRepository,
            IArchiveRepository archiveRepository, CustomUserManager customUserManager,
            ISettingRepository settingRepository)
        {
            _metaRepository = metaRepository;
            _relationshipRepository = relationshipRepository;
            _archiveRepository = archiveRepository;
            _customUserManager = customUserManager;
            _settingRepository = settingRepository;
        }

        public ViewResult Index(string slug, int? page)
        {
            Meta currentMeta = _metaRepository.GetMetaBySlug(slug);
            if (currentMeta == null)
            {
                Response.StatusCode = 404;
                return View("ArchiveNotFound");
            }

            // 获取用户设置的每页包含文章数
            var settings = _settingRepository.GetSettings();
            int archivesCountPerPage = Convert.ToInt32(settings["ArchivesCountPerPage"]);
            int pageCount = _metaRepository.GetArchiveOfMetaCount(currentMeta.Mid) / archivesCountPerPage + 1;
            if (page != null && page > pageCount)
            {
                Response.StatusCode = 404;
                return View("ArchiveNotFound");
            }
            List<Archive> archives =
                _metaRepository.GetArchivesByMetaAndPage(currentMeta.Mid, page ?? 1, archivesCountPerPage);
            List<User> authors = new List<User>();
            List<List<Meta>> categories = new List<List<Meta>>();
            foreach (var archive in archives)
            {
                archive.Text = _archiveRepository.MarkdownToPlainText(archive.Text);
                archive.TitleImage ??= _archiveRepository.GetDefaultTitleImage();
                authors.Add(_customUserManager.GetUserById(archive.Uid));
                categories.Add(_relationshipRepository.GetMetasByArchiveId(archive.Aid, "category"));
            }
            MetaViewModel metaViewModel = new MetaViewModel
            {
                CurrentMeta = currentMeta,
                Archives = archives,
                Authors = authors,
                Categories = categories,
                PageNum = page ?? 1,
                PageCount = pageCount
            };
            if (page == null || page == 1)
            {
                ViewBag.Title = currentMeta.Name + " - " + settings["BlogName"];
            }
            else
            {
                ViewBag.Title = "第" + page + "页" + " - " + currentMeta.Name + " - " + settings["BlogName"];
            }

            return View(metaViewModel);
        }
    }
}