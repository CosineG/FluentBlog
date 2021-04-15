using FluentBlog.DataRepositories;
using FluentBlog.Enum;
using FluentBlog.Models;
using FluentBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace FluentBlog.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly ISettingRepository _settingRepository;
        private readonly IArchiveRepository _archiveRepository;
        private readonly IFeedRepository _feedRepository;
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IMetaRepository _metaRepository;
        private readonly IFriendRepository _friendRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public AdminController(IRelationshipRepository relationshipRepository, IMetaRepository metaRepository,
            ISettingRepository settingRepository, IArchiveRepository archiveRepository, IFeedRepository feedRepository,
            IFriendRepository friendRepository, UserManager<User> userManager, SignInManager<User> signInManager) :
            base(settingRepository)
        {
            _metaRepository = metaRepository;
            _relationshipRepository = relationshipRepository;
            _settingRepository = settingRepository;
            _userManager = userManager;
            _signInManager = signInManager;
            _archiveRepository = archiveRepository;
            _feedRepository = feedRepository;
            _friendRepository = friendRepository;
        }

        /// <summary>
        /// 登录页面
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewBag.Title = "登录" + " - ";
            return View();
        }

        /// <summary>
        /// 登录操作
        /// </summary>
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

        /// <summary>
        /// 注册页面
        /// </summary>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 注册操作
        /// </summary>
        /// <param name="model"></param>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    DisplayName = model.DisplayName
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    return RedirectToAction("Index", "Home");
                }
            }
            model.LastRegisterFailed = true;
            return View(model);
        }



        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// 文章编辑页面
        /// </summary>
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

        /// <summary>
        /// 编辑文章
        /// </summary>
        [HttpPost]
        public IActionResult EditArchive(EditArchiveViewModel model)
        {
            var archive = model.Archive;
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
                // archive.Aid = _archiveRepository.GetMinAvailableId();
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

            return RedirectToAction("Manage", "Admin", new {manageItemType = "archive"});
        }


        /// <summary>
        /// 管理页面
        /// </summary>
        [HttpGet]
        [Route("[controller]/[action]/{manageItemType}")]
        public IActionResult Manage(string manageItemType)
        {
            ManageItemType itemType;
            try
            {
                itemType = (ManageItemType) System.Enum.Parse(typeof(ManageItemType), manageItemType, true);
            }
            catch (Exception)
            {
                return NotFound();
            }

            ManageViewModel manageViewModel = new ManageViewModel();
            switch (itemType)
            {
                // 文章
                case ManageItemType.Archive:
                    List<Archive> archives = _archiveRepository.GetAllArchives();
                    List<User> archivesAuthors = new List<User>();
                    List<List<Meta>> archivesCategories = new List<List<Meta>>();
                    foreach (var archive in archives)
                    {
                        archivesAuthors.Add(_userManager.FindByIdAsync(archive.Uid).Result);
                        archivesCategories.Add(
                            _relationshipRepository.GetMetasByArchiveId(archive.Aid, MetaType.Category));
                    }

                    manageViewModel = new ManageViewModel
                    {
                        Archives = archives,
                        Authors = archivesAuthors,
                        ArchivesCategories = archivesCategories
                    };
                    ViewBag.Title = "管理文章" + " - ";
                    break;
                // 分类
                case ManageItemType.Category:
                    var categories = _metaRepository.GetMetasAndCountIncluded(MetaType.Category);
                    manageViewModel = new ManageViewModel
                    {
                        Metas = categories.Item1,
                        MetasCounts = categories.Item2
                    };
                    ViewBag.Title = "管理分类" + " - ";
                    break;
                // 标签
                case ManageItemType.Tag:
                    var tags = _metaRepository.GetMetasAndCountIncluded(MetaType.Tag);
                    manageViewModel = new ManageViewModel
                    {
                        Metas = tags.Item1,
                        MetasCounts = tags.Item2
                    };
                    ViewBag.Title = "管理标签" + " - ";
                    break;
                // 动态
                case ManageItemType.Feed:
                    List<Feed> feeds = _feedRepository.GetAllFeeds();
                    List<User> feedsAuthors =
                        feeds.Select(feed => _userManager.FindByIdAsync(feed.Uid).Result).ToList();

                    manageViewModel = new ManageViewModel
                    {
                        Feeds = feeds,
                        Authors = feedsAuthors,
                    };
                    ViewBag.Title = "管理动态" + " - ";
                    break;
                // 友链
                case ManageItemType.Friend:
                    var friends = _friendRepository.GetAllFriends();
                    manageViewModel = new ManageViewModel
                    {
                        Friends = friends
                    };
                    ViewBag.Title = "管理友链" + " - ";
                    break;
            }

            manageViewModel.ManageItemType = itemType;
            return View(manageViewModel);
        }


        /// <summary>
        /// 添加操作
        /// </summary>
        [HttpPost]
        public string Insert(ModalViewModel modalViewModel)
        {
            switch (modalViewModel.ManageItemType)
            {
                // 分类和标签
                case ManageItemType.Category:
                case ManageItemType.Tag:
                    ModelState.Remove("SelectedMeta.Mid");
                    if (ModelState.IsValid && _metaRepository.CheckUnique(modalViewModel.SelectedMeta))
                    {
                        _metaRepository.Insert(modalViewModel.SelectedMeta);
                        return "ok";
                    }
                    else
                    {
                        return "添加失败，名称或缩略名不能重复";
                    }
                // 动态
                case ManageItemType.Feed:
                    modalViewModel.SelectedFeed.Uid = _userManager.GetUserAsync(HttpContext.User).Result.Id;
                    modalViewModel.SelectedFeed.Created = DateTime.Now;
                    ModelState.Remove("SelectedFeed.Fid");
                    ModelState.Remove("SelectedFeed.Uid");
                    ModelState.Remove("SelectedFeed.Created");
                    if (ModelState.IsValid)
                    {
                        _feedRepository.Insert(modalViewModel.SelectedFeed);
                        return "ok";
                    }
                    else
                    {
                        return "添加失败，请检查内容是否有误";
                    }
                // 友链
                case ManageItemType.Friend:
                    ModelState.Remove("SelectedFriend.Fid");
                    if (ModelState.IsValid)
                    {
                        _friendRepository.Insert(modalViewModel.SelectedFriend);
                        return "ok";
                    }
                    else
                    {
                        return "添加失败，请检查内容是否有误";
                    }
            }

            return "未知错误，请重试";
        }

        /// <summary>
        /// 修改操作
        /// </summary>
        [HttpPost]
        public string Update(ModalViewModel modalViewModel)
        {
            switch (modalViewModel.ManageItemType)
            {
                case ManageItemType.Category:
                case ManageItemType.Tag:
                    if (ModelState.IsValid && _metaRepository.CheckUnique(modalViewModel.SelectedMeta))
                    {
                        _metaRepository.Update(modalViewModel.SelectedMeta);
                        return "ok";
                    }
                    else
                    {
                        return "修改失败，名称或缩略名不能重复";
                    }
                case ManageItemType.Feed:
                    if (ModelState.IsValid)
                    {
                        _feedRepository.Update(modalViewModel.SelectedFeed);
                        return "ok";
                    }
                    else
                    {
                        return "修改失败，请检查内容是否有误";
                    }
                case ManageItemType.Friend:
                    if (ModelState.IsValid)
                    {
                        _friendRepository.Update(modalViewModel.SelectedFriend);
                        return "ok";
                    }
                    else
                    {
                        return "修改失败，请检查内容是否有误";
                    }
            }

            return "未知错误，请重试";
        }

        /// <summary>
        /// 删除操作
        /// </summary>
        [HttpPost]
        public IActionResult Delete([FromForm] ManageItemType manageItemType, [FromForm] List<int> items)
        {
            switch (manageItemType)
            {
                case ManageItemType.Archive:
                    foreach (var item in items)
                    {
                        _archiveRepository.Delete(item);
                    }

                    break;
                case ManageItemType.Category:
                case ManageItemType.Tag:
                    foreach (var item in items)
                    {
                        _metaRepository.Delete(item);
                    }

                    break;
                case ManageItemType.Feed:
                    foreach (var item in items)
                    {
                        _feedRepository.Delete(item);
                    }

                    break;
                case ManageItemType.Friend:
                    foreach (var item in items)
                    {
                        _friendRepository.Delete(item);
                    }

                    break;
            }

            return RedirectToAction("Manage", "Admin", new {manageItemType = manageItemType});
        }

        /// <summary>
        /// 显示编辑窗口
        /// </summary>
        [HttpPost]
        public IActionResult ShowModal([FromBody] Dictionary<string, int> inputModel)
        {
            ModalViewModel modalViewModel = new ModalViewModel
                {ManageItemType = (ManageItemType) inputModel["manageItemType"], OperateType = OperateType.Insert};
            if (inputModel["id"] == 0) return ViewComponent("ManageModal", new {modalViewModel = modalViewModel});
            switch ((ManageItemType) inputModel["manageItemType"])
            {
                case ManageItemType.Category:
                case ManageItemType.Tag:
                    modalViewModel.SelectedMeta = _metaRepository.GetMetaById(inputModel["id"]);
                    break;
                case ManageItemType.Feed:
                    modalViewModel.SelectedFeed = _feedRepository.GetFeedById(inputModel["id"]);
                    break;
                case ManageItemType.Friend:
                    modalViewModel.SelectedFriend = _friendRepository.GetFriendById(inputModel["id"]);
                    break;
            }

            modalViewModel.OperateType = OperateType.Update;
            return ViewComponent("ManageModal", new {modalViewModel = modalViewModel});
        }


        /// <summary>
        /// 设置页面
        /// </summary>
        [HttpGet]
        public IActionResult Config()
        {
            ConfigViewModel configViewModel = new ConfigViewModel
            {
                Settings = _settingRepository.GetSettings()
            };
            ViewBag.Title = "设置" + " - ";
            return View(configViewModel);
        }

        /// <summary>
        /// 更新设置
        /// </summary>
        [HttpPost]
        public IActionResult Config(ConfigViewModel configViewModel)
        {
            var settings = configViewModel.Settings;
            _settingRepository.UpdateMultipleSettings(settings);
            return RedirectToAction("Config");
        }
    }
}