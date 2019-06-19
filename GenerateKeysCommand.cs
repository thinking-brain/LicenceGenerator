using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using System.IO;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.X509;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Crypto.Generators;

namespace LicenceGenerator
{
    public class GenerateKeysCommand : CommandLineApplication
    {
        public GenerateKeysCommand()
        {
            Name = "generate-key";
            Description = "Generate an RSA private and public key for usage as Licence generator";
            this.HelpOption();
            UsePagerForHelpText = false;
            OnExecute(() =>
                {
                    var keyGen = new RsaKeyPairGenerator();
                    keyGen.Init(new KeyGenerationParameters(new SecureRandom(), 4096));
                    var keyPair = keyGen.GenerateKeyPair();

                    PrivateKeyInfo pkInfo = PrivateKeyInfoFactory.CreatePrivateKeyInfo(keyPair.Private);
                    String privateKey = Convert.ToBase64String(pkInfo.GetDerEncoded());

                    var textWriter = new StreamWriter(new FileStream("./keys/private.pem", FileMode.Create));
                    var pemWriter = new PemWriter(textWriter);
                    pemWriter.WriteObject(keyPair.Private);
                    pemWriter.Writer.Flush();
                    textWriter.Close();

                    SubjectPublicKeyInfo info = SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo(keyPair.Public);
                    String publicKey = Convert.ToBase64String(info.GetDerEncoded());

                    var text2Writer = new StreamWriter(new FileStream("./keys/public.pub", FileMode.Create));
                    var pubWriter = new PemWriter(text2Writer);
                    pubWriter.WriteObject(keyPair.Public);
                    pubWriter.Writer.Flush();
                    text2Writer.Close();

                    Console.WriteLine("Public and private key generated successfully.");
                    return 1;
                });
        }
    }
}