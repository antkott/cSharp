using Kafka.Lens.Backend.Tools;
using System;

namespace Kafka.Lens.Runner
{
    class Runner
    {
        //https://docs.confluent.io/current/tutorials/examples/clients/docs/csharp.html?utm_source=github&utm_medium=demo&utm_campaign=ch.examples_type.community_content.clients-ccloud

        static void Main(string[] args)
        {
            ApplicationVersion.PrintConsoleAbout();
            ApplicationVersion.PrintAbout();
            var infraChecker = new InfraChecker();
            infraChecker.Check();
        }
    }
}
