using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public Vector2Int posActuel;
    
    //distance from start to node x
    private int gCost;
    //heuristique score : calculer entre le point au goal
    private int hCost;
    private int fCost;
    private Node parent;

    public Node(Vector2Int posActuel,  int g, int h ,Node parentNode)
    {
        this.posActuel = posActuel;
        gCost = g;
        hCost = h;
        fCost = g + h;
        parent = parentNode;
    }

    public int getfCost()
    {
        return fCost;
    }

    public Vector2Int GetPosition()
    {
        return posActuel;
    }
    public Node getParent()
    {
        return parent;
    }
    public int getGcost()
    {
        return gCost;
    }
    public void setGcost(int g)
    {
         gCost = g;
    }
    public void setParent(Node parent)
    {
        this.parent = parent;
    }
}
