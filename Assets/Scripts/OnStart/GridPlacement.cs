using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPlacement : MonoBehaviour
{
    //Grid Size
    public Vector2Int gridSize;

    //Start position of the grid (-x, -z)
    public Vector2Int gridStart;

    public float zOffset;

    public GameObject spawnPrefab;
    public Transform parentTransform;
    public bool destroyAfterSpawn;

    // Start is called before the first frame update
    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = gridStart.x; x <= gridStart.x + gridSize.x; x++)
        {
            for (int y = gridStart.y; y <= gridStart.y + gridSize.y; y++)
            {
                GameObject temp = Instantiate(spawnPrefab);
                Transform tempTransform = temp.transform;
                if (parentTransform)
                {
                    tempTransform.SetParent(parentTransform);
                }
                else
                {
                    tempTransform.SetParent(transform);
                }
                tempTransform.position = new Vector3(x, y, zOffset);
            }
        }

        if (destroyAfterSpawn)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
