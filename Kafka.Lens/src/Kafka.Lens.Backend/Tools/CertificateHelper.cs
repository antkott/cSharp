using NLog;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Kafka.Lens.Backend.Tools
{
    public static class CertificateHelper
    {
        private static Logger _logger => LogManager.GetCurrentClassLogger();

        public static X509Certificate2 GetCertificate(
            string CertificateSubject,
            StoreName store =StoreName.Root, 
            StoreLocation location =StoreLocation.CurrentUser)
        {
            var x509Store = new X509Store(store, location);
            x509Store.Open(OpenFlags.ReadOnly);
            var certSubj = $"CN={CertificateSubject}";

            X509Certificate2Collection col = x509Store.Certificates;
            foreach (var certificate in col)
            {
                if (certificate.Subject.StartsWith(certSubj)) {
                    _logger.Info($"find the '{CertificateSubject}' certificate");
                    x509Store.Dispose();
                    
                    return certificate;
                }
            }
            x509Store.Dispose();
            throw new CertificateException($"can't find a certificate by the '{certSubj}' subject");
        }

        /// <summary>
        /// Export a certificate to a PEM format file
        /// </summary>
        /// <param name="Certificate">The certificate to export</param>
        /// <param name="PathToFile">The file path </param>
        public static void ExportToPEMFile(X509Certificate Certificate, string PathToFile)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("-----BEGIN CERTIFICATE-----");
            builder.AppendLine(Convert.ToBase64String(Certificate.Export(X509ContentType.Cert), Base64FormattingOptions.InsertLineBreaks));
            builder.AppendLine("-----END CERTIFICATE-----");
            File.WriteAllText(PathToFile, builder.ToString());
            var fileInfo = new FileInfo(PathToFile);
            string fullname = fileInfo.FullName;
            if (File.Exists(fullname))
            {
                _logger.Info($"certificate exported to the '{fullname}' file");
            }
            else {
                throw new CertificateException($"can't find an exported certificate by '{fullname}' path");
            }
        }

    }
}
