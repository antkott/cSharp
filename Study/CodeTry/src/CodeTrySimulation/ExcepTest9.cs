public class ExcepTest7
{

    public readonly int VersionNo;

    public ExcepTest7()
    {
        VersionNo = 23;
        throw new Exception();
    }

    public static void Main()
    {
        //ExcepTest7 myProgram = null;
        //try
        //{

        //    myProgram = new();
        //}
        //catch (Exception ex)
        //{
        //    Console.WriteLine("1");
        //}
        //Console.WriteLine(myProgram.VersionNo);

        Object o1 = new Object();
        B b1 = new B();
        B b2 = new D();
        Object o2 = b2;
        D d1 = (D) b1;
        B b3 = (B) o1;

    }
}

class B { };
class D: B { };
