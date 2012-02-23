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
		public void BasicIntTest()
		{
			var rng=DummyProvider.CreateFromInts(1, 2, 3, 4, 5);
			Assert.AreEqual(1, rng.Int32());
			Assert.AreEqual(2, rng.Int32());
			Assert.AreEqual(3, rng.Int32());
			Assert.AreEqual(4, rng.Int32());
			Assert.AreEqual(5, rng.Int32());
		}
	}
}
