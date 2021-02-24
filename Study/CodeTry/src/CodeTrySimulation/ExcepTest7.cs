public class ExcepTest9
{

    public static void Main()
    {
        try {
            using (A a = new A())
            {
                a.Do();
            }
        
        }        
        catch (Exception e) {
            Console.WriteLine(e.Message);
        }

    }
}

class A : IDisposable
{
    public A()
    {
        throw new Exception("A::Exception");
    }
    public void Do()
    {
        Console.WriteLine("A::Do()");
    }
    public void Dispose()
    {
        Console.WriteLine("A::Dispose()");
    }
}
