using System;
using System.Collections.Generic;
using System.Linq;

namespace linq_slideviews;

public static class ExtensionsTask
{
	/// <summary>
	/// Медиана списка из нечетного количества элементов — это серединный элемент списка после сортировки.
	/// Медиана списка из четного количества элементов — это среднее арифметическое 
    /// двух серединных элементов списка после сортировки.
	/// </summary>
	/// <exception cref="InvalidOperationException">Если последовательность не содержит элементов</exception>
	public static double Median(this IEnumerable<double> items)
	{
		var sortedList = items
			.OrderBy(x => x)
			.ToList();
		var count = sortedList.Count;
		if (count == 0) throw new InvalidOperationException();
		return (count % 2 == 0) ? (sortedList[count / 2] + sortedList[count / 2 - 1]) / 2 : sortedList[count / 2];
	}

	/// <returns>
	/// Возвращает последовательность, состоящую из пар соседних элементов.
	/// Например, по последовательности {1,2,3} метод должен вернуть две пары: (1,2) и (2,3).
	/// </returns>
	public static IEnumerable<(T First, T Second)> Bigrams<T>(this IEnumerable<T> items)
	{
		var q = new Queue<T>();
		foreach (var item in items)
		{
			var second = item;
			q.Enqueue(item);			
			if (q.Count == 1) continue;			
			var first = q.Dequeue();
			yield return (first, second);
		}
	}
}