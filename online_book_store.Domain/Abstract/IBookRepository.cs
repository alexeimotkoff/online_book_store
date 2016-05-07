using System.Collections.Generic;
using online_book_store.Domain.Entities;

namespace online_book_store.Domain.Abstract
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get; }
        void SaveBook(Book book);
        Book DeleteBook(int bookId);
    }
}
