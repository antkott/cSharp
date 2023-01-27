public class ThreadTest8
{

    static int i;

    static void f(Object num) {
        lock ((Object)i) {
            for (int j = 0; j < 3; j++)
            {
                Console.Write(num);
            }
        }
    }

    public static void Main()
    {
        Thread t1 = new Thread(() => ThreadTest8.f(1));
        Thread t2 = new Thread(() => ThreadTest8.f(2));

        t1.Start(); t2.Start();

       t1.Join(); t2.Join();

    }
}
