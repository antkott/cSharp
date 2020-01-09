using System;

namespace AbstractFactory
{

    // https://www.codeproject.com/Articles/1137307/Factory-Patterns-Abstract-Factory-Pattern

    //AbstractFactory(IElectricalFactory)
    //ConcreteFactory(IndianElectricalFactory, USElectricalFactory)
    //AbstractProduct(IFan, ITubeLight)
    //ConcreteProduct(IndianFan, IndianTubelight, USFan, USTubelight)

    class Program
    {
        static void Main()
        {
            USElecticalFactory uSElecticalFactory = new USElecticalFactory();
            IFan fan = uSElecticalFactory.GetFan(); 
            fan.SwitchOn();
            fan.SwitchOff();
            ITubelight tubelight = uSElecticalFactory.GetTubelight();
            tubelight.SwitchOn();
            tubelight.SwitchOff();

            IndianElecticalFactory indianElecticalFactory = new IndianElecticalFactory();
            IFan fan2 = indianElecticalFactory.GetFan();
            fan2.SwitchOn();
            fan2.SwitchOff();
            ITubelight tubelight2 = indianElecticalFactory.GetTubelight();
            tubelight2.SwitchOn();
            tubelight2.SwitchOff();
        }
    }
}
