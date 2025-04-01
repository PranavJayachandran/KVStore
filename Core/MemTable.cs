using System.Net;
using Utils;
namespace Core;

public class Memtable{
  private readonly SkipList _skipList;
  private readonly IHash _hash;
  public Memtable(IHash hash){
    _skipList = new SkipList(3);
    _hash = hash;
  }
  public void Add(string key, string val){
    _skipList.Insert(_hash.GetHash(key),val);
  }
  public bool TryGet(string key, out string val){
    if(_skipList.TrySearch(_hash.GetHash(key), out string result)){
      val = result;
      return true;
    }
    val = null;
    return false;
  }
  public void Remove(string key){
    _skipList.Delete(_hash.GetHash(key));
  }

  public void Print(){
    _skipList.Print();
  }
}



