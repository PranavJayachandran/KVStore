//namespace Utils.RBTree;

//internal class RBTree : IOrderedList
//{
    //private Node root { get; set; } = null;
    //private int counter = 0;
//
    //public void Insert(int key, string val)
    //{
        //var newNode = new Node(key, val, null, Color.Red);
        //if (root == null)
        //{
            //root = newNode;
            //root.Color = Color.Black;
            //return;
        //}
        //var temp = root;
        //while (true)
        //{
            //if (temp.Key < key)
            //{
                //if (temp.RightChild != null)
                //{
                    //temp = temp.RightChild;
                //}
                //else
                //{
                    //temp.RightChild = newNode;
                    //newNode.Parent = temp;
                    //break;
                //}
            //}
            //else
            //{
                //if (temp.LeftChild != null)
                //{
                    //temp = temp.LeftChild;
                //}
                //else
                //{
                    //temp.LeftChild = newNode;
                    //newNode.Parent = temp;
                    //break;
                //}
            //}
        //}
        //ReBalance(newNode);
    //}
//
    //public void Delete(int key)
    //{
        //var node = FindNode(root, key);
        //if (node == null)
            //return;
//
        //DeleteNode(node);
    //}
//
    //public void Print()
    //{
        //if (root == null)
        //{
            //Console.WriteLine("(empty)");
            //return;
        //}
//
        //Queue<Node> q = new();
        //q.Enqueue(root);
        //while (q.Count > 0)
        //{
            //var size = q.Count;
            //while (size-- > 0)
            //{
                //var node = q.Dequeue();
                //Console.Write(node.Val + "-" + (node.Color == Color.Red ? "Red" : "Black"));
                //if (node.LeftChild != null)
                    //q.Enqueue(node.LeftChild);
                //if (node.RightChild != null)
                    //q.Enqueue(node.RightChild);
            //}
            //Console.WriteLine();
        //}
    //}
//
    //public bool TrySearch(int key, out string val)
    //{
      //var node = FindNode(root, key);
      //if(node == null){
        //val = null;
        //return false;
      //}
      //val = node.Val;
      //return true;
    //}
//
    //private void DeleteNode(Node node)
    //{
        //Node y = node;
        //Node x = null;
        //Color originalColor = y.Color;
//
        //if (node.LeftChild == null)
        //{
            //x = node.RightChild;
            //Transplant(node, node.RightChild);
        //}
        //else if (node.RightChild == null)
        //{
            //x = node.LeftChild;
            //Transplant(node, node.LeftChild);
        //}
        //else
        //{
            //y = Minimum(node.RightChild);
            //originalColor = y.Color;
            //x = y.RightChild;
//
            //if (y.Parent == node)
            //{
                //if (x != null)
                    //x.Parent = y;
            //}
            //else
            //{
                //Transplant(y, y.RightChild);
                //y.RightChild = node.RightChild;
                //if (y.RightChild != null)
                    //y.RightChild.Parent = y;
            //}
//
            //Transplant(node, y);
            //y.LeftChild = node.LeftChild;
            //if (y.LeftChild != null)
                //y.LeftChild.Parent = y;
            //y.Color = node.Color;
        //}
//
        //if (originalColor == Color.Black)
            //FixDelete(x, node.Parent);
    //}
//
    //private void FixDelete(Node x, Node parent)
    //{
        //while (x != root && (x == null || x.Color == Color.Black))
        //{
            //if (x == parent.LeftChild)
            //{
                //var w = parent.RightChild;
                //if (w != null && w.Color == Color.Red)
                //{
                    //w.Color = Color.Black;
                    //parent.Color = Color.Red;
                    //LeftRotate(w);
                    //w = parent.RightChild;
                //}
//
                //if ((w.LeftChild == null || w.LeftChild.Color == Color.Black) &&
                    //(w.RightChild == null || w.RightChild.Color == Color.Black))
                //{
                    //if (w != null) w.Color = Color.Red;
                    //x = parent;
                    //parent = x.Parent;
                //}
                //else
                //{
                    //if (w.RightChild == null || w.RightChild.Color == Color.Black)
                    //{
                        //if (w.LeftChild != null) w.LeftChild.Color = Color.Black;
                        //w.Color = Color.Red;
                        //RightRotate(w.LeftChild);
                        //w = parent.RightChild;
                    //}
//
                    //if (w != null) w.Color = parent.Color;
                    //parent.Color = Color.Black;
                    //if (w.RightChild != null) w.RightChild.Color = Color.Black;
                    //LeftRotate(w.RightChild);
                    //x = root;
                //}
            //}
            //else
            //{
                //var w = parent.LeftChild;
                //if (w != null && w.Color == Color.Red)
                //{
                    //w.Color = Color.Black;
                    //parent.Color = Color.Red;
                    //RightRotate(w.RightChild);
                    //w = parent.LeftChild;
                //}
//
                //if ((w.RightChild == null || w.RightChild.Color == Color.Black) &&
                    //(w.LeftChild == null || w.LeftChild.Color == Color.Black))
                //{
                    //if (w != null) w.Color = Color.Red;
                    //x = parent;
                    //parent = x.Parent;
                //}
                //else
                //{
                    //if (w.LeftChild == null || w.LeftChild.Color == Color.Black)
                    //{
                        //if (w.RightChild != null) w.RightChild.Color = Color.Black;
                        //w.Color = Color.Red;
                        //LeftRotate(w.RightChild);
                        //w = parent.LeftChild;
                    //}
