using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.DataRepositories;
using FluentBlog.Models;
using FluentBlog.ViewModels;
using System.Text.Json;

namespace FluentBlog.ViewComponents
{
    // 联系方式
    public class ContactBlockViewComponent : ViewComponent
    {
        private readonly ISettingRepository _settingRepository;

        public ContactBlockViewComponent(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public IViewComponentResult Invoke()
        {
            var settings = _settingRepository.GetSettings();
            var contacts = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(settings["Contacts"]);
            ContactViewModel contactViewModel = new ContactViewModel
            {
                Contacts = contacts
            };
            return View(contactViewModel);
        }
    }
}