using System;
using System.Threading;

namespace ParallelIntegral
{
	public class Integral
	{
		private Thread[] threads = new Thread[Environment.ProcessorCount];
		private readonly Func<double, double> function;

		private readonly double left;
		private readonly double right;
		private readonly double eps;
		private double result = 0;

		public Integral(Func<double, double> function, double left, double right, double eps)
		{
			this.function = function;
			this.left = left;
			this.right = right;
			this.eps = eps;
		}

		public double Start()
		{
			var distance = right - left;
			var optimalThreads = distance / Environment.ProcessorCount;

			for (int i = 0; i < threads.Length; i++)
			{
				threads[i] = new Thread(Calculate);
				threads[i].Start(new double[] { i * optimalThreads, (i + 1) * optimalThreads });
			}

			var watch = System.Diagnostics.Stopwatch.StartNew();

			foreach (var item in threads)
				item.Join();

			Console.WriteLine("Время вычисления в многопоточном режиме: " + watch.Elapsed);

			return result;
		}

		private void Calculate(object items)
		{
			var numbers = (double[])items;

			var left = numbers[0];
			var right = numbers[1];

			var distance = right - left;
			var height = distance / 2;
			var center = left + height;

			if (distance < eps)
				result += function(center) * distance;
			else
			{
				Calculate(new double[] { left, center });
				Calculate(new double[] { center, right });
			}
		}

		public double Rectangle(int n)
		{
			var watch = System.Diagnostics.Stopwatch.StartNew();
			var h = (right - left) / n;
			var sum = (function(left) + function(right)) / 2;

			for (var i = 1; i < n; i++)
			{
				var x = left + h * i;
				sum += function(x);
			}

			var result = h * sum;

			Console.WriteLine("Время вычисления в однопоточном режиме: " + watch.Elapsed);
			return result;
		}
	}
}
