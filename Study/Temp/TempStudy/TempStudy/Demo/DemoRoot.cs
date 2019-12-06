using System;
using System.Collections.Generic;
using System.Text;

namespace StudyDemo.Demo
{
    public abstract class DemoRoot
    {

        public void ShowRunDemoInformation() {
            Console.WriteLine("");
            Console.WriteLine("---------");
            Console.WriteLine($"executing {this.GetType().Name}");
        }
    }
}
