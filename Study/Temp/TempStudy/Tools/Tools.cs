using System;
using System.Collections;
using static System.Console;

namespace Toolset
{
    public class Tools
    {

        public static void ShowItemsEnumerable(IEnumerable enumerable)
        {
            Write("[ ");
            foreach (var item in enumerable)
            {
                Write(item + ", ");
            }
            WriteLine(" ]");
        }
    }
}
