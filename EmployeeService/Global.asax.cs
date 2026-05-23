using Autofac;
using Autofac.Integration.Wcf;
using EmployeeService.Implementation;
using System;
using System.Web;

namespace EmployeeService
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new ProgramModule());
            builder.RegisterType<EmployeeWcfService>()
               .As<IEmployeeService>()
               .AsSelf()
               .SingleInstance();

            var container = builder.Build();
            bool exists = container.IsRegistered<EmployeeWcfService>();
            AutofacHostFactory.Container = container;
        }
    }
}
