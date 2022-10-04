using System;

namespace triangle
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                var t = new Triangle(args);
                Console.Write(t.TriangleType());
            } catch(Exception e)
            {
                Console.Write(e.Message);
            }
        }
    }
}
