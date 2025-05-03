using System.Linq;
using System.Collections.Generic;

public class PriorityQueue<T>
{
	private List<PriorityQueueElement<T>> list = new();

	public int Count => list.Count;
	public T Dequeue() => list.PopFirst().Element;

	public void Clear()
	{
		list.Clear();
	}

	public void Enqueue(PriorityQueueElement<T> priorityQueueElement)
	{
		list.Add(priorityQueueElement);
		
		list = list.OrderBy(element => element.Priority).ToList();
	}
}