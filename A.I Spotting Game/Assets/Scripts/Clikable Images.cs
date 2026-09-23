using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ClikableImages : MonoBehaviour
{
    private Image leftImage;
    private Image rightImage;

    public bool leftReal;
    public bool rightReal;

    public List<Sprite> beginnerRealImages = new List<Sprite>();
    public List<Sprite> beginnerAIImages = new List<Sprite>();

    void Start()
    {
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
    }
}