using System.Text;
using KeyValueStore;
using Utils;
public class NoSql{
  public static void Main(){
    KVStore kv = new KVStore(new RollingHash(2000));
    kv.Add("two", "Haha");
    for(int i=0;i<10;i++){
      kv.Add(GenerateRandomString(5), GenerateRandomString(10));
    }
    kv.Remove("two");
    if(kv.TryGetValue("two", out string val)){
      Console.WriteLine("Found" + val);
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
