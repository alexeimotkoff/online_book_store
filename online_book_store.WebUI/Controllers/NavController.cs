using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using online_book_store.Domain.Abstract;

namespace online_book_store.WebUI.Controllers
{
    public class NavController : Controller //контроллер меню навигации по категориям
    {
        //
        // GET: /Nav/
        private IBookRepository repository;
        
        public NavController(IBookRepository repo)
        {
            repository = repo;
        }
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = repository.Books
                .Select(book => book.Category)
                .Distinct()
                .OrderBy(x => x);
            return PartialView(categories);
        }
	}
}