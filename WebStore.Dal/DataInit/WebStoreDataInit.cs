using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebStore.Dal.Context;
using WebStore.Dal.Interfaces;
using WebStore.Domain.Entities;
using WebStore.Domain.Identity;

namespace WebStore.Dal.DataInit
{
    /// <summary> Инициализатор данных базы данных </summary>
    public class WebStoreDataInit : IWebStoreDataInit
    {
        private readonly Random _rnd = new Random();
        private readonly WebStoreContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ILogger<WebStoreDataInit> _logger;
        public WebStoreDataInit(
            WebStoreContext context, 
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            ILogger<WebStoreDataInit> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }
        /// <summary> Пересоздание базы данных </summary>
        public IWebStoreDataInit RecreateDatabase()
        {
            _context.Database.EnsureDeleted();
            _logger.LogInformation($"{DateTime.Now} Удаление БД выполнено");
            _context.Database.Migrate();
            _logger.LogInformation($"{DateTime.Now} Миграция БД выполнена");
            return this;
        }
        /// <summary> Заполнение начальными данными </summary>
        public IWebStoreDataInit InitData()
        {
            var timer = Stopwatch.StartNew();
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
                _logger.LogInformation($"{DateTime.Now} Миграция БД выполнена, время: {timer.Elapsed.TotalSeconds} сек.");
            }
            else
            {
                _logger.LogInformation($"{DateTime.Now} Миграция БД не требуется");
            }
            try
            {
                InitProducts(_context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} Ошибка при инициализации данных базы данных");
                throw;
            }
            try
            {
                InitializeIdentityAsync().GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} Ошибка при инициализации БД системы Identity");
                throw;
            }
            try
            {
                InitWorkers(_context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{DateTime.Now} Ошибка инициализации начальных данных работников");
                throw;
            }
            _logger.LogInformation($"{DateTime.Now} Инициализация БД выполнена, время: {timer.Elapsed.TotalSeconds} сек.");
            return this;
        }

        #region Инициализация базы данных начальными тестовыми данными

        private void InitProducts(WebStoreContext context)
        {
            foreach (var section in _getSections.Where(s => s.ParentId is null))
            {
                var parent = new Section
                {
                    Name = section.Name,
                    Order = section.Order,
                };
                context.Sections.Add(parent);
                var child = _getSections
                    .Where(s => s.ParentId == section.Id)
                    .Select(s => new Section
                {
                    Name = s.Name,
                    Order = s.Order,
                    Parent = parent,
                });
                context.Sections.AddRange(child);
            }
            context.SaveChanges();
            var brands = _getBrands.Select(b => new Brand
            {
                Name = b.Name,
                Order = b.Order,
            });
            context.Brands.AddRange(brands);
            context.SaveChanges();

            var tmpSections = context.Sections.ToArray();
            var tmpBrands = context.Brands.ToArray();

            var products = _getProducts.Select(p => new Product
            {
                Name = p.Name,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Order = p.Order,
                Section = tmpSections[_rnd.Next(tmpSections.Length)],
                Brand = tmpBrands[_rnd.Next(tmpBrands.Length)],
            });
            context.Products.AddRange(products);
            context.SaveChanges();
            _logger.LogInformation($"{DateTime.Now} Инициализация продуктов, категорий и брендов выполнена успешно");
        }

        #endregion

        #region Инициализация базы данных данными Identity

