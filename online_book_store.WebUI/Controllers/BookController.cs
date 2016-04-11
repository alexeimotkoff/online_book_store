﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using online_book_store.Domain.Abstract;
using online_book_store.Domain.Entities;
using online_book_store.WebUI.Models;

namespace online_book_store.WebUI.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository repository;
        public int pageSize = 4;
       public BookController(IBookRepository repo)
        {
            repository = repo;
        }
       public ViewResult List(int page = 1)
       {
           BooksListViewModel model = new BooksListViewModel
           {
               Books = repository.Books
               .OrderBy(book => book.BookId)
               .Skip((page - 1) * pageSize)
               .Take(pageSize),
               PagingInfo = new PagingInfo
               {
                   CurrentPage = page,
                   ItemsPerPage = pageSize,
                   TotalItems = repository.Books.Count()
               }
           };
           return View(model);
       }
	}
}