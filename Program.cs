using System;
using System.IO;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;

namespace LicenceGenerator
{
    class Program
    {
        static int Main(string[] args)
        {

            var app = new CommandLineApplication();
            app.HelpOption();
            app.UsePagerForHelpText = false;
            app.Commands.Add(new GenerateCommand());
            app.Commands.Add(new AddKeysCommand());
            app.Commands.Add(new GenerateKeysCommand());

            app.VersionOption("-v| --version", () =>
            {
                var versionString = Assembly.GetEntryAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion.ToString();
                return versionString;
            });
            app.OnExecute(() =>
            {
                return 1;
            });
            return app.Execute(args);
        }
    }
}
