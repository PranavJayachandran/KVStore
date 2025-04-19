using System.Text;
using KeyValueStore;
using Utils;
public class NoSql{
  public static void Main(){
    KVStore kv = new KVStore(new RollingHash(2000));
    kv.Add("one","one");
    kv.Add("two","two");
    kv.Add("three","three");
    kv.Add("one1","one");
    kv.Add("two2","two");
    kv.Remove("three");
    kv.Add("three2","three");

    kv.Add("three3","three");
    kv.Add("three4","three");
    kv.Add("three5","three");
    kv.Add("three6","three");
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
