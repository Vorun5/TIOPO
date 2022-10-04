using System;

namespace triangle
{
    class Triangle
    { 
        private double a;
        private double b;
        private double c;
        private String unknownError = "неизвестная ошибка";
        private double max = 1000;
        public Triangle(string[] args)
        {
            if (args.Length != 3) {
                throw new Exception(unknownError);
            }
            a = ToInt(args[0]);
            b = ToInt(args[1]);
            c = ToInt(args[2]);
        }
        private double ToInt(String str)
        {
            double num;
            try
            { 
                num = double.Parse(str);
            } catch 
            {
                throw new Exception(unknownError);
            }
            if (num > max)
            {
                throw new Exception(unknownError);
            }
            return num;
        }
        public String TriangleType()
        {
            if (a + b <= c || a + c <= b || b + c <= a)
            {
                return "не треугольник";
            }
            if (a != b && a != c && b != c)
            {
                return "обычный";
            }
            else {
                if (a == b && b == c)
                {
                    return "равносторонний"; 
                }
                else
                { 
                    return "равнобедренный"; 
                }
            }
        }

    }
}