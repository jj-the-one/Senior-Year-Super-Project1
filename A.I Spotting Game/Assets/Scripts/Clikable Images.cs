using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class ClikableImages : MonoBehaviour
{
    private PlayerInventory inventory;

    public Image leftImage;
    public Image rightImage;

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
        realIndex = Random.Range(0, beginnerRealImages.Count);
        aiIndex = Random.Range(0, beginnerAIImages.Count);

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
        SetSimilarSize(leftImage);
        SetSimilarSize(rightImage);
    }
    public bool canClciked() {
        return canClick;
    }
    public bool CheckAnswer(bool clickedLeft) {
        if (!canClick) {
            return false;
        }
        if (clickedLeft && leftReal) {
            Debug.Log("Correct! Left Image is real.");
            inventory.AddScore(1);
            Debug.Log("true (if)");
            return true;
        }
        else if (!clickedLeft && rightReal) {
            Debug.Log("Right true");
            inventory.AddScore(1);
            return true;
        }
        else {
            Debug.Log("Incorrect");
            inventory.TakeDamage(1);
            return false;
        }
 
    }
    public IEnumerator delay() {
        canClick = false;
        yield return new WaitForSeconds(2.0f);
        LoadSet();
        canClick = true;
    }
    public void LoadSet()
    {
        

        realIndex = Random.Range(0, beginnerRealImages.Count);
        aiIndex = Random.Range(0, beginnerAIImages.Count);
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
        if (leftReal) {
            leftImage.sprite = beginnerRealImages[realIndex];
            rightImage.sprite = beginnerAIImages[aiIndex];
        }
        else {
            leftImage.sprite = beginnerAIImages[aiIndex];
            rightImage.sprite = beginnerRealImages[realIndex];
        }
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
        image.rectTransform.localScale = new Vector3(scale, scale, 1f);
    }

    
}