using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireLayer
{
    public WireLayerNode[,] layer;

    public WireLayer(Vector2Int gridSize)
    {
        layer = new WireLayerNode[gridSize.x, gridSize.y];

        for(int x = 0; x < layer.GetLength(0); x++)
        {
            for(int y = 0; y < layer.GetLength(1); y++)
            {
                layer[x, y] = new WireLayerNode(WireLayerNode.Type.None, false, new Vector2Int(x,y));
            }
        }
    }

    public string PrintLayer()
    {
        string output = "<mspace=mspace=10>";
        for (int y = layer.GetLength(1) - 1; y >= 0; y--)
        {
            output += "|";
            for (int x = 0; x < layer.GetLength(0); x++)
            {
                output += layer[x, y].GetStringType() + " ";
            }
            output += "|\n";
        }
        output += "</mspace>";
        return output;
    }

    public bool hasThing(Vector2Int gridPos)
    {
        return !(layer[gridPos.x, gridPos.y].GetNodeType() == WireLayerNode.Type.None);
    }

    public void PlaceThing(Vector2Int gridPos, WireLayerNode.Type type)
    {
        layer[gridPos.x, gridPos.y].ChangeType(type);
    }

    public void UpdateThing(Vector2Int gridPos, WireLayerNode.Type type)
    {
        layer[gridPos.x, gridPos.y].ChangeType(type);
    }
    public void ActivateLayer()
    {
        for (int x = 0; x < layer.GetLength(0); x++)
        {
            for (int y = 0; y < layer.GetLength(1); y++)
            {
                if(layer[x,y].GetNodeType() != WireLayerNode.Type.None)
                {
                    layer[x, y].SetState(true);
                }
            }
        }
    }

    public void DeActivateLayer(Vector2Int currentPos)
    {
        for (int x = 0; x < layer.GetLength(0); x++)
        {
            for (int y = 0; y < layer.GetLength(1); y++)
            {
                if (layer[x, y].GetNodeType() == WireLayerNode.Type.HardPowered)
                {
                    if(x == currentPos.x && y == currentPos.y) { continue; }
                    if (layer[x, y].GetHardState())
                    {
                        return;
                    }
                }
            }
        }

        for (int x = 0; x < layer.GetLength(0); x++)
        {
            for (int y = 0; y < layer.GetLength(1); y++)
            {
                if (layer[x, y].GetNodeType() != WireLayerNode.Type.None)
                {
                    layer[x, y].SetState(false);
                }
            }
        }
    }

    public bool GetStateOfNode(Vector2Int gridPos)
    {
        return layer[gridPos.x, gridPos.y].GetState();
    }
}

public class WireLayerNode
{
    public enum Type { None,HardPowered,Wire}
    private Type type;
    private bool isPowered;
    private bool isHardPowered;
    public Vector2Int gridPos;

    public WireLayerNode(Type t, bool state, Vector2Int pos)
    {
        type = t;
        isPowered = state;
        gridPos = pos;
    }

    public bool GetState()
    {
        return isPowered;
    }

    public void SetState(bool newState)
    {
        isPowered = newState;

        Board board = BoardManager.bm.GetCurrentBoard();
        Vector2Int pos = new Vector2Int(gridPos.x, gridPos.y);
        board.board[pos.x,pos.y].UpdateActive(isPowered);
    }

    public bool GetHardState()
    {
        return isHardPowered;
    }

    public void SetHardState(bool newState)
    {
        isHardPowered = newState;
    }

    public Type GetNodeType()
    {
        return type;
    }

    public void ChangeType(Type newType)
    {
        type = newType;
    }

    public string GetStringType()
    {
        if(type == Type.None)
        {
            return " ";
        }
        if(type == Type.Wire)
        {
            return "1";
        }
        if(type == Type.HardPowered)
        {
            return "2";
        }
        return " ";
    }
}