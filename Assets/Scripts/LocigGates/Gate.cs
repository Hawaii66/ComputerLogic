using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public enum GateTypes { None, AND, OR, XOR, NOR,NOT,NAND }
    public GateTypes type;
    public Vector2Int[] sizeOffsets;

    public Vector2Int[] inputs;
    public Vector2Int[] outputs;

    private void Update()
    {
        Board board = BoardManager.bm.GetCurrentBoard();
        Vector2Int startPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        if (type == GateTypes.AND) { And(board, startPos); }
        if(type == GateTypes.OR) { Or(board, startPos); }
        if (type == GateTypes.XOR) { Xor(board, startPos); }
        if (type == GateTypes.NOR) { Nor(board, startPos); }
        if (type == GateTypes.NOT) { Not(board, startPos); }
        if (type == GateTypes.NAND) { Nand(board, startPos); }
    }

    private void And(Board board, Vector2Int startPos)
    {
        bool allActive = true;
        for (int i = 0; i < inputs.Length; i++)
        {
            Vector2Int gridPos = startPos + inputs[i];
            if (!board.isInsideBoardSingle(gridPos)) { continue; }
            if (!board.board[gridPos.x - board.boardStart.x, gridPos.y - board.boardStart.y].isActive())
            {
                allActive = false;
            }
        }

        for (int i = 0; i < outputs.Length; i++)
        {
            Vector2Int gridPos = startPos + outputs[i];
            if (!board.isInsideBoardSingle(gridPos)) { continue; }
            board.UpdateWiresLayer(gridPos, allActive);
        }
    }

    private void Or(Board board, Vector2Int startPos)
    {
        bool oneActive = false;
        for (int i = 0; i < inputs.Length; i++)
        {
            Vector2Int gridPos = startPos + inputs[i];
            if (!board.isInsideBoardSingle(gridPos)) { continue; }
            if (board.board[gridPos.x - board.boardStart.x, gridPos.y - board.boardStart.y].isActive())
            {
                oneActive = true;
            }
        }

        for (int i = 0; i < outputs.Length; i++)
        {
            board.UpdateWiresLayer(startPos + outputs[i], oneActive);
        }
    }

    private void Xor(Board board, Vector2Int startPos)
    {
        int activeCount = 0;
        for (int i = 0; i < inputs.Length; i++)
        {
            Vector2Int gridPos = startPos + inputs[i];
            if (!board.isInsideBoardSingle(gridPos)) { continue; }
            if (board.board[gridPos.x - board.boardStart.x, gridPos.y - board.boardStart.y].isActive())
            {
                activeCount += 1;
            }
        }
        for (int i = 0; i < outputs.Length; i++)
        {
            board.UpdateWiresLayer(startPos + outputs[i], activeCount == 1 ? true : false);
        }
    }

    private void Nand(Board board, Vector2Int startPos)
    {
        bool allActive = true;
        for (int i = 0; i < inputs.Length; i++)
        {
            Vector2Int gridPos = startPos + inputs[i];
            if (!board.isInsideBoardSingle(gridPos)) { continue; }
            if (!board.board[gridPos.x - board.boardStart.x, gridPos.y - board.boardStart.y].isActive())
            {
                allActive = false;
            }
        }

        for (int i = 0; i < outputs.Length; i++)
        {
            board.UpdateWiresLayer(startPos + outputs[i], !allActive);
        }
    }

    private void Not(Board board, Vector2Int startPos)
    {
        bool oneActive = false;
        for(int i = 0; i < inputs.Length; i++)
        {
            Vector2Int gridPos = startPos + inputs[i];
            if (!board.isInsideBoardSingle(gridPos)) { continue; }
            if (board.board[gridPos.x - board.boardStart.x, gridPos.y - board.boardStart.y].isActive())
            {
                oneActive = true;
            }
        }

        for (int i = 0; i < outputs.Length; i++)
        {
            board.UpdateWiresLayer(startPos + outputs[i], !oneActive);
        }
    }

    private void Nor(Board board, Vector2Int startPos)
    {
        bool oneActive = false;
        for (int i = 0; i < inputs.Length; i++)
        {
            Vector2Int gridPos = startPos + inputs[i];
            if (!board.isInsideBoardSingle(gridPos)) { continue; }
            if (board.board[gridPos.x - board.boardStart.x, gridPos.y - board.boardStart.y].isActive())
            {
                oneActive = true;
            }
        }

        for (int i = 0; i < outputs.Length; i++)
        {
            board.UpdateWiresLayer(startPos + outputs[i], oneActive ? false : true);
        }
    }
}
