using System;

namespace Kafka.Lens.Backend.Tools
{
    public class CertificateException : Exception
    {
        public CertificateException(string message) : base(message)
        {

        }

        public CertificateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
