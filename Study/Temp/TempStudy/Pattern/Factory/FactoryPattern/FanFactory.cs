using System;
using System.Reflection;

namespace FactoryPattern
{

    enum FanType
    {
        TableFan,
        CeilingFan,
        ExchaustFan
    }

    interface IFan
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

    interface IFanFactory
    {
        IFan CreateFan();
    }

    class TableFan : IFan { }

    class CeilingFan : IFan { }

    class ExhaustFan : IFan { }

    class TableFanFactory : IFanFactory
    {
        public IFan CreateFan()
        {
            Console.WriteLine($" {GetType().Name} performed" +
             $" {MethodBase.GetCurrentMethod().Name}");
            Console.WriteLine($"{nameof(TableFan)} created");
            return new TableFan();
        }
    }

    class CeilingFanFactory : IFanFactory
    {
        public IFan CreateFan()
        {
            Console.WriteLine($" {GetType().Name} performed" +
             $" {MethodBase.GetCurrentMethod().Name}");
            Console.WriteLine($"{nameof(CeilingFan)} created");
            return new CeilingFan();
        }
    }

    class ExhaustFanFactory : IFanFactory
    {
        public IFan CreateFan()
        {
            Console.WriteLine($" {GetType().Name} performed" +
             $" {MethodBase.GetCurrentMethod().Name}");
            Console.WriteLine($"{nameof(ExhaustFan)} created");
            return new ExhaustFan();
        }
    }
}
