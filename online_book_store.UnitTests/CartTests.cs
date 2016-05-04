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
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // ќрганизаци¤ - создание нескольких тестовых книг
            Book book1 = new Book { BookId = 1, Name = " Книга1" };
            Book book2 = new Book { BookId = 2, Name = " Книга2" };

            // ќрганизаци¤ - создание корзины
            Cart cart = new Cart();

            // ƒействие
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // ”тверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Book, book1);
            Assert.AreEqual(results[1].Book, book2);
        }
        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // ќрганизаци¤ - создание нескольких тестовых книг
            Book book1 = new Book { BookId = 1, Name = " Книга1" };
            Book book2 = new Book { BookId = 2, Name = " Книга2" };

            // ќрганизаци¤ - создание корзины
            Cart cart = new Cart();

            // ƒействие
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 1);
            cart.AddItem(book1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Book.BookId).ToList();

            // ”тверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпл¤ров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }
        [TestMethod]
        public void Can_Remove_Line()
        {
            // ќрганизаци¤ - создание нескольких тестовых книг
            Book book1 = new Book { BookId = 1, Name = " Книга1" };
            Book book2 = new Book { BookId = 2, Name = " Книга2" };
            Book book3 = new Book { BookId = 3, Name = " Книга3" };

            // ќрганизаци¤ - создание корзины
            Cart cart = new Cart();

            // ќрганизаци¤ - добавление нескольких книг в корзину
            cart.AddItem(book1, 1);
            cart.AddItem(book2, 4);
            cart.AddItem(book3, 2);
            cart.AddItem(book2, 1);

            // ƒействие
            cart.RemoveLine(book2);

            // ”тверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Book == book2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }
    }
}
