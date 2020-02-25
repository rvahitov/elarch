using Autofac;

namespace ElArch.WebApi.Infrastructure
{
    public sealed class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ActorSystemModule());
        }
    }
}