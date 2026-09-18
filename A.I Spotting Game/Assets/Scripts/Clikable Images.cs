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
    -Showing which answer was right based on the task for the round
    -lookin for images in the arraylist of real and ai images.
===================================================================

    The code:
    private PlayerInventory inventory;
    GameObject player = GameObject.FindGameObjectWithTag("Player");
    PlayerInventory inventory = player.GetComponent<PlayerInventory>();

    bool rightAnswer = false;

    for (int i = 0; i < beginnerimage.Count; i++)
    {
        if (beginnerimage[i] == currentImage)
        {
            rightAnswer = true;
            break;
        }
    }

    if (rightAnswer)
    {
        inventory.AddScore(1);
    }
    else
    {
        inventory.TakeDamage(1);
    }
*/
