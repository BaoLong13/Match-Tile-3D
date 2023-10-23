using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TileController _tileController;
    [SerializeField] private UIController _uiController;
    
    public static GameManager Instance;
    public GameState State;

    private int currLevelID = 1;
    private void Awake()
    {
        Instance = this;
    }
    
    public void UpdateGameState(GameState newState)
    {
        State = newState;
        switch (State)
        {
            case GameState.Start: 
                _tileController.GenerateTile(1);
                break;
            case GameState.Lose:
                _tileController.ClearSlot();
                _tileController.GenerateTile(currLevelID);
                break;
            case GameState.NextStage:
                currLevelID += 1;
                if (currLevelID > 3)
                {
                    UpdateGameState(GameState.End);
                }
                _tileController.ClearSlot();
                _tileController.GenerateTile(currLevelID);
                break;
            case GameState.End:
                _tileController.ClearSlot();
                _tileController.GenerateTile(1);
                break;
        }
    }
    
    public enum GameState
    {
        Start,
        Lose,
        NextStage,
        End
    };
}
