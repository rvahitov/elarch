using Autofac;
using MediatR;

namespace ElArch.WebApi.Infrastructure
{
    public sealed class MediatorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).Assembly).AsImplementedInterfaces();
            builder.Register<ServiceFactory>(ctx =>
            {
                var container = ctx.Resolve<IComponentContext>();
                return serviceType => container.Resolve(serviceType);
            });
        }
    }
}