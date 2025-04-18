using System.Text;
using Core;

namespace Utils;


//Format key value#IsDelete (followed by padding to get the 30 bytes size)
//Every line should be 30 bytes in size. This is to improve search, if every line is of fixed length querying would be easier.
public static class SST{
  private static readonly int maxLevel = 3;
  private static readonly int[] fileSizePerLevel = [100, 200];
  private static readonly int entrySize = 30;
  private static readonly string baseDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../SST"));

  public static void SetUpSSTFolder(){
    string baseDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../SST"));

    //Remove this when you add fault tolerance
    //For ease remove recovery and let the system start from start
    Directory.Delete(baseDir,true);

    if(!Directory.Exists(baseDir)){
      Directory.CreateDirectory(baseDir);
      for(int i=0;i<maxLevel;i++){
        AddNewLevel();
      }
    }
  }

  public static bool TryGetValue(int key, out string val){
    int level = 1;
    while(level <= maxLevel){
      string filePath = GetLevelFilePath(level);
      if(FindPosFromFile(filePath, key, out val)){
        return true;
      }
      level++;
    }
    val = default;
    return false;
  }

  public static void WriteToSST(List<StoredData> data){
    AddValueAtLevel(data, 1);
    Compaction();
  }

  private static void Compaction(){
    //Start with level 1 
    int level = 1;
    while(level < maxLevel){
      string filePath = GetLevelFilePath(level);
      List<StoredData> removedData = [];
      FileInfo fileInfo = new FileInfo(filePath);
      long size = fileInfo.Length;
      if(size >= fileSizePerLevel[level - 1]){

        List<string> fileData = ReadEntries(filePath);
        Random rand = new Random();
        while(size >= fileSizePerLevel[level - 1] && fileData.Count > 0){
          int index = rand.Next(fileData.Count);
          string line = fileData[index];
          removedData.Add(GetStoredDataFromString(line));
          fileData.RemoveAt(index);  
          size -= 30;
        }
        WriteEntries(filePath, fileData);
        removedData.Sort();
        AddValueAtLevel(removedData, level + 1);
      }
      else{
        break;
      }
      level++;
    }
  }

  private static void AddValueAtLevel(List<StoredData> data, int level){
    string filePath = GetLevelFilePath(level);
    List<string> fileData = ReadEntries(filePath);
    foreach(var val in data){
      int index = FindInsertionPosFromLines(fileData, val.Key, out bool IsMatch);
      if(IsMatch){
        fileData[index] = FormatAndPaddString(val);
      } else{
        fileData.Insert(index, FormatAndPaddString(val));
      }
    }
    WriteEntries(filePath, fileData);
  }

  private static StoredData GetKthEntryFromFile(string filePath, int index){
    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)){
      long offset = index * entrySize;
      if(offset + entrySize - 3 > stream.Length){
        throw new Exception($"Trying to read from file {filePath}, entry number {index} which doesnot exist");
      }

      byte[] buffer = new byte[entrySize];
      stream.Seek(offset, SeekOrigin.Begin);
      stream.Read(buffer, 0, entrySize - 3);
      return GetStoredDataFromString(Encoding.UTF8.GetString(buffer).TrimEnd('\0'));
    }
  }
  private static List<string> ReadEntries(string filePath){
    List<string> entries = [];
    using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read)){
      long totalEntries = stream.Length / entrySize ;
      if(totalEntries > 0){
        totalEntries++;
      }
      for(long i = 0; i < totalEntries; i++){
        byte[] buffer = new byte[entrySize];
        stream.Read(buffer, 0, entrySize);
        // Convert the byte array to string and trim any excess characters (if needed)
        string entry = Encoding.UTF8.GetString(buffer, 0, entrySize - 3).TrimEnd('\0');
        entries.Add(entry);
      }
    }
    return entries;
  }

  private static void WriteEntries(string filePath, List<string>data){
    File.WriteAllText(filePath, string.Join(" | ",data));
  }

  //binary search to find the position where the insertion should happen
  private static int FindInsertionPosFromLines(List<string> fileData, int key, out bool IsMatch){
    int low = 0, high = fileData.Count-1;
    int index = fileData.Count;
    IsMatch = false;
    while(low <= high){
      int mid = (low + high)/2;
      int tempVal = GetStoredDataFromString(fileData[mid]).Key;
      if(tempVal < key){
        low = mid + 1;
      } else if(tempVal > key){
        index = mid;
        high = mid - 1;
      } else {
        index = mid;
        IsMatch = true;
        break;
      }
    }
    return index;
  }

  private static bool FindPosFromFile(string filePath, int key, out string val){
    FileInfo fileInfo = new FileInfo(filePath);
    long size = fileInfo.Length;
    if(size == 0){
      val = default;
      return false;
    }
    int totalEntries = (int)size / entrySize ;
    int low = 0, high = totalEntries;
    while(low <= high){
      int mid = (low + high)/2;
      StoredData data = GetKthEntryFromFile(filePath, mid);
      if(data.Key < key){
        low = mid + 1;
      } else if(data.Key > key){
        high = mid - 1;
      } else{
        if(data.IsDelete){
          break;
        }
        val = data.Val;
        return true;
      }
    }
    val = default;
    return false;
  }

  private static StoredData GetStoredDataFromString(string s){
    if(string.IsNullOrEmpty(s)){
      return null;
    }
    string[] keyValue = s.Split(' ', StringSplitOptions.RemoveEmptyEntries);
    if(keyValue.Length > 2){
      throw new Exception($"corrupt format for key value {s}");
    }
    bool IsDelete = keyValue[1].EndsWith('#');
    StoredData sd = new StoredData{
      Key = int.Parse(keyValue[0]),
      Val = IsDelete ? keyValue[1][..^1] : keyValue[1],
      IsDelete = IsDelete
    };
    return sd;
  }

  //If the level and file already exists nothing would happen
  private static void AddNewLevel(){
    int currentLevelCount = Directory.GetDirectories(baseDir).Length;
    Directory.CreateDirectory(GetLevelFolderPath(currentLevelCount + 1));
    using (File.Create(GetLevelFilePath(currentLevelCount + 1))){}
  }

  private static string GetLevelFolderPath(int level){
    return Path.Combine(baseDir, level.ToString());
  }
  private static string GetLevelFilePath(int level){
    return Path.Combine(Path.Combine(baseDir, level.ToString()), level.ToString() + ".txt");
  }

  //converts the data in the format we need to save and padds to get the 30bytes limit.
  private static string FormatAndPaddString(StoredData data){
    string s = $"{data.Key} {data.Val + (data.IsDelete ? "#" : "")}";
    int totalSize = entrySize - 3; //30 bytes, 3 characteres would be added when joining -> " | "
    int currentsize = Encoding.UTF8.GetByteCount(s);
    if(currentsize > totalSize){
      throw new Exception($"The key value size is more than {totalSize} bytes");
    }
    int paddingSize = totalSize - currentsize;
    string padding = new string(' ', paddingSize);
    return s + padding;
  }
}

