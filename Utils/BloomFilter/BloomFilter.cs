namespace Utils;

public class BloomFilter(int size){
  private readonly bool[] bits = new bool[size];
  private readonly static List<Func<string,int>> hashFunctions = new List<Func<string,int>>{
      t => Hash(37,t),
      t => Hash(31,t),
      t => Hash(29,t)
  };
  public void Add(string message){
    hashFunctions.Select(func => func(message)).ToList().ForEach(Mark);
  }
  public bool Exists(string message){
    return hashFunctions.Select(func => func(message)).All(IsMarked);
  }
  private void Mark(int pos){
    bits[pos%size] = true;
  }
  private bool IsMarked(int pos){
    return bits[pos%size];
  }
  private static int Hash(int seed, string val){
    int hash = 0;
    int prod = 1;
    for(int i = val.Length - 1; i >= 0; i--){
      hash += val[i] * prod;
      prod *= 10;
    }
    return hash%seed;
  }
}
