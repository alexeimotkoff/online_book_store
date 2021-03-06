﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using online_book_store.WebUI.Infrastructure.Abstract;

namespace online_book_store.WebUI.Infrastructure.Concrete
{
    public class FormAuthProvider : IAuthProvider //реализация аутентификации
    {
        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, password); //проверка через формы
            if (result)
                FormsAuthentication.SetAuthCookie(username, false);
            return result;
        }
    }
}