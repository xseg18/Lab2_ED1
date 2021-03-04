using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
namespace E_Arboles
{
    public class Binary<T> where T:IComparable
    {
        public class Node
        {
            public Node Right;
            public Node Left;
            public T Key;
            public int Data;
        }
        Node Root;

        public void Add(T add, int c)
        {
            Node temp = new Node();
            temp.Data = c;
            temp.Key = add;
            if(Root == null)
            {
                Root = temp;
                return;
            }
            else
            {
                Node run = Root;
                while(run != null)
                {
                    if(temp.Key.CompareTo(run.Key) > 0)
                    {
                        if(run.Right == null)
                        {
                            run.Right = temp;
                            return;
                        }
                        else
                        {
                            run = run.Right;
                        }
                    }
                    else
                    {
                        if (run.Left == null)
                        {
                            run.Left = temp; return;
                        }
                        else
                        {
                            run = run.Left;
                        }
                    }
                }
            }
        }
        public void Delete(T data)
        {
            if (data.CompareTo(Root.Key) == 0)
            {
                Node temp = Root;
                Node prev = null;
                while (temp.Right != null)
                {
                    prev = temp;
                    temp = temp.Right;
                }
                Root.Data = temp.Data;
                prev.Right = null;
            }
            else if(data.CompareTo(Root.Key) > 0 )
            {
                Node temp = Root;
                Node prev = null;
                while(temp != null)
                {
                    if(temp.Right.Key.CompareTo(data) == 0)
                    {
                        prev = temp;
                        temp = temp.Right;break;
                    }
                    else if(temp.Left.Key.CompareTo(data) == 0)
                    {
                        prev = temp;
                        temp = temp.Left; break;
                    }
                    else
                    {
                        temp = temp.Right;
                    }
                }
                if(temp.Left == null && temp.Right == null)
                {
                    if(prev.Right == temp)
                    {
                        prev.Right = null;
                    }
                    else if(prev.Left == temp)
                    {
                        prev.Left = null;
                    }
                }
                else
                {
                    if(temp.Right != null && temp.Left == null)
                    {
                        temp = temp.Right;
                    }
                    else if(temp.Left != null && temp.Right == null)
                    {
                        temp = temp.Left;
                    }
                    else
                    {
                        Node temp1 = Root;
                        Node prev1 = null;
                        while (temp1.Left != null)
                        {
                            prev1 = temp;
                            temp1 = temp.Left;
                        }
                        Root.Key = temp1.Key;
                        prev1.Left = null;
                    }
                }
            }
            else
            {
                Node temp = Root;
                Node prev = null;
                while (temp != null)
                {
                    if (temp.Right.Key.CompareTo(data) == 0)
                    {
                        prev = temp;
                        temp = temp.Right; break;
                    }
                    else if (temp.Left.Key.CompareTo(data) == 0)
                    {
                        prev = temp;
                        temp = temp.Left; break;
                    }
                    else
                    {
                        temp = temp.Left;
                    }
                }
                if (temp.Left == null && temp.Right == null)
                {
                    if (prev.Right == temp)
                    {
                        prev.Right = null;
                    }
                    else if (prev.Left == temp)
                    {
                        prev.Left = null;
                    }
                }
                else
                {
                    if (temp.Right != null && temp.Left == null)
                    {
                        temp = temp.Right;
                    }
                    else if (temp.Left != null && temp.Right == null)
                    {
                        temp = temp.Left;
                    }
                    else
                    {
                        Node temp1 = Root;
                        Node prev1 = null;
                        while (temp1.Right != null)
                        {
                            prev1 = temp1;
                            temp1 = temp1.Right;
                        }
                        Root.Key = temp1.Key;
                        prev1.Right = null;
                    }
                }
            }
        }
        public int Find(T data)
        {
            if(Root.Key.CompareTo(data) < 0)
            {
                Node temp = Root.Right;
                while (temp != null)
                {
                    if (temp.Key.Equals(data))
                    {
                        return temp.Data;
                    }
                    else
                    {
                        if (temp.Left.Key.Equals(data) && temp.Left != null)
                        {
                            return temp.Left.Data;
                        }
                        else
                        {
                            temp = temp.Right;
                        }
                    }
                }
            }
            else
            {
                Node temp = Root.Left;
                while (temp != null)
                {
                    if (temp.Key.Equals(data))
                    {
                        return temp.Data;
                    }
                    else
                    {
                        if (temp.Right.Key.Equals(data) && temp.Right != null)
                        {
                            return temp.Right.Data;
                        }
                        else
                        {
                            temp = temp.Left;
                        }
                    }
                }
            }
            return -1;
        }
    }
}
