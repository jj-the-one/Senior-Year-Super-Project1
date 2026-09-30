using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class ClikableImages : MonoBehaviour
{
    private PlayerInventory inventory;

    public Image leftImage;
    public Image rightImage;
    public TMP_Text resultsText;
    public SceneController nextLevel;

    public bool leftReal;
    public bool rightReal;

    private bool canClick = true;

    public List<Sprite> beginnerRealImages = new List<Sprite>();
    public List<Sprite> beginnerAIImages = new List<Sprite>();

    private int currentSet;
    private int realIndex;
    private int aiIndex;

    void Start()
    {   
        nextLevel = FindObjectOfType<SceneController>();
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
        resultsText = GameObject.Find("Results Text(Correct)").GetComponent<TMP_Text>();

        // Hide result text at the beginning
        resultsText.gameObject.SetActive(false);

        // Load images
        beginnerRealImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Beginner Level Real")
        );

        beginnerAIImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Beginner Level A.I")
        );

        Debug.Log("Real images loaded: " + beginnerRealImages.Count);
        Debug.Log("AI images loaded: " + beginnerAIImages.Count);

        // Load the first set
        LoadSet();

        // Make sure images are visible
        leftImage.gameObject.SetActive(true);
        rightImage.gameObject.SetActive(true);
    }

    public bool canClciked()
    {
        return canClick;
    }

    public bool CheckAnswer(bool clickedLeft)
    {
        if (!canClick)
        {
            return false;
        }

        bool correct = false;

        // Check if the clicked image is the real image
        if (clickedLeft && leftReal)
        {
            Debug.Log("Correct! Left Image is real.");
            correct = true;
        }
        else if (!clickedLeft && rightReal)
        {
            Debug.Log("Correct! Right Image is real.");
            correct = true;
        }
        else
        {
            Debug.Log("Incorrect");
            correct = false;
        }

        // Give/take score
        if (correct)
        {
            inventory.AddScore(1);
            resultsText.text = "Correct!";
            if (inventory.score == 10) {
                nextLevel.IntermediateStage();
                inventory.Heal(5);
            }
        }
        else
        {
            inventory.TakeDamage(1);
            resultsText.text = "Incorrect!";
        }

        // Start the transition
        StartCoroutine(delay());

        return correct;
    }

    public IEnumerator delay()
    {
        // Disable clicking
        canClick = false;

        // Hide the images
        leftImage.gameObject.SetActive(false);
        rightImage.gameObject.SetActive(false);

        // Show result text
        resultsText.gameObject.SetActive(true);

        // Wait 2 seconds
        yield return new WaitForSeconds(2.0f);

        // Hide result text
        resultsText.gameObject.SetActive(false);

        // Load new images
        LoadSet();

        // Show the new images
        leftImage.gameObject.SetActive(true);
        rightImage.gameObject.SetActive(true);

        // Allow clicking again
        canClick = true;
    }

    public void LoadSet()
    {
        realIndex = Random.Range(0, beginnerRealImages.Count);
        aiIndex = Random.Range(0, beginnerAIImages.Count);

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

        // Put images into the two spots
        if (leftReal)
        {
            leftImage.sprite = beginnerRealImages[realIndex];
            rightImage.sprite = beginnerAIImages[aiIndex];
        }
        else
        {
            leftImage.sprite = beginnerAIImages[aiIndex];
            rightImage.sprite = beginnerRealImages[realIndex];
        }

        // Make their sizes similar
        SetSimilarSize(leftImage);
        SetSimilarSize(rightImage);
    }

    public void NextSet()
    {
        StartCoroutine(delay());
    }

    private void SetSimilarSize(Image image)
    {
        float maxWidth = 850f;
        float maxHeight = 600f;

        float imageWidth = image.sprite.rect.width;
        float imageHeight = image.sprite.rect.height;

        float widthScale = maxWidth / imageWidth;
        float heightScale = maxHeight / imageHeight;

        float scale = Mathf.Min(widthScale, heightScale);

        image.SetNativeSize();

        image.rectTransform.localScale =
            new Vector3(scale, scale, 1f);
    }
}