using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public List<Board> boards;

    public static BoardManager bm;
    public Board currentBoard;

    public Vector2Int DEBUG_gridPos;
    public bool DEBUG_newState;

    private void Awake()
    {
        bm = this;
    }

    public Board GetBoardById(string id)
    {
        for(int i = 0; i < boards.Count; i++)
        {
            if(boards[i].boardID == id)
            {
                return boards[i];
            }
        }
        return null;
    }

    public void AddBoard(Board newBoard)
    {
        boards.Add(newBoard);

        currentBoard = newBoard;
    }

    public void RemoveBoard(Board removeBoard)
    {
        boards.Remove(removeBoard);
    }

    public Board GetCurrentBoard()
    {
        return currentBoard;
    }

}
