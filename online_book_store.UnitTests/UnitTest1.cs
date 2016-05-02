﻿using System;
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
    		});
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;
            // Действие (act)
            BooksListViewModel result = (BooksListViewModel)controller.List(null, 2).Model;
            // Утверждение
            List<Book> books = result.Books.ToList();
            Assert.IsTrue(books.Count == 2);
            Assert.AreEqual(books[0].Name, "Книга4");
            Assert.AreEqual(books[1].Name, "Книга5");
        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }
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
    		});
            BookController controller = new BookController(mock.Object);
            controller.pageSize = 3;

            // Act
            BooksListViewModel result
            = (BooksListViewModel)controller.List(null, 2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
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
                .Books.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Книга2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Книга4" && result[1].Category == "Cat2");
        }
	}
}
