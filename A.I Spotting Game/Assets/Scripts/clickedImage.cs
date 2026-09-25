using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class clickedImage : MonoBehaviour, IPointerClickHandler
{
    public ClikableImages gameManager;
    private bool isLeftImage = true;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("left is clicked");
        gameManager.CheckAnswer(isLeftImage);
    }
    
}