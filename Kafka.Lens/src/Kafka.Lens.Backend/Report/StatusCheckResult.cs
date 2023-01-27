using System.Text;

namespace Kafka.Lens.Backend.Report
{
    public class StatusCheckResult
    {
        public InfraType InfraType { get; set; }

        public ReportStatus Status { get; set; }

        public StringBuilder LogOutput { get; set; }

    }
}
