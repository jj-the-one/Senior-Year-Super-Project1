using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClikableImages : MonoBehaviour
{
    private PlayerInventory inventory;

    private Image leftImage;
    private Image rightImage;

    public bool leftReal;
    public bool rightReal;

    public List<Sprite> beginnerRealImages = new List<Sprite>();
    public List<Sprite> beginnerAIImages = new List<Sprite>();

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            inventory = player.GetComponent<PlayerInventory>();
        }
        else
        {
            Debug.LogWarning("Player with the 'Player' tag could not be found.");
        }
        leftImage = GameObject.Find("ImageSpotLeft").GetComponent<Image>();
        rightImage = GameObject.Find("ImageSpotRight").GetComponent<Image>();

        // Load images
        beginnerRealImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Beginner Level Real")
        );

        beginnerAIImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Beginner Level A.I")
        );

        Debug.Log("Real images loaded: " + beginnerRealImages.Count);
        Debug.Log("AI images loaded: " + beginnerAIImages.Count);

        // Randomly decide which side is Real
        int x = Random.Range(0, 2);

        if (x == 0)
        {
            leftReal = true;
            rightReal = false;
        }
        else
        {
            leftReal = false;
            rightReal = true;
        }

        // Pick random images
        int realIndex = Random.Range(0, beginnerRealImages.Count);
        int aiIndex = Random.Range(0, beginnerAIImages.Count);

        Sprite realImage = beginnerRealImages[realIndex];
        Sprite aiImage = beginnerAIImages[aiIndex];

        // Put them into the two spots
        if (leftReal)
        {
            leftImage.sprite = realImage;
            rightImage.sprite = aiImage;
        }
        else
        {
            leftImage.sprite = aiImage;
            rightImage.sprite = realImage;
        }
        leftImage.SetNativeSize();
        leftImage.rectTransform.localScale = Vector3.one * 0.5f;
        rightImage.SetNativeSize();
        rightImage.rectTransform.localScale = Vector3.one * 0.5f;
    }
    public bool CheckAnswer(bool clickedLeft) {
        if (clickedLeft && leftReal) {
            Debug.Log("Correct! Left Image is real.");
            inventory.AddScore(1);
            return true;
        }
        else {
            Debug.Log("Wrong! Right image is AI.");
            inventory.TakeDamage(1);
            return false;
        }
        if (!clickedLeft && rightReal) {
            Debug.Log("Correct! Right image is real.");
            inventory.AddScore(1);
            return true;
        }
        else {
            Debug.Log("Wrong! Left image is AI.");
            inventory.TakeDamage(1);
            return false;
        }
    }

    
}