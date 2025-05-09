public readonly struct IntegerRange
{
	public int MinimumBound {get;}
	public int MaximumBound {get;}

	public IntegerRange(int minimumBound, int maximumBound)
	{
		MinimumBound = minimumBound;
		MaximumBound = maximumBound;
	}
}