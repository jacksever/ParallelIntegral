using System;
using System.Threading;

namespace ParallelIntegral
{
	public class Integral
	{
		private readonly Semaphore semaphore = new(1, 1);
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

		public double GetResult => result;

		public void Start()
		{
			new Thread(Calculate).Start(new double[] { left, right });
		}

		private void Calculate(object items)
		{
			semaphore.WaitOne();
			var numbers = (double[])items;

			var left = numbers[0];
			var right = numbers[1];

			var distance = right - left;
			var height = distance / 2;
			var center = left + height;

			if (distance < eps)
			{
				result += function(center) * distance;
			}
			else
			{
				new Thread(Calculate).Start(new double[] { left, center });
				new Thread(Calculate).Start(new double[] { center, right });
			}

			semaphore.Release();
		}
	}
}
