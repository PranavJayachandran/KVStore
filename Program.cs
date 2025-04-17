using Core;
using Utils;

public class NoSql{
  public static void Main(){
    Memtable memtable = new Memtable(new RollingHash(20));
    memtable.Print();
    if(memtable.TryGet("one", out string val)){
      Console.WriteLine(val);
    }
  }
}
