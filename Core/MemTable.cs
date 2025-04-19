using Utils;
namespace Core;

public class Memtable{
  private readonly IOrderedList _list;
  private readonly IHash _hash;
  private readonly WriteAheadLog _wal;

  public Memtable(IHash hash){
    _list = new SkipList(5, 100);
    _hash = hash;
    _wal = new WriteAheadLog("wal.txt");
    if(!_wal.CreateLogFileIfNotExist()){
      //RecoverUsingWal();
    }
  }

  public void Add(string key, string val, bool IsDelete = false, bool isRecovery = false){
    _list.Insert(_hash.GetHash(key),val, IsDelete);
    if (!isRecovery){
      WriteLog(LogType.Insert, [key,val]);
    }
  }

  public bool TryGet(string key, out string val){
    if(_list.TrySearch(_hash.GetHash(key), out string result)){
      val = result;
      return true;
    }
    val = null;
    return false;
  }

  public void Remove(string key, bool isRecovery = false){
    _list.Delete(_hash.GetHash(key));
    if(!isRecovery){
      WriteLog(LogType.Delete, [key]);
    }
  }

  public void Print(){
    _list.Print();
  }

  public List<StoredData> GetAllData(){
    return _list.GetAllData();
  }

  public bool IsFull(){
    return _list.IsFull();
  }

  public void CleanWAL(){
    _wal.CleanLogFile();
  }

  private void WriteLog(LogType type, List<string> val){
    string message = "";
    // Validation for the type and vakues
    switch(type){
      case LogType.Insert:
        if(val.Count != 2){
          throw new Exception("Insert log should have only 2 elements");
        }
        message += "Insert ";
        break;
      case LogType.Delete:
        if(val.Count != 1){
          throw new Exception("Delete log should have only 1 element");
        }
        message += "Delete ";
        break;
   }

    foreach(var el in val){
      message += el + " ";
    }

    _wal.WriteAsync(message);
  }

  private void RecoverUsingWal(){
    List<string> lines= _wal.ReadAllLines();
    for(int i = 0; i < lines.Count; i++){
      var line = lines[i];
      string[] data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
      if(data.Length == 0)
        continue;
      //Validation to check if the log is right (incase any corruption)
      switch(data[0]){
        case "Insert": {
           if(data.Length != 3){
             Console.WriteLine($"Wal is Corrupt at line number {i+1}");
             return;
           }
           Add(data[1], data[2], true);
           break;
        }
        case "Delete": {
          if(data.Length != 2){
             Console.WriteLine($"Wal is Corrupt at line number {i+1}");
             return;
          }
          Remove(data[1], true); 
          break;
        }
        default :{
          Console.WriteLine($"Wal is Corrupt at line number {i+1}");
          return;
        }
      }
    }
  }
}

enum LogType{
  Insert,
  Delete,
}
