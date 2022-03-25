using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleLinkedList
{
    public DoubleLinkedListNode firstNode;

    public void Create(GameObject first)
    {
        firstNode = new DoubleLinkedListNode(first, null, null);
    }

    public void AddNode(GameObject middle)
    {
        DoubleLinkedListNode node = firstNode;
        while(node.GetNext() != null)
        {
            node = node.GetNext();
        }

        DoubleLinkedListNode newNode = new DoubleLinkedListNode(middle, node, null);

        node.SetNextNode(newNode);
    }

    public GameObject GetNode(int index)
    {
        DoubleLinkedListNode node = firstNode;
        for(int i = 0; i < index; i++)
        {
            if(node.GetNext() == null)
            {
                return null;
            }

            node = node.GetNext();

        }

        return node.getObject();
    }

    public DoubleLinkedListNode GetRealNode(int index)
    {
        DoubleLinkedListNode node = firstNode;
        for (int i = 0; i < index; i++)
        {
            if (node.GetNext() == null)
            {
                return null;
            }

            node = node.GetNext();

        }

        return node;
    }
 
    
}

public class DoubleLinkedListNode
{
    private GameObject obj;
    private DoubleLinkedListNode previousNode;
    private DoubleLinkedListNode nextNode;
    public DoubleLinkedListNode(GameObject obj, DoubleLinkedListNode pre, DoubleLinkedListNode next)
    {
        this.obj = obj;
        previousNode = pre;
        nextNode = next;
    }

    public GameObject getObject()
    {
        return obj;
    }

    public DoubleLinkedListNode GetPrevious()
    {
        return previousNode;
    }

    public DoubleLinkedListNode GetNext()
    {
        return nextNode;
    }

    public GameObject GetPreviousGameObject()
    {
        return previousNode.getObject();
    }

    public GameObject GetNextGameObject()
    {
        return nextNode.getObject();
    }

    public void SetNextNode(DoubleLinkedListNode node)
    {
        nextNode = node;
    }

    public void SetPreviousNode(DoubleLinkedListNode node)
    {
        previousNode = node;
    }
}