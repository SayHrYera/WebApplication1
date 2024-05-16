using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        AppDbContext _context;
        public ProductController()
        {
            _context = new AppDbContext();
        }
        public IActionResult Index()
        {
            var data = _context.Products.ToList();
            return View(data);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Edit(Guid id)
        {
            var editItem = _context.Products.Find(id);
            return View(editItem);
        }
        [HttpPost]
        public IActionResult Edit(Product product)
        {
            var editItem = _context.Products.Find(product.Id);  
            editItem.Name = product.Name;
            editItem.Description = product.Description;
            _context.Products.Update(editItem);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Delete(Guid id)
        {
            var editItem = _context.Products.Find(id);
            _context.Remove(editItem);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult AddtoCart(Guid id, int quantity)
        {
            var check = HttpContext.Session.GetString("username");
            if (String.IsNullOrEmpty(check))
            {
                return RedirectToAction("Login", "Account");
            }else
            {
                var product = _context.Products.FirstOrDefault(x => x.Id == id);
                if(quantity > product.Amount)
                {
                    TempData["Error"] = "Số lượng lớn hơn số lượng hiện có";
                    return RedirectToAction("Index", "Product");
                }
                var cartItem = _context.CartsDetails.FirstOrDefault(p => p.ProductId == id && p.Username == check);
                if (cartItem == null) {
                    CartDetails cartDetails = new CartDetails()
                    {
                        Id = Guid.NewGuid(), ProductId = id, Quantity = quantity, Status = 1, Username = check
                    };
                    _context.CartsDetails.Add(cartDetails); _context.SaveChanges();
                }else
                {
                    cartItem.Quantity = cartItem.Quantity + quantity;
                    _context.CartsDetails.Update(cartItem);
                    _context.SaveChanges();
                }
                return RedirectToAction("Index", "Product");
            }
        }
    }
}
