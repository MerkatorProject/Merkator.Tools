using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Merkator.Tools;

namespace Merkator.Tools.Tests.Random
{
	[TestClass]
	public class StatisticalTests
	{
		/// <summary>
		/// Samples a double-generator to bins within a given range. Results outside the range will be ignored. Will throw an exception if more than 4096 values lie outside the range per value inside it.
		/// </summary>
		/// <param name="nbins"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="generator"></param>
		/// <param name="samples">Total amount of samples taken inside the given range</param>
		/// <returns></returns>
		internal ulong[] FillToBins(uint nbins, double min, double max, Func<double> generator, ulong samples)
		{
			ulong[] bins = new ulong[nbins];
			ulong hits = 0, fails = 0;

			while (hits < samples)
			{
				double random = generator();
				int n = (int)((random - min) / (max - min) * nbins);
				if (n >= 0 && n < bins.Length)
				{
					bins[n]++;
					hits++;
				}
				else
				{
					fails++;
					if (fails > 0xFFF && fails > 0x1000 * hits)
						throw new InvalidOperationException("Less than one in 4000 taken samples was within the given range for binning. Aborted to prevent endless loop.");
				}
			}

			return bins;
		}

		[TestMethod]
		public void TestUniform23Bins()
		{
			// lol this should pass... unless it's off by 4 sigma, ahaha.
			ulong samples = 8000000;
			uint binCount = 23;
			ulong e = samples / binCount;

			ulong[] bins = FillToBins(binCount, 0.0, 1.0, RandomGen.Default.Uniform, samples);

			foreach (ulong n in bins)
				Assert.IsTrue(Math.Abs((long)(n - e)) < 4 * Math.Sqrt(e));
		}
	}
}
