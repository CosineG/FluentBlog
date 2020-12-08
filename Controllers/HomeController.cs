using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FluentBlog.DataRepositories;
using FluentBlog.Models;
using FluentBlog.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace FluentBlog.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IMetaRepository _metaRepository;
        private readonly IArchiveRepository _archiveRepository;
        private readonly CustomUserManager _customUserManager;
        private readonly ISettingRepository _settingRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFeedRepository _feedRepository;

        public HomeController(IRelationshipRepository relationshipRepository, IMetaRepository metaRepository,
            IArchiveRepository archiveRepository, CustomUserManager customUserManager,
            ISettingRepository settingRepository, IHttpContextAccessor httpContextAccessor,
            IFeedRepository feedRepository) : base(settingRepository)
        {
            _metaRepository = metaRepository;
            _relationshipRepository = relationshipRepository;
            _archiveRepository = archiveRepository;
            _customUserManager = customUserManager;
            _settingRepository = settingRepository;
            _httpContextAccessor = httpContextAccessor;
            _feedRepository = feedRepository;
        }

        // 首页
        [Route("")]
        // 全部文章
        [Route("/page/{page}")]
        // 分类下的文章
        [Route("/meta/{slug}/{page?}")]
        public ViewResult Index(int? page, string slug = null)
        {
            // 获取用户设置的每页包含文章数
            var settings = _settingRepository.GetSettings();
            int archivesCountPerPage = Convert.ToInt32(settings["ArchivesCountPerPage"]);

            HomeViewModel homeViewModel;

            // 未选择分类
            if (slug == null)
            {
                // 得到总页数
                int pageCount = _archiveRepository.GetArchivesCount() / archivesCountPerPage + 1;
                if (page != null && page > pageCount)
                {
                    Response.StatusCode = 404;
                    return View("ArchiveNotFound");
                }

                // 得到文章详情
                List<Archive> archives = _archiveRepository.GetArchivesByPage(page ?? 1, archivesCountPerPage);
                List<User> authors = new List<User>();
                List<List<Meta>> categories = new List<List<Meta>>();
                foreach (var archive in archives)
                {
                    archive.Text = _archiveRepository.MarkdownToPlainText(archive.Text);
                    archive.TitleImage ??= _archiveRepository.GetDefaultTitleImage();
                    authors.Add(_customUserManager.GetUserById(archive.Uid));
                    categories.Add(_relationshipRepository.GetMetasByArchiveId(archive.Aid, 0));
                }

                homeViewModel = new HomeViewModel()
                {
                    Archives = archives,
                    Authors = authors,
                    Categories = categories,
                    PageNum = page ?? 1,
                    PageCount = pageCount
                };
                if (page == null || page == 1)
                {
                    ViewBag.Title = "";
                }
                else
                {
                    ViewBag.Title = "第" + page + "页" + " - ";
                }
            }
            // 选择分类
            else
            {
                // 得到当前分类
                Meta currentMeta = _metaRepository.GetMetaBySlug(slug);
                if (currentMeta == null)
                {
                    Response.StatusCode = 404;
                    return View("ArchiveNotFound");
                }

                // 得到总页数
                int pageCount = _metaRepository.GetArchiveOfMetaCount(currentMeta.Mid) / archivesCountPerPage + 1;
                if (page != null && page > pageCount)
                {
                    Response.StatusCode = 404;
                    return View("ArchiveNotFound");
                }

                // 得到文章详情
                List<Archive> archives =
                    _metaRepository.GetArchivesByMetaAndPage(currentMeta.Mid, page ?? 1, archivesCountPerPage);
                List<User> authors = new List<User>();
                List<List<Meta>> categories = new List<List<Meta>>();
                foreach (var archive in archives)
                {
                    archive.Text = _archiveRepository.MarkdownToPlainText(archive.Text);
                    archive.TitleImage ??= _archiveRepository.GetDefaultTitleImage();
                    authors.Add(_customUserManager.GetUserById(archive.Uid));
                    categories.Add(_relationshipRepository.GetMetasByArchiveId(archive.Aid, 0));
                }

                homeViewModel = new HomeViewModel
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
                    ViewBag.Title = currentMeta.Name + " - ";
                }
                else
                {
                    ViewBag.Title = "第" + page + "页" + " - " + currentMeta.Name + " - ";
                }
            }

            return View(homeViewModel);
        }

        // 文章页
        [Route("/archive/{aid}")]
        public ViewResult Archive(int aid)
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
            List<Meta> categories = _relationshipRepository.GetMetasByArchiveId(archive.Aid, 0);
            List<Meta> tags = _relationshipRepository.GetMetasByArchiveId(archive.Aid, 1);

            ArchiveViewModel archiveViewModel = new ArchiveViewModel
            {
                Archive = archive,
                Author = author,
                DefaultTitleImage = _archiveRepository.GetDefaultTitleImage(),
                Url = _httpContextAccessor.HttpContext?.Request?.GetDisplayUrl(),
                Categories = categories,
                Tags = tags
            };
            ViewBag.Title = archive.Title + " - ";
            //将ViewModel对象传递给View()方法
            return View(archiveViewModel);
        }

        // 动态
        [Route("/feed")]
        public ViewResult Feed()
        {
            return View();
        }

        // 给动态点赞
        [HttpGet]
        public JsonResult AddFeedLikesCount(int fid)
        {
            Feed feed = _feedRepository.AddLikesCount(fid);
            return Json(feed);
        }

        // 归档
        [Route("/filing")]
        public ViewResult Filing()
        {
            // 统计信息
            int archiveCount = _archiveRepository.GetArchivesCount();
            int categoryCount = _metaRepository.GetMetaCount(0);
            int tagCount = _metaRepository.GetMetaCount(1);

            // 获取文章
            List<Archive> archives = _archiveRepository.GetAllArchives();
            List<List<Archive>> archivesSortByYear = new List<List<Archive>>();
            List<List<List<Archive>>> archivesSortByMonth = new List<List<List<Archive>>>();
            List<Archive> tempArchives = new List<Archive>();
            List<List<Archive>> tempArchivesLists = new List<List<Archive>>();
            // 得到按年分的文章列表
            for (int i = 0; i < archives.Count; i++)
            {
                if (i != 0 && archives[i].Created.Year != archives[i - 1].Created.Year)
                {
                    archivesSortByYear.Add(tempArchives.GetRange(0, tempArchives.Count));
                    tempArchives.Clear();
                }

                tempArchives.Add(archives[i]);
            }

            archivesSortByYear.Add(tempArchives.GetRange(0, tempArchives.Count));

            // 得到按年-月分的文章列表
            foreach (var archiveList in archivesSortByYear)
            {
                tempArchives.Clear();
                for (int i = 0; i < archiveList.Count; i++)
                {
                    if (i != 0 && archiveList[i].Created.Month != archiveList[i - 1].Created.Month)
                    {
                        tempArchivesLists.Add(tempArchives.GetRange(0, tempArchives.Count));
                        tempArchives.Clear();
                    }

                    tempArchives.Add(archiveList[i]);
                }

                tempArchivesLists.Add(tempArchives.GetRange(0, tempArchives.Count));
                archivesSortByMonth.Add(tempArchivesLists.GetRange(0, tempArchivesLists.Count));
                tempArchivesLists.Clear();
            }

            // 获得所有分类/标签以及其下的文章数目
            Tuple<List<Meta>, List<int>> categories = _metaRepository.GetMetasAndCountIncluded(0);
            Tuple<List<Meta>, List<int>> tags = _metaRepository.GetMetasAndCountIncluded(1);
            FilingViewModel filingViewModel = new FilingViewModel
            {
                Count = new Tuple<int, int, int>(archiveCount, categoryCount, tagCount),
                Archives = archivesSortByMonth,
                Categories = categories,
                Tags = tags
            };
            ViewBag.Title = "归档" + " - ";
            return View(filingViewModel);
        }

        // 友链
        [Route("/friend")]
        public ViewResult Friend()
        {
            return View();
        }

        // 关于
        [Route("/about")]
        public ViewResult About()
        {
            Archive archive = _archiveRepository.GetArchiveById(0);
            ArchiveViewModel archiveViewModel = new ArchiveViewModel
            {
                Archive = archive
            };
            return View(archiveViewModel);
        }
    }
}