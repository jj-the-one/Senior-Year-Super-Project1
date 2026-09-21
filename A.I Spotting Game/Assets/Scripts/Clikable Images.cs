using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClikableImages : MonoBehaviour, IPointerClickHandler
{
    private PlayerInventory inventory;
    public List<Texture2D> beginnerRealImages = new List<Texture2D>();
    public List<Texture2D> beginnerAIImages = new List<Texture2D>();
    public Texture2D currentImage;
    public void Start() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<PlayerInventory>();
        //Load images from the two folders
        beginnerRealImages.AddRange(Resources.LoadAll<Texture2D>("Photos/Beginner Level Real"));
        beginnerAIImages.AddRange(Resources.LoadAll<Texture2D>("Photos/Beginner Level A.I"));

        Debug.Log("Real images loaded: " + beginnerRealImages.Count);
        Debug.Log("AI images loaded: " + beginnerAIImages.Count);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Image was clicked!");
        
    }

    public void CheckAnswer()
    {
        bool rightAnswer = beginnerRealImages.Contains(currentImage);

        if (rightAnswer)
        {
            inventory.AddScore(1);
        }
        else
        {
            inventory.TakeDamage(1);
        }
    }
}

/*
if the image was clicked then it has to initiate sequence of actoons:
    -Random change of text
    -Showing which answer was right based on the task for the round
    -lookin for images in the arraylist of real and ai images.
===================================================================

    The code:
   using System.Collections.Generic;
using UnityEngine;

public class ImageQuiz : MonoBehaviour
{
    private PlayerInventory inventory;

    public List<Texture2D> beginnerRealImages = new List<Texture2D>();
    public List<Texture2D> beginnerAIImages = new List<Texture2D>();

    public Texture2D currentImage;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<PlayerInventory>();

        // Load images from the two folders
        beginnerRealImages.AddRange(
            Resources.LoadAll<Texture2D>("Photos/Beginner Level Real")
        );

        beginnerAIImages.AddRange(
            Resources.LoadAll<Texture2D>("Photos/Beginner Level A.I")
        );

        Debug.Log("Real images loaded: " + beginnerRealImages.Count);
        Debug.Log("AI images loaded: " + beginnerAIImages.Count);
    }

    public void CheckAnswer()
    {
        bool rightAnswer = beginnerRealImages.Contains(currentImage);

        if (rightAnswer)
        {
            inventory.AddScore(1);
        }
        else
        {
            inventory.TakeDamage(1);
        }
    }
}

*/
