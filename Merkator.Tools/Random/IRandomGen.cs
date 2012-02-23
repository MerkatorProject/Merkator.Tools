using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics.Contracts;
using System.Diagnostics;

namespace Merkator.Tools
{
	[ContractClass(typeof(ContractForIRandomGen))]
	public interface IRandomGen
	{
		double Uniform();
		double UniformStartEnd(double start, double exclusiveEnd);
		double UniformStartLength(double start, double length);

		float UniformSingle();
		float UniformSingleStartEnd(float start, float exclusiveEnd);
		float UniformSingleStartLength(float start, float length);

		double Gaussian();
		double Gaussian(double standardDeviation);
		double Gaussian(double mean, double standardDeviation);

		double Exponential();
		double ExponentialMean(double mean);
		double ExponentialRate(double rate);

		bool Bool();
		bool Bool(double probability);

		SByte SByte();
		Byte Byte();
		Int16 Int16();
		UInt16 UInt16();
		Int32 Int32();
		UInt32 UInt32();
		Int64 Int64();
		UInt64 UInt64();

		int UniformInt(int count);
		int UniformIntStartEnd(int start, int inclusiveEnd);
		int UniformIntStartCount(int start, int count);

		long UniformInt(long count);
		long UniformIntStartEnd(long start, long inclusiveEnd);
		long UniformIntStartCount(long start, long count);

		uint UniformUInt(uint count);
		ulong UniformUInt(ulong count);

		int Poisson(double mean);
		int Binomial(int n, double probability);

		void Bytes(byte[] data, int start, int count);
	}

	[ContractClassFor(typeof(IRandomGen))]
	internal abstract class ContractForIRandomGen : IRandomGen
	{
		double IRandomGen.Uniform()
		{
			Contract.Ensures(Contract.Result<double>() >= 0);
			Contract.Ensures(Contract.Result<double>() < 1);
			throw new Exception();
		}

		double IRandomGen.UniformStartEnd(double start, double exclusiveEnd)
		{
			Contract.Requires(start < exclusiveEnd);
			Contract.Requires(start > double.NegativeInfinity);
			Contract.Requires(exclusiveEnd < double.PositiveInfinity);
			Contract.Ensures(Contract.Result<double>() >= start);
			Contract.Ensures(Contract.Result<double>() < exclusiveEnd);
			throw new Exception();
		}

		double IRandomGen.UniformStartLength(double start, double length)
		{
			Contract.Requires(length > 0);
			Contract.Requires(start > double.NegativeInfinity);
			Contract.Requires(length < double.PositiveInfinity);
			Contract.Ensures(Contract.Result<double>() >= start);
			Contract.Ensures(Contract.Result<double>() < start + length);
			throw new Exception();
		}

		float IRandomGen.UniformSingle()
		{
			Contract.Ensures(Contract.Result<float>() >= 0);
			Contract.Ensures(Contract.Result<float>() < 1);
			throw new Exception();
		}

		float IRandomGen.UniformSingleStartEnd(float start, float exclusiveEnd)
		{
			Contract.Requires(start < exclusiveEnd);
			Contract.Requires(start > float.NegativeInfinity);
			Contract.Requires(exclusiveEnd < float.PositiveInfinity);
			Contract.Ensures(Contract.Result<float>() >= start);
			Contract.Ensures(Contract.Result<float>() < exclusiveEnd);
			throw new Exception();
		}

		float IRandomGen.UniformSingleStartLength(float start, float length)
		{
			Contract.Requires(length > 0);
			Contract.Requires(start > float.NegativeInfinity);
			Contract.Requires(length < float.PositiveInfinity);
			Contract.Ensures(Contract.Result<float>() >= start);
			Contract.Ensures(Contract.Result<float>() < start + length);
			throw new Exception();
		}

		double IRandomGen.Gaussian()
		{
			throw new Exception();
		}

		double IRandomGen.Gaussian(double standardDeviation)
		{
			Contract.Requires(standardDeviation >= 0);
			Contract.Requires(standardDeviation < double.PositiveInfinity);
			throw new Exception();
		}

		double IRandomGen.Gaussian(double mean, double standardDeviation)
		{
			Contract.Requires(mean > double.NegativeInfinity);
			Contract.Requires(mean < double.PositiveInfinity);
			Contract.Requires(standardDeviation >= 0);
			Contract.Requires(standardDeviation < double.PositiveInfinity);
			throw new Exception();
		}

		double IRandomGen.Exponential()
		{
			Contract.Ensures(Contract.Result<double>() >= 0);
			throw new Exception();
		}

		double IRandomGen.ExponentialMean(double mean)
		{
			Contract.Requires(mean >= 0);
			Contract.Ensures(Contract.Result<double>() >= 0);
			throw new Exception();
		}

		double IRandomGen.ExponentialRate(double rate)
		{
			Contract.Requires(rate >= 0);
			Contract.Ensures(Contract.Result<double>() >= 0);
			throw new Exception();
		}

		int IRandomGen.Poisson(double mean)
		{
			Contract.Requires(mean >= 0);
			Contract.Ensures(Contract.Result<int>() >= 0);
			throw new Exception();
		}

		bool IRandomGen.Bool()
		{
			throw new Exception();
		}

		bool IRandomGen.Bool(double probability)
		{
			Contract.Requires(probability >= 0);
			Contract.Requires(probability <= 1);
			throw new Exception();
		}

		sbyte IRandomGen.SByte()
		{
			throw new Exception();
		}

		byte IRandomGen.Byte()
		{
			throw new Exception();
		}

		short IRandomGen.Int16()
		{
			throw new Exception();
		}

		ushort IRandomGen.UInt16()
		{
			throw new Exception();
		}

		int IRandomGen.Int32()
		{
			throw new Exception();
		}

		uint IRandomGen.UInt32()
		{
			throw new Exception();
		}

		long IRandomGen.Int64()
		{
			throw new Exception();
		}

		ulong IRandomGen.UInt64()
		{
			throw new Exception();
		}

		int IRandomGen.UniformInt(int count)
		{
			Contract.Requires(count >= 1);
			Contract.Ensures(Contract.Result<int>() >= 0);
			Contract.Ensures(Contract.Result<int>() < count);
			throw new Exception();
		}

		int IRandomGen.UniformIntStartEnd(int start, int inclusiveEnd)
		{
			Contract.Requires(inclusiveEnd >= start);
			Contract.Ensures(Contract.Result<int>() >= start);
			Contract.Ensures(Contract.Result<int>() <= inclusiveEnd);
			throw new Exception();
		}

		int IRandomGen.UniformIntStartCount(int start, int count)
		{
			Contract.Requires(count >= 1);
			Contract.Requires(start + count >= start);//No int overflow
			Contract.Ensures(Contract.Result<int>() >= start);
			Contract.Ensures(Contract.Result<int>() < start + count);
			throw new Exception();
		}

		long IRandomGen.UniformInt(long count)
		{
			Contract.Requires(count >= 1);
			Contract.Ensures(Contract.Result<long>() >= 0);
			Contract.Ensures(Contract.Result<long>() < count);
			throw new Exception();
		}

		long IRandomGen.UniformIntStartEnd(long start, long inclusiveEnd)
		{
			Contract.Requires(inclusiveEnd >= start);
			Contract.Ensures(Contract.Result<long>() >= start);
			Contract.Ensures(Contract.Result<long>() <= inclusiveEnd);
			throw new Exception();
		}

		long IRandomGen.UniformIntStartCount(long start, long count)
		{
			Contract.Requires(count >= 1);
			Contract.Requires(start + count >= start);//No int overflow
			Contract.Ensures(Contract.Result<long>() >= start);
			Contract.Ensures(Contract.Result<long>() < start + count);
			throw new Exception();
		}

		int IRandomGen.Binomial(int n, double probability)
		{
			Contract.Requires(n >= 0);
			Contract.Requires(probability >= 0);
			Contract.Requires(probability <= 1);
			Contract.Ensures(Contract.Result<int>() >= 0);
			Contract.Ensures(Contract.Result<int>() <= n);
			throw new Exception();
		}

		void IRandomGen.Bytes(byte[] data, int start, int count)
		{
			Contract.Requires(data != null);
			Contract.Requires(start >= 0);
			Contract.Requires(count >= 0);
			Contract.Requires(start + count <= data.Length);
			Contract.Requires(start + count >= start);//No int overflow
			throw new Exception();
		}


		public uint UniformUInt(uint count)
		{
			Contract.Ensures(Contract.Result<uint>() < count);
			throw new Exception();
		}

		public ulong UniformUInt(ulong count)
		{
			Contract.Ensures(Contract.Result<ulong>() < count);
			throw new Exception();
		}
	}
}
