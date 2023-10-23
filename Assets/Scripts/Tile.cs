using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Tile : MonoBehaviour
{
   public string TileName;
   public Rigidbody rigidBody;
   public bool isClicked = false;

   private void Start()
   {
      rigidBody = GetComponent<Rigidbody>();
      rigidBody.isKinematic = true;
      rigidBody.freezeRotation = true;
   }

   private void OnMouseDown()
   {
      ActionManager.onTileClicked?.Invoke(this);
   }
}
