using System;

class Program
{
    static double Function(double x) => Math.Pow(x, 6) + 2 * Math.Pow(x, 5) + 3 * Math.Pow(x, 4) - 3;

    static double DerivativeFunction(double x) => 6 * Math.Pow(x, 5) + 10 * Math.Pow(x, 4) + 12 * Math.Pow(x, 3);

    static double SecondDerivativeFunction(double x) => 30 * Math.Pow(x, 4) + 40 * Math.Pow(x, 3) + 36 * Math.Pow(x, 2);

    static (double a, double b, double c)[] QuadraticSpline()
    {
        double[] x = [3, 5, 7];

        int n = x.Length - 1;
        var splines = new (double a, double b, double c)[n];

        splines[0].a = Function(x[0]);
        splines[0].b = DerivativeFunction(x[0]);
        splines[0].c = (Function(x[1]) - splines[0].a - 2 * splines[0].b) / 4;

        splines[1].a = Function(x[1]);
        splines[1].b = splines[0].b + 4 * splines[0].c;
        splines[1].c = (Function(x[2]) - splines[1].a - 2 * splines[1].b) / 4;

        return splines;
    }

    static void PrintQuadraticSpline((double a, double b, double c)[] splines)
    {
        double[] x = [3, 5, 7];

        for (int i = 0; i < splines.Length; i++)
        {
            Console.WriteLine($"[{x[i]}, {x[i + 1]}]: P{i + 1}(x) : P{i + 1}(x) = {splines[i].a} + {splines[i].b} * (x - {x[i]}) + {splines[i].c} * (x - {x[i]})^2");
        }
    }

    static void CubicSpline()
    {
        double[] x = { 3, 5, 7 };
        double[] y = { Function(x[0]), Function(x[1]), Function(x[2]) };

        double f_dd_3 = SecondDerivativeFunction(3);
        double f_dd_7 = SecondDerivativeFunction(7);

        double h = 2;
        double[] a = { y[0], y[1], y[2] };
        double[] alpha = { 3 * (a[1] - a[0]) / h - f_dd_3 / 2, 3 * (a[2] - a[1]) / h - f_dd_7 / 2 };

        double[] c = new double[3];
        c[0] = f_dd_3 / 2;
        c[2] = f_dd_7 / 2;

        double[] b = new double[2];
        double[] d = new double[2];

        c[1] = (alpha[1] - alpha[0]) / (2 * (h + h));
        b[0] = (a[1] - a[0]) / h - h * (2 * c[0] + c[1]) / 3;
        b[1] = (a[2] - a[1]) / h - h * (2 * c[1] + c[2]) / 3;
        d[0] = (c[1] - c[0]) / h;
        d[1] = (c[2] - c[1]) / h;

        Console.WriteLine("\nCubic spline on intervals:");
        Console.WriteLine($"[3, 5]: S0(x) = {a[0]} + {b[0]} * (x - 3) + {c[0]} * (x - 3)^2 + {d[0]} * (x - 3)^3");
        Console.WriteLine($"[5, 7]: S1(x) = {a[1]} + {b[1]} * (x - 5) + {c[1]} * (x - 5)^2 + {d[1]} * (x - 5)^3");
    }

    static void LinearSpline()
    {
        double start = 3.0;
        double end = 7.0;
        double step = 0.5;

        int n = (int)((end - start) / step) + 1;
        double[] x = new double[n];
        double[] y = new double[n];

        for (int i = 0; i < n; i++)
        {
            x[i] = start + i * step;
            y[i] = Function(x[i]);
        }

        Console.WriteLine("\nLinear spline on intervals: ");
        for (int i = 0; i < n - 1; i++)
        {
            double a = y[i];
            double b = (y[i + 1] - y[i]) / (x[i + 1] - x[i]);
            Console.WriteLine($"[{x[i]}, {x[i + 1]}]: P{i + 1}(x) = {a} + {b} * (x - {x[i]})");
        }
    }

    static void Main()
    {
        var splines = QuadraticSpline();
        Console.WriteLine("Quadratic spline:");
        PrintQuadraticSpline(splines);

        CubicSpline();

        LinearSpline();
    }
}