        private async Task InitializeIdentityAsync()
        {
            async Task CheckRole(string RoleName)
            {
                if (!await _roleManager.RoleExistsAsync(RoleName))
                {
                    _logger.LogInformation("Роль {0} отсутствует. Создаю...", RoleName);
                    await _roleManager.CreateAsync(new Role { Name = RoleName });
                    _logger.LogInformation("Роль {0} создана успешно", RoleName);
                }
            }

            await CheckRole(Role.Administrators);
            await CheckRole(Role.Users);
            await CheckRole(Role.Clients);

            if (await _userManager.FindByNameAsync(User.Administrator) is null)
            {
                _logger.LogInformation($"Пользователь {User.Administrator} отсутствует, создаю ...");
                var admin = new User
                {
                    UserName = User.Administrator,
                };

                var result = await _userManager.CreateAsync(admin, User.DefaultAdministratorPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, Role.Administrators);
                    _logger.LogInformation($"{DateTime.Now} Пользователь {admin.UserName} успешно создан и наделен ролью {Role.Administrators}");
                }
                else
                {
                    var errors = result.Errors.Select(e => e.Description).ToArray();
                    _logger.LogError("Учётная запись администратора не создана по причине: {0}", 
                        string.Join(",", errors));
                    throw new InvalidOperationException($"Ошибка при создании пользователя {admin.UserName}, список ошибок: {string.Join(",", errors)}");
                }
            }
            _logger.LogInformation($"{DateTime.Now} Инициализация системы Identity в базе данных выполнено успешно");
        }

        #endregion

        #region Инициализация базы данных начальными данными о сотрудниках

        private void InitWorkers(WebStoreContext context)
        {
            var workers = _GetTestWorkers.Select(w => new Worker
            {
                LastName = w.LastName,
                FirstName = w.FirstName,
                Patronymic = w.Patronymic,
                Age = w.Age,
                Birthday = w.Birthday,
                CountChildren = w.CountChildren,
                EmploymentDate = w.EmploymentDate,
            });

            using (_context.Database.BeginTransaction())
            {
                context.Workers.AddRange(workers);

                _context.SaveChanges();
                _context.Database.CommitTransaction();

                _logger.LogInformation($"{DateTime.Now} Инициализация работников, начальных данных работников прошла успешно");
            }
        }

        #endregion

        #region Ценные тестовые данные

        private IEnumerable<Section> _getSections = new[]
        {
              new Section { Id = 1, Name = "Спорт", Order = 0 },
              new Section { Id = 2, Name = "Nike", Order = 0, ParentId = 1 },
              new Section { Id = 3, Name = "Under Armour", Order = 1, ParentId = 1 },
              new Section { Id = 4, Name = "Adidas", Order = 2, ParentId = 1 },
              new Section { Id = 5, Name = "Puma", Order = 3, ParentId = 1 },
              new Section { Id = 6, Name = "ASICS", Order = 4, ParentId = 1 },
              new Section { Id = 7, Name = "Для мужчин", Order = 1 },
              new Section { Id = 8, Name = "Fendi", Order = 0, ParentId = 7 },
              new Section { Id = 9, Name = "Guess", Order = 1, ParentId = 7 },
              new Section { Id = 10, Name = "Valentino", Order = 2, ParentId = 7 },
              new Section { Id = 11, Name = "Диор", Order = 3, ParentId = 7 },
              new Section { Id = 12, Name = "Версачи", Order = 4, ParentId = 7 },
              new Section { Id = 13, Name = "Армани", Order = 5, ParentId = 7 },
              new Section { Id = 14, Name = "Prada", Order = 6, ParentId = 7 },
              new Section { Id = 15, Name = "Дольче и Габбана", Order = 7, ParentId = 7 },
              new Section { Id = 16, Name = "Шанель", Order = 8, ParentId = 7 },
              new Section { Id = 17, Name = "Гуччи", Order = 9, ParentId = 7 },
              new Section { Id = 18, Name = "Для женщин", Order = 2 },
              new Section { Id = 19, Name = "Fendi", Order = 0, ParentId = 18 },
              new Section { Id = 20, Name = "Guess", Order = 1, ParentId = 18 },
              new Section { Id = 21, Name = "Valentino", Order = 2, ParentId = 18 },
              new Section { Id = 22, Name = "Dior", Order = 3, ParentId = 18 },
              new Section { Id = 23, Name = "Versace", Order = 4, ParentId = 18 },
              new Section { Id = 24, Name = "Для детей", Order = 3 },
              new Section { Id = 25, Name = "Мода", Order = 4 },
              new Section { Id = 26, Name = "Для дома", Order = 5 },
              new Section { Id = 27, Name = "Интерьер", Order = 6 },
              new Section { Id = 28, Name = "Одежда", Order = 7 },
              new Section { Id = 29, Name = "Сумки", Order = 8 },
              new Section { Id = 30, Name = "Обувь", Order = 9 },
        };
        private IEnumerable<Brand> _getBrands = new[]
        {
            new Brand { Id = 1, Name = "Acne", Order = 0 },
            new Brand { Id = 2, Name = "Grune Erde", Order = 1 },
            new Brand { Id = 3, Name = "Albiro", Order = 2 },
            new Brand { Id = 4, Name = "Ronhill", Order = 3 },
            new Brand { Id = 5, Name = "Oddmolly", Order = 4 },
            new Brand { Id = 6, Name = "Boudestijn", Order = 5 },
            new Brand { Id = 7, Name = "Rosch creative culture", Order = 6 },
        };
        private IEnumerable<Product> _getProducts = new[]
        {
            new Product { Id = 1, Name = "Белое платье", Price = 1025, ImageUrl = "product1.jpg", Order = 0, SectionId = 2, BrandId = 1 },
            new Product { Id = 2, Name = "Розовое платье", Price = 1025, ImageUrl = "product2.jpg", Order = 1, SectionId = 2, BrandId = 1 },
            new Product { Id = 3, Name = "Красное платье", Price = 1025, ImageUrl = "product3.jpg", Order = 2, SectionId = 2, BrandId = 1 },
            new Product { Id = 4, Name = "Джинсы", Price = 1025, ImageUrl = "product4.jpg", Order = 3, SectionId = 2, BrandId = 1 },
            new Product { Id = 5, Name = "Лёгкая майка", Price = 1025, ImageUrl = "product5.jpg", Order = 4, SectionId = 2, BrandId = 2 },
            new Product { Id = 6, Name = "Лёгкое голубое поло", Price = 1025, ImageUrl = "product6.jpg", Order = 5, SectionId = 2, BrandId = 1 },
            new Product { Id = 7, Name = "Платье белое", Price = 1025, ImageUrl = "product7.jpg", Order = 6, SectionId = 2, BrandId = 1 },
            new Product { Id = 8, Name = "Костюм кролика", Price = 1025, ImageUrl = "product8.jpg", Order = 7, SectionId = 25, BrandId = 1 },
            new Product { Id = 9, Name = "Красное китайское платье", Price = 1025, ImageUrl = "product9.jpg", Order = 8, SectionId = 25, BrandId = 1 },
            new Product { Id = 10, Name = "Женские джинсы", Price = 1025, ImageUrl = "product10.jpg", Order = 9, SectionId = 25, BrandId = 3 },
            new Product { Id = 11, Name = "Джинсы женские", Price = 1025, ImageUrl = "product11.jpg", Order = 10, SectionId = 25, BrandId = 3 },
            new Product { Id = 12, Name = "Летний костюм", Price = 1025, ImageUrl = "product12.jpg", Order = 11, SectionId = 25, BrandId = 3 },
            new Product { Id = 13, Name = "Белое платье два", Price = 1025, ImageUrl = "product1.jpg", Order = 12, SectionId = 3, BrandId = 1 },
            new Product { Id = 14, Name = "Розовое платье два", Price = 1025, ImageUrl = "product2.jpg", Order = 13, SectionId = 3, BrandId = 1 },
            new Product { Id = 15, Name = "Красное платье два", Price = 1025, ImageUrl = "product3.jpg", Order = 14, SectionId = 3, BrandId = 1 },
            new Product { Id = 16, Name = "Джинсы два", Price = 1025, ImageUrl = "product4.jpg", Order = 15, SectionId = 3, BrandId = 1 },
            new Product { Id = 17, Name = "Лёгкая майка два", Price = 1025, ImageUrl = "product5.jpg", Order = 16, SectionId = 3, BrandId = 2 },
            new Product { Id = 18, Name = "Лёгкое голубое поло два", Price = 1025, ImageUrl = "product6.jpg", Order = 17, SectionId = 3, BrandId = 1 },
            new Product { Id = 19, Name = "Платье белое два", Price = 1025, ImageUrl = "product7.jpg", Order = 18, SectionId = 3, BrandId = 1 },
            new Product { Id = 20, Name = "Костюм кролика два", Price = 1025, ImageUrl = "product8.jpg", Order = 19, SectionId = 24, BrandId = 1 },
            new Product { Id = 21, Name = "Красное китайское платье два", Price = 1025, ImageUrl = "product9.jpg", Order = 20, SectionId = 24, BrandId = 1 },
            new Product { Id = 22, Name = "Женские джинсы два", Price = 1025, ImageUrl = "product10.jpg", Order = 21, SectionId = 24, BrandId = 3 },
            new Product { Id = 23, Name = "Джинсы женские два", Price = 1025, ImageUrl = "product11.jpg", Order = 22, SectionId = 24, BrandId = 3 },
            new Product { Id = 24, Name = "Летний костюм два", Price = 1025, ImageUrl = "product12.jpg", Order = 23, SectionId = 24, BrandId = 3 },
            new Product { Id = 25, Name = "Белое платье три", Price = 1025, ImageUrl = "product1.jpg", Order = 24, SectionId = 4, BrandId = 1 },
            new Product { Id = 26, Name = "Розовое платье три", Price = 1025, ImageUrl = "product2.jpg", Order = 25, SectionId = 4, BrandId = 1 },
            new Product { Id = 27, Name = "Красное платье три", Price = 1025, ImageUrl = "product3.jpg", Order = 26, SectionId = 4, BrandId = 1 },
            new Product { Id = 28, Name = "Джинсы три", Price = 1025, ImageUrl = "product4.jpg", Order = 27, SectionId = 4, BrandId = 1 },
            new Product { Id = 29, Name = "Лёгкая майка три", Price = 1025, ImageUrl = "product5.jpg", Order = 28, SectionId = 4, BrandId = 2 },
            new Product { Id = 30, Name = "Лёгкое голубое поло три", Price = 1025, ImageUrl = "product6.jpg", Order = 29, SectionId = 4, BrandId = 1 },
            new Product { Id = 31, Name = "Платье белое три", Price = 1025, ImageUrl = "product7.jpg", Order = 30, SectionId = 4, BrandId = 1 },
            new Product { Id = 32, Name = "Костюм кролика три", Price = 1025, ImageUrl = "product8.jpg", Order = 31, SectionId = 23, BrandId = 1 },
            new Product { Id = 33, Name = "Красное китайское платье три", Price = 1025, ImageUrl = "product9.jpg", Order = 32, SectionId = 23, BrandId = 1 },
            new Product { Id = 34, Name = "Женские джинсы три", Price = 1025, ImageUrl = "product10.jpg", Order = 33, SectionId = 23, BrandId = 3 },
            new Product { Id = 35, Name = "Джинсы женские три", Price = 1025, ImageUrl = "product11.jpg", Order = 34, SectionId = 23, BrandId = 3 },
            new Product { Id = 36, Name = "Летний костюм три", Price = 1025, ImageUrl = "product12.jpg", Order = 35, SectionId = 23, BrandId = 3 },
        };
        public IEnumerable<Worker> _GetTestWorkers => Enumerable.Range(1, 10).Select(p => new Worker
        {
            Id = p,
            FirstName = $"Иван_{p}",
            LastName = $"Иванов_{p + 1}",
            Patronymic = $"Иванович_{p + 2}",
            Age = p + 20,
            Birthday = new DateTime(1980 + p, 1, 1),
            EmploymentDate = DateTime.Now.AddYears(- p).AddMonths(p),
            CountChildren = p,
        });

        #endregion
    }
}
