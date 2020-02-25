﻿using Autofac;
using ElArch.WebApi.Controllers.DocumentTypes.Dto;
using MediatR;

namespace ElArch.WebApi.Controllers.DocumentTypes
{
    public sealed class DocumentTypeControllerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new CreateDocumentTypeRequestHandler(c.Resolve<IMediator>())).AsImplementedInterfaces();
            builder.Register(_ => new CreateDocumentTypeRequestValidator()).AsImplementedInterfaces();
        }
    }
}