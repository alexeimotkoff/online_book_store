using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using online_book_store.Domain.Entities;

namespace online_book_store.WebUI.Models
{
    public class BooksListViewModel
    {
        public IEnumerable<Book> Books { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}