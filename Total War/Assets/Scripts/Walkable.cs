using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walkable : MonoBehaviour
{
    public bool walkable = false;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            walkable = true;
        }
    }
}
