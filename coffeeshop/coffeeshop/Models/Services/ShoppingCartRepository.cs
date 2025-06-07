using coffeeshop.Data;
using coffeeshop.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace coffeeshop.Models.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CoffeeShopDbContext dbContext;

        public string? ShoppingCartId { get; set; }

        public List<ShoppingCartItem>? ShoppingCartItems { get; set; }

        public ShoppingCartRepository(CoffeeShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public static ShoppingCartRepository GetCart(IServiceProvider services)
        {
            ISession? session =services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;
            CoffeeShopDbContext context = services.GetService<CoffeeShopDbContext>() ??
           throw new Exception("Error initializing coffeeshopdbcontext");
            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();
            session?.SetString("CartId", cartId);
            return new ShoppingCartRepository(context) { ShoppingCartId = cartId };
        }
        public void AddToCart(Product product)
        {
            var item = dbContext.ShoppingCartItems
                        .FirstOrDefault(p => p.Product.Id == product.Id && p.ShoppingCartId == ShoppingCartId);

            if (item == null)
            {
                item = new ShoppingCartItem
                {
                    Product = product,
                    Qty = 1,
                    ShoppingCartId = ShoppingCartId
                };
                dbContext.ShoppingCartItems.Add(item);
            }
            else
            {
                item.Qty++;
            }

            dbContext.SaveChanges();
        }

        public int RemoveFromCart(Product product)
        {
            var item = dbContext.ShoppingCartItems
                        .FirstOrDefault(p => p.Product.Id == product.Id && p.ShoppingCartId == ShoppingCartId);

            int qty = 0;

            if (item != null)
            {
                if (item.Qty > 1)
                {
                    item.Qty--;
                    qty = item.Qty;
                }
                else
                {
                    dbContext.ShoppingCartItems.Remove(item);
                }
                dbContext.SaveChanges();
            }

            return qty;
        }

        public List<ShoppingCartItem> GetAllShoppingCartItems()
        {
            return ShoppingCartItems ??= dbContext.ShoppingCartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Include(p => p.Product)
                .ToList();
        }
        public void ClearCart()
        {
            var items = dbContext.ShoppingCartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId);

            dbContext.ShoppingCartItems.RemoveRange(items);
            dbContext.SaveChanges();
        }

        public decimal GetShoppingCartTotal()
        {
            return dbContext.ShoppingCartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Product.Price * c.Qty)
                .Sum();
        }
    }
}
