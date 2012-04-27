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
						throw new InvalidOperationException(
							"Less than one in 4000 taken samples was within the given range for binning. Aborted to prevent endless loop.");
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

		[TestMethod]
		public void PerfTest()
		{
			var rng = RandomGen.CreateFast();
			var buf = new byte[8 * 1024];
			for (int i = 0; i < 100000; i++)
				rng.Bytes(buf);
		}

		[TestMethod]
		public void PerfTest2()
		{
			var rng = RandomGen.Create();
			for (int i = 0; i < 100000000; i++)
				rng.UniformUInt(127);
		}

		[TestMethod]
		public void UniformIntTest1()
		{
			const int samplesPerBucket = 1000;
			UniformIntTest(5, samplesPerBucket);
			UniformIntTest(96, samplesPerBucket);

			UniformIntTest(126, samplesPerBucket);
			UniformIntTest(127, samplesPerBucket);
			UniformIntTest(128, samplesPerBucket);
			UniformIntTest(129, samplesPerBucket);
			UniformIntTest(130, samplesPerBucket);

			UniformIntTest(254, samplesPerBucket);
			UniformIntTest(255, samplesPerBucket);
			UniformIntTest(256, samplesPerBucket);
			UniformIntTest(257, samplesPerBucket);
			UniformIntTest(258, samplesPerBucket);

			UniformIntTest(65534, samplesPerBucket);
			UniformIntTest(65535, samplesPerBucket);
			UniformIntTest(65536, samplesPerBucket);
			UniformIntTest(65537, samplesPerBucket);
			UniformIntTest(65538, samplesPerBucket);
		}

		private static void UniformIntTest(int buckets, int samplesPerBucket)
		{
			var rng = RandomGen.Create();
			var hist = new int[buckets];
			for (int i = 0; i < samplesPerBucket * buckets; i++)
			{
				hist[rng.UniformUInt((uint)buckets)]++;
			}
			var error = hist.Select(n => (long)(n - samplesPerBucket)).Select(delta => delta * delta).Sum()
				* 1.0 / Math.Sqrt(buckets) / samplesPerBucket;
			//Assert.IsTrue(error < 20, buckets + " " + error);
		}
	}
}
