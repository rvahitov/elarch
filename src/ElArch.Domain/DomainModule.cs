using Autofac;
using ElArch.Domain.Models.DocumentTypeModel;

namespace ElArch.Domain
{
    public sealed class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new DocumentTypeModule());
        }
    }
}