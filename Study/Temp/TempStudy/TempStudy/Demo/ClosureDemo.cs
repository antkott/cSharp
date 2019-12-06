using System;
using System.Collections.Generic;
using System.Linq;
using StudyDemo.Demo;

namespace StudyDemo
{
    public class ClosureDemo : DemoRoot
    {

        private List<Action> actions = new List<Action>();

        public void RunDemo() {
            base.ShowRunDemoInformation();
            this.RunDemo1();
            this.RunDemo2();
        }

        private void RunDemo1()
        {
            foreach (var i in Enumerable.Range(1, 3))
            {
                actions.Add(() => Console.WriteLine(i));
            }

            foreach (var action in this.actions)
            {
                action();
            }
        }

        private void RunDemo2()
        {
            var funcs = new List<Func<int>>();

            for (int i = 0; i < 3; i++)
            {
                //Func<int> item = delegate {
                //    return i;
                //};
                //funcs.Add(item);

                funcs.Add(() => i);
            }

            foreach (var f in funcs)
            {
                Console.WriteLine(f());
            }
        }
    }
}
