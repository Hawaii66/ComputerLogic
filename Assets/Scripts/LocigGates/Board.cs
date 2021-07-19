using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public string boardID;
    public Vector2Int boardSize;

    public Cell[,] board;

    private void Start()
    {
        board = new Cell[boardSize.x, boardSize.y];
        for(int x = 0;  x < board.GetLength(0); x++)
        {
            for(int y = 0; y < board.GetLength(1); y++)
            {
                board[x, y] = new Cell(boardID, new Vector2Int(x, y));
            }
        }

        RedrawBoard();
    }

    public void RedrawBoard()
    {
        Debug.Log("Redraw");
    }
}
