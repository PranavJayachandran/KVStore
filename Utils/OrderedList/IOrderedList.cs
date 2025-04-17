using Core;
namespace Utils;

public interface IOrderedList{
  public void Insert(int key, string val);
  public void Delete(int key);
  public void Print();
  public bool TrySearch(int key, out string val);
  public List<StoredData> GetAllData();
}
