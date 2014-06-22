using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CDLinkedList
{
    public class acctList<T> where T : IComparable<T>
    {
        public class Node
        {
            private T data;
            private Node nodeNext;
            private Node nodePrevious;

            public Node(T dataValue)
            {
                Data = dataValue;
                NodeNext = null;
                NodePrevious = null;
            }

            public T Data 
            { 
                get { return data; }
                set { data = value; }
            }

            public Node NodeNext 
            {
                get { return nodeNext; }
                set { nodeNext = value; }
            }

            public Node NodePrevious 
            {
                get { return nodePrevious; }
                set { nodePrevious = value; }
            }
        }

        public class List
        {
            private Node first;
            private Node last;
            public Node current, listNext, listPrevious;

            public List()
            {
                First = first;
                Last = last;
            }

            public Node First
            {
                get { return first; }
                set { first = value; }
            }

            public Node Last
            {
                get { return last; }
                set { last = value; }
            }

            public void Add(T newItem)
            {
                Node newNode = new Node(newItem);

                if (First == null)
                {
                    First = newNode;
                    Last = newNode;
                    current = First;
                }
                else
                {
                    current = First;
                    listPrevious = current.NodePrevious;
                    listNext = current.NodeNext;
                }

                //if only 1 item in list
                if (First == Last)
                {
                    //if newNode > current
                    if (current.Data.CompareTo(newNode.Data) < 0)
                    {
                        First = current;
                        Last = newNode;
                        First.NodeNext = Last;
                        First.NodePrevious = Last;
                        Last.NodeNext = First;
                        Last.NodePrevious = First;
                    }
                    //if newNode < current
                    else
                    {                      
                        Last = current;
                        First = newNode;
                        First.NodeNext = Last;
                        First.NodePrevious = Last;
                        Last.NodeNext = First;
                        Last.NodePrevious = First;
                    }
                }
                //multiple or no items in list
                else
                {
                    //compares newNode with all in list except Last
                    while (current != Last && current.Data.CompareTo(newNode.Data) < 0)
                    {
                        listPrevious = current;
                        current = listNext;
                        listNext = listNext.NodeNext;
                    }
                                                      
                    //if list is empty
                    if (current == null)
                    {
                        Last = newNode;
                        First = newNode;
                        Last.NodeNext = newNode;
                        First.NodePrevious = newNode;
                    }
                    //newNode goes at beginning
                    else if (current == First)
                    {
                        newNode.NodeNext = current;
                        listPrevious.NodeNext = newNode;
                        newNode.NodePrevious = listPrevious;
                        current.NodePrevious = newNode;
                        current = newNode;
                        listNext = current.NodeNext;
                        listPrevious = current.NodePrevious;
                        First = current;
                    }
                    //new node goes at end
                    else if (current == Last)
                    {
                        //newNode goes before Last
                        if (current.Data.CompareTo(newNode.Data) > 0)
                        {
                            newNode.NodeNext = current;
                            listPrevious.NodeNext = newNode;
                            newNode.NodePrevious = listPrevious;
                            current.NodePrevious = newNode;
                            current = newNode;
                            listNext = current.NodeNext;
                            listPrevious = current.NodePrevious;
                        }
                        //newNode becomes Last
                        else
                        {
                            newNode.NodePrevious = current;
                            newNode.NodeNext = listNext;
                            listNext.NodePrevious = newNode;
                            current.NodeNext = newNode;
                            current = newNode;
                            listNext = current.NodeNext;
                            listPrevious = current.NodePrevious;
                            Last = current;
                        }
                    }
                    //newNode goes between two nodes
                    else
                    {
                        newNode.NodeNext = current;
                        listPrevious.NodeNext = newNode;
                        newNode.NodePrevious = listPrevious;
                        current.NodePrevious = newNode;
                        current = newNode;
                        listNext = current.NodeNext;
                        listPrevious = current.NodePrevious;
                    }
                }
            }

            //removes item from list/re-directs node references around item to be deleted
            public void Delete(T deleteItem)
            {
                //removed first and sets next item to first
                if (current == First)
                {
                    current = current.NodeNext;
                    listNext = listNext.NodeNext;
                    current.NodePrevious = listPrevious;
                    listPrevious.NodeNext = current;
                    First = current;
                }
                //removes last and sets previous item to last
                else if (current == Last)
                {
                    current = current.NodePrevious;
                    listPrevious = listPrevious.NodePrevious;
                    current.NodeNext = listNext;
                    listNext.NodePrevious = current;
                    Last = current;
                }
                //removes current item and sets next item to current
                else
                {
                    current = current.NodeNext;
                    listNext = listNext.NodeNext;
                    current.NodePrevious = listPrevious;
                    listPrevious.NodeNext = current;
                }
            }
        }
    }
}
