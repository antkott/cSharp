using System;
using System.Collections.Generic;
using System.Text;

namespace Adapter
{
    
    interface ITransport {
        void Drive();
    }

    class Auto : ITransport
    {
        public void Drive()
        {
            Console.WriteLine($"Car rides on the road");
        }
    }

    class Driver {
        public void Travel(ITransport transport) {
            transport.Drive();
        }
    }

}
