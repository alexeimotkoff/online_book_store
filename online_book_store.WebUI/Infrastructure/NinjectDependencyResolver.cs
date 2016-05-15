using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Moq;
using Ninject;
using online_book_store.Domain.Abstract;
using online_book_store.Domain.Entities;
using online_book_store.Domain.Concrete;
using online_book_store.WebUI.Infrastructure.Abstract;
using online_book_store.WebUI.Infrastructure.Concrete;

namespace online_book_store.WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver //настройка контроллера DI. Распознавание зависимостей
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

        private void AddBindings() // Привязка
        {
            kernel.Bind<IBookRepository>().To<EFBookRepository>();
            EmailSettings emailSettings = new EmailSettings
            {
                WriteAsFile = bool.Parse(ConfigurationManager
                    .AppSettings["Email.WriteAsFile"] ?? "false")
            };

            kernel.Bind<IOrderProcessor>().To<EmailOrderProcessor>()
                .WithConstructorArgument("settings", emailSettings);
            
            kernel.Bind<IAuthProvider>().To<FormAuthProvider>();
        }
    }
}