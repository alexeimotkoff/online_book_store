﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using online_book_store.Domain.Abstract;
using online_book_store.Domain.Entities;
using online_book_store.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;

namespace GameStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrieve_Image_Data()
        {
            // Организация - создание объекта Book с данными изображения
            Book book = new Book
            {
                BookId = 2,
                Name = "Книга 2",
                ImageData = new byte[] { },
                ImageMimeType = "image/png"
            };

            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
                new Book {BookId = 1, Name = "Книга1"},
                book,
                new Book {BookId = 3, Name = "Книга3"}
            }.AsQueryable());

            // Организация - создание контроллера
            BookController controller = new BookController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(2);

            // Утверждение
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FileResult));
            Assert.AreEqual(game.ImageMimeType, ((FileResult)result).ContentType);
        }

        [TestMethod]
        public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
        {
            // Организация - создание имитированного хранилища
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book> {
                new Book {BookId = 1, Name = "Книга1"},
                new Book {BookId = 2, Name = "Книга2"}
            }.AsQueryable());

            // Организация - создание контроллера
            BookController controller = new BookController(mock.Object);

            // Действие - вызов метода действия GetImage()
            ActionResult result = controller.GetImage(10);

            // Утверждение
            Assert.IsNull(result);
        }
    }
}
