using System;

namespace SimpleFactory
{
    class Program
    {
        //https://www.codeproject.com/Articles/1131770/Factory-Patterns-Simple-Factory-Pattern
        static void Main()
        {
            IFanFactory simpleFanFactory = new FanFactory();
            IFan fan = simpleFanFactory.CreateFan(FanType.TableFan);
            fan.SwitchOn();
            fan.SwitchOff();
        }
    }
}
