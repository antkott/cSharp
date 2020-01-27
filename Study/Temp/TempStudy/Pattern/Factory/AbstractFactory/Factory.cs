using System;
using System.Collections.Generic;
using System.Text;

namespace AbstractFactory
{
    interface IElectricalFactory
    {
        IFan GetFan();
        ITubelight GetTubelight();

    }

    class USElecticalFactory : IElectricalFactory

    {
        public IFan GetFan()
        {
            return new USFan();
        }

        public ITubelight GetTubelight()
        {
            return new USTubeligth();
        }

        //public USElecticalFactory(IFan fan, ITubelight tubelight)
        //{

        //}
    }

    class IndianElecticalFactory : IElectricalFactory
    {
        public IFan GetFan()
        {
            return new IndianFan();
        }

        public ITubelight GetTubelight()
        {
            return new IndianTubeligth();
        }
    }
}
