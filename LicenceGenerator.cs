using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace LicenceGenerator
{
    public class LicenceGenerator
    {
        private byte[] SignBytes(byte[] messageBytes)
        {
            byte[] licBytes = File.ReadAllBytes("./keys/private.pem");

            TextReader tx = new StreamReader(new MemoryStream(licBytes));
            AsymmetricCipherKeyPair pair = (AsymmetricCipherKeyPair)new PemReader(tx).ReadObject();
            RsaKeyParameters priv = (RsaKeyParameters)pair.Private;
            ISigner signer = SignerUtilities.GetSigner("SHA1withRSA");
            signer.Init(true, priv);
            signer.BlockUpdate(messageBytes, 0, messageBytes.Length);
            return signer.GenerateSignature();
        }

        public string GenerateLicence(string suscritor, string aplicacion, int mesesDeValides, string fileName)
        {
            var licencia = GenerateLicence(suscritor, aplicacion, mesesDeValides);
            var result = SaveLicence(licencia, fileName);
            return result;
        }

        public Licencia GenerateLicence(string suscritor, string aplicacion, int mesesDeValides)
        {
            var fechaDeVencimiento = DateTime.Now.AddMonths(mesesDeValides);
            var licencia = new Licencia() { Suscriptor = suscritor, Aplicacion = aplicacion, FechaDeVencimiento = fechaDeVencimiento };
            var key = licencia.GetKey;
            var mensaje = Encoding.UTF8.GetBytes(key);
            var hash = SignBytes(mensaje);
            licencia.LicenceHash = hash;
            return licencia;
        }

        public string SaveLicence(Licencia licencia, string directory)
        {
            try
            {
                if (!Directory.Exists(directory))
                {
                    var drirInfo = Directory.CreateDirectory(directory);
                }
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                using (Stream stream = File.OpenWrite(Path.Combine(directory, "licence.lic")))
                {
                    binaryFormatter.Serialize(stream, licencia);
                }
                return "Licence saved successfully in " + directory;
            }
            catch (Exception ex)
            {
                return "error: Can't save the licence.\r\n" + ex.Message;
            }
        }

    }
}
