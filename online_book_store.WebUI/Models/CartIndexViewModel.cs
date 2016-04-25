using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using online_book_store.Domain.Entities;

namespace online_book_store.WebUI.Models
{
	public class CartIndexViewModel
	{
        public Cart Cart { get; set; }
        public string ReturnUrl { get; set; }
	}
}