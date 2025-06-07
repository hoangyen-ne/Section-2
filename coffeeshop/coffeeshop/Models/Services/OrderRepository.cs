using coffeeshop.Data;
using coffeeshop.Models.Interfaces;

namespace coffeeshop.Models.Services
{
    public class OrderRepository: IOrderRepository
    {
        private  CoffeeShopDbContext dbContext;
        private  IShoppingCartRepository shoppingCartRepository;

        public OrderRepository(CoffeeShopDbContext dbContext, IShoppingCartRepository shoppingCartRepository)
        {
            this.dbContext = dbContext;
            this.shoppingCartRepository = shoppingCartRepository;
        }

        public void PlaceOrder(Order order)
        { 
            var items = shoppingCartRepository.GetAllShoppingCartItems();
            order.OrderDetails = new List <  OrderDetail>();
            foreach(var item in items)
            {
               var OrderDetail = new OrderDetail
                {
                    ProductId = item.Qty,
                    Quantity = item.Product.Id,
                    Price = item.Product.Price
                };
                order.OrderDetails.Add(OrderDetail);
            }
            order.OrderPlaced = DateTime.Now;
            order.OrderTotal=shoppingCartRepository.GetShoppingCartTotal();
            dbContext.Order.Add(order);
            dbContext.SaveChanges();
        }
    }
}
