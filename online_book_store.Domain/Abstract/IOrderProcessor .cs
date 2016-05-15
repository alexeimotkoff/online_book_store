using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using online_book_store.Domain.Entities;

namespace online_book_store.Domain.Abstract
{
    public interface IOrderProcessor //интерфейс совершения покупок в корзине
    {
        void ProcessOrder(Cart cart, ShippingDetails shippingDetails); //покупка
    }
}
