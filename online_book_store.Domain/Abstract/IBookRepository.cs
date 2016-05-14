using System.Collections.Generic;
using online_book_store.Domain.Entities;

namespace online_book_store.Domain.Abstract
{
    public interface IBookRepository //Интерфейс реализации связи с базой данных
    {
        IEnumerable<Book> Books { get; }
        void SaveBook(Book book);
        Book DeleteBook(int bookId);
    }
}
