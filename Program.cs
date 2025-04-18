using System.Text;
using Core;
using Utils;

public class NoSql{
  public static void Main(){
    Memtable memtable = new Memtable(new RollingHash(1000));

    SST.SetUpSSTFolder();
    memtable.Add("two", "Haha");
    for(int i=0;i<4;i++){
      memtable.Add(GenerateRandomString(5), GenerateRandomString(10));
    }
    memtable.Print();

    var hash = new RollingHash(1000);
    var p = memtable.GetAllData();
    Console.WriteLine(p[0].Key);
    Console.WriteLine(p[p.Count- 1].Key);
    SST.WriteToSST(memtable.GetAllData());
    if(SST.TryGetValue(hash.GetHash("two"), out string vali)){
      Console.WriteLine("VALLLL :" + vali);
    }
    if(memtable.TryGet("one", out string val)){
      Console.WriteLine(val);
    }
  }
  static string GenerateRandomString(int length)
  {
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    Random rand = new Random();
    StringBuilder result = new StringBuilder(length);
  
    for (int i = 0; i < length; i++)
    {
      result.Append(chars[rand.Next(chars.Length)]);
    }
    return result.ToString();  
  }
}
