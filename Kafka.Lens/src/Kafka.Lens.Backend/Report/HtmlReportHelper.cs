using Kafka.Lens.Backend.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kafka.Lens.Backend.Report
{
    public class HtmlReportHelper
    {
        //https://www.tablesgenerator.com/html_tables

        public string PopulateTemplate(List<Report> reportList)
        {
            string body = string.Empty;
            using (StreamReader reader = new StreamReader(@"template.htm"))
            {
                body = reader.ReadToEnd();
            }
            var counter = 0;
            foreach (var report in reportList)
            {
                counter++;
                body = body.Replace($"$env{counter}$", report.EnvName);
                body = body.Replace($"$KafkaStatus{counter}$", report.KafkaStatus);
                body = body.Replace($"$MongoStatus{counter}$", report.MongoStatus);
            }
            File.WriteAllText(@"Report.html", body);
            return body;
        }


    }
}
