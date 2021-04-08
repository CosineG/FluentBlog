using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Controllers;
using FluentBlog.DataRepositories;
using FluentBlog.Models;
using FluentBlog.ViewModels;
using FluentBlog.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FluentBlog.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IArchiveRepository _archiveRepository;
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IMetaRepository _metaRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public AdminController(IRelationshipRepository relationshipRepository, IMetaRepository metaRepository,
            ISettingRepository settingRepository, IArchiveRepository archiveRepository,
            UserManager<User> userManager, SignInManager<User> signInManager) : base(settingRepository)
        {
            _metaRepository = metaRepository;
            _relationshipRepository = relationshipRepository;
            _settingRepository = settingRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _archiveRepository = archiveRepository;
        }

        // 登录页面
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewBag.Title = "登录" + " - ";
            return View();
        }

        // 登录操作
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email, model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                model.LastLoginFailed = true;
                return View(model);
            }


            if (string.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home");

            // 返回登录前原页面
            return Redirect(returnUrl);
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Title = "控制台" + " - ";
            return View();
        }

        [HttpGet]
        [Route("[controller]/[action]/{aid?}")]
        public IActionResult EditArchive(int? aid)
        {
            ViewBag.Title = "写文章" + " - ";
            Meta defaultMeta = _metaRepository.GetDefaultCategory();
            var allCategories = _metaRepository.GetMetasAndCountIncluded(MetaType.Category).Item1;
            var allTags = _metaRepository.GetMetasAndCountIncluded(MetaType.Tag).Item1;
            allCategories.Remove(defaultMeta);
            ViewBag.AllCategories = allCategories;
            ViewBag.AllTags = allTags;

            if (aid == null) return View();
            Archive archive = _archiveRepository.GetArchiveById(aid.Value);
            //判断文章是否存在
            if (archive == null)
            {
                return NotFound();
            }

            // 查询类别
            var ownedCategories = _relationshipRepository.GetMetasByArchiveId(archive.Aid, MetaType.Category);
            ownedCategories.Remove(defaultMeta);
            var ownedTags = _relationshipRepository.GetMetasByArchiveId(archive.Aid, MetaType.Tag);

            var categories = allCategories.ToDictionary(c => c.Mid, c => ownedCategories.Contains(c));
            var tags = allTags.ToDictionary(t => t.Mid, t => ownedTags.Contains(t));

            EditArchiveViewModel editArchiveViewModel = new EditArchiveViewModel
            {
                Archive = archive,
                Categories = categories,
                Tags = tags
            };
            return View(editArchiveViewModel);
        }

        [HttpPost]
        public IActionResult EditArchive(EditArchiveViewModel model, [FromForm(Name = "editor-markdown-doc")]
            string text)
        {
            var archive = model.Archive;
            archive.Text = text;
            var categories = model.Categories.Where(c => c.Value).Select(c => c.Key).ToList();
            var tags = model.Tags.Where(t => t.Value).Select(t => t.Key).ToList();
            var metas = categories.Union(tags).ToList();

            int defaultMid = _metaRepository.GetDefaultCategory().Mid;
            if (!categories.Any())
            {
                metas.Add(defaultMid);
            }
            else if (categories.Count > 1 && categories.Contains(defaultMid))
            {
                metas.Remove(defaultMid);
            }

            // 新文章
            if (archive.Aid == 0)
            {
                archive.Aid = _archiveRepository.GetMinAvailableId();
                archive.Uid = _userManager.GetUserAsync(HttpContext.User).Result.Id;
                archive.Created = DateTime.Now;
                _archiveRepository.Insert(archive);
            }
            // 修改老文章
            else
            {
                _archiveRepository.Update(archive);
            }

            _relationshipRepository.Update(archive.Aid, metas);

            return Json(archive);
        }
    }
}