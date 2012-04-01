using System;
using System.Diagnostics.Contracts;

namespace Merkator.Tools
{
	/// <summary>
	/// Delegate that is used to fill the int array with random data
	/// </summary>
	/// <param name="randomData">the array to fill</param>
	public delegate void RandomIntDataProvider(int[] randomData);
	public delegate void RandomByteDataProvider(byte[] randomData);

	/// <summary>
	/// The default implementation for IRandomGen
	/// </summary>
	public class RandomGen : IRandomGen
	{

		private static readonly DefaultRandomGen _default = new DefaultRandomGen();
		private readonly RandomIntDataProvider _provider;

		protected readonly int[] Buffer;
		protected int Index;
		private uint _bitStore;
		private uint _byteStore;
		private uint _shortStore;
		private double _gaussStore;
		private bool _refilling;


		[ContractInvariantMethod]
		void ObjectInvariant()
		{
			Contract.Invariant(_provider != null);
			Contract.Invariant(Buffer != null);
			Contract.Invariant(Index >= 0);
			Contract.Invariant(Index <= Buffer.Length);
		}

		private void Refill()
		{
			if (_refilling)
				throw new InvalidOperationException("Refill reentered");
			try
			{
				_refilling = true;
				_provider(Buffer);
			}
			finally
			{
				_refilling = false;
			}
		}

		protected void Require(int numberOfInts)
		{
			Contract.Requires(numberOfInts >= 0);
			Contract.Requires(numberOfInts <= Buffer.Length);
			unchecked
			{
				Index -= numberOfInts;
				if (Index < 0)
				{
					Refill();
					Index = Buffer.Length - numberOfInts;
				}
			}
		}

		public double Uniform()
		{
			unchecked
			{
				ulong value = UInt64();
				//chosen so that result < 1 even when value&mask==mask
				const ulong count = 1ul << 53;
				const ulong mask = count - 1;
				const double multiplier = (1 / (double)count); //Exactly representable
				double result = (value & mask) * multiplier;
				return result;
			}
		}

		public double UniformStartEnd(double start, double exclusiveEnd)
		{
			double result;
			do
			{
				// The seemingly redundant cast is necessary to coerce the value to double
				// else the result might have a higher precision

				// ReSharper disable RedundantCast
				result = (double)((exclusiveEnd - start) * Uniform() + start);
				// ReSharper restore RedundantCast
			} while (result >= exclusiveEnd);
			// The loop is necessary, because rounding errors might push the result so it's equal to exclusiveEnd, which would violate the contract
			// But that event is *very* rare
			return result;
		}

		public double UniformStartLength(double start, double length)
		{
			return length * Uniform() + start;
		}

		public float UniformSingle()
		{
			unchecked
			{
				uint value = UInt32();
				//chosen so that result < 1 even when value&mask==mask
				const ulong count = 1ul << 24;
				const ulong mask = count - 1;
				const float multiplier = (1 / (float)count); //Exactly representable
				float result = (value & mask) * multiplier;
				return result;
			}
		}

		public float UniformSingleStartEnd(float start, float exclusiveEnd)
		{
			float result;
			do
			{
				// The seemingly redundant cast is necessary to coerce the value to float
				// else the result might have a higher precision
				// ReSharper disable RedundantCast
				result = (float)((exclusiveEnd - start) * UniformSingle() + start);
				// ReSharper restore RedundantCast
			} while (result >= exclusiveEnd);
			// The loop is necessary, because rounding errors might push the result so it's equal to exclusiveEnd, which would violate the contract
			// But that event is *very* rare
			return result;
		}

		public float UniformSingleStartLength(float start, float length)
		{
			return length * UniformSingle() + start;
		}

		public double Gaussian()
		{
			double localGaussStore = _gaussStore;
			if (!double.IsNaN(localGaussStore))
			{
				_gaussStore = double.NaN;
				return localGaussStore;
			}
			else
			{
				double u = Uniform();
				double v = Uniform();

				double radius = Math.Sqrt(-2 * Math.Log(u));
				double angle = (2 * Math.PI) * v;

				double gauss1 = radius * Math.Cos(angle);
				double gauss2 = radius * Math.Sin(angle);
				_gaussStore = gauss2;
				return gauss1;
			}
		}

		public double Gaussian(double standardDeviation)
		{
			return Gaussian() * standardDeviation;
		}

		public double Gaussian(double mean, double standardDeviation)
		{
			return Gaussian() * standardDeviation + mean;
		}

		public RandomGen(RandomByteDataProvider provider, int bufferSizeInBytes)
		{
			Contract.Requires(bufferSizeInBytes >= 8);
			Contract.Requires(bufferSizeInBytes % 4 == 0);
			Contract.Requires(provider != null);
			var byteBuffer = new byte[bufferSizeInBytes];
			Buffer = new int[bufferSizeInBytes / 4];
			_provider = randomIntData =>
				{
					provider(byteBuffer);
					System.Buffer.BlockCopy(byteBuffer, 0, randomIntData, 0, bufferSizeInBytes);
				};
		}

		public RandomGen(RandomIntDataProvider provider, int bufferSizeInBytes)
		{
			Contract.Requires(bufferSizeInBytes >= 8);
			Contract.Requires(bufferSizeInBytes % 4 == 0);
			Contract.Requires(provider != null);
			_provider = provider;
			Buffer = new int[bufferSizeInBytes / 4];
		}

		public double Exponential()
		{
			double u = Uniform();
			return Math.Log(u);
		}


		public double ExponentialMean(double mean)
		{
			return Exponential() * mean;
		}

		public double ExponentialRate(double rate)
		{
			return Exponential() / rate;
		}

		public int Poisson(double mean)
		{
			throw new NotImplementedException();
		}

		public bool Bool()
		{
			unchecked
			{

				var localBitStore = _bitStore;
				if (localBitStore > 1)
				{
					_bitStore = localBitStore >> 1;
					return (localBitStore & 1) != 0;
				}
				else
				{
					localBitStore = UInt32();
					_bitStore = (localBitStore >> 1) | 0x80000000;
					return (localBitStore & 1) != 0;
				}
			}
		}

		public bool Bool(double probability)
		{
			return Uniform() < probability;
		}

		public sbyte SByte()
		{
			unchecked
			{
				return (sbyte)Byte();
			}
		}

		public byte Byte()
		{
			unchecked
			{
				var localByteStore = _byteStore;
				if (localByteStore >= 0x100)
				{
					_byteStore = localByteStore >> 8;
					return (byte) localByteStore;
				}
				else
				{
					localByteStore = UInt32();
					_byteStore = (localByteStore >> 8) | 0x01000000;
					return (byte) localByteStore;
				}
			}
		}

		public short Int16()
		{
			unchecked
			{
				return (short) UInt16();
			}
		}

		public ushort UInt16()
		{
			unchecked
			{
				var localShortStore = _shortStore;
				if (localShortStore >= 0x10000)
				{
					_shortStore = localShortStore >> 16;
					return (ushort) localShortStore;
				}
				else
				{
					localShortStore = UInt32();
					_shortStore = localShortStore >> 16 | 0x00010000;
					return (ushort) localShortStore;
				}
			}
		}

		public int Int32()
		{
			unchecked
			{
				int index = --Index;
				if (index < 0)
				{
					Refill();
					index = Buffer.Length - 1;
					Index = index;
				}
				return Buffer[index];
			}
		}

		public uint UInt32()
		{
			unchecked
			{
				int index = --Index;
				if (index < 0)
				{
					Refill();
					index = Buffer.Length - 1;
					Index = index;
				}
				return (uint) Buffer[index];
			}
		}

		public Int64 Int64()
		{
			unchecked
			{
				return (long) UInt64();
			}
		}

		public UInt64 UInt64()
		{
			unchecked
			{
				int index = (Index -= 2);
				if (index < 0)
				{
					Refill();
					index = Buffer.Length - 2;
					Index = index;
				}
				return (ulong) (uint) Buffer[index] << 32 | (uint) Buffer[index + 1];
			}
		}

		public void Bytes(byte[] data, int start, int count)
		{
			unchecked
			{
				int byteIndex = Index*sizeof (int);
				while (count > byteIndex)
				{
					System.Buffer.BlockCopy(Buffer, 0, data, start, byteIndex);
					start += byteIndex;
					count -= byteIndex;
					Refill();
					byteIndex = Buffer.Length*sizeof (int);
				}
				byteIndex -= count;
				System.Buffer.BlockCopy(Buffer, byteIndex, data, start, count);
				Index = byteIndex/sizeof (int);
			}
		}

		public void Bytes(byte[] data)
		{
			Bytes(data, 0, data.Length);
		}

		/// <summary>
		/// Returns a threadsafe global instance of `IRandomGen`.
		/// Due to thread safety it's performance will be lower than if you create your own instance
		/// </summary>
		public static IRandomGen Default
		{
			get
			{
				return _default;
			}
		}

		private uint InternalUniformUInt(uint maxResult)
		{
			unchecked
			{
				uint rand;
				uint count = maxResult + 1;

				if (maxResult < 0x100)
				{
					uint usefulCount = (0x100/count)*count;
					do
					{
						rand = Byte();
					} while (rand >= usefulCount);
					return rand%count;
				}
				else if (maxResult < 0x10000)
				{
					uint usefulCount = (0x10000/count)*count;
					do
					{
						rand = UInt16();
					} while (rand >= usefulCount);
					return rand%count;
				}
				else if (maxResult != uint.MaxValue)
				{
					uint usefulCount = (uint.MaxValue/count)*count; //reduces upper bound by 1, to avoid long division
					do
					{
						rand = UInt32();
					} while (rand >= usefulCount);
					return rand%count;
				}
				else
				{
					return UInt32();
				}
			}
		}

		private ulong InternalUniformUInt(ulong maxResult)
		{
			unchecked
			{
				if (maxResult < 0x100000000)
					return InternalUniformUInt((uint) maxResult);
				else if (maxResult < ulong.MaxValue)
				{
					ulong rand;
					ulong count = maxResult + 1;
					ulong usefulCount = (ulong.MaxValue/count)*count; //reduces upper bound by 1, since ulong can't represent any more
					do
					{
						rand = UInt64();
					} while (rand >= usefulCount);
					return rand%count;
				}
				else
					return UInt64();
			}
		}

		public int UniformInt(int count)
		{
			unchecked
			{
				return (int) InternalUniformUInt((uint) count - 1);
			}
		}

		public uint UniformUInt(uint count)
		{
			unchecked
			{
				return InternalUniformUInt(count - 1);
			}
		}

		public long UniformInt(long count)
		{
			unchecked
			{
				return (long) InternalUniformUInt((ulong) count - 1);
			}
		}

		public ulong UniformUInt(ulong count)
		{
			unchecked
			{
				return InternalUniformUInt(count - 1);
			}
		}

		public int UniformIntStartEnd(int start, int inclusiveEnd)
		{
			unchecked
			{
				return start + (int) InternalUniformUInt((uint) inclusiveEnd - (uint) start);
			}
		}

		public int UniformIntStartCount(int start, int count)
		{
			unchecked
			{
				return start + UniformInt(count);
			}
		}

		public long UniformIntStartEnd(long start, long inclusiveEnd)
		{
			unchecked
			{
				return start + (long) InternalUniformUInt((ulong) inclusiveEnd - (ulong) start);
			}
		}

		public long UniformIntStartCount(long start, long count)
		{
			unchecked
			{
				return start + UniformInt(count);
			}
		}

		public int Binomial(int n, double probability)
		{
			unchecked
			{
				int result = 0;
				for (int i = 0; i < n; i++)
				{
					if (Bool(probability))
						result++;
				}
				return result;
			}
		}

		/// <summary>
		/// Builds on RNGCryptoServiceProvider
		/// </summary>
		public static RandomGen CreateRNGCryptoServiceProvider()
		{
			Contract.Ensures(Contract.Result<RandomGen>() != null);
			return new RandomGen(new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes, 1024 * 8);
		}

		/// <summary>
		/// Based on the Well512 PRNG
		/// Fast, but not secure
		/// </summary>
		public static RandomGen CreateWell512()
		{
			Contract.Ensures(Contract.Result<RandomGen>() != null);
			return new RandomGen(new Well512().GenerateInts, 1024);
		}

		public static RandomGen CreateWell512(long seed)
		{
			Contract.Ensures(Contract.Result<RandomGen>() != null);
			return new RandomGen(new Well512(seed).GenerateInts, 1024);
		}

		/// <summary>
		/// cryptographically secure
		/// </summary>
		/// <returns></returns>
		public static RandomGen CreateSecure()
		{
			Contract.Ensures(Contract.Result<RandomGen>() != null);
			return CreateRNGCryptoServiceProvider();
		}

		/// <summary>
		/// Good enough for most simulation needs
		/// </summary>
		public static RandomGen CreateFast()
		{
			Contract.Ensures(Contract.Result<RandomGen>() != null);
			return CreateWell512();
		}

		public static RandomGen CreateFast(long seed)
		{
			Contract.Ensures(Contract.Result<RandomGen>() != null);
			return CreateWell512(seed);
		}

		/// <summary>
		/// Good enough for all simulation needs
		/// But not cryptographically secure
		/// </summary>
		public static RandomGen Create()
		{
			Contract.Ensures(Contract.Result<RandomGen>() != null);
			return CreateRNGCryptoServiceProvider();
		}

		public static RandomGen Create(long seed)
		{
			throw new NotImplementedException();
		}
	}
}
