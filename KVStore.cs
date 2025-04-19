using Utils;
using Core;
namespace KeyValueStore;

public class KVStore(IHash hash){
  private Memtable _memtable = new Memtable(hash);
  private readonly SST _sst = new SST(hash);
  public void Add(string key, string val){
    _memtable.Add(key, val);
    if(_memtable.IsFull()){
      _sst.WriteToSST(_memtable.GetAllData());
      Console.WriteLine(_memtable.GetAllData().Count);
      _memtable.CleanWAL();
      _memtable = new Memtable(hash);
    }
  }
  public void Remove(string key){
    if(_memtable.TryGet(key, out _)){
      _memtable.Remove(key);
    } else if(_sst.TryGetValue(key, out _)){
      _memtable.Add(key, "tombstone", true);
    }
  }
  public bool TryGetValue(string key, out string val){
    if(_memtable.TryGet(key, out val)){
      if(val == null)
        return false;
      return true;
    }
    if(_sst.TryGetValue(key, out val)){
      return true;
    }
    val = default;
    return false;
  }
}
