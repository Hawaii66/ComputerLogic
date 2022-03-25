using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public static Mouse m;

    public bool follow;
    public GameObject obj;
    public float worldPosOffset;
    Vector3 followPrevPos;
    Transform prevParent;
    ObjectClicked.objectType followType;
    Vector2Int[] sizeOffsets;

    bool pressedPrevious;

    private void Awake()
    {
        m = this;
    }

    private void Update()
    {
        if (follow)
        {
            Vector3 prevPos = transform.position;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPosition.z = worldPosOffset;
            transform.position = snapPosition(worldPosition);

            if (Input.GetMouseButtonUp(0))
            {
                if (ObjectClicked.ReleaseOnboard(followType))
                {
                    follow = false;
                    OnReleaseObject(worldPosition);
                }
                else
                {
                    if (BoardManager.bm.GetCurrentBoard().isInsideBoard(snapPosition(worldPosition),sizeOffsets))
                    {
                        obj.GetComponent<Wire>().StartWire(snapPosition2D(worldPosition));
                    }
                }
            }

            if(followType == ObjectClicked.objectType.Wire)
            {
                if (BoardManager.bm.GetCurrentBoard().isInsideBoard(snapPosition(worldPosition), sizeOffsets))
                {
                    obj.GetComponent<Wire>().UpdateWireEnd(snapPosition2D(worldPosition));
                }
                else
                {
                    transform.position = prevPos;
                }
            }
        }
        else
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
    }

    private void OnReleaseObject(Vector3 worldPosition)
    {
        if (BoardManager.bm.GetCurrentBoard().isInsideBoard(snapPosition(worldPosition),sizeOffsets))
        {
            transform.GetChild(0).localScale = Vector3.one;
            if (BoardManager.bm.GetCurrentBoard().canHaveObject(snapPosition2D(worldPosition)))
            {
                BoardManager.bm.GetCurrentBoard().ChangeCell(snapPosition2D(worldPosition), transform.GetChild(0).gameObject);
            }
            else
            {
                obj.transform.position = followPrevPos;
                if (prevParent)
                {
                    obj.transform.SetParent(prevParent);
                }
                else
                {
                    DestroyImmediate(obj);
                }
            }
        }
        else
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    private Vector3Int snapPosition(Vector3 unsnappedPos)
    {
        Vector3Int newPos = Vector3Int.zero;
        newPos.x = Mathf.RoundToInt(unsnappedPos.x);
        newPos.y = Mathf.RoundToInt(unsnappedPos.y);
        newPos.z = Mathf.RoundToInt(unsnappedPos.z);
        return newPos;
    }

    private Vector2Int snapPosition2D(Vector3 unsnappedPos)
    {
        Vector3Int snapped = snapPosition(unsnappedPos);
        return new Vector2Int(snapped.x, snapped.y);
    }

    public void SetObject(GameObject newObj, bool inst)
    { 
        if (inst)
        {
            obj = Instantiate(newObj);
            prevParent = null;
        }
        else
        {
            obj = newObj;
            prevParent = obj.transform.parent;

            BoardManager.bm.GetCurrentBoard().ClearCell(snapPosition2D(obj.transform.position));
        }

        if (obj.GetComponent<ObjectClicked>())
        {
            followType = obj.GetComponent<ObjectClicked>().type;
        }
        else if (obj.GetComponent<Switch>())
        {
            sizeOffsets = new Vector2Int[0];
            followType = ObjectClicked.objectType.Switch;
        }
        else if (obj.GetComponent<Wire>())
        {
            sizeOffsets = new Vector2Int[0];
            followType = ObjectClicked.objectType.Wire;
        }
        else if (obj.GetComponent<Gate>())
        {
            sizeOffsets = obj.GetComponent<Gate>().sizeOffsets;
            followType = ObjectClicked.objectType.Gate;
        }

        followPrevPos = obj.transform.position;
        obj.transform.SetParent(transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one * 0.8f;
    }
}
