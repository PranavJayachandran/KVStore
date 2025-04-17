namespace Utils;

public class WriteAheadLog{
  private readonly string filePath;

  public WriteAheadLog(string fileName){
    string baseDir =  AppDomain.CurrentDomain.BaseDirectory;
    string logDir = Path.GetFullPath(Path.Combine(baseDir, @"../../../log"));
    if(!Directory.Exists(logDir)){
      Directory.CreateDirectory(logDir);
    }

    filePath= Path.Combine(logDir, fileName);
  }

  //Returns true if the file was not already present
  public bool CreateLogFileIfNotExist(){
    if(!File.Exists(filePath)){
      using (File.Create(filePath)){}
      return true;
    }
    return false;
  }

  public List<string> ReadAllLines(){
    return File.ReadAllLines(filePath)
      .Where(line => !string.IsNullOrWhiteSpace(line)).ToList();
  }

  public Task<string> ReadFileAsync(){
    return File.ReadAllTextAsync(filePath);
  }


  public void WriteAsync(string content){
    File.AppendAllTextAsync(filePath, content + Environment.NewLine);
  }
} 
