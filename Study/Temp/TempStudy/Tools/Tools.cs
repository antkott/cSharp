using System.Collections;
using static System.Console;

namespace Toolset
{
    public class Tools
    {

        public static void ShowItemsEnumerable(IEnumerable enumerable)
        {
            Write("[ ");
            foreach (object item in enumerable)
            {
                if (item.GetType().ToString().Equals("System.Collections.DictionaryEntry"))
                {
                    DictionaryEntry itemdict = (DictionaryEntry)item;
                    Write($"{itemdict.Key}:{itemdict.Value}, ");
                }
                else
                {
                    Write(item + ", ");
                }
            }

            WriteLine(" ]");
        }
    }
}
