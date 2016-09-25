public class ObjectContainer <T> {

	private T item;

	public bool Used {get; private set;}

	public void SetUsed()
	{
		Used = true;
	}

	public T Item
	{
		get
		{
			return item;
		}
		set
		{
			item = value;
		}
	}

	public void SetFree()
	{
		Used = false;
	}
	
}
