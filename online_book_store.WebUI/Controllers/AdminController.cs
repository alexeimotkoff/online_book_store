using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using online_book_store.Domain.Abstract;

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
	}
}