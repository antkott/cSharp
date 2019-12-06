using System;
using System.Collections.Generic;
using System.Text;

namespace Adapter
{
    class CamelToTransportAdapter : ITransport
    {
        Camel camel;

        public CamelToTransportAdapter(Camel camel)
        {
            this.camel = camel;
        }

        public void Drive()
        {
            camel.Move();
        }
    }
}
