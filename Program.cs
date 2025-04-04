using Core;
using Utils;

public class NoSql{
  public static void Main(){
    Memtable memtable = new Memtable(new RollingHash(20));
    memtable.Add("one","one1");
    memtable.Add("two","two1");
    memtable.Print(); 
    memtable.Add("one","one2");
    memtable.Print();
    if(memtable.TryGet("one", out string val)){
      Console.WriteLine(val);
    }
  }
}
