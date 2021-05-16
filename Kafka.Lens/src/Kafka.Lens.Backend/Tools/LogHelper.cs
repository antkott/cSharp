using System;
using System.Text;

namespace Kafka.Lens.Backend.Tools
{
    public class LogHelper
    {
        public StringBuilder LogStash { get; set; }

        private string CurrentDateTime => DateTime.Now.ToString("dd.MM.yy HH:mm:ss");

        public LogHelper(string initialMessage = null)
        {
            if (string.IsNullOrEmpty(initialMessage))
            {
                LogStash = new StringBuilder();
            }
            else {
                LogStash = new StringBuilder(initialMessage);
                LogStash.AppendLine(string.Empty);
            }
            
        }

        public void Info(string message) {
            LogStash.AppendLine($"|{CurrentDateTime}|INFO| {message}");
        }

        public void Error(string message)
        {
            LogStash.AppendLine($"|{CurrentDateTime}|ERR| {message}");
        }
    }
}
