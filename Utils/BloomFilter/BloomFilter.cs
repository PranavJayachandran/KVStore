namespace Utils;

public class BloomFilter
{
    private readonly int size;
    private bool[] bits;
    private readonly List<Func<int, int>> hashFunctions;

    public BloomFilter(int size)
    {
        this.size = size;
        bits = new bool[size];
        hashFunctions = new List<Func<int, int>>
        {
            t => Hash(37, t),
            t => Hash(31, t),
            t => Hash(29, t)
        };
    }

    public void Add(int message)
    {
        foreach (var func in hashFunctions)
        {
            Mark(func(message));
        }
    }

    public void Clear()
    {
      bits = new bool[size];
    }

    public bool Exists(int message)
    {
        return hashFunctions.All(func => IsMarked(func(message)));
    }

    private void Mark(int pos)
    {
        bits[pos % size] = true;
    }

    private bool IsMarked(int pos)
    {
        return bits[pos % size];
    }

    private int Hash(int seed, int val)
    {
        return Math.Abs(seed * val + 13) % size;
    }
}
