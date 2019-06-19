using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using System.IO;

namespace LicenceGenerator
{
    public class AddKeysCommand : CommandLineApplication
    {
        public AddKeysCommand()
        {
            Name = "add-key";
            var privateArg = Argument("private-key", "The file with the private key.");
            var publicArg = Argument("public-key", "The file with the public key.");
            UsePagerForHelpText = false;
            OnExecute(() =>
                {
                    if (privateArg.Value == String.Empty || privateArg.Value == null)
                    {
                        Console.WriteLine("error: private-key argument is missing.");
                        ShowHelp();
                        return 1;
                    }
                    if (publicArg.Value == String.Empty || publicArg.Value == null)
                    {
                        Console.WriteLine("error: public-key argument is missing.");
                        ShowHelp();
                        return 1;
                    }
                    if (!File.Exists(privateArg.Value))
                    {
                        Console.WriteLine("error: public-key file doesn't exist.");
                    }
                    if (!File.Exists(privateArg.Value))
                    {
                        Console.WriteLine("error: private-key file doesn't exist.");
                    }
                    File.Copy(privateArg.Value, "./", true);
                    File.Copy(publicArg.Value, "./", true);
                    Console.WriteLine("public and private key files copied successfully.");
                    return 1;
                });
        }
    }
}