using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private string boardID;
    Board currentBoard;
    private Vector2Int gridPosition;
    private Vector2Int worldPos;
    private GameObject objectOnCell;

    private bool powered;

    public Cell(string id, Vector2Int pos, Vector2Int worldP)
    {
        boardID = id;
        currentBoard = BoardManager.bm.GetBoardById(boardID);

        gridPosition = pos;
        worldPos = worldP;
        objectOnCell = null;

        UpdateSprite();
    }

    public GameObject setObject(GameObject newObj)
    {
        if (newObj)
        {

            objectOnCell = newObj;
            objectOnCell.transform.position = new Vector3Int(worldPos.x, worldPos.y, -5);
            objectOnCell.transform.SetParent(currentBoard.transform);
        }
        else
        {
            objectOnCell = null;
        }

        currentBoard.RedrawBoard();
        //UpdateSprite();

        return objectOnCell;
    }

    public GameObject getGameObject()
    {
        return objectOnCell;
    }

    public void UpdateActive(bool newState)
    {
        powered = newState;
        UpdateSprite();
    }

    public bool isActive()
    {
        return powered;
    }

    public void UpdateSprite()
    {
        if(objectOnCell == null) { return; }
        if(objectOnCell.GetComponent<Lamp>())
        {
            objectOnCell.GetComponent<Lamp>().ChangeState(powered);
        }
    }
}
