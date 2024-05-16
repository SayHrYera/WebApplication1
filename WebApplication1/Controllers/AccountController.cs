using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        AppDbContext context;
        public AccountController()
        {
            context = new AppDbContext();
        }
        public IActionResult Login(string username, string password)
        {
            if (username == null && password == null)
            {
                return View();
            }
            else
            {
                var data = context.Accounts.FirstOrDefault(p => p.Username == username && p.Password == password);
                if (data == null)
                {
                    return Content("Đăng nhập thất bại mời kiểm tra lại");
                }
                else 
                {
                    HttpContext.Session.SetString("username", username);
                    return RedirectToAction("Index", "Home");
                }
            }
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SignUp(Account account)
        {
            try
            {
                context.Accounts.Add(account);
                Cart cart = new Cart()
                {
                    Username = account.Username,
                    Status = 1
                };
                context.Carts.Add(cart);
                context.SaveChanges();
                TempData["Status"] = "Tạo tài khoản thành công";
                return RedirectToAction("Login");
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index", "Home");
        }
    }
}
