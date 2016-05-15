using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace online_book_store.Domain.Entities
{
    public class Cart //корзина
    {
        private List<CartLine> lineCollection = new List<CartLine>(); //список строк

        public void AddItem(Book book, int quantity) //добавляем новый товар
        {
            CartLine line = lineCollection
                .Where(g => g.Book.BookId == book.BookId)
                .FirstOrDefault();

            if (line == null) //если книги нет в корзине
            {
                lineCollection.Add(new CartLine
                {
                    Book = book,
                    Quantity = quantity
                }); //добавляем её туда
            }
            else //если есть
            {
                line.Quantity += quantity; //добавляем счётчик экземпляров этой книги в корзине
            }
        }

        public void RemoveLine(Book book) // удаление книги из корзины
        {
            lineCollection.RemoveAll(l => l.Book.BookId == book.BookId);
        }

        public decimal ComputeTotalValue() //общая цена
        {
            return lineCollection.Sum(e => e.Book.Price * e.Quantity);

        }
        public void Clear() //очистить корзину
        {
            lineCollection.Clear();
        }

        public IEnumerable<CartLine> Lines
        {
            get { return lineCollection; }
        }
    }

    public class CartLine //класс экземпляров товаров в корзине
    {
        public Book Book { get; set; } //экземпляр книги
        public int Quantity { get; set; } //их колличество
    }
    
}
