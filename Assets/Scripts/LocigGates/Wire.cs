using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public GameObject drawSprite;
    public Vector2Int startPos;
    public Vector2Int endPos;
    bool hasStart = false;
    public void StartWire(Vector2Int start)
    {
        if (hasStart)
        {
            BoardManager.bm.GetCurrentBoard().CreateWire(transform);

            return;
        }

        startPos = start;
        hasStart = true;
        UpdateWire();
    }

    public void UpdateWireEnd(Vector2Int end)
    {
        if (!hasStart) { return; }

        if (end == endPos) { return; }

        endPos = end;
        UpdateWire();
    }

    private void UpdateWire() { 
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        Vector2Int currentPos = startPos;
        bool endLessThanStartX = endPos.x < startPos.x;
        bool endLessThanStartY = endPos.y < startPos.y;
        if (endLessThanStartX)
        {
            while (currentPos.x > endPos.x)
            {
                GameObject temp = Instantiate(drawSprite);
                temp.transform.position = new Vector3(currentPos.x, currentPos.y, -5);
                temp.transform.SetParent(transform);

                currentPos.x -= 1;
            }
        }
        else 
        {
            while (currentPos.x < endPos.x)
            {
                GameObject temp = Instantiate(drawSprite);
                temp.transform.position = new Vector3(currentPos.x, currentPos.y, -5);
                temp.transform.SetParent(transform);

                currentPos.x += 1;
            }
        }

        if (endLessThanStartY)
        {
            while(currentPos.y >= endPos.y)
            {
                GameObject temp = Instantiate(drawSprite);
                temp.transform.position = new Vector3(currentPos.x, currentPos.y, -5);
                temp.transform.SetParent(transform);

                currentPos.y -= 1;
            }
        }
        else
        {
            while (currentPos.y <= endPos.y)
            {
                GameObject temp = Instantiate(drawSprite);
                temp.transform.position = new Vector3(currentPos.x, currentPos.y, -5);
                temp.transform.SetParent(transform);

                currentPos.y += 1;
            }
        }
    }
}
