using Core;

namespace Utils;


//Format key value#IsDelete
public static class SST{
  private static readonly int maxLevel = 3;
  private static readonly string baseDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../SST"));

  public static void SetUpSSTFolder(){
    string baseDir = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../SST"));
    if(!Directory.Exists(baseDir)){
      Directory.CreateDirectory(baseDir);
      TryAddNewLevel(out _);
    }
  }

  public static void GetValue(){}

  public static void AddValue(List<StoredData> data){
    AddValueAtLevel(data, 1);
  }

  private static void AddValueAtLevel(List<StoredData> data, int level){
    string filePath = Path.Combine(Path.Combine(baseDir, level.ToString(), level.ToString()+ ".txt"));
    List<string> fileData = File.ReadAllLines(filePath).ToList();
    foreach(var val in data){
      int index = FindPos(fileData, val.Key, out bool IsMatch);
      if(IsMatch){
        fileData[index] = $"{val.Key} {val.Key + (val.IsDelete ? "#" : "")}";
      } else{
        fileData.Insert(index, $"{val.Key} {val.Key + (val.IsDelete ? "#" : "")}");
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
      string[] temp = fileData[mid].Split(' ');
      int tempVal = int.Parse(temp[0]);
      if(tempVal < key){
        low = mid + 1;
      } else if(tempVal > key){
        index = mid;
        high = mid - 1;
      } else {
        index = key;
        IsMatch = true;
      }
    }
    return index;
  }

  private static bool TryAddNewLevel(out string levelFolderName){
    int currentLevelCount = Directory.GetDirectories(baseDir).Length;
    if(currentLevelCount < maxLevel){
      levelFolderName = (currentLevelCount + 1).ToString();
      Directory.CreateDirectory(Path.Combine(baseDir, levelFolderName));
      File.Create(Path.Combine(Path.Combine(baseDir, levelFolderName), levelFolderName + ".txt"));
      return true;
    }
    levelFolderName = default;
    return false;
  }
}

