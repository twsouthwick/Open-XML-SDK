// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Serilog;
using XmlSchemaClassGenerator;

namespace IsoSchemaGenerator.ClassGenerator
{
    public class SchemaGeneratorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileOutputWriter>()
                .As<OutputWriter>()
                .InstancePerLifetimeScope();

            builder.RegisterType<OpenXmlNamespaceProvider>()
                .As<NamespaceProvider>()
                .InstancePerLifetimeScope();

            builder.Register(ctx =>
            {
                var logger = ctx.Resolve<ILogger>();

                return new Generator
                {
                    OutputWriter = ctx.Resolve<OutputWriter>(),
                    Log = s => logger.Information(s),
                    GenerateNullables = false,
                    NamespaceProvider = ctx.Resolve<NamespaceProvider>(),
                };
            }).InstancePerLifetimeScope();

            builder.RegisterType<GeneratorClassGenerator>()
                .As<IClassGenerator>()
                .InstancePerLifetimeScope();
        }
    }
}
