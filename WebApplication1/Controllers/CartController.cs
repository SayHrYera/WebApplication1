using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class CartController : Controller
    {
        AppDbContext _context;
        public CartController()
        {
            _context = new AppDbContext();
        }
        public IActionResult Index()
        {
            var check = HttpContext.Session.GetString("username");
            if (String.IsNullOrEmpty(check))
            {
                return RedirectToAction("Login", "Account");
            }else
            {
                var cartItems = _context.CartsDetails.Where(p=>p.Username== check).ToList();
                return View(cartItems);
            } 
        }
        [HttpPost]
        public IActionResult RemoveFromCart(Guid id)
        {
            var cartItem = _context.CartsDetails.Find(id);

            if (cartItem != null)
            {
                _context.CartsDetails.Remove(cartItem);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Checkout()
        {
            var username = HttpContext.Session.GetString("username");
            if (String.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }
            var cartItems = _context.CartsDetails.Where(c => c.Username == username).ToList();
            if (cartItems.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống.";
                return RedirectToAction("Index");
            }
            var bill = new Bill
            {
                Id = Guid.NewGuid().ToString(),
                Username = username,
                CreateDate = DateTime.Now,
                Money = 0,
                Status = 1,
                Details = new List<BillDetails>()
            };
            _context.Bills.Add(bill);
            foreach (var item in cartItems)
            {
                var product = _context.Products.Find(item.ProductId);
                if(product.Amount < item.Quantity)
                {
                    TempData["Error"] = $"Không đủ hàng cho sản phẩm {product?.Name}";
                    return RedirectToAction("Index");
                }
                product.Amount -= item.Quantity;

                var billDetails = new BillDetails
                {
                    Id = Guid.NewGuid(),
                    BillId = bill.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    ProductPrice = product.Price,
                    Status = 1
                };
                bill.Details.Add(billDetails);
                _context.BillDetails.Add(billDetails);
            }
            bill.Money = bill.Details.Sum(d => d.ProductPrice * d.Quantity);
            _context.CartsDetails.RemoveRange(cartItems);
            _context.Bills.Add(bill);
            _context.SaveChanges();
            return RedirectToAction("Index", "Bill");
        }
    }
}
