namespace Core;

public class StoredData : IComparable<StoredData>{
  public int Key {get; init;}
  public string Val {get; init;}
  public bool IsDelete {get; init;}

  public int CompareTo(StoredData sd){
    return Key.CompareTo(sd.Key);
  }
}
