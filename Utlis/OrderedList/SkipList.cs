namespace Utils ;

internal class SkipList : IOrderedList{
  private int maxLevel;
  Node head;
  private static readonly Random rand = new Random();

  public SkipList(int maxLevel){
    this.maxLevel = maxLevel;
    head = new Node(-1, "HEAD", maxLevel-1);
  }
  public void Insert(int key, string val){
    List<Node>update = null; 
    Node node = Search(key,out update);
    if(node.Forward[0]?.key != key){
      int level = GenerateRandomLevel();
      Node newNode = new Node(key,val,level);
      for(int i = 0;  i <= level; i++){
        newNode.Forward[i] = update[i].Forward[i];
        update[i].Forward[i] = newNode;
      }
    }
    else{
      node.Forward[0].Val = val;
    }
  }
  public void Delete(int key){
    Node node = Search(key, out List<Node> update);
    node = node.Forward[0];
    if(node.key == key){
      for(int i = 0; i < update.Count ; i++){
        if(update[i].Forward[i]?.key == key){
        update[i].Forward[i] = node.Forward[i];
        }
      }
    }
  }
  public void Print(){
    int level = maxLevel-1;
    while(level >= 0){
      var node = head.Forward[level];
      while(node != null){
        Console.Write($"{node.key} -> {node.Val} :: ");
        node = node.Forward[level];
      }
      Console.WriteLine();
      level--;
    }
    Console.WriteLine();
  }

  public bool TrySearch(int key, out string val){
    Node node = Search(key, out List<Node> _);
    node = node.Forward[0];
    if(node.key == key){
      val = node.Val;
      return true;
    }
    val = null;
    return false;
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
}



internal class Node{
  public readonly int key;
  public string Val {get; set;}
  public List<Node> Forward {get; set;}
  public int CurrentLevel {get; set;}
  private readonly int level;

  public Node(int key, string val, int level){
    this.key = key;
    Val = val;
    this.level = level;
    CurrentLevel = level;
    Forward = Enumerable.Repeat<Node>(null,level+1).ToList();
  }

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
