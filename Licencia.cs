using System;

namespace LicenceGenerator
{
    [Serializable]
    public class Licencia
    {
        public string Suscriptor { get; set; }

        public string Aplicacion { get; set; }

        public DateTime FechaDeVencimiento { get; set; }

        public byte[] LicenceHash { get; set; }

        public string GetKey
        {
            get
            {
                return String.Format("S:{0} A:{1} Fv:{2}", Suscriptor, Aplicacion, FechaDeVencimiento.ToShortDateString());
            }
        }
    }
}