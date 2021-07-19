using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    private string boardID;
    Board currentBoard;
    private Vector2Int gridPosition;
    private GameObject objectOnCell;

    public Cell(string id, Vector2Int pos)
    {
        boardID = id;
        currentBoard = BoardManager.bm.GetBoardById(boardID);

        gridPosition = pos;
        objectOnCell = null;
    }

    public GameObject setObject(GameObject newObj)
    {
        objectOnCell = newObj;

        currentBoard.RedrawBoard();

        return objectOnCell;
    }

    public GameObject getGameObject()
    {
        return objectOnCell;
    }
}
