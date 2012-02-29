using System;
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
			int[] inputCopy = input.Reverse().ToArray();
			RandomIntDataProvider provider = (buf) =>
				{
					if (inputCopy == null)
						throw new InvalidOperationException("Read beyond end of dummy provider");
					Contract.Assert(buf.Length == inputCopy.Length);
					Array.Copy(inputCopy, buf, inputCopy.Length);
					inputCopy = null;
				};
			return new RandomGen(provider, inputCopy.Length * sizeof(int));
		}
	}
}
