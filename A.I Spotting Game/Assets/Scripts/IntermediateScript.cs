using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntermediateScript : MonoBehaviour
{
    private PlayerInventory inventory;
    public Image leftImage;
    public Image rightImage;
    public Image oneImage;
    public TMP_Text resultsText;
    public SceneController nextLevel;

    public bool leftReal;
    public bool rightReal;
    public bool oneReal;
    

    private bool canClick = true;

    public List<Sprite> IntermediateRealImages = new List<Sprite>();
    public List<Sprite> IntermediateAIImages = new List<Sprite>();

    private int currentSet;
    private int realIndex;
    private int aiIndex;
    private int y;
    private int u;
    private int whatToCheckFor;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) {
            inventory = player.GetComponent<PlayerInventory>();
        }
        else {
            Debug.LogWarning("Player with tag \"Player\" could not be found.");
        }
        //Finds the image objects
        leftImage = GameObject.Find("LeftImage").GetComponent<Image>();
        rightImage = GameObject.Find("RightImage").GetComponent<Image>();
        oneImage = GameObject.Find("OneImage").GetComponent<Image>();
        resultsText = GameObject.Find("ResultsText").GetComponent<TMP_Text>();
        //Sets 'Correct' 'Incorrect' messages to invisible.
        resultsText.gameObject.SetActive(false);

        //Loads the images into the lists from the folders
        IntermediateAIImages.AddRange(Resources.LoadAll<Sprite>("Photos/Intermediate A.I"));
        IntermediateRealImages.AddRange(Resources.LoadAll<Sprite>("Photos/Intermediate Real"));

        Debug.Log("Intermediate A.I loaded " + IntermediateAIImages.Count);
        Debug.Log("Intermediate Real loaded " + IntermediateRealImages.Count);
        loadSet();
    }

    // Update is called once per frame
    public bool canBeClicked() {
        return canClick;
    }
    public void loadSet() {
        //Setting Images to AI and Real (Left or Right)
        oneImage.gameObject.SetActive(false);
        int x = Random.Range(0, 2);
        if (x == 0) {
            leftReal = true;
            rightReal = false;
        }
        else {
            leftReal = false;
            rightReal = true;
        }
        /*Chooses what kind of round to give to the player
        1. Spot the real Image
        2. Spot the AI image
        3. Determine whever the single image AI or Real
        */
        y = Random.Range(0, 3);
        realIndex = Random.Range(0, IntermediateRealImages.Count);
        aiIndex = Random.Range(0, IntermediateAIImages.Count);
        if (leftReal)
        {
        leftImage.sprite = IntermediateRealImages[realIndex];
        rightImage.sprite = IntermediateAIImages[aiIndex];
        }
        else
        {
            leftImage.sprite = IntermediateAIImages[aiIndex];
            rightImage.sprite = IntermediateRealImages[realIndex];
        }
        
        if (y == 0) {
            Debug.Log("Find real");
            leftImage.gameObject.SetActive(true);
            rightImage.gameObject.SetActive(true);
            SetSimilarSize(leftImage);
            SetSimilarSize(rightImage);
        }
        else if (y == 1) {
            Debug.Log("Find AI");
            leftImage.gameObject.SetActive(true);
            rightImage.gameObject.SetActive(true);
            SetSimilarSize(leftImage);
            SetSimilarSize(rightImage);
        }
        else {
            leftImage.gameObject.SetActive(false);
            rightImage.gameObject.SetActive(false);
            u = Random.Range(0, 2);
            if (u == 0) {
                //Real one image
                oneImage.sprite = IntermediateRealImages[realIndex];
                oneImage.gameObject.SetActive(true);
                SetSimilarSize(oneImage);
            }
            else {
                //Fake one image
                oneImage.sprite = IntermediateAIImages[aiIndex];
                oneImage.gameObject.SetActive(true);
                SetSimilarSize(oneImage);
            }
        }
    }
    public bool CheckAnswer(bool clickedLeft) {
        //To indentify the real image
        if (y == 0) {
            if (!canClick) {
            Debug.Log("Cannot be clicked.");
                return false;
            }
            bool correct = false;
            if (clickedLeft && leftReal) {
                Debug.Log("Left is real.");
                correct = true;
            }
            else if (!clickedLeft && rightReal) {
                Debug.Log("Right is real.");
                correct = true;
            }
            else {
                correct = false;
            }

            if (correct == true){
                inventory.AddScore(1);
                if (inventory.score == 7) {
                    nextLevel.AdvancedStage();
                    inventory.Heal(5);
                }
                StartCoroutine(delay());
                return correct;
            }
            else {
                inventory.TakeDamage(1);
                StartCoroutine(delay());
                return correct;
            }
        }
        //To indentify AI image
        else if (y == 1) {
            if (!canClick) {
                Debug.Log("Cannot be clicked.");
                return false;
            }
            bool correct = false;
            if (!clickedLeft && !leftReal) {
                Debug.Log("Left is real.");
                correct = true;
            }
            else if (!clickedLeft && !rightReal) {
                Debug.Log("Right is real.");
                correct = true;
            }
            else {
                correct = false;
            }

            if (correct == true){
                inventory.AddScore(1);
                if (inventory.score == 7) {
                    nextLevel.AdvancedStage();
                    inventory.Heal(5);
                }
                StartCoroutine(delay());
                return correct;
            }
            else {
                inventory.TakeDamage(1);
                StartCoroutine(delay());
                return correct;
            }
        }
        else {
            return false;
        }
        

    }
    public IEnumerator delay()
    {
        // Disable clicking
        canClick = false;

        // Hide the images
        leftImage.gameObject.SetActive(false);
        rightImage.gameObject.SetActive(false);
        oneImage.gameObject.SetActive(false);

        // Show result text
        resultsText.gameObject.SetActive(true);

        // Wait 2 seconds
        yield return new WaitForSeconds(2.0f);

        // Hide result text
        resultsText.gameObject.SetActive(false);

        // Load new images
        loadSet();

        // Show the new images
        

        // Allow clicking again
        canClick = true;
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
