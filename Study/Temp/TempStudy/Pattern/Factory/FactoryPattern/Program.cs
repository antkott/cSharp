using System;

namespace FactoryPattern
{
    class Program
    {
        //https://www.codeproject.com/Articles/1135918/Factory-Patterns-Factory-Method-Pattern

        static void Main()
        {
            IFanFactory tableFanFactory = new TableFanFactory();
            IFan tableFan = tableFanFactory.CreateFan();
            tableFan.SwitchOn();
            tableFan.SwitchOff();

            IFanFactory ceilingFanFactory = new CeilingFanFactory(); ;
            IFan ceilingFan = ceilingFanFactory.CreateFan();
            ceilingFan.SwitchOn();
            ceilingFan.SwitchOff();

            IFanFactory exhaustFanFactory = new ExhaustFanFactory(); ;
            IFan exhaustFan = exhaustFanFactory.CreateFan();
            exhaustFan.SwitchOn();
            exhaustFan.SwitchOff();
        }
    }
}
