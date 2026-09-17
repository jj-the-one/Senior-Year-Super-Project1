using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClikableImages : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Image was clicked!");
    }
}
/*
if the image was clicked then it has to initiate sequence of actoons:
    -Random change of text
    -Showing whic answer was right based on the task for the round
    -
*/
