using System;
using System.Threading;

namespace ParallelIntegral
{
	public class Program
	{
		public static void Main()
		{
			Console.WriteLine($"Интеграл x / Sqrt(x^4 + 16), промежуток от 0 до Sqrt(3)");

			Integral integral = new(x => x / Math.Sqrt(Math.Pow(x, 4)+ 16), 0, Math.Sqrt(3), 0.001);
			integral.Start();
			Thread.Sleep(2000);
			Console.WriteLine($"Интеграл равен: {integral.GetResult}");
			Console.ReadKey();
		}
	}
}
