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
    public TMP_Text introduction;
    public Image leftBorder;
    public Image rightBorder;

    public bool leftReal;
    public bool rightReal;
    public AudioSource audioSource;
    public AudioClip correctSound; 
    public AudioClip wrongSound;

    private bool canClick = true;

    public List<Sprite> beginnerRealImages = new List<Sprite>();
    public List<Sprite> beginnerAIImages = new List<Sprite>();

    private int currentSet;
    private int realIndex;
    private int aiIndex;
    private float borderThickness = 1f;

    IEnumerator Start()
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
        leftBorder = GameObject.Find("BorderLeft").GetComponent<Image>();
        rightBorder = GameObject.Find("BorderRight").GetComponent<Image>();
        leftImage = GameObject.Find("ImageSpotLeft").GetComponent<Image>();
        rightImage = GameObject.Find("ImageSpotRight").GetComponent<Image>();
        resultsText = GameObject.Find("Results Text(Correct)").GetComponent<TMP_Text>();
        introduction = GameObject.Find("Introduction").GetComponent<TMP_Text>();
        leftImage.gameObject.SetActive(false);
        rightImage.gameObject.SetActive(false);
        leftBorder.gameObject.SetActive(false);
        rightBorder.gameObject.SetActive(false);

        resultsText.gameObject.SetActive(false);
        introduction.text = "Find the Real image.";
        introduction.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        introduction.gameObject.SetActive(false);
        // Hide result text at the beginning
       

        // Load images
        beginnerRealImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Beginner Level Real")
        );

        beginnerAIImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Beginner Level A.I")
        );

        Debug.Log("Real images loaded: " + beginnerRealImages.Count);
        Debug.Log("AI images loaded: " + beginnerAIImages.Count);
        introduction.text = "Find the Real image.";
        
        
      
        
        // Load the first set
        LoadSet();

        // Make sure images are visible
        leftBorder.gameObject.SetActive(true);
        leftImage.gameObject.SetActive(true);
        rightBorder.gameObject.SetActive(true);
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
            audioSource.PlayOneShot(correctSound);
            inventory.AddScore(1);
            resultsText.text = "Correct!";
            if (inventory.score == 7) {
                nextLevel.IntermediateStage();
                inventory.Heal(5);
            }
        }
        else
        {
            audioSource.PlayOneShot(wrongSound);
            inventory.TakeDamage(1);
            resultsText.text = "Incorrect!";
        }

        // Start the transition
        StartCoroutine(delay());

        return correct;
    }
    public IEnumerator waiting() {
        introduction.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        introduction.gameObject.SetActive(false);

    }

    public IEnumerator delay()
    {
        // Disable clicking
        canClick = false;

        // Hide the images
        leftBorder.gameObject.SetActive(false);
        leftImage.gameObject.SetActive(false);
        rightBorder.gameObject.SetActive(false);
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
        leftBorder.gameObject.SetActive(true);
        leftImage.gameObject.SetActive(true);
        rightBorder.gameObject.SetActive(true);
        rightImage.gameObject.SetActive(true);

        // Allow clicking again
        canClick = true;
    }
    public IEnumerator hold() {
        yield return new WaitForSeconds(2.0f);
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
        SetSimilarSize(leftImage, leftBorder);
        SetSimilarSize(rightImage, rightBorder);
    }

    public void NextSet()
    {
        StartCoroutine(delay());
    }

 private void SetSimilarSize(Image image, Image border)
{
    float maxWidth = 850f;
    float maxHeight = 600f;
    float borderThickness = 8f;

    float imageWidth = image.sprite.rect.width;
    float imageHeight = image.sprite.rect.height;

    float widthScale = maxWidth / imageWidth;
    float heightScale = maxHeight / imageHeight;

    float scale = Mathf.Min(widthScale, heightScale);

    // Original image sizing
    image.SetNativeSize();

    image.rectTransform.localScale =
        new Vector3(scale, scale, 1f);

    // Calculate border size
    RectTransform borderRect = border.rectTransform;

    borderRect.localScale = new Vector3(scale, scale, 1f);

    borderRect.sizeDelta = new Vector2(
        imageWidth + (borderThickness * 2 / scale),
        imageHeight + (borderThickness * 2 / scale)
    );

    // Match position
    borderRect.position = image.rectTransform.position;

    // Place border behind the image if they are siblings
    if (border.transform.parent == image.transform.parent)
    {
        border.transform.SetSiblingIndex(image.transform.GetSiblingIndex());
        image.transform.SetAsLastSibling();
    }
}
}