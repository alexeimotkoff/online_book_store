using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using online_book_store.Domain.Abstract;
using online_book_store.Domain.Entities;
using online_book_store.WebUI.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace online_book_store.UnitTests
{
    [TestClass]
    public class AdminTests
    {
		//Метод действия Index
        [TestMethod]
        public void Index_Contains_All_Books()
        {
            // Организация - создание имитированного хранилища данных
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book { BookId = 1, Name = "Книга1"},
                new Book { BookId = 2, Name = "Книга2"},
                new Book { BookId = 3, Name = "Книга3"},
                new Book { BookId = 4, Name = "Книга4"},
                new Book { BookId = 5, Name = "Книга5"}
            });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие
            List<Book> result = ((IEnumerable<Book>)controller.Index().
                ViewData.Model).ToList();

            // Утверждение
            Assert.AreEqual(result.Count(), 5);
            Assert.AreEqual("Книга1", result[0].Name);
            Assert.AreEqual("Книга2", result[1].Name);
            Assert.AreEqual("Книга3", result[2].Name);
        }
		//Метод действия Edit
		[TestMethod]
		public void Can_Edit_Book()
		{
			// Организация - создание имитированного хранилища данных
			Mock<IBookRepository> mock = new Mock<IBookRepository>();
			mock.Setup(m => m.Books).Returns(new List<Book>
			{
				new Book { BookId = 1, Name = "Книга1"},
				new Book { BookId = 2, Name = "Книга2"},
				new Book { BookId = 3, Name = "Книга3"},
				new Book { BookId = 4, Name = "Книга4"},
				new Book { BookId = 5, Name = "Книга5"}
			});

			// Организация - создание контроллера
			AdminController controller = new AdminController(mock.Object);

			// Действие
			Book book1 = controller.Edit(1).ViewData.Model as Book;
			Book book2 = controller.Edit(2).ViewData.Model as Book;
			Book book3 = controller.Edit(3).ViewData.Model as Book;

			// Assert
			Assert.AreEqual(1, book1.BookId);
			Assert.AreEqual(2, book2.BookId);
			Assert.AreEqual(3, book3.BookId);
		}
		
		[TestMethod]
		public void Cannot_Edit_Nonexistent_Book()
		{
			// Организация - создание имитированного хранилища данных
			Mock<IBookRepository> mock = new Mock<IBookRepository>();
			mock.Setup(m => m.Books).Returns(new List<Book>
			{
				new Book { BookId = 1, Name = "Книга1"},
				new Book { BookId = 2, Name = "Книга2"},
				new Book { BookId = 3, Name = "Книга3"},
				new Book { BookId = 4, Name = "Книга4"},
				new Book { BookId = 5, Name = "Книга5"}
			});

			// Организация - создание контроллера
			AdminController controller = new AdminController(mock.Object);

			// Действие
			Book result = controller.Edit(6).ViewData.Model as Book;

			// Assert
		}
