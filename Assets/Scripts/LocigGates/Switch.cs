using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool isActive;
    public Sprite activeSprite;
    public Sprite inActiveSprite;
    public ObjectClicked.objectType type;

    private void Start()
    {
        isActive = false;

        
        UpdateSprite();
    }

    public void OnClick()
    {
        isActive = !isActive;
        
        BoardManager.bm.GetCurrentBoard().UpdateWiresLayer(new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y)), isActive);

        UpdateSprite();
    }

    public void UpdateSprite()
    {
        if(isActive)
        {
            GetComponent<SpriteRenderer>().sprite = activeSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = inActiveSprite;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100f))
            {
                if (hit.transform.gameObject == gameObject)
                {
                    OnClick();
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
            Vector2Int[] temp = new Vector2Int[0];
            if (BoardManager.bm.GetCurrentBoard().isInsideBoard(gridPos,temp))
            {
                BoardManager.bm.GetCurrentBoard().UpdateType(gridPos, WireLayerNode.Type.HardPowered);

            }
        } 
    }
}
