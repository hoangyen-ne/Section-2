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

        // ⭐ Phương thức quan trọng: Lấy Cart từ Session
        public static ShoppingCartRepository GetCart(IServiceProvider services)
        {
            // lấy session từ context
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext?.Session;

            var context = services.GetService<CoffeeShopDbContext>()
                          ?? throw new Exception("DbContext not found");

            // tạo hoặc lấy CartId từ session
            string cartId = session?.GetString("CartId") ?? Guid.NewGuid().ToString();

            session?.SetString("CartId", cartId);

            return new ShoppingCartRepository(context) { ShoppingCartId = cartId };
        }

        // ✅ Thêm sản phẩm vào giỏ
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

        // ✅ Xoá 1 sản phẩm khỏi giỏ (giảm số lượng)
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

        // ✅ Lấy toàn bộ sản phẩm trong giỏ
        public List<ShoppingCartItem> GetAllShoppingCartItems()
        {
            return ShoppingCartItems ??= dbContext.ShoppingCartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Include(p => p.Product)
                .ToList();
        }

        // ✅ Xoá toàn bộ giỏ
        public void ClearCart()
        {
            var items = dbContext.ShoppingCartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId);

            dbContext.ShoppingCartItems.RemoveRange(items);
            dbContext.SaveChanges();
        }

        // ✅ Tổng tiền trong giỏ
        public decimal GetShoppingCartTotal()
        {
            return dbContext.ShoppingCartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Product.Price * c.Qty)
                .Sum();
        }
    }
}
