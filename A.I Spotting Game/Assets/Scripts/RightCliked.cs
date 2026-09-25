using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class RightCliked : MonoBehaviour, IPointerClickHandler
{
    public ClikableImages gameManager;
    private bool isRightClicked = false;
    // Start is called before the first frame update
    public void OnPointerClick(PointerEventData eventData) {
        Debug.Log("Right is clicked");
        gameManager.CheckAnswer(isRightClicked);
        gameManager.NextSet();
    }
}
