using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class BillController : Controller
    {
        AppDbContext _context;
        public BillController()
        {
            _context = new AppDbContext();
        }
        public IActionResult Index(string id)
        {
            var username = HttpContext.Session.GetString("username");
            var bills = _context.Bills.Where(b => b.Username == username).ToList();
            return View(bills);
        }
        [HttpPost]
        public IActionResult CancelBill(string id)
        {
            var bill = _context.Bills.FirstOrDefault(b => b.Id == id);
            var billDetail = _context.BillDetails.Where(d => d.BillId == id).ToList();
            foreach(var detail in billDetail)
            {
                var product = _context.Products.Find(detail.ProductId);
                if(product != null)
                {
                    product.Amount += detail.Quantity;
                }
            }
            bill.Status = 100;
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public IActionResult Detail(string id)
        {
            var bill = _context.Bills.Include(b => b.Details).FirstOrDefault(b => b.Id == id);
            if (bill == null)
            {
                return NotFound();
            }

            return View(bill);

        }
        public IActionResult BuyAgain(string id)
        {
            var bill = _context.Bills.Include(b => b.Details).FirstOrDefault(b => b.Id == id);
            var username = HttpContext.Session.GetString("username");
            if (String.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Account");
            }
            foreach (var billDetail in bill.Details)
            {
                var product = _context.Products.Find(billDetail.ProductId);
                if(product != null)
                {
                    var cartItem = _context.CartsDetails.FirstOrDefault(p => p.ProductId == billDetail.ProductId && p.Username == username);
                    if(cartItem == null)
                    {
                        cartItem = new CartDetails
                        {
                            Id = Guid.NewGuid(),
                            ProductId = billDetail.ProductId,
                            Quantity = billDetail.Quantity,
                            Status = 1,
                            Username = username
                        };
                        _context.CartsDetails.Add(cartItem);
                    }
                    else
                    {
                        cartItem.Quantity += billDetail.Quantity;
                        _context.CartsDetails.Update(cartItem);
                    }
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Index", "Cart");
        }
    }
}
