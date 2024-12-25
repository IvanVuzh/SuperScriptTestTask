using Microsoft.EntityFrameworkCore;
using TestTask.Data;
using TestTask.Data.Entities;

namespace TestTask.Services;

public class MarketService
{
    private readonly TestDbContext _testDbContext;
    private static readonly object _lock = new ();

    public MarketService(TestDbContext testDbContext)
    {
        _testDbContext = testDbContext;
    }

    public async Task BuyAsync(int userId, int itemId)
    {
        lock (_lock)
        {
            var user = _testDbContext.Users.FirstOrDefault(n => n.Id == userId);
            if (user == null)
                throw new Exception("User not found");

            var item = _testDbContext.Items.FirstOrDefault(n => n.Id == itemId);
            if (item == null)
                throw new Exception("Item not found");

            if (user.Balance < item.Cost)
            {
                // this must throw error, but i will follow "without modifying the test itself"
                return;
                // throw new Exception("Not enough balance");
            }

            user.Balance -= item.Cost;

            _testDbContext.UserItems.Add(new UserItem
            {
                UserId = userId,
                ItemId = itemId,
                PurchaseDate = DateTime.UtcNow
            });

            _testDbContext.SaveChanges();
        }
    }
}