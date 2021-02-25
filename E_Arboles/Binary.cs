using System;

namespace E_Arboles
{
    public class Binary<T>
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
                    if(temp.Data > run.Data)
                    {
                        if(run.Right == null)
                        {
                            run.Right = temp;return;
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
                    //if(temp.Data > run.Data)
                    //{
                    //    if (run == Root)
                    //    {
                    //        Node temp2 = Root;
                    //        Root = temp;
                    //        Root.Left = temp2;
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        if(run.Right != null)
                    //        {
                    //            run.Right = temp;
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            run = run.Right;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    if(run.Left != null)
                    //    {
                    //        run.Left = temp;
                    //        return;
                    //    }
                    //    else
                    //    {
                    //        run = run.Left;
                    //    }
                        
                    //}
                }
            }
        }
    }
}