//
                    //if (w != null) w.Color = parent.Color;
                    //parent.Color = Color.Black;
                    //if (w.LeftChild != null) w.LeftChild.Color = Color.Black;
                    //RightRotate(w.LeftChild);
                    //x = root;
                //}
            //}
        //}
//
        //if (x != null) x.Color = Color.Black;
    //}
//
    //private Node FindNode(Node root, int key)
    //{
        //while (root != null)
        //{
            //if (key < root.Key)
                //root = root.LeftChild;
            //else if (key > root.Key)
                //root = root.RightChild;
            //else
                //return root;
        //}
        //return null;
    //}
//
    //private void Transplant(Node u, Node v)
    //{
        //if (u.Parent == null)
            //root = v;
        //else if (u == u.Parent.LeftChild)
            //u.Parent.LeftChild = v;
        //else
            //u.Parent.RightChild = v;
//
        //if (v != null)
            //v.Parent = u.Parent;
    //}
//
    //private Node Minimum(Node node)
    //{
        //while (node.LeftChild != null)
            //node = node.LeftChild;
        //return node;
    //}
//
//
    //private void ReBalance(Node node)
    //{
        //if (node.Parent == null || node.Parent.Color != Color.Red)
        //{
            //return;
        //}
        //if (counter++ > 10)
            //return;
        //Console.WriteLine("Balance " + node.Key);
        //var uncle = FindUncle(node);
        //var isUncleBlackOrNull = uncle == null || uncle.Color == Color.Black;
        //var nextNode = node.Parent;
        //if (!isUncleBlackOrNull)
        //{
            //node.Parent.Color = Color.Black;
            //if (node.Parent.Parent != root)
                //node.Parent.Parent.Color = Color.Red;
            //uncle.Color = Color.Black;
            //ReBalance(node.Parent.Parent);
        //}
        //else
        //{
            //var isTriangle = IsTriangle(node);
            //if (isTriangle)
            //{
                //if (node.Parent.LeftChild == node)
                    //RightRotate(node);
                //else
                    //LeftRotate(node);
                //ReBalance(nextNode);
            //}
            //else
            //{
                //var parent = node.Parent;
                //var grandParent = parent.Parent;
                //if (node.Parent.LeftChild == node)
                    //RightRotate(node.Parent);
                //else
                    //LeftRotate(node.Parent);
//
                //parent.Color = Color.Black;
                //if (grandParent != root)
                    //grandParent.Color = Color.Red;
                //ReBalance(grandParent);
            //}
        //}
    //}
//
    //private void RightRotate(Node node)
    //{
        //var parent = node.Parent;
        //var grandParent = parent.Parent;
        //parent.LeftChild = node.RightChild;
        //if (node.RightChild != null)
            //node.RightChild.Parent = parent;
        //node.RightChild = parent;
        //node.Parent = parent.Parent;
        //parent.Parent = node;
        //if (grandParent != null)
            //if (grandParent.LeftChild == parent)
            //{
                //grandParent.LeftChild = node;
            //}
            //else
            //{
                //grandParent.RightChild = node;
            //}
        //if (parent == root)
        //{
            //root = node;
            //root.Color = Color.Black;
        //}
    //}
//
    //private void LeftRotate(Node node)
    //{
        //var parent = node.Parent;
        //var grandParent = parent.Parent;
        //parent.RightChild = node.LeftChild;
        //if (node.LeftChild != null)
            //node.LeftChild.Parent = parent;
        //node.LeftChild = parent;
        //node.Parent = parent.Parent;
        //parent.Parent = node;
        //if (grandParent != null)
            //if (grandParent.LeftChild == parent)
            //{
                //grandParent.LeftChild = node;
            //}
            //else
            //{
                //grandParent.RightChild = node;
            //}
        //if (parent == root)
        //{
            //root = node;
            //root.Color = Color.Black;
        //}
    //}
//
    //private static Node FindUncle(Node node)
    //{
        //if (node.Parent != null && node.Parent.Parent != null)
        //{
            //var parent = node.Parent;
            //var grandParent = parent.Parent;
            //if (grandParent.LeftChild == parent)
                //return grandParent.RightChild;
            //return grandParent.LeftChild;
        //}
        //return null;
    //}
//
    //private bool IsTriangle(Node node)
    //{
        //if (node.Parent == null || node.Parent.Parent == null)
        //{
            //throw new ArgumentException(node.Parent.Key + " has no grandparent to check triangle relationship");
        //}
        //var parent = node.Parent;
        //var grandParent = node.Parent.Parent;
        //if (grandParent.LeftChild == parent && parent.RightChild == node)
            //return true;
        //if (grandParent.RightChild == parent && parent.LeftChild == node)
            //return true;
        //return false;
    //}
//}
//
//internal class Node
//{
    //public int Key { get; set; }
    //public string Val { get;set; }
    //public Node Parent { get; set; }
    //public Node LeftChild { get; set; }
    //public Node RightChild { get; set; }
    //public Color Color { get; set; }
//
    //public Node(int key, string val, Node parent, Color color)
    //{
        //Key = key;
        //Parent = parent;
        //Color = color;
        //Val = val;
    //}
//}
//
//internal enum Color
//{
    //Red,
    //Black
//}
