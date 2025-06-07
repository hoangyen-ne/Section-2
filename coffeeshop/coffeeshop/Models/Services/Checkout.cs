using System.ComponentModel.DataAnnotations;

namespace coffeeshop.Models.Services
{
    public class Checkout
    {
  
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }

        public string? Address { get; set; }
    }
}
