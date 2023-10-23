using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : MonoBehaviour
{
    public Tile occupiedTile;
    public bool isEmpty;

    private void Start()
    {
        isEmpty = true;
    }
}
