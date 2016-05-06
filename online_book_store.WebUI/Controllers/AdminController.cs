using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using online_book_store.Domain.Abstract;
using online_book_store.Domain.Entities;

namespace online_book_store.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IBookRepository repository;

        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }
        public ActionResult Index()
        {
            return View(repository.Books);
        }
        public ViewResult Edit(int bookId)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(Book book)
        {
            if (ModelState.IsValid)
            {
                repository.SaveBook(book);
                TempData["message"] = string.Format("Изменения в книге \"{0}\" были сохранены", book.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(book);
            }
        }
	}
}