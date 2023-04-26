﻿using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechSquad.Models.Models;
using TechSquad.Models.ViewModels;
using TechSquad.Services.IServices;

namespace TechSquad.Application.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserManagerService _userManagerService;
        private readonly IMapper _mapper;
        private readonly IToastNotification _toastNotification;
        private readonly UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        public AdminController(IUserManagerService userManagerService, IMapper mapper, IToastNotification toastNotification, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManagerService = userManagerService;
            _mapper = mapper;
            _toastNotification = toastNotification;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View("AdminDashboard");
        }

        #region User Settings
        public IActionResult GetAllUser()
        {
            //var res = _userManagerService.GetAllUser();
            List<ApplicationUser> user  = _userManager.Users.ToList();
            return View("UserList", user );
        }

        [HttpGet]
        public IActionResult AddUser()
        {
            return View(new ApplicationUser());
        }

        [HttpPost]
        public async Task<IActionResult> AddUserAsync(ApplicationUser model)
        {
            //var res = _mapper.Map<AspNetUser>(model);
            //Add AutoGenerated Password
            RandomNumberGenerator obj = new RandomNumberGenerator();
            var Password = obj.RandomPassword();
            var status = await _userManager.CreateAsync(model, Password);
            if (status != null)
            {
                _toastNotification.AddSuccessToastMessage("User Added Successfully.password is :"+Password);
            }
            else
            {
                _toastNotification.AddErrorToastMessage("Some Error Occured");
            }
            return View("UserList");
        }

        [HttpGet]
        public async Task<IActionResult> EditUserAsync(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
           // var res = _mapper.Map<UserViewModel>(user);
            return View("AddUser",user);
        }

        public async Task<IActionResult> DeleteUserAsync(string id)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    _toastNotification.AddErrorToastMessage("Some Error Occured");
            }
            else
                ModelState.AddModelError("", "User Not Found");
            return View("UserList", _userManager.Users);
           
        }

        [HttpGet]
        public async Task<IActionResult> AddRole()
        {
           var res = await _roleManager.CreateAsync(new IdentityRole("Writers"));
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(string Role)
        {
            var res = await _roleManager.CreateAsync(new IdentityRole(Role));
            // var res = _mapper.Map<UserViewModel>(user);
            return View("AddRole", res);
        }
       
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                IdentityResult result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    _toastNotification.AddErrorToastMessage("Some Error Occured");
            }
            else
                ModelState.AddModelError("", "No role found");
            return View("Index", _roleManager.Roles);
        }
        #endregion

    }
}
