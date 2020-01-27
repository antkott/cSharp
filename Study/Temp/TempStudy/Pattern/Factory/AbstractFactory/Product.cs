using System;
using System.Reflection;

namespace AbstractFactory

{

    interface IElectricalEqipment
    {
        void SwitchOn()
        {
            Console.WriteLine($" {GetType().Name} performed " +
                           $"{MethodBase.GetCurrentMethod().Name}");
        }

        void SwitchOff()
        {
            Console.WriteLine($" {GetType().Name} performed" +
                           $" {MethodBase.GetCurrentMethod().Name}");
        }
    }

    interface IFan : IElectricalEqipment
    {
    }

    interface ITubelight : IElectricalEqipment { }

    class IndianFan : IFan { }

    class IndianTubeligth : ITubelight { }

    class USFan : IFan { }

    class USTubeligth : ITubelight { }
}
