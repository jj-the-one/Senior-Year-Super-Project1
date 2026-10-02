using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class InterClickedRight : MonoBehaviour, IPointerClickHandler
{
    // Start is called before the first frame update
    public IntermediateScript gameManager;
    private bool isLeftImage = false;
    public void OnPointerClick(PointerEventData eventData) {
        if (gameManager.canBeClicked()) {
            Debug.Log("Clicked");
            gameManager.CheckAnswer(isLeftImage);
            
        }
    }
}
