﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using online_book_store.Domain.Entities;
using System.Web.Mvc;

namespace online_book_store.WebUI.Infrastructure.Binders
{
    public class CartModelBinder : IModelBinder //связывание модели корзины
    {
        private const string sessionKey = "Cart";
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            // Получить объект Cart из сеанса
            Cart cart = null;
            if (controllerContext.HttpContext.Session != null)
            {
                cart = (Cart)controllerContext.HttpContext.Session[sessionKey];
            }
            // Создать объект Cart если он не обнаружен в сеансе
            if (cart == null)
            {
                cart = new Cart();
                if (controllerContext.HttpContext.Session != null)
                {
                    controllerContext.HttpContext.Session[sessionKey] = cart;
                }
            }
            // Возвратить объект Cart
            return cart;
        }
    }
}