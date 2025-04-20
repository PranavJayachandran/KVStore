using System.Text;
using Core;

namespace Utils ;

internal class SkipList(int maxLevel, int maxSize): IOrderedList{
  private readonly int maxLevel = maxLevel;
  private readonly Node head = new Node(-1, "HEAD", maxLevel-1);
  private static readonly Random rand = new Random();
  private long size;
  private readonly int maxSize = maxSize;

  public void Insert(int key, string val, bool IsDelete = false){
    List<Node>update; 
    Node node = Search(key,out update);
    if(node.Forward[0]?.key != key){
      int level = GenerateRandomLevel();
      Node newNode = new Node(key,val,level,IsDelete);
      for(int i = 0;  i <= level; i++){
        newNode.Forward[i] = update[i].Forward[i];
        update[i].Forward[i] = newNode;
      }
      size += EstimateSize(node);
    }
    else{
      size += EstimateSizeOfString(val) - EstimateSizeOfString(node.Forward[0].Val);
      node.Forward[0].Val = val;
      node.Forward[0].IsDelete = false;
    }
  }

  //the delete should be soft delete. Append over update 
  public void Delete(int key){
    Node node = Search(key, out _);
    node = node.Forward[0];
    if(node.key == key){
      node.IsDelete = true;
      //for(int i = 0; i < update.Count ; i++){
      //  if(update[i].Forward[i]?.key == key){
      //  update[i].Forward[i] = node.Forward[i];
      //  }
      //}
    }
  }
  public void Print(){
    int level = maxLevel-1;
    while(level >= 0){
      var node = head.Forward[level];
      while(node != null){
        Console.Write($"{node.key} -> {node.Val}{(node.IsDelete?"#SoftDelete#":"")} :: ");
        node = node.Forward[level];
      }
      Console.WriteLine();
      level--;
    }
    Console.WriteLine();
  }

  public List<StoredData> GetAllData(){
    int level = 0;
    List<StoredData> data = [];
    var node = head.Forward[level];
    while(node!=null){
      var temp = new StoredData{Key = node.key, Val = node.Val, IsDelete = node.IsDelete}; 
      data.Add(temp);
      node = node.Forward[level];
    }
    return data;
  }

  public bool TrySearch(int key, out string val){
    Node node = Search(key, out List<Node> _);
    node = node.Forward[0];
    if(node is not null && node.key == key){
      val = node.Val;
      if(node.IsDelete){
        val = null;
      }
      return true;
    }
    val = null;
    return false;
  }

  public bool IsFull(){
    return size >= maxSize;
  }

  private Node Search(int key, out List<Node> update){
    update = Enumerable.Repeat<Node>(null,maxLevel).ToList();
    var node = head;
    for(int i = maxLevel - 1;i>=0;i--){
      while(node!=null && node.Forward[i]?.key < key){
        node = node.Forward[i];
      }
      update[i] = node;
    }
    return node;
  }
  private int GenerateRandomLevel(){
    return rand.Next(maxLevel);
  }

  //the size is a very very vague estimate
  private static int EstimateSize(Node node){
    int size = 0;
    size += EstimateSizeOfString(node.Val); //for Val
    size += 40; // default size for node
    size += 8 * node.Val.Length;
    return size;
  }
  private static int EstimateSizeOfString(string s){
    return Encoding.Unicode.GetByteCount(s);
  }
}



internal class Node(int key, string val, int level, bool del = false){
  public readonly int key = key;
  public string Val {get; set;} = val;
  public List<Node> Forward {get; set;} = Enumerable.Repeat<Node>(null,level+1).ToList();
  public int CurrentLevel {get; set;} = level;
  private readonly int level;
  public bool IsDelete {get;set;} = del;

  public void Reset(){
    CurrentLevel = level;
  }

  public Node GetNext(){
    var node = Forward[CurrentLevel];
    if(node != null)
      node.Reset();
    return node;
  }

  public void MoveToNextLevel(){
    CurrentLevel--;
  }
}
