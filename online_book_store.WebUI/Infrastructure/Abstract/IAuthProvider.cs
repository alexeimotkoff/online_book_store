using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace online_book_store.WebUI.Infrastructure.Abstract
{
    public interface IAuthProvider //интерфейс аутентификации
    {
        bool Authenticate(string username, string password);
    }
}
