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
    public class TopArchivesBlockViewComponent : ViewComponent
    {
        private readonly IArchiveRepository _archiveRepository;

        public TopArchivesBlockViewComponent(IArchiveRepository archiveRepository)
        {
            _archiveRepository = archiveRepository;
        }

        public IViewComponentResult Invoke()
        {
            List<Archive> archivesByView = _archiveRepository.GetTopArchives(TopArchiveRule.View);
            List<Archive> archivesByComment = _archiveRepository.GetTopArchives(TopArchiveRule.Comment);
            TopArchivesViewModel topArchivesViewModel = new TopArchivesViewModel
            {
                ArchivesByView = archivesByView,
                ArchivesByComment = archivesByComment
            };
            return View(topArchivesViewModel);
        }
    }
}
