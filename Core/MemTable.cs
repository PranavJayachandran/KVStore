using System.Net;
using Utils.RBTree;
using Utils;
namespace Core;

public class Memtable{
  private readonly IOrderedList _list;
  private readonly IHash _hash;
  public Memtable(IHash hash){
    _list = new RBTree();
    _hash = hash;
  }
  public void Add(string key, string val){
    _list.Insert(_hash.GetHash(key),val);
  }
  public bool TryGet(string key, out string val){
    if(_list.TrySearch(_hash.GetHash(key), out string result)){
      val = result;
      return true;
    }
    val = null;
    return false;
  }
  public void Remove(string key){
    _list.Delete(_hash.GetHash(key));
  }

  public void Print(){
    _list.Print();
  }
}



