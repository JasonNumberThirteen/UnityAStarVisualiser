public readonly struct PriorityQueueElement<T>
{
	public T Element {get;}
	public float Priority {get;}

	public PriorityQueueElement(T element, float priority)
	{
		Element = element;
		Priority = priority;
	}
}