using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTask.API.Controllers;
using TestTask.Data.Entities;
using TestTask.Utils.DTOs;

namespace TestTask.API.Tests
{
    public class ReportTest : BaseTest
    {
        private static List<Item> Items = new()
        {
            new Item { Id = 1, Cost = 10, Name = "Apple" },
            new Item { Id = 2, Cost = 20, Name = "Orange" },
            new Item { Id = 3, Cost = 30, Name = "PineApple" }
        };

        private static List<User> Users = new()
        {
            new User
            {
                Email = "user_0_@gmail.com",
                Balance = 1000
            },
            new User
            {
                Email = "user_1_@gmail.com",
                Balance = 1000
            },
            new User
            {
                Email = "user_2_@gmail.com",
                Balance = 1000
            },
            new User
            {
                Email = "user_3_@gmail.com",
                Balance = 1000
            },
            new User
            {
                Email = "user_4_@gmail.com",
                Balance = 1000
            },
        };

        protected override async Task SetupBase()
        {
            await Context.DbContext.Users.AddRangeAsync(Users);
            await Context.DbContext.Items.AddRangeAsync(Items);

            await Context.DbContext.SaveChangesAsync();
        }

        [Test]
        public async Task GetPopularityReport_ShouldReturnCorrect()
        {
            // Arrange
            List<UserItem> UserItems432 = new()
            {
                new UserItem
                {
                    Id = 1,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                new UserItem
                {
                    Id = 2,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                new UserItem
                {
                    Id = 3,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                new UserItem
                {
                    Id = 4,
                    Item = Items[1],
                    ItemId = Items[1].Id,
                    PurchaseDate = new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[1],
                    UserId = Users[1].Id
                },
                new UserItem
                {
                    Id = 5,
                    Item = Items[1],
                    ItemId = Items[1].Id,
                    PurchaseDate = new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[1],
                    UserId = Users[1].Id
                },
                new UserItem
                {
                    Id = 6,
                    Item = Items[2],
                    ItemId = Items[2].Id,
                    PurchaseDate = new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[2],
                    UserId = Users[2].Id
                },
                new UserItem
                {
                    Id = 7,
                    Item = Items[2],
                    ItemId = Items[2].Id,
                    PurchaseDate = new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[2],
                    UserId = Users[2].Id
                },
                new UserItem
                {
                    Id = 8,
                    Item = Items[2],
                    ItemId = Items[2].Id,
                    PurchaseDate = new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[2],
                    UserId = Users[2].Id
                },
                new UserItem
                {
                    Id = 9,
                    Item = Items[1],
                    ItemId = Items[1].Id,
                    PurchaseDate = new DateTime(2023, 4, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[3],
                    UserId = Users[3].Id
                },
                new UserItem
                {
                    Id = 10,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2023, 5, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[4],
                    UserId = Users[4].Id
                },
                new UserItem
                {
                    Id = 11,
                    Item = Items[1],
                    ItemId = Items[1].Id,
                    PurchaseDate = new DateTime(2023, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[4],
                    UserId = Users[4].Id
                },
                new UserItem
                {
                    Id = 12,
                    Item = Items[2],
                    ItemId = Items[2].Id,
                    PurchaseDate = new DateTime(2023, 3, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[2],
                    UserId = Users[2].Id
                }
            };

            await Context.DbContext.UserItems.AddRangeAsync(UserItems432);
            await Context.DbContext.SaveChangesAsync();

            // Act
            var reports = (await Rait<ReportController>().Call(controller => controller.GetReport())).Value;

            // Assert
            var expectedResult = new List<YearlyReportDTO>()
            {
                new YearlyReportDTO
                {
                    ItemId = Items[0].Id,
                    TotalBuyOrders = 3,
                    ItemName = Items[0].Name,
                    Year = 2023
                },
                new YearlyReportDTO
                {
                    ItemId = Items[1].Id,
                    TotalBuyOrders = 2,
                    ItemName = Items[1].Name,
                    Year = 2023
                },
                new YearlyReportDTO
                {
                    ItemId = Items[2].Id,
                    TotalBuyOrders = 4,
                    ItemName = Items[2].Name,
                    Year = 2023
                },
            }
            .OrderByDescending(i => i.TotalBuyOrders).ToList();

            Assert.That(expectedResult, Has.Count.EqualTo(reports.Count));
            Assert.That(reports, Is.EquivalentTo(expectedResult));
        }

        [Test]
        public async Task GetPopularityReport_ShouldReturnForDifferentYears()
        {
            // Arrange
            List<UserItem> UserItemsSameItemsDifferentYear = new()
            {
                new UserItem
                {
                    Id = 1,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                new UserItem
                {
                    Id = 2,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                new UserItem
                {
                    Id = 3,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                new UserItem
                {
                    Id = 4,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2021, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                new UserItem
                {
                    Id = 5,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2021, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                new UserItem
                {
                    Id = 6,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2021, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                new UserItem
                {
                    Id = 7,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2021, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                
            };

            await Context.DbContext.UserItems.AddRangeAsync(UserItemsSameItemsDifferentYear);
            await Context.DbContext.SaveChangesAsync();

            // Act
            var reports = (await Rait<ReportController>().Call(controller => controller.GetReport())).Value;

            // Assert
            var expectedResult = new List<YearlyReportDTO>()
            {
                new YearlyReportDTO
                {
                    ItemId = Items[0].Id,
                    TotalBuyOrders = 3,
                    ItemName = Items[0].Name,
                    Year = 2023
                },
                new YearlyReportDTO
                {
                    ItemId = Items[0].Id,
                    TotalBuyOrders = 4,
                    ItemName = Items[0].Name,
                    Year = 2021
                }
            }
            .OrderByDescending(i => i.TotalBuyOrders).ToList();

            Assert.That(expectedResult, Has.Count.EqualTo(reports.Count));
            Assert.That(reports, Is.EquivalentTo(expectedResult));
        }

        [Test]
        public async Task GetPopularityReport_ShouldAccountForUser()
        {
            // Arrange
            List<UserItem> UserItemsSameItemDifferentUser = new()
            {
                new UserItem
                {
                    Id = 1,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                new UserItem
                {
                    Id = 2,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[0],
                    UserId = Users[0].Id
                },
                new UserItem
                {
                    Id = 3,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[1],
                    UserId = Users[1].Id
                },
                new UserItem
                {
                    Id = 4,
                    Item = Items[0],
                    ItemId = Items[0].Id,
                    PurchaseDate = new DateTime(2023, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                    User = Users[2],
                    UserId = Users[2].Id
                },
            };

            await Context.DbContext.UserItems.AddRangeAsync(UserItemsSameItemDifferentUser);
            await Context.DbContext.SaveChangesAsync();

            // Act
            var reports = (await Rait<ReportController>().Call(controller => controller.GetReport())).Value;

            // Assert
            var expectedResult = new List<YearlyReportDTO>()
            {
                new YearlyReportDTO
                {
                    ItemId = Items[0].Id,
                    TotalBuyOrders = 2,
                    ItemName = Items[0].Name,
                    Year = 2023
                }
            }
            .OrderByDescending(i => i.TotalBuyOrders).ToList();

            Assert.That(expectedResult, Has.Count.EqualTo(reports.Count));
            Assert.That(reports, Is.EquivalentTo(expectedResult));
        }
    }
}
