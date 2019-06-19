using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;

namespace LicenceGenerator
{
    public class GenerateCommand : CommandLineApplication
    {
        public GenerateCommand()
        {
            Name = "generate";
            Description = "Generate a Licence using RSA public and private key";
            HelpOption("-h| --help");
            UsePagerForHelpText = false;
            var subscriptorArg = Argument("subscriptor", "The name of the subscriptor of the licence.");
            var appArg = Argument("application", "The name of the application the licence is for.");
            var monthArg = this.Argument<int>("month", "Duration of the licence in month from the actual date.");
            var outputOption = Option("-o", "The output directory", CommandOptionType.SingleValue);
            OnExecute(() =>
                {
                    if (subscriptorArg.Value == String.Empty || subscriptorArg.Value == null)
                    {
                        Console.WriteLine("error: subscriptor argument is missing.");
                        ShowHelp();
                        return 1;
                    }
                    if (appArg.Value == String.Empty || appArg.Value == null)
                    {
                        Console.WriteLine("error: Application argument is missing.");
                        ShowHelp();
                        return 1;
                    }
                    if (monthArg.Value == String.Empty || monthArg.Value == null)
                    {
                        Console.WriteLine("error: Month argument is missing.");
                        ShowHelp();
                        return 1;
                    }
                    var gl = new LicenceGenerator();
                    var lic = gl.GenerateLicence(subscriptorArg.Value, appArg.Value, monthArg.ParsedValue);
                    var directory = outputOption.HasValue() ? outputOption.Value() : "./licence/";
                    var result = gl.SaveLicence(lic, directory);
                    Console.WriteLine(result);
                    return 1;
                });

        }
    }
}