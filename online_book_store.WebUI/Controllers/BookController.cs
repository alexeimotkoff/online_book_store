using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using online_book_store.Domain.Abstract;
using online_book_store.Domain.Entities;

namespace online_book_store.WebUI.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository repository;
       public BookController(IBookRepository repo)
        {
            repository = repo;
        }
       public ViewResult List()
       {
           return View(repository.Books);
       }
	}
}