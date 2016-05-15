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
    public class UnitTest1
    {
        //Разбиение на страницы
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    		{
        		new Book { BookId = 1, Name = "Книга1"},
        		new Book { BookId = 2, Name = "Книга2"},
        		new Book { BookId = 3, Name = "Книга3"},
        		new Book { BookId = 4, Name = "Книга4"},
        		new Book { BookId = 5, Name = "Книга5"}
    		}); //создание иммитированного хранилища
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3; // сколько товаров на странице может отображаться
            // Действие (act)
            BooksListViewModel result = (BooksListViewModel)controller.List(null, 2).Model; //создаём модель и переходим на 2-ю страницу
            // Утверждение
            List<Book> books = result.Books.ToList(); //конвертируем в список
            Assert.IsTrue(books.Count == 2); //всего книг на 2 странице 2:
            Assert.AreEqual(books[0].Name, "Книга4"); //4-я
            Assert.AreEqual(books[1].Name, "Книга5"); //и 5-я
        }
        //Создание ссылок на страницы
        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2, //текущая страница
                TotalItems = 28, //всего товаров
                ItemsPerPage = 10 //товаров на странице
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>" //вторая страница выделена (selected)
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }
        //Данные разбиения для модели представления
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    		{
        		new Book { BookId = 1, Name = "Книга1"},
        		new Book { BookId = 2, Name = "Книга2"},
        		new Book { BookId = 3, Name = "Книга3"},
        		new Book { BookId = 4, Name = "Книга4"},
        		new Book { BookId = 5, Name = "Книга5"}
    		}); //иммитированное хранилище
            BookController controller = new BookController(mock.Object); //создаём контроллер
            controller.pageSize = 3; //3 товара на странице

            // Act
            BooksListViewModel result
            = (BooksListViewModel)controller.List(null, 2).Model; //создаём модель представления, переходим на 2 страницу

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2); //текущая страница
            Assert.AreEqual(pageInfo.ItemsPerPage, 3); //товаров на странице
            Assert.AreEqual(pageInfo.TotalItems, 5); //всего товаров
            Assert.AreEqual(pageInfo.TotalPages, 2); //всего страниц
        }
        //Фильтрации по категории
        [TestMethod]
        public void Can_Filter_Books()
        {
            // Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    		{
        		new Book { BookId = 1, Name = "Книга1", Category="Cat1"},
        		new Book { BookId = 2, Name = "Книга2", Category="Cat2"},
        		new Book { BookId = 3, Name = "Книга3", Category="Cat1"},
        		new Book { BookId = 4, Name = "Книга4", Category="Cat2"},
        		new Book { BookId = 5, Name = "Книга5", Category="Cat3"}
    		});
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Book> result = ((BooksListViewModel)controller.List("Cat2", 1).Model)
                .Books.ToList(); //переходим на 1-ю страницу 2-й категории

            // Assert
            Assert.AreEqual(result.Count(), 2); //должно быть 2 категории
            Assert.IsTrue(result[0].Name == "Книга2" && result[0].Category == "Cat2"); //книга 2 во 2-й категории
            Assert.IsTrue(result[1].Name == "Книга4" && result[1].Category == "Cat2"); //книга 4 во второй категории
        }
        //Генерация списка категорий
        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
        		new Book { BookId = 1, Name = "Книга1", Category="Поэзия"},
        		new Book { BookId = 2, Name = "Книга2", Category="Поэзия"},
        		new Book { BookId = 3, Name = "Книга3", Category="Фэнтези"},
        		new Book { BookId = 4, Name = "Книга4", Category="Классическая литература"},
    		});

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора отсортированных категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3); //должно быть 3 категории
            Assert.AreEqual(results[0], "Классическая литература");
            Assert.AreEqual(results[1], "Поэзия");
            Assert.AreEqual(results[2], "Фэнтези");
        }
        //Сообщение о выбранной категории
        [TestMethod]
        public void Indicates_Selected_Category()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new Book[] {
        		new Book { BookId = 1, Name = "Игра1", Category="Поэзия"},
        		new Book { BookId = 2, Name = "Игра2", Category="Фэнтези"}
    		});

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Организация - определение выбранной категории
            string categoryToSelect = "Фэнтези";

            // Действие
            string result = target.Menu(categoryToSelect).ViewBag.SelectedCategory;

            // Утверждение
            Assert.AreEqual(categoryToSelect, result);
        }
        //Счетчик товаров определенной категории
        [TestMethod]
        public void Generate_Category_Specific_Book_Count()
        {
            /// Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    		{
        		new Book { BookId = 1, Name = "Книга1", Category="Cat1"},
        		new Book { BookId = 2, Name = "Книга2", Category="Cat2"},
        		new Book { BookId = 3, Name = "Книга3", Category="Cat1"},
        		new Book { BookId = 4, Name = "Книга4", Category="Cat2"},
        		new Book { BookId = 5, Name = "Книга5", Category="Cat3"}
    		});
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3; //товаров на странице

            // Действие - тестирование счетчиков товаров для различных категорий
            int res1 = ((BooksListViewModel)controller.List("Cat1").Model).PagingInfo.TotalItems; //сколько книг 1-й категории
            int res2 = ((BooksListViewModel)controller.List("Cat2").Model).PagingInfo.TotalItems; //2-й категории
            int res3 = ((BooksListViewModel)controller.List("Cat3").Model).PagingInfo.TotalItems; //3-й категории
            int resAll = ((BooksListViewModel)controller.List(null).Model).PagingInfo.TotalItems; //сколько всего книг

            // Утверждение
            Assert.AreEqual(res1, 2);
            Assert.AreEqual(res2, 2);
            Assert.AreEqual(res3, 1);
            Assert.AreEqual(resAll, 5);
        }
	}
}
