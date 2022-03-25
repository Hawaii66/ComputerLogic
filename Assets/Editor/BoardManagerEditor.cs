using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoardManager))]
public class BoardManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BoardManager bm = (BoardManager)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Add item"))
        {
            bm.GetCurrentBoard().UpdateWiresLayer(bm.DEBUG_gridPos + bm.GetCurrentBoard().boardStart, bm.DEBUG_newState);
        }
    }
}
