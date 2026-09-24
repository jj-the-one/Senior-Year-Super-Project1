using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class clickedImage : MonoBehaviour, IPointerClickHandler
{
    public ClikableImages gameManager;
    public bool isLeftImage;

    public void OnPointerClick(PointerEventData eventData)
    {
        gameManager.CheckAnswer(isLeftImage);
    }
    
}