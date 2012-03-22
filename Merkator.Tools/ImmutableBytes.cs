using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Merkator.Tools
{
	public struct ImmutableBytes : IEquatable<ImmutableBytes>, IList<byte>
	{
		private readonly byte[] _data;

		public bool IsNull
		{
			get { return _data == null; }
		}

		public static ImmutableBytes Null
		{
			get { return default(ImmutableBytes); }
		}

		public ImmutableBytes(byte[] data, int start, int length)
		{
			_data = new byte[length];
			Array.Copy(data, start, _data, 0, length);
		}

		public ImmutableBytes(byte[] data)
			: this(data, 0, data.Length)
		{
		}

		public bool Equals(ImmutableBytes other)
		{
			return (_data == other._data) ||
				((_data.Length == other._data.Length) && _data.SequenceEqual(other._data));
		}

		public override string ToString()
		{
			return ToHex();
		}

		public override bool Equals(object obj)
		{
			return (obj != null) && Equals((ImmutableBytes)obj);
		}

		public override int GetHashCode()
		{
			if (_data == null)
				return 0;
			unchecked
			{
				int result = _data.Length;
				for (int i = 0; i < _data.Length; i++)
				{
					result = result * 1051 + _data[i];
				}
				return result;
			}
		}

		public void CopyTo(byte[] array, int arrayIndex)
		{
			_data.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return _data.Length; }
		}

		public byte this[int index]
		{
			get { return _data[index]; }
		}

		#region IList<byte> Members

		int IList<byte>.IndexOf(byte item)
		{
			return ((IList<byte>)_data).IndexOf(item);
		}

		void IList<byte>.Insert(int index, byte item)
		{
			throw new NotSupportedException();
		}

		void IList<byte>.RemoveAt(int index)
		{
			throw new NotSupportedException();
		}

		byte IList<byte>.this[int index]
		{
			get { return _data[index]; }
			set { throw new NotSupportedException(); }
		}

		void ICollection<byte>.Add(byte item)
		{
			throw new NotSupportedException();
		}

		void ICollection<byte>.Clear()
		{
			throw new NotSupportedException();
		}

		bool ICollection<byte>.Contains(byte item)
		{
			throw new NotSupportedException();
		}

		bool ICollection<byte>.IsReadOnly { get { return true; } }

		bool ICollection<byte>.Remove(byte item)
		{
			throw new NotSupportedException();
		}

		public IEnumerator<byte> GetEnumerator()
		{
			return ((IEnumerable<byte>)_data).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _data.GetEnumerator();
		}

		#endregion

		public string ToBase32()
		{
			throw new NotImplementedException();
		}

		public string ToHex()
		{
			if (_data == null)
				return "<null>";
			else
				return BitConverter.ToString(_data).Replace("-", "");
		}

		public static bool operator ==(ImmutableBytes b1, ImmutableBytes b2)
		{
			return b1.Equals(b2);
		}
		public static bool operator !=(ImmutableBytes b1, ImmutableBytes b2)
		{
			return !(b1 == b2);
		}
	}
}