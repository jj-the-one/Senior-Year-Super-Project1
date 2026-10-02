using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class InterClickedImage : MonoBehaviour, IPointerClickHandler
{
    public IntermediateScript gameManager;
    private bool isLeftImage = true;
    public void OnPointerClick(PointerEventData eventData) {
        if (gameManager.canBeClicked()) {
            Debug.Log("Clicked");
            gameManager.CheckAnswer(isLeftImage);
            
        }
    }
    


}
