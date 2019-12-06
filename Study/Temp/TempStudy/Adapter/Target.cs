using System;
using System.Collections.Generic;
using System.Text;

namespace Adapter
{
    
    interface IAnimal {
        public void Move();
    }

    class Camel : IAnimal
    {
        public void Move()
        {
            Console.WriteLine($"Camel walks through the desert sands");
        }
    }
}
