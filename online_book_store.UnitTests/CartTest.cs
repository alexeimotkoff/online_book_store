using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using online_book_store.Domain.Abstract;
using online_book_store.Domain.Entities;
using online_book_store.WebUI.Controllers;
using online_book_store.WebUI.Models;
using online_book_store.WebUI.HtmlHelpers;

namespace online_book_store.UnitTests
{
    [TestClass]
    public class CartTests
    {
        //Проверка корзины
        //Добавление объекта
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Организация - создание нескольких тестовых книг
            Book book1 = new Book { BookId = 1, Name = "Книга1" };
            Book book2 = new Book { BookId = 2, Name = "Книга2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Book, book1);
            Assert.AreEqual(results[1].Book, book2);
        }
        //Добавление нескольких объектов в одну корзину
        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых книг
            Book book1 = new Book { BookId = 1, Name = "Книга1" };
            Book book2 = new Book { BookId = 2, Name = "Книга2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            cart.AddItem(book1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Book.BookId).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2); // всего книг
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров 1-й книги добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);    // 1 экземпляр 2-й книги добавлен в корзину
        }
        //Удаление объекта
        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых книг
            Book book1 = new Book { BookId = 1, Name = "Книга1" };
            Book book2 = new Book { BookId = 2, Name = "Книга2" };
            Book book3 = new Book { BookId = 3, Name = "Книга3" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - добавление нескольких книг в корзину
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 4);
            cart.AddItem(book3, 2);
            cart.AddItem(book2, 1);

            // Действие
            cart.RemoveLine(book2); //удаление 2-й книги

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Book == book2).Count(), 0); //второй книги нет в корзине
            Assert.AreEqual(cart.Lines.Count(), 2); //в корзине 2 осталось 2 книги
        }
        //Вычисление общей стоимости объектов в корзине
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых книг
            Book book1 = new Book { BookId = 1, Name = "Книга1", Price = 100 };
            Book book2 = new Book { BookId = 2, Name = "Книга2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            cart.AddItem(book1, 5);
            decimal result = cart.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 655);
        }
        //Очистка корзины
        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
            Book book1 = new Book { BookId = 1, Name = "Книга1", Price = 100 };
            Book book2 = new Book { BookId = 2, Name = "Книга2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            cart.AddItem(book1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }
        //Проверка добавления в корзину
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
            new Book {BookId = 1, Name = "Книга1", Category = "Кат1"},
            }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object, null); //тестируем контроллер

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1); //всего книг должно быть 1
            Assert.AreEqual(cart.Lines.ToList()[0].Book.BookId, 1); //отображается правильная книга
        }
        //Перенаправление на страницу корзины
        [TestMethod]
        public void Adding_Book_To_Cart_Goes_To_Cart_Screen()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
            new Book {BookId = 1, Name = "Книга1", Category = "Кат1"},
            }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object, null);

            // Действие - добавить книгу в корзину
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }
        // Проверяем URL, по которому можно вернуться в каталог
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController target = new CartController(null, null);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
        //Обработка заказа
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart() //заказ невозможно обработать, если корзина пустая
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            Cart cart = new Cart();

            // Организация - создание деталей о доставке
            ShippingDetails shippingDetails = new ShippingDetails();

            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails() //заказ невозможно обработать, если введены некорректные данные по заказу
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Can_Checkout_And_Submit_Order() //обработка нормальных заказов
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Book(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ передан обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Once());

            // Утверждение - проверка, что метод возвращает представление 
            Assert.AreEqual("Completed", result.ViewName);

            // Утверждение - проверка, что представлению передается допустимая модель
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
