using System;
using System.Diagnostics.Contracts;

namespace Merkator.Tools
{
	internal sealed class SystemRandomWrapper : Random
	{
		private readonly IRandomGen _randomGen;

		public SystemRandomWrapper(IRandomGen randomGen)
		{
			Contract.Requires<ArgumentNullException>(randomGen != null);
			_randomGen = randomGen;
		}

		public override int Next()
		{
			return _randomGen.UniformInt(int.MaxValue);//Yes, it can't return int.MaxValue
		}

		public override int Next(int maxValue)
		{
			if (maxValue < 0)
				throw new ArgumentOutOfRangeException("maxValue");
			if (maxValue == 0)
				return 0;
			else
				return _randomGen.UniformInt(maxValue);
		}

		public override int Next(int minValue, int maxValue)
		{
			if (minValue > maxValue)
				throw new ArgumentOutOfRangeException("minValue", "'minValue' cannot be greater than maxValue");
			if (minValue == maxValue)
				return minValue;
			else
				return _randomGen.UniformIntStartEnd(minValue, maxValue - 1);
		}

		public override void NextBytes(byte[] buffer)
		{
			if (buffer == null)
				throw new ArgumentNullException("buffer");
			_randomGen.Bytes(buffer);
		}

		public override double NextDouble()
		{
			return _randomGen.Uniform();
		}

		protected override double Sample()
		{
			return _randomGen.Uniform();
		}
	}
}
