using BLL.ProductBLL;
using BLL.UserBLL;
using Entities;
using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Yad2.Models;

namespace Yad2.Controllers
{
    public class ProductsController : Controller
    {
        private IProductService _productService;
        private IUserService _userService;
        public ProductsController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;

        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowProducts()
        {
            if (HttpContext.Request.Cookies.ContainsKey("userName"))
            {
                try
                {
                    List<ProductModel> list = _productService.GetAll().ToList();
                    if (TempData.ContainsKey("OrderByDate") && TempData["OrderByDate"] != null)
                        return View(_productService.OrderBYDate(list).ToList());
                    if (TempData.ContainsKey("OrderByTitle") && TempData["OrderBYTitle"] != null)
                        return View(_productService.OrderBYTitle(list).ToList());
                    return View(list);
                }
                catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }

            }
            else if (HttpContext.Request.Cookies.ContainsKey("guestCart"))
            {
                List<string> product = HttpContext.Request.Cookies["guestCart"].Split(',').ToList();
                List<int> productId = new List<int>();
                foreach (var item in product)
                {
                    productId.Add(int.Parse(item));
                }
                try
                {
                    if (TempData.ContainsKey("OrderByDate") && TempData["OrderByDate"] != null)
                        return View(_productService.OrderBYDate(_productService.GetAll_Anonimus(productId).ToList()).ToList());
                    else if (TempData.ContainsKey("OrderBYTitle") && TempData["OrderBYTitle"] != null)
                        return View(_productService.OrderBYTitle(_productService.GetAll_Anonimus(productId).ToList()).ToList());
                    return View(_productService.GetAll_Anonimus(productId).ToList());
                }
                catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
            }
            else
            {
                try
                {
                    if (TempData.ContainsKey("OrderByDate") && TempData["OrderByDate"] != null)
                        return View(_productService.OrderBYDate(_productService.GetAll().ToList()).ToList());
                    if (TempData.ContainsKey("OrderByTitle") && TempData["OrderBYTitle"] != null)
                        return View(_productService.OrderBYTitle(_productService.GetAll().ToList()).ToList());
                    return View(_productService.GetAll().ToList());
                }
                catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
            }
        }
        [HttpGet]
        public IActionResult ProductDetails(int id)
        {
            try
            {
                ProductModel product = _productService.GetProductModelById(id);
                return View(product);
            }
            catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
        }
        [HttpGet]
        public IActionResult AddProductToWeb()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddProductToWeb(ProductModel product, List<IFormFile> image)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            for (int i = 0; i < image.Count; i++)
            {
                if (image[i].Length > 0)
                {
                    using MemoryStream ms = new MemoryStream();
                    image[i].CopyTo(ms);
                    try
                    {
                        product.GetType().GetProperty($"Image{i + 1}").SetValue(product, ms.ToArray());
                    }
                    catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
                }
            }
            string userName = HttpContext.Request.Cookies["userName"];
            try
            {
                UserModel user = _userService.GetUser(userName);
                _productService.AddProductToData(product, user.Id);
            }
            catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
            return RedirectToAction("ShowProducts");
        }

        public IActionResult CartProducts()
        {
            if (HttpContext.Request.Cookies.ContainsKey("userName"))
            {
                try
                {
                    string userName = HttpContext.Request.Cookies["userName"];
                    List<ProductModel> list = _productService.GetAllProductsInCart(userName).ToList();
                    return View(list);
                }
                catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
            }
            else if (HttpContext.Request.Cookies.ContainsKey("guestCart"))
            {
                List<int> ProductIDs = new List<int>();
                List<string> products = HttpContext.Request.Cookies["guestCart"].Split(',').ToList();
                foreach (var item in products)
                {
                    ProductIDs.Add(int.Parse(item));
                }
                try
                {
                    List<ProductModel> listProducs = _productService.GetAllProductsInCartById_AndState(ProductIDs).ToList();
                    return View(listProducs);
                }
                catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
            }
            else
            {
                return View(new List<ProductModel>());
            }
        }
        public IActionResult AddToCart(int Id)
        {
            if (HttpContext.Request.Cookies.ContainsKey("userName"))
            {
                try
                {
                    string userName = HttpContext.Request.Cookies["userName"];
                    ProductModel product = _productService.GetProductModelById(Id);
                    UserModel user = _userService.GetUser(userName);
                    _productService.AddProductToCart(product, user);
                }
                catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
            }
            else if (HttpContext.Request.Cookies.ContainsKey("guestCart"))
            {
                HttpContext.Response.Cookies.Append("guestCart", HttpContext.Request.Cookies["guestCart"] + $",{Id}", new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
            }
            else
            {
                HttpContext.Response.Cookies.Append("guestCart", $"{Id}", new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
            }
            return RedirectToAction("ShowProducts");
        }

        public IActionResult RemoveFromCart(int id)
        {
            if (HttpContext.Request.Cookies.ContainsKey("userName"))
            {
                try
                {
                    ProductModel pro = _productService.GetProductModelById(id);
                    _productService.RemoveProductFromCart(pro);
                }
                catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
            }
            else
            {
                List<string> guestCartItems = HttpContext.Request.Cookies["guestCart"].Split(',').ToList();
                guestCartItems.Remove(id.ToString());
                if (guestCartItems.Count == 0)
                {
                    HttpContext.Response.Cookies.Delete("guestCart");
                }
                else
                {
                    HttpContext.Response.Cookies.Append("guestCart", string.Join(',', guestCartItems), new CookieOptions() { Expires = DateTime.Now.AddDays(3) });
                }
            }
            return RedirectToAction("CartProducts");
        }
        public async Task<IActionResult> Buy()
        {
            if (HttpContext.Request.Cookies.ContainsKey("userName"))
            {
                string userName = HttpContext.Request.Cookies["userName"];
                try
                {
                    List<ProductModel> list = _productService.GetAllProductsInCart(userName).ToList();
                    await _productService.RemoveProductFromData(list.Select(l => l.Id).ToList());
                }
                catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
            }
            else if (HttpContext.Request.Cookies.ContainsKey("guestCart"))
            {
                List<string> list = HttpContext.Request.Cookies["guestCart"].Split(',').ToList();
                List<int> ProductIDs = new List<int>();
                foreach (var item in list)
                {
                    ProductIDs.Add(int.Parse(item));
                }
                HttpContext.Response.Cookies.Delete("guestCart");
                try
                {
                    await _productService.RemoveProductFromData(ProductIDs);
                }
                catch (Exception) { return View("/Views/Shared/Error.cshtml", new ErrorViewModel()); }
            }
            return RedirectToAction("ShowProducts");
        }

        public IActionResult OrderByDate(string id)
        {
            if (TempData.ContainsKey("OrderByDate"))
            {
                TempData["OrderByTitle"] = null;
                return RedirectToAction("ShowProducts");
            }
            TempData.Add("OrderByDate", id);
            return RedirectToAction("ShowProducts");
        }
        public IActionResult OrderByTitle(string id)
        {
            if (TempData.ContainsKey("OrderByTitle"))
            {
                TempData["OrderByDate"] = null;
                return RedirectToAction("ShowProducts");
            }
            TempData.Add("OrderByTitle", id);
            return RedirectToAction("ShowProducts");
        }

        [HttpPost]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
