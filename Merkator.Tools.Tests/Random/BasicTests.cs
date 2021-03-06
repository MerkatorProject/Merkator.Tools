﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Merkator.Tools.Tests
{
	[TestClass]
	public class RandomGenTest
	{
		public RandomGenTest()
		{

		}

		private static bool IsJustLess(double expected, double actual)
		{
			return (actual < expected) && (actual > expected - 1E15);
		}

		private static bool IsJustLess(float expected, float actual)
		{
			return (actual < expected) && (actual > expected - 1E7);
		}

		private TestContext testContextInstance;

		/// <summary>
		///Gets or sets the test context which provides
		///information about and functionality for the current test run.
		///</summary>
		public TestContext TestContext
		{
			get
			{
				return testContextInstance;
			}
			set
			{
				testContextInstance = value;
			}
		}

		#region Additional test attributes
		//
		// You can use the following additional attributes as you write your tests:
		//
		// Use ClassInitialize to run code before running the first test in the class
		// [ClassInitialize()]
		// public static void MyClassInitialize(TestContext testContext) { }
		//
		// Use ClassCleanup to run code after all tests in a class have run
		// [ClassCleanup()]
		// public static void MyClassCleanup() { }
		//
		// Use TestInitialize to run code before running each test 
		// [TestInitialize()]
		// public void MyTestInitialize() { }
		//
		// Use TestCleanup to run code after each test has run
		// [TestCleanup()]
		// public void MyTestCleanup() { }
		//
		#endregion

		[TestMethod]
		public void Int32()
		{
			var rng = DummyProvider.CreateFromInts(1, 2, 3, 4, 5);
			Assert.AreEqual(1, rng.Int32());
			Assert.AreEqual(2, rng.Int32());
			Assert.AreEqual(3, rng.Int32());
			Assert.AreEqual(4, rng.Int32());
			Assert.AreEqual(5, rng.Int32());
		}

		[TestMethod]
		public void Uniform0()
		{
			var rng = DummyProvider.CreateFromInts(0, 0);
			Assert.AreEqual(0.0, rng.Uniform());
		}

		[TestMethod]
		public void Uniform1()
		{
			var rng = DummyProvider.CreateFromInts(-1, -1);
			Assert.IsTrue(IsJustLess(1, rng.Uniform()));
		}

		[TestMethod]
		public void UniformSingle0()
		{
			var rng = DummyProvider.CreateFromInts(0, 0x39393939);
			Assert.AreEqual(0.0, rng.UniformSingle());
		}

		[TestMethod]
		public void UniformSingle1()
		{
			var rng = DummyProvider.CreateFromInts(-1, 0x39393939);
			Assert.IsTrue(IsJustLess(1, rng.UniformSingle()));
		}

		[TestMethod]
		public void UniformStartEnd()
		{
			var rng = DummyProvider.CreateFromInts(0, 0, -1, -1);
			var n1 = rng.UniformStartEnd(1, 4);
			var n2 = rng.UniformStartEnd(1, 4);
			Assert.AreEqual(1, n1);
			Assert.IsTrue(IsJustLess(4, n2));
		}

		[TestMethod]
		public void UniformStartLength()
		{
			var rng = DummyProvider.CreateFromInts(0, 0, -1, -1);
			var n1 = rng.UniformStartLength(1, 3);
			var n2 = rng.UniformStartLength(1, 3);
			Assert.AreEqual(1, n1);
			Assert.IsTrue(IsJustLess(4, n2));
		}

		[TestMethod]
		public void UniformSingleStartEnd()
		{
			var rng = DummyProvider.CreateFromInts(0, 0, -1, -1);
			var n1 = rng.UniformSingleStartEnd(1, 4);
			var n2 = rng.UniformSingleStartEnd(1, 4);
			Assert.AreEqual(1, n1);
			Assert.IsTrue(IsJustLess(4, n2));
		}

		[TestMethod]
		public void UniformSingleStartLength()
		{
			var rng = DummyProvider.CreateFromInts(0, 0, -1, -1);
			var n1 = rng.UniformSingleStartLength(1, 3);
			var n2 = rng.UniformSingleStartLength(1, 3);
			Assert.AreEqual(1, n1);
			Assert.IsTrue(IsJustLess(4, n2));
		}

		[TestMethod]
		public void Bool()
		{
			for (int i = 0; i < 64; i++)
			{
				int n1, n2;
				if (i < 32)
				{
					n1 = 1 << i;
					n2 = 0;
				}
				else
				{
					n1 = 0;
					n2 = 1 << (i - 32);
				}
				var rng = DummyProvider.CreateFromInts(n1, n2);
				var bools = Enumerable.Range(0, 64).Select(_ => rng.Bool()).ToArray();
				var expectedBools = Enumerable.Range(0, 64).Select(j => i == j);
				Assert.IsTrue(expectedBools.SequenceEqual(bools));
			}
		}

		[TestMethod]
		public void Byte()
		{
			var rng = DummyProvider.CreateFromInts(0x03020100, 0x07060504);
			var bytes = Enumerable.Range(0, 8).Select(_ => rng.Byte()).ToArray();
			Assert.IsTrue(bytes.SequenceEqual(Enumerable.Range(0, 8).Select(i => (byte)i)));
		}

		[TestMethod]
		public void UInt16()
		{
			var rng = DummyProvider.CreateFromInts(0x00010000, 0x00030002, 0x00050004, 0x00070006);
			var shorts = Enumerable.Range(0, 8).Select(_ => rng.UInt16()).ToArray();
			Assert.IsTrue(shorts.SequenceEqual(Enumerable.Range(0, 8).Select(i => (ushort)i)));
		}

		[TestMethod]
		public void UInt64()
		{
			var rng = DummyProvider.CreateFromInts(0x03020100, 0x07060504, 0x0B0A0908, 0x0F0E0D0C);
			UInt64 n1 = rng.UInt64();
			UInt64 n2 = rng.UInt64();
			Assert.AreEqual(0x0706050403020100ul, n1);
			Assert.AreEqual(0x0F0E0D0C0B0A0908ul, n2);
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentNullException))]
		public void ArgumentCheckingIsOn()
		{
			RandomGen.Default.Bytes(null);
		}
	}
}
