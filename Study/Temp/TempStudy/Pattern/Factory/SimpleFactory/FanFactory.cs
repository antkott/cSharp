using System;
using System.Reflection;

namespace SimpleFactory
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
        IFan CreateFan(FanType type);
    }

    class TableFan : IFan { }

    class CeilingFan : IFan { }

    class ExhaustFan : IFan { }

    class FanFactory : IFanFactory
    {
        public IFan CreateFan(FanType type)
        {
            switch (type)
            {
                case FanType.TableFan:
                    return new TableFan();
                case FanType.CeilingFan:
                    return new CeilingFan();
                case FanType.ExchaustFan:
                    return new ExhaustFan();
                default:
                    return new TableFan();
            }
        }
    }
}
