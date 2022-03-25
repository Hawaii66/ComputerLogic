using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lamp : MonoBehaviour
{
    public Sprite activeSprite;
    public Sprite inActiveSprite;

    public void ChangeState(bool newState)
    {
        if (newState)
        {
            GetComponent<SpriteRenderer>().sprite = activeSprite;
        }
        else
        {
            GetComponent<SpriteRenderer>().sprite = inActiveSprite;
        }
    }
}
