using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace InterfaceImplementationLibrary
{
	public class Point : IEnumerable<double>, IEquatable<Point>
	{
		private int _dimension;

		public int Dimension
		{
			get => _dimension;
			private set
			{
				if (value <= 0)
					throw new ArgumentOutOfRangeException(nameof(Dimension));

				_dimension = value;
			}
		}

		private readonly double[] _data;
		
		public Point(int dimesion = 3)
		{
			Dimension = dimesion;

			var piIsNearE = (3.14).IsNear(2.72);
			
			_data = new double[Dimension];
			
			var random = new Random();
			for (var i = 0; i < Dimension; i++)
			{
				_data[i] = random.NextDouble();
			}
		}

		public IEnumerator<double> GetEnumerator()
		{
			for (var i = 0; i < Dimension; ++i)
				yield return _data[i];
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public bool Equals(Point other) => other
			?.Zip(this, (first, second) => first.IsNear(second))
			.All(x => x)
		    ?? throw new ArgumentNullException(nameof(other));

		public override string ToString() => this.StringJoin("; ", "{", "}");
	}
	
	
	public static class ExtensionMethods
	{
		public static bool IsNear(this double x, double y, double eps = double.Epsilon) => Math.Abs(x - y) <= eps;

		public static string StringJoin<T>(
			this IEnumerable<T> collection,
			string separator,
			string left = null,
			string right = null) => $"{left}{string.Join(separator, collection)}{right}";
	}
}