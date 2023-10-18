using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class TileController : MonoBehaviour
{
    [SerializeField] private Transform slotPosition;
    private GameObject tilePrefab1;
    private GameObject tilePrefab2;
    private void Start()
    {
        tilePrefab1 = Instantiate(Resources.Load<GameObject>("Tile/" + TileName.Bamboo),transform);
        tilePrefab2 = Instantiate(Resources.Load<GameObject>("Tile/" + TileName.Chicken),transform);
        ActionManager.onTileClicked += ClickAction;
    }
    
    public void ClickAction(Tile tile)
    {
        tile.transform.rotation = new Quaternion(0,0,0,0);
        tile.rigidBody.freezeRotation = true;
        tile.transform.DOMove(new Vector3(slotPosition.position.x, tile.transform.position.y, slotPosition.position.z), 1f);
        slotPosition.GetComponent<Slot>().occupiedTile = tile.GetComponent<Tile>();
    }
    
    public enum TileName
    {
        Bamboo,
        Chicken,
        Dragon,
        Flower,
        Han,
        Jade,
        White
    };
}