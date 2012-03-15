using System;
using System.Diagnostics.Contracts;

namespace Merkator.Tools
{
	//http://stackoverflow.com/a/1227137
	internal class Well512
	{
		public Well512()
		{
			_rngstate = new UInt32[16];
			for (int i = 0; i < 16; i++)
				_rngstate[i] = RandomGen.Default.UInt32();
		}

		[ContractInvariantMethod]
		void ObjectInvariant()
		{
			Contract.Invariant(_rngstate != null);
			Contract.Invariant(_rngstate.Length == 16);
			Contract.Invariant(_index >= 0);
			Contract.Invariant(_index < 16);
		}

		public Well512(long seed)
		{
			_rngstate = new UInt32[16];
			var seedBytes = BitConverter.GetBytes(seed);
			using (var sha512 = System.Security.Cryptography.SHA512.Create())
			{
				byte[] initialBytes = sha512.ComputeHash(seedBytes);
				for (int i = 0; i < 16; i++)
					_rngstate[i] = BitConverter.ToUInt32(initialBytes, 4 * i);
			}
		}

		/* initialize state to random bits */
		readonly UInt32[] _rngstate;
		/* init should also reset this to 0 */
		int _index;

		public UInt32 GenerateUInt32()
		{
			uint a = _rngstate[_index];
			uint c = _rngstate[(_index + 13) & 15];
			uint b = a ^ c ^ (a << 16) ^ (c << 15);
			c = _rngstate[(_index + 9) & 15];
			c ^= (c >> 11);
			a = _rngstate[_index] = b ^ c;
			uint d = a ^ ((a << 5) & 0xDA442D20u);
			_index = (_index + 15) & 15;
			a = _rngstate[_index];
			_rngstate[_index] = a ^ b ^ d ^ (a << 2) ^ (b << 18) ^ (c << 28);
			return _rngstate[_index];
		}

		public void GenerateInts(int[] randomData)
		{
			Contract.Requires(randomData != null);
			for (int i = 0; i < randomData.Length; i++)
				randomData[i] = (int)GenerateUInt32();
		}
	}
}
