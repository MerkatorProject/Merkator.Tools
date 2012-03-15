using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Merkator.Tools
{
	public static class RandomGenExtensions
	{
		public static void ShuffleInplace<T>(this IList<T> list)
		{
			Contract.Requires<ArgumentNullException>(list != null);
			ShuffleInplace(list, DefaultRandomGen.Instance);
		}

		public static void ShuffleInplace<T>(this IList<T> list, IRandomGen randomGen)
		{
			Contract.Requires<ArgumentNullException>(list != null);
			Contract.Requires<ArgumentNullException>(randomGen != null);
			for (int i = list.Count - 1; i >= 0; i--)//Evaluate list.Count only once
			{
				int otherIndex = randomGen.UniformInt(i + 1);
				T temp = list[i];
				list[i] = list[otherIndex];
				list[otherIndex] = temp;
			}
		}

		public static IEnumerable<T> Shuffle<T>(IEnumerable<T> sequence)
		{
			Contract.Requires<ArgumentNullException>(sequence != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
			return Shuffle(sequence, DefaultRandomGen.Instance);
		}

		public static IEnumerable<T> Shuffle<T>(IEnumerable<T> sequence, IRandomGen randomGen)
		{
			Contract.Requires<ArgumentNullException>(sequence != null);
			Contract.Requires<ArgumentNullException>(randomGen != null);
			Contract.Ensures(Contract.Result<IEnumerable<T>>() != null);
			var list = sequence.ToList();//ToDo: Investigate array vs. List<T>. I assume List has better allocation properties(one large array less), whereas array has faster indexed access
			for (int i = list.Count - 1; i > 0; i--)//Makes index calculations easier
			{
				int otherIndex = randomGen.UniformInt(i + 1);
				yield return list[otherIndex];
				list[otherIndex] = list[i];//We don't need to actually swap, since that part of the list will no longer be used anyways
			}
			yield return list[0];
		}

		public static Random ToSystemRandom(this IRandomGen randomGen)
		{
			Contract.Requires<ArgumentNullException>(randomGen != null);
			Contract.Ensures(Contract.Result<Random>() != null);
			return new SystemRandomWrapper(randomGen);
		}
	}
}
