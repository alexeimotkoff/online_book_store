using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using online_book_store.WebUI.Infrastructure.Abstract;
using online_book_store.WebUI.Models;

namespace online_book_store.WebUI.Controllers
{
    public class AccountController : Controller //контроллер аутентификации
    {
        IAuthProvider authProvider;
        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        public ViewResult Login() //отображение страницы автризации
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl) //отправка результатов аутентификации от пользователя
        {

            if (ModelState.IsValid) //если совпадают со словарём в Web.config
            {
                if (authProvider.Authenticate(model.UserName, model.Password)) //если пароль и логин правильные
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Admin")); //показываем страницу админ-панели
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин или пароль");
                    return View();
                }
            }
            else //если нет
            {
                return View(); //ещё раз показываем страницу авторизации
            }
        }
	}
}