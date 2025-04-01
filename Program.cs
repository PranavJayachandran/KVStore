using Core;
using Utils;

public class NoSql{
  public static void Main(){
    Memtable memtable = new Memtable(new RollingHash(20));
    memtable.Add("one","one1");
    memtable.Add("two","two1");
    memtable.Print(); 
    memtable.Remove("one");
    memtable.Add("one","one2");
    memtable.Print();
  }
}
