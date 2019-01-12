// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Serilog;

namespace IsoSchemaGenerator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var options = new GeneratorOptions
            {
                SchemaPath = args[0],
            };

            var builder = new ContainerBuilder();

            builder.RegisterModule<ProgramModule>();

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope(b => b.RegisterInstance(options)))
            {
                var generator = scope.Resolve<Generator>();

                generator.Run();
            }
        }

        private class ProgramModule : Module
        {
            protected override void Load(ContainerBuilder builder)
            {
                builder.RegisterType<Generator>()
                    .InstancePerLifetimeScope();

                builder.RegisterType<ZipSchemaBuilder>()
                    .As<ISchemaBuilder>()
                    .InstancePerLifetimeScope();

                builder.Register(ctx =>
                {
                    return new LoggerConfiguration()
                        .WriteTo.ColoredConsole()
                        .MinimumLevel.Debug()
                        .CreateLogger();
                }).As<ILogger>();
            }
        }
    }
}
