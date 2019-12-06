using System;
using System.Collections.Generic;
using System.Text;
using StudyDemo.Demo;

namespace StudyDemo.Demo
{
    class LambdaDemo : DemoRoot
    {
        private delegate void Hello(); // делегат без параметров
        private delegate void HelloParameters(string mess); // делегат без параметров

        public void RunDemo()
        {
            this.ShowRunDemoInformation();
            this.RunDemo1();
            this.RunDemo2();
        }

        private void RunDemo1()
        {
            Hello hello1 = delegate
            {
                Console.WriteLine("Hello");
            };
            Hello hello2 = () => Console.WriteLine("Welcome");
            hello1();       // Hello
            hello2();       // Welcome
        }

        private void RunDemo2()
        {
            HelloParameters hello1 = delegate (string mess)
            {
                Console.WriteLine($"Hello {mess}");
            };
            HelloParameters hello2 = (mess) => Console.WriteLine($"Hello {mess}");
            hello1("test");
            hello2("test");
        }
    }
}
