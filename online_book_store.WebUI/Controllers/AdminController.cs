using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using online_book_store.Domain.Abstract;
using online_book_store.Domain.Entities;

namespace online_book_store.WebUI.Controllers
{
    [Authorize]
    public class AdminController : Controller //контроллер админ-панели
    {
        IBookRepository repository;

        public AdminController(IBookRepository repo)
        {
            repository = repo;
        }
        public ViewResult Index()
        {
            return View(repository.Books); //отображаем список книг
        }
        public ViewResult Edit(int bookId) //редактирование книги
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);
            return View(book);
        }
        [HttpPost]
        public ActionResult Edit(Book book, HttpPostedFileBase image = null) //изменения от пользователя
        {
            if (ModelState.IsValid) //если данные введены нормально
            {
                if (image != null) //если есть картинка
                {
                    book.ImageMimeType = image.ContentType;
                    book.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(book.ImageData, 0, image.ContentLength);
                }
                repository.SaveBook(book); //сохраняем изменения
                TempData["message"] = string.Format("Изменения в книге \"{0}\" были сохранены", book.Name);
                return RedirectToAction("Index");
            }
            else
            {
                // Что-то не так со значениями данных
                return View(book);
            }
        }
        public ViewResult Create() //добавляем новую книгу
        {
            return View("Edit", new Book());
        }
        [HttpPost]
        public ActionResult Delete(int bookId) // удаляем книгу
        {
            Book deletedBook = repository.DeleteBook(bookId); //удаляем выбранную
            if (deletedBook != null)
            {
                TempData["message"] = string.Format("Книга \"{0}\" была удалена",
                    deletedBook.Name);
            }
            return RedirectToAction("Index");
        }
	}
}