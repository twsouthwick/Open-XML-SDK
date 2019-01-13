// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.Xml.Schema;

namespace IsoSchemaGenerator
{
    public class ProgramModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SchemaGenerator>()
                .InstancePerLifetimeScope();

            builder.RegisterType<ZipSchemaBuilder>()
                .As<ISchemaBuilder>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(Program).Assembly)
                .AssignableTo<ISchemaCleaner>()
                .As<ISchemaCleaner>()
                .InstancePerLifetimeScope();

            builder.Register(ctx =>
            {
                return new LoggerConfiguration()
                    .WriteTo.ColoredConsole()
                    .MinimumLevel.Debug()
                    .Destructure.With<XmlSchemaTypeDestructuringPolicy>()
                    .CreateLogger();
            }).As<ILogger>();
        }

        private class XmlSchemaTypeDestructuringPolicy : IDestructuringPolicy
        {
            public bool TryDestructure(object value, ILogEventPropertyValueFactory propertyValueFactory, out LogEventPropertyValue result)
            {
                if (value is XmlSchemaType type)
                {
                    result = new ScalarValue(type.QualifiedName);
                    return true;
                }

                result = null;
                return false;
            }
        }
    }
}
