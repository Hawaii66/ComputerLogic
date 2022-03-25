using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Board : MonoBehaviour
{
    public string boardID;
    public Vector2Int boardSize;
    public Vector2Int boardStart;

    public bool drawBoard;

    public TextMeshProUGUI debugOutput;

    public Cell[,] board;

    public List<WireLayer> wireLayers;

    private void Start()
    {
        board = new Cell[boardSize.x, boardSize.y];
        wireLayers = new List<WireLayer>();

        BoardManager.bm.AddBoard(this);

        for(int x = 0;  x < board.GetLength(0); x++)
        {
            for(int y = 0; y < board.GetLength(1); y++)
            {
                board[x, y] = new Cell(boardID, new Vector2Int(x, y), new Vector2Int(x + boardStart.x, y + boardStart.y));
            }
        }

        RedrawBoard();
    }

    public void LateUpdate()
    {
        if (drawBoard)
        {
            RedrawBoard();
        }
    }

    public void CreateWire(Transform wireParent)
    {
        int currentLayerIndex = -1;

        int layerCount = 0;
        for (int i = 0; i < wireParent.childCount; i++)
        {
            Transform temp = wireParent.GetChild(i);
            Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(temp.position.x) - boardStart.x, Mathf.RoundToInt(temp.position.y) - boardStart.y);
            for (int j = 0; j < wireLayers.Count; j++)
            {
                if (wireLayers[j].hasThing(gridPos))
                {
                    currentLayerIndex = j;
                    layerCount += 1;
                }
            }
        }

        if(currentLayerIndex == -1)
        {
            WireLayer layer = new WireLayer(boardSize);
            wireLayers.Add(layer);
            currentLayerIndex = wireLayers.Count - 1;
        }

        WireLayer currentLayer = wireLayers[currentLayerIndex];
        
        for(int i = 0; i < wireParent.childCount; i++)
        {
            Transform temp = wireParent.GetChild(i);
            Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(temp.position.x) - boardStart.x, Mathf.RoundToInt(temp.position.y) - boardStart.y);
            currentLayer.PlaceThing(gridPos, WireLayerNode.Type.Wire);
        }

        MergeLayersStart();

        for (int i = wireParent.childCount - 1; i >= 0; i--)
        {
            wireParent.GetChild(i).SetParent(transform);
        }

        Destroy(wireParent.gameObject);
        Mouse.m.follow = false;

        debugOutput.text = currentLayer.PrintLayer();
    }

    public void MergeLayersStart()
    {
        bool isDone = false;
        while (!isDone)
        {
            bool hasMerged = false;
            for(int i = 0; i < wireLayers.Count; i++)
            {
                for(int x = 0; x < boardSize.x; x++)
                {
                    for(int y = 0;y < boardSize.y; y++)
                    {
                        if(wireLayers[i].hasThing(new Vector2Int(x, y))){
                            for(int j = 0; j < wireLayers.Count; j++)
                            {
                                if(j == i) { continue; }
                                if(wireLayers[j].hasThing(new Vector2Int(x, y)))
                                {
                                    wireLayers[i] = MergeLayers(wireLayers[i], wireLayers[j]);
                                    wireLayers.RemoveAt(j);
                                    hasMerged = true;
                                    goto BreakPlace;
                                }
                            }
                        }
                    }
                }
            }

            BreakPlace: if(!hasMerged)
            {
                isDone = true;
            }
        }
    }

    public WireLayer MergeLayers(WireLayer layer1, WireLayer layer2)
    {
        bool shouldBePowered = false;
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                if (layer1.layer[x, y].GetState())
                {
                    shouldBePowered = true;
                }
            }
        }
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                if (layer2.layer[x, y].GetState())
                {
                    shouldBePowered = true;
                }
            }
        }

        WireLayer newLayer = new WireLayer(boardSize);
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                if (layer1.layer[x, y].GetNodeType() == WireLayerNode.Type.HardPowered)
                {
                    newLayer.layer[x, y].ChangeType(WireLayerNode.Type.HardPowered);
                }
                else if (layer2.layer[x, y].GetNodeType() == WireLayerNode.Type.HardPowered)
                {
                    newLayer.layer[x, y].ChangeType(WireLayerNode.Type.HardPowered);
                }
                else if (layer1.layer[x, y].GetNodeType() == WireLayerNode.Type.Wire)
                {
                    newLayer.layer[x, y].ChangeType(WireLayerNode.Type.Wire);
                }
                else if (layer2.layer[x, y].GetNodeType() == WireLayerNode.Type.Wire)
                {
                    newLayer.layer[x, y].ChangeType(WireLayerNode.Type.Wire);
                }
                else
                {
                    newLayer.layer[x, y].ChangeType(WireLayerNode.Type.None);
                }

                newLayer.layer[x, y].SetState(shouldBePowered);
            }
        }
        return newLayer;
    }

    public void UpdateWiresLayer(Vector2Int startGridPos, bool newState)
    {
        Debug.Log("Working");
        Vector2Int gridPos = new Vector2Int(startGridPos.x - boardStart.x, startGridPos.y - boardStart.y);

        for (int i = 0; i < wireLayers.Count; i++)
        {
            if (wireLayers[i].hasThing(gridPos))
            {
                Debug.Log("Wire layer has thing");
                if(wireLayers[i].layer[gridPos.x,gridPos.y].GetNodeType() == WireLayerNode.Type.HardPowered)
                {
                    wireLayers[i].layer[gridPos.x, gridPos.y].SetHardState(newState);
                }

                if (newState)
                {
                    wireLayers[i].ActivateLayer();
                }
                else
                {
                    wireLayers[i].DeActivateLayer(gridPos);
                }
            }
        }
    }
    
    public void UpdateType(Vector2Int gridPos, WireLayerNode.Type newType)
    {
        gridPos = new Vector2Int(gridPos.x - boardStart.x, gridPos.y - boardStart.y);
        for (int i = 0; i < wireLayers.Count; i++)
        {
            if (wireLayers[i].hasThing(gridPos))
            {
                wireLayers[i].UpdateThing(gridPos, newType);    
            }
        }
    }

    public void RedrawBoard()
    {
        for (int x = 0; x < board.GetLength(0); x++)
        {
            for (int y = 0; y < board.GetLength(1); y++)
            {
                board[x, y].UpdateSprite();
            }
        }
    }

    public void ChangeCell(Vector2Int gridPos, GameObject newObj)
    {
        gridPos.x -= boardStart.x;
        gridPos.y -= boardStart.y;

        if(newObj != board[gridPos.x, gridPos.y].getGameObject())
        {
            DestroyImmediate(board[gridPos.x, gridPos.y].getGameObject());
        }
        board[gridPos.x, gridPos.y].setObject(newObj);
    }

    public bool canHaveObject(Vector2Int gridPos)
    {
        gridPos.x -= boardStart.x;
        gridPos.y -= boardStart.y;

        if (board[gridPos.x, gridPos.y].getGameObject() == null)
        {
            return true;
        }
        return false;
    }

    public void ClearCell(Vector2Int gridPos)
    {
        gridPos.x -= boardStart.x;
        gridPos.y -= boardStart.y;

        board[gridPos.x, gridPos.y].setObject(null);
    }

    public GameObject getObjectAtPos(Vector2Int gridPos)
    {
        return board[gridPos.x, gridPos.y].getGameObject();
    }

    public bool isInsideBoard(Vector3Int pos, Vector2Int[] sizeOffsets) => isInsideBoard(new Vector2Int(pos.x, pos.y), sizeOffsets);
    public bool isInsideBoard(Vector2Int pos, Vector2Int[] sizeOffsets)
    {
        if (pos.x < boardStart.x || pos.y < boardStart.y)
        {
            return false;
        }
        if (pos.x > boardStart.x + boardSize.x || pos.y > boardStart.y + boardSize.y)
        {
            return false;
        }
        if (sizeOffsets.Length == 0) { return true; }
        Vector2Int offsetPos = pos;
        for (int i = 0; i < sizeOffsets.Length; i++)
        {
            offsetPos = pos + sizeOffsets[i];
            if (offsetPos.x < boardStart.x || offsetPos.y < boardStart.y)
            {
                return false;
            }
            if (offsetPos.x > boardStart.x + boardSize.x || offsetPos.y > boardStart.y + boardSize.y)
            {
                return false;
            }
        }
        return true;
    }
    public bool isInsideBoardSingle(Vector2Int pos)
    {
        if (pos.x < boardStart.x || pos.y < boardStart.y)
        {
            return false;
        }
        if (pos.x > boardStart.x + boardSize.x || pos.y > boardStart.y + boardSize.y)
        {
            return false;
        }
        return true;
    }

}
