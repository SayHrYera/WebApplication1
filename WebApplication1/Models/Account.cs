using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Account
    {
        public string Username { get; set; }
        public string Password { get; set; }
        [RegularExpression("^(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}$",
            ErrorMessage = "Số điện thoại phải đúng format và có 10 chữ số")]
        public string PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "Email chua dung dinh dang")]
        public string Email { get; set; }
        public string Address { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual List<Bill> Bills { get; set; }
    }
}
