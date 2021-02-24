
namespace CodeTrySimulation
{
    public static class PiApprox
    {
        public static double Approx(Point[] pts)
        {
            int into = 0;
            foreach (Point p in pts)
            {
                if (p.x * p.x + p.y * p.y <= 1) into++;
            }
            // pi / 4 = into / n
            return into / (double)(pts.Length) * 4d;
        }
    }

    public class Point
    {
        public double x, y;
    }
}
