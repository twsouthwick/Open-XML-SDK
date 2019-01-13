// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Autofac;
using Serilog;
using System;
using System.IO;

namespace IsoSchemaGenerator
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var options = new GeneratorOptions
            {
                SchemaPath = args[0],
                OutputDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()),
            };

            var builder = new ContainerBuilder();

            builder.RegisterAssemblyModules(typeof(Program).Assembly);

            using (var container = builder.Build())
            using (var scope = container.BeginLifetimeScope(b => b.RegisterInstance(options)))
            {
                scope.Resolve<ILogger>().Information("{@Options}", scope.Resolve<GeneratorOptions>());

                var generator = scope.Resolve<SchemaGenerator>();

                generator.Run();
            }
        }
    }
}
