using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using EmployeeService.Implementation.Contracts.Requests;
using EmployeeService.Implementation.Contracts.Responses;
using EmployeeService.Implementation.ErrorHandling;
using EmployeeService.Implementation.Handlers;
using EmployeeService.Implementation.Repository;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace EmployeeService.Implementation
{
    public class ProgramModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<ILoggerFactory>(x => new SerilogLoggerFactory(Log.Logger))
                   .As<ILoggerFactory>()
                   .SingleInstance();

            builder.RegisterGeneric(typeof(Logger<>))
                   .As(typeof(ILogger<>))
                   .SingleInstance();

            builder.Register(c => c.Resolve<ILoggerFactory>().CreateLogger("Default"))
                   .As<Microsoft.Extensions.Logging.ILogger>()
                   .SingleInstance();

            builder.RegisterAutoMapper(typeof(MappingProfile).Assembly);

            builder.RegisterMediatR(MediatRConfigurationBuilder
                .Create(typeof(EmployeeManagementHandler).Assembly)
                .Build());
            builder.RegisterType<EmployeeManagementHandler>()
                   .As<MediatR.IRequestHandler<GetEmployeeRequest, GetEmployeeResponse>>()
                   .As<MediatR.IRequestHandler<EnableEmployeeRequest, EnableEmployeeResponse>>()
                   .InstancePerDependency();

            builder.RegisterType<EmployeeRepository>()
                .As<IEmployeeRepository>()
                .InstancePerDependency();

            builder.RegisterType<RequestHandler>()
                .As<IRequestHandler>()
                .InstancePerDependency();
        }
    }
}
