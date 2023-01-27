using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeTrySimulation
{
    public sealed class Foo
    {
        private readonly Int32 _value;

        public Foo(int value)
        {
            _value = value;
        }

        public override Int32 GetHashCode() => 1;
    }

    public sealed class Foo2
    {
        private readonly Int32 _value;

        public Foo2(int value)
        {
            _value = value;
        }

        public override Int32 GetHashCode() => 1;

        public override bool Equals(object? obj)
        {
            return obj is Foo2 other && _value ==other._value;
        }
    }

    public static class FooDictionary
    {
        public static Dictionary<Foo, object> FooDictionaryIns => new ();

        public static Dictionary<Foo2, object> Foo2DictionaryIns => new();
    }
}
