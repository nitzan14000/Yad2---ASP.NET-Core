using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BLL.ProductBLL;
using BLL.UserBLL;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Yad2.Models;

namespace Yad2.Controllers
{
    public class UserController : Controller
    {
        private IUserService _userService;
        private IProductService _productService;
        public UserController(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult CheckUserNameAvailability(string id)
        {
            System.Threading.Thread.Sleep(200);
            var searchData = _userService.GetUser(id);
            if (searchData != null)
            {
                return Json(1);
            }
            else
            {
                return Json(0);
            }
        }
        [HttpPost]
        public IActionResult LogIn(SignInModel u)
        {
            if (!ModelState.IsValid)
            {
                return Redirect(Request.Headers["Referer"]);// מחזיר לעמוד שמוגדר בסתאראת אפ
            }
            try
            {
                if (_userService.IsUserLogin(u.UserName, u.Password, out UserModel user))
                {
                    Cookies(user);
                    return RedirectToAction("ShowProducts", "Products");
                }
                else
                {
                    TempData["ErrorLogIn"] = "one of the fillds is errow";
                    return Redirect(Request.Headers["Referer"]);
                }
            }
            catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
        }
        public IActionResult LogOut()
        {
            HttpContext.Response.Cookies.Delete("userFName");
            HttpContext.Response.Cookies.Delete("userLName");
            HttpContext.Response.Cookies.Delete("userName");
            HttpContext.Response.Cookies.Delete("userPassword");
            return RedirectToAction("ShowProducts", "Products");
        }
        [HttpGet]
        public IActionResult SignUp()
        {
            string UserName = HttpContext.Request.Cookies["userName"];
            if (UserName != null)
            {
                try
                {
                    UserModel existUser = _userService.GetUser(UserName);
                    return View(existUser);
                }
                catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
            }
            return View(new UserModel());
        }
        [HttpPost]
        public IActionResult SignUp(UserModel user)
        {
            if (ModelState.IsValid)
            {
                string UserName = HttpContext.Request.Cookies["userName"];
                if (UserName != null)
                {
                    try
                    {
                        var user1 = _userService.GetUser(UserName);
                        user.Id = user1.Id;
                        _userService.Update(user);
                        Cookies(user);
                        return RedirectToAction("ShowProducts", "Products");
                    }
                    catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
                }
                else
                {
                    try
                    {
                        if (_userService.IsUserNameExist(user.UserName))
                        {
                            return View(user);
                        }
                        _userService.AddUser(user);
                        Cookies(user);
                        return RedirectToAction("ShowProducts", "Products");
                    }
                    catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
                }
            }
            return View(user);
        }

        private void Cookies(UserModel user)
        {
            HttpContext.Response.Cookies.Append("userFName", user.FirstName, new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
            HttpContext.Response.Cookies.Append("userLName", user.LastName, new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
            HttpContext.Response.Cookies.Append("userName", user.UserName, new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
            HttpContext.Response.Cookies.Append("userPassword", user.Password, new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
        }

        public IActionResult AboutAs()
        {
            return View();
        }
        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
