using AutoMapper;
using ECommerce.Api.Orders.Db.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Api.Orders.Db.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext dbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext dbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.logger = logger;
            this.mapper = mapper;
            SeedData();
        }

        private void SeedData()
        {
            if (!dbContext.Orders.Any())
            {
                dbContext.Orders.Add(new Db.Order() { Id = 1, CustomerId = 1, OrderDate = DateTime.Now, Total = 20, Items = { } });
                dbContext.Orders.Add(new Db.Order() { Id = 2, CustomerId = 1, OrderDate = DateTime.Now, Total = 215, Items = { } });
                dbContext.Orders.Add(new Db.Order() { Id = 3, CustomerId = 2, OrderDate = DateTime.Now, Total = 190, Items = { } });

                dbContext.SaveChanges();

                if (!dbContext.OrderItems.Any())
                {
                    dbContext.OrderItems.Add(new Db.OrderItem() { Id = 1, OrderId = 1, ProductId = 1, Quantity = 1, UnitPrice = 20 });
                    dbContext.OrderItems.Add(new Db.OrderItem() { Id = 2, OrderId = 2, ProductId = 2, Quantity = 3, UnitPrice = 5 });
                    dbContext.OrderItems.Add(new Db.OrderItem() { Id = 3, OrderId = 2, ProductId = 4, Quantity = 1, UnitPrice = 200 });
                    dbContext.OrderItems.Add(new Db.OrderItem() { Id = 4, OrderId = 3, ProductId = 3, Quantity = 1, UnitPrice = 150 });
                    dbContext.OrderItems.Add(new Db.OrderItem() { Id = 5, OrderId = 3, ProductId = 1, Quantity = 2, UnitPrice = 20 });

                    dbContext.SaveChanges();
                }
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> Orders, string ErrorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                logger?.LogInformation("Querying orders");
                var orders = await dbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
                if (orders != null && orders.Any())
                {
                    logger?.LogInformation($"{orders.Count} order(s) found.");
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger?.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
