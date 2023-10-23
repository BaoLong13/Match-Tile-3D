using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class UIController : MonoBehaviour
{
  [SerializeField] private Image background;
  [SerializeField] private Image _buttonImage;
  [SerializeField] private GameObject _panel;
  [SerializeField] private TMP_Text _text;

  private Sprite START, NEXT, REPEAT;
  
  private void Start()
  {
    START = Resources.Load<Sprite>("UI/play");
    NEXT = Resources.Load<Sprite>("UI/arrow_right");
    REPEAT = Resources.Load<Sprite>("UI/repeat");
    Show(GameManager.Instance.State);
  }

  public void Show(GameManager.GameState state)
  {
    background.enabled = true;
    _panel.SetActive(true);
    GameManager.Instance.State = state;
    
    switch (state)
    {
      case GameManager.GameState.Start:
        _buttonImage.sprite = START;
        _text.SetText("Press Icon to start");
        break;
      
      case GameManager.GameState.NextStage:
        _buttonImage.sprite = NEXT;
        _text.SetText("Stage complete");
        break;
      
      case GameManager.GameState.Lose:
        _buttonImage.sprite = REPEAT;
        _text.SetText("You lose");
        break;
      
      case GameManager.GameState.End:
        _buttonImage.sprite = REPEAT;
        _text.SetText("End of prototype");
        break;
    }
  }

  public void OnClicked()
  {
    background.enabled = false; 
    _panel.SetActive(false);
    GameManager.Instance.UpdateGameState(GameManager.Instance.State);
  }
}
