using System.Linq;
using System.Web.Mvc;
using online_book_store.Domain.Entities;
using online_book_store.Domain.Abstract;
using online_book_store.WebUI.Models;

namespace online_book_store.WebUI.Controllers
{
    public class CartController : Controller //контроллер корзины
    {
        private IBookRepository repository;
        private IOrderProcessor orderProcessor;
        public CartController(IBookRepository repo, IOrderProcessor processor)
        {
            repository = repo;
            orderProcessor = processor;
        }
        public ViewResult Index(Cart cart, string returnUrl) //отображение списка добавленных товаров
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }
        public RedirectToRouteResult AddToCart(Cart cart, int bookId, string returnUrl) //добавление в корзину
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                cart.AddItem(book, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int bookId, string returnUrl) //удаление из корзины
        {
            Book book = repository.Books
                .FirstOrDefault(g => g.BookId == bookId);

            if (book != null)
            {
                cart.RemoveLine(book);
            }
            return RedirectToAction("Index", new { returnUrl });
        }
        public PartialViewResult Summary(Cart cart) //отображение на верхней панели кнопки заказать
        {
            return PartialView(cart);
        }
        public ViewResult Checkout() //оформление заказа
        {
            return View(new ShippingDetails());
        }
        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails) //информация об оформленном заказе от пользователя
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Извините, ваша корзина пуста!");
            }

            if (ModelState.IsValid) //если заказ обработан удачно
            {
                orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else //если неудачно
            {
                return View(shippingDetails);
            }
        }
	}
}