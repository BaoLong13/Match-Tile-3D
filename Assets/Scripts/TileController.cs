using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = System.Random;

public class TileController : MonoBehaviour
{
    [SerializeField] private Slot[] slotPosition;
    [SerializeField] private LevelData[] _levelDatas;
    [SerializeField] private UIController _uiController;
    
    private float XOffset = 20f;
    private float YOffset = 15f;

    private int totalTile;
    private bool isMoving = false;
    
    private string[] tilePrefabs = new string[7]
    {
        "Bamboo",
        "Chicken",
        "Dragon",
        "Flower",
        "Han",
        "Jade",
        "White"
    };
    
    private void OnEnable()
    {
        ActionManager.onTileClicked += ClickAction;
    }

    private void OnDisable()
    {
        ActionManager.onTileClicked -= ClickAction;
    }

    private void Start()
    {
        //GameManager.Instance.UpdateGameState(GameManager.GameState.Start);
    }

    private void ClickAction(Tile tile)
    {
        if (isMoving)
            return;

        isMoving = true;
        for (int i = 0; i < slotPosition.Length; ++i)
        {
            if (slotPosition[i].isEmpty)
            {
                if (i > 0)
                {
                    if (!slotPosition[i - 1].occupiedTile.TileName.Equals(tile.TileName))
                    {
                        for (int j = i - 1; j >= 0; --j)
                        {
                            if (slotPosition[j].occupiedTile.TileName.Equals(tile.TileName))
                            {
                                int numberOfStep = Math.Abs(i - j) - 1;
                                for (int k = 1; k <= numberOfStep; ++k)
                                {
                                    slotPosition[i - k].occupiedTile.transform.DOMove(
                                        new Vector3( slotPosition[i - k + 1].transform.position.x, slotPosition[i - k + 1].transform.position.y,
                                            slotPosition[i - k + 1].transform.position.z), 0.2f);
                                    slotPosition[i - k + 1].occupiedTile = slotPosition[i - k].occupiedTile;
                                    slotPosition[i - k + 1].isEmpty = false;
                                    if (k == numberOfStep)
                                    {
                                        tile.transform.rotation = new Quaternion(0,0,0,0);
                                        tile.GetComponent<BoxCollider>().enabled = false;
                                        tile.transform.DOMove(new Vector3(slotPosition[i - k].transform.position.x, slotPosition[i - k].transform.position.y, slotPosition[i - k].transform.position.z), 0.1f).OnComplete(
                                            () =>
                                            {
                                                DOVirtual.DelayedCall(0.25f, () =>
                                                {
                                                    isMoving = false;
                                                });
                                            });
                                        slotPosition[i - k].occupiedTile = tile.GetComponent<Tile>();
                                        slotPosition[i - k].isEmpty = false;
                                        if (MatchCheck(i - k))
                                        { 
                                            MatchLogic(i - k);
                                            DOVirtual.DelayedCall(0.4f, () => { TileShift(i - k + 1); });
                                        }
                                        else
                                        {
                                            if (CheckLoseCondition())
                                            {
                                                DOVirtual.DelayedCall(0.1f, () =>
                                                {
                                                    _uiController.Show(GameManager.GameState.Lose);
                                                });
                                               
                                            }
                                        }
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                tile.transform.rotation = new Quaternion(0,0,0,0);
                tile.GetComponent<BoxCollider>().enabled = false;
                tile.transform.DOMove(
                    new Vector3(slotPosition[i].transform.position.x, slotPosition[i].transform.position.y,
                        slotPosition[i].transform.position.z), 0.1f).OnComplete(() =>
                {
                    DOVirtual.DelayedCall(0.25f, () =>
                    {
                        isMoving = false;
                    });
                });
                slotPosition[i].GetComponent<Slot>().occupiedTile = tile.GetComponent<Tile>();
                slotPosition[i].isEmpty = false;
                if (MatchCheck(i))
                {
                    MatchLogic(i);
                }
                else
                {
                    if (CheckLoseCondition())
                    {
                        DOVirtual.DelayedCall(0.1f, () =>
                        {
                            _uiController.Show(GameManager.GameState.Lose);
                        });

                    }
                }
                break;
            }
        }
    }

    private void MatchLogic(int index)
    {
        DOVirtual.DelayedCall(0.2f, () =>
            {
                MatchAnim(index);
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    Destroy(slotPosition[index].occupiedTile.gameObject);
                    slotPosition[index].occupiedTile = null;
                    slotPosition[index].isEmpty = true;
                    Destroy(slotPosition[index - 1].occupiedTile.gameObject);
                    slotPosition[index - 1].occupiedTile = null;
                    slotPosition[index - 1].isEmpty = true;
                    Destroy(slotPosition[index - 2].occupiedTile.gameObject);
                    slotPosition[index - 2].occupiedTile = null;
                    slotPosition[index - 2].isEmpty = true;
                });
            });
            totalTile -= 3;
            if (totalTile <= 0)
            {
                _uiController.Show(GameManager.GameState.NextStage);
            }
    }

    private void TileShift(int currIndex)
    {
        if (currIndex >= 3)
        {
            for (int i = currIndex; i < slotPosition.Length; ++i)
            {
                if (!slotPosition[i].isEmpty)
                {
                    slotPosition[i].occupiedTile.transform
                        .DOMove(
                            new Vector3(slotPosition[i - 3].transform.position.x,
                                slotPosition[i - 3].transform.position.y, slotPosition[i - 3].transform.position.z), 0.2f).OnComplete(
                            () =>
                            {
                                DOVirtual.DelayedCall(0.25f, () =>
                                {
                                    isMoving = false;
                                });
                              
                            });
                    slotPosition[i - 3].occupiedTile = slotPosition[i].occupiedTile;
                    slotPosition[i - 3].isEmpty = false;
                    slotPosition[i].occupiedTile = null;
                    slotPosition[i].isEmpty = true;
                }
            }
        }
    }

    private bool CheckLoseCondition()
    {
        for (int i = 0; i < slotPosition.Length; ++i)
        {
            if (slotPosition[i].isEmpty)
                return false;
        }

        return true;
    }
    
    private bool MatchCheck(int currIndex)
    {
        if (currIndex >= 2)
        {
            if (slotPosition[currIndex].occupiedTile.TileName.Equals(slotPosition[currIndex - 1].occupiedTile.TileName)
                &&
                slotPosition[currIndex].occupiedTile.TileName.Equals(slotPosition[currIndex - 2].occupiedTile.TileName
                ))
            {
                return true;
            }
            return false;
        }

        return false;
    }

    private void MatchAnim(int currIndex)
    {
        slotPosition[currIndex].occupiedTile.transform.DOScale(0, 0.1f).OnStart(() =>
            {
                slotPosition[currIndex - 1].occupiedTile.transform.DOScale(0, 0.1f).OnStart(() =>
                    {
                        slotPosition[currIndex - 2].occupiedTile.transform.DOScale(0, 0.1f);
                    });
            });
    }

    public void ClearSlot()
    {
        for (int i = 0; i < slotPosition.Length; ++i)
        {
            if (slotPosition[i].occupiedTile != null)
            {
                Destroy(slotPosition[i].occupiedTile.gameObject);
                slotPosition[i].occupiedTile = null;
                slotPosition[i].isEmpty = true;
            }
        }
    }
    
    public void GenerateTile(int levelID)
    {
        isMoving = false;
        if (levelID > _levelDatas.Length)
        {
            return;
        }
        LevelData newLevel = new LevelData();
        foreach (var data in _levelDatas)
        {
            if (levelID == data.levelID)
                newLevel = data;
        }
        for (int i = 0; i < tilePrefabs.Length; ++i)
        {
            for (int j = 0; j < newLevel.tileAmount; ++j)
            { 
                GameObject obj =  Instantiate(Resources.Load<GameObject>("Tile/" + tilePrefabs[i]),new Vector3(UnityEngine.Random.Range(-XOffset,XOffset),UnityEngine.Random.Range(1f,YOffset), UnityEngine.Random.Range(-5f,15f)), new Quaternion(0,0,0,0),transform);
                obj.GetComponent<Tile>().TileName = tilePrefabs[i];
            }
        }

        totalTile = newLevel.tileAmount * (tilePrefabs.Length);
        Debug.Log(totalTile);
    }
}