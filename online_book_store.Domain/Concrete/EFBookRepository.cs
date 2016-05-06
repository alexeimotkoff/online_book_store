using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using online_book_store.Domain.Entities;
using online_book_store.Domain.Abstract;

namespace online_book_store.Domain.Concrete
{
    public class EFBookRepository : IBookRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Book> Books
        {
            get { return context.Books; }
        }
        public void SaveBook(Book book)
        {
            if (book.BookId == 0)
                context.Books.Add(book);
            else
            {
                Book dbEntry = context.Books.Find(book.BookId);
                if (dbEntry != null)
                {
                    dbEntry.Name = book.Name;
                    dbEntry.Author = book.Author;
                    dbEntry.Description = book.Description;
                    dbEntry.Price = book.Price;
                    dbEntry.Category = book.Category;
                }
            }
            context.SaveChanges();
        }
    }
}
