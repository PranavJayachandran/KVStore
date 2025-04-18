using System.Text;
using Core;
using Utils;

public class NoSql{
  public static void Main(){
    Memtable memtable = new Memtable(new RollingHash(1000));
    SST.SetUpSSTFolder();
    for(int i=0;i<20;i++){
      memtable.Add(GenerateRandomString(5), GenerateRandomString(10));
    }
    memtable.Print();
    if(memtable.TryGet("one", out string val)){
      Console.WriteLine(val);
    }
    SST.WriteToSST(memtable.GetAllData());
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
