using System.Linq;
using System.Web.Mvc;
using online_book_store.Domain.Entities;
using online_book_store.Domain.Abstract;
using online_book_store.WebUI.Models;

namespace online_book_store.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IBookRepository repository;
        public CartController(IBookRepository repo)
        {
            repository = repo;
        }
        public RedirectToRouteResult AddToCart(int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                GetCart().AddItem(book, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int bookId, string returnUrl)
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                GetCart().RemoveLine(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }
	}
}