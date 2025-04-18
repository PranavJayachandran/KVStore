using Core;

namespace Utils;


//Format key value#IsDelete
public static class SST{
  private static readonly int maxLevel = 3;
  private static readonly int[] fileSizePerLevel = [50, 100];
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

  public static void GetValue(){}

  public static void WriteToSST(List<StoredData> data){
    AddValueAtLevel(data, 1);
    Compaction();
  }

  private static void Compaction(){
    //Start with level 1 
    int level = 1;
    while(level < maxLevel){
      string filePath = GetLevelFileName(level);
      List<StoredData> removedData = [];
      FileInfo fileInfo = new FileInfo(filePath);
      long size = fileInfo.Length;
      if(size >= fileSizePerLevel[level - 1]){

        List<string> fileData = File.ReadAllLines(filePath).ToList();
        Random rand = new Random();
        while(size >= fileSizePerLevel[level - 1] && fileData.Count > 0){
          Console.WriteLine(size);
          int index = rand.Next(fileData.Count);
          string line = fileData[index];
          removedData.Add(GetStoredDataFromString(line));
          long lineBytes = System.Text.Encoding.UTF8.GetByteCount(line + Environment.NewLine);
          size -= lineBytes;
          fileData.RemoveAt(index);
        }
        File.WriteAllLines(filePath, fileData);
        removedData.Sort();
        AddValueAtLevel(removedData, level + 1);
      }
      else{
        break;
      }
      Console.Write(level);
      level++;
    }
  }

  private static void AddValueAtLevel(List<StoredData> data, int level){
    string filePath = GetLevelFileName(level);
    List<string> fileData = File.ReadAllLines(filePath).ToList();
    foreach(var val in data){
      int index = FindPos(fileData, val.Key, out bool IsMatch);
      if(IsMatch){
        fileData[index] = $"{val.Key} {val.Val + (val.IsDelete ? "#" : "")}";
      } else{
        fileData.Insert(index, $"{val.Key} {val.Val + (val.IsDelete ? "#" : "")}");
      }
    }
    File.WriteAllLines(filePath, fileData);
  }

  //binary search to find the position where the insertion should happen
  private static int FindPos(List<string> fileData, int key, out bool IsMatch){
    int low = 0, high = fileData.Count-1;
    int index = 0;
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

  private static StoredData GetStoredDataFromString(string s){
    if(string.IsNullOrEmpty(s)){
      return null;
    }
    string[] keyValue = s.Split(' ');
    if(keyValue.Length != 2){
      throw new Exception($"Corrupt sst file with entry {s}");
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
    Directory.CreateDirectory(GetLevelFolderName(currentLevelCount + 1));
    using (File.Create(GetLevelFileName(currentLevelCount + 1))){}
  }

  private static string GetLevelFolderName(int level){
    return Path.Combine(baseDir, level.ToString());
  }
  private static string GetLevelFileName(int level){
    return Path.Combine(Path.Combine(baseDir, level.ToString()), level.ToString() + ".txt");
  }
}

