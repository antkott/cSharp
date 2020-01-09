using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace AntKott.Study.Searching
{
    public class OtherAlgorithm
    {

        public HashSet<string> Filtering<T>(IEnumerable<T> input)
        {
            HashSet<string> withoutDupes = new HashSet<string>();
            foreach (T item in input)
            {
                withoutDupes.Add(item.ToString());
            }
            return withoutDupes;
        }

        public Hashtable ValueCounter(IEnumerable input)
        {
            Hashtable output = new Hashtable();
            foreach (object item in input)
            {
                if (output.ContainsKey(item))
                {
                    output[item] = (int)output[item] + 1;
                }
                else
                {
                    output[item] = 1;
                }
            }
            return output;
        }

        //public T MaxValue<T>(IEnumerable<T> input) {

        //    var count = input.Count();

        //    if (count==1)
        //    {
        //        T i = input.First<T>;

        //    }
        //    return (T)1;
        //}

        public int MaxValue(List<int> input)
        {

            var count = input.Count;

            if (count == 1)
            {
                return input.First();

            }
            var op1 = input.First();
            var op2 = MaxValue(input.GetRange(1, input.Count-1));

            if (op1 > op2)
            {
                return op1;
            }
            else {
                return op2;
            }
        }
    }
}
