using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Moq;
using Ninject;
using online_book_store.Domain.Abstract;
using online_book_store.Domain.Entities;

namespace online_book_store.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
    {
        new Book { Name = "fg", Price = 1488 },
        new Book { Name = "ff", Price=2299 },
        new Book { Name = "rr", Price=899.4M }
    });
            kernel.Bind<IBookRepository>().ToConstant(mock.Object);
        }
    }
}