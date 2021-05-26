using Jering.Javascript.NodeJS;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Test.ProjectPath
{
    class Program
    {
        static async Task Main()
        {
            // Create INodeJSService
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddNodeJS();
            serviceCollection.Configure<NodeJSProcessOptions>(options => options.ProjectPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Javascript")); // AppDomain.CurrentDomain.BaseDirectory is your bin/<configuration>/<targetframework> directory
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var nodeJSService = serviceProvider.GetRequiredService<INodeJSService>();

            // Invoke from file
            string result1 = await nodeJSService.InvokeFromFileAsync<string>("./interop.js", args: new[] { "testString" }).ConfigureAwait(false);
            Console.WriteLine(result1);

            // Invoke from string
            string interopModuleString = @"const lodash = require('lodash');

module.exports = (callback, arg) => {
    callback(null, lodash.snakeCase(arg));
}";
            string result2 = await nodeJSService.InvokeFromStringAsync<string>(interopModuleString, args: new[] { "testString" }).ConfigureAwait(false);
            Console.WriteLine(result2);

            // Expected output:
            //
            // test-string
            // test_string
        }
    }
}
