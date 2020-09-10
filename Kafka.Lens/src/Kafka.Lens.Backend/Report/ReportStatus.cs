using System;
using System.Collections.Generic;
using System.Text;

namespace Kafka.Lens.Backend.Report
{
    public static class ReportStatus
    {
        public const string
        Undefined = "Undefined",
        Ok = "Ok",
        Error = "Error",
        CertificateError = "CertificateError";
    }
}
