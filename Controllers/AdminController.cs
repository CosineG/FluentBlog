using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentBlog.Controllers;
using FluentBlog.DataRepositories;
using FluentBlog.Models;
using FluentBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace FluentBlog.Controllers
{
    [Authorize]
    public class AdminController : BaseController
    {
        private readonly ISettingRepository _settingRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public AdminController(ISettingRepository settingRepository, UserManager<User> userManager,
            SignInManager<User> signInManager) : base(settingRepository)
        {
            _settingRepository = settingRepository;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // 登录页面
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
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
            return View();
        }
    }
}