﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Merkator.Tools;
using System.Diagnostics.Contracts;

namespace Merkator.Tools.Tests
{
	class DummyProvider
	{
		public static RandomGen CreateFromInts(params int[] input)
		{
			int[] inputCopy = (int[])input.Clone();
			RandomIntDataProvider provider = (buf) =>
				{
					Contract.Assert(buf.Length == inputCopy.Length);
					Array.Copy(inputCopy, buf, inputCopy.Length);
				};
			return new RandomGen(provider, inputCopy.Length * sizeof(int));
		}
	}
}
