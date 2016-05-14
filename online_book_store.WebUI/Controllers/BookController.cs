using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using online_book_store.Domain.Abstract;
using online_book_store.Domain.Entities;
using online_book_store.WebUI.Models;

namespace online_book_store.WebUI.Controllers
{ 
    public class BookController : Controller
    {
        private IBookRepository repository;
        public int pageSize = 4;
       public BookController(IBookRepository repo)
        {
            repository = repo;
        }
       public ViewResult List(string category, int page = 1)
       {
           BooksListViewModel model = new BooksListViewModel
           {
               Books = repository.Books
               .Where(p => category == null || p.Category == category)
               .OrderBy(book => book.BookId)
               .Skip((page - 1) * pageSize)
               .Take(pageSize),
               PagingInfo = new PagingInfo
               {
                   CurrentPage = page,
                   ItemsPerPage = pageSize,
                   TotalItems = category == null ?
                   repository.Books.Count() :
                   repository.Books.Where(book => book.Category == category).Count()
               },
               CurrentCategory = category
           };
           return View(model);
       }
       public FileContentResult GetImage(int bookId)
       {
           Book book = repository.Books
               .FirstOrDefault(g => g.BookId == bookId);

           if (book != null)
           {
               return File(book.ImageData, book.ImageMimeType);
           }
           else
           {
               return null;
           }
       }
	}
}