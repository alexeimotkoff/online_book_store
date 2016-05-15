using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using online_book_store.Domain.Entities;
using online_book_store.Domain.Abstract;

namespace online_book_store.Domain.Concrete
{
    public class EFBookRepository : IBookRepository //Реализация интерфейса
    {
        EFDbContext context = new EFDbContext(); //Создание контекста базы данных
        public IEnumerable<Book> Books //получить все книги
        {
            get { return context.Books; }
        }
        public void SaveBook(Book book) // добавление в базу данных новых книг
        {
            if (book.BookId == 0) //если 0, то добавляем товар в бд
                context.Books.Add(book);
            else //если не равно 0, то изменяем существующую книгу
            {
                Book dbEntry = context.Books.Find(book.BookId); // находим книгу
                if (dbEntry != null)
                {
                    dbEntry.Name = book.Name;
                    dbEntry.Author = book.Author;
                    dbEntry.Description = book.Description;
                    dbEntry.Price = book.Price;
                    dbEntry.Category = book.Category;
                    dbEntry.ImageData = book.ImageData;
                    dbEntry.ImageMimeType = book.ImageMimeType;
                }
            }
            context.SaveChanges(); // сохраняем изменения
        }
        public Book DeleteBook(int bookId) // удаление книг
        {
            Book dbEntry = context.Books.Find(bookId); //находим книгу
            if (dbEntry != null)
            {
                context.Books.Remove(dbEntry); //удаляем
                context.SaveChanges(); //сохраняем изменения
            }
            return dbEntry;
        }
    }
}
