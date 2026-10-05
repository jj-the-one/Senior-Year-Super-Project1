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
    public TMP_Text instructions;

    public SceneController nextLevel;

    public bool leftReal;
    public bool rightReal;
    public bool oneReal;

    private bool canClick = false;

    public List<Sprite> IntermediateRealImages = new List<Sprite>();
    public List<Sprite> IntermediateAIImages = new List<Sprite>();

    private int currentSet;
    private int realIndex;
    private int aiIndex;
    private int y;
    private int u;
    private int whatToCheckFor;


    // ---------------------------------------------------------
    // START
    // ---------------------------------------------------------

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            inventory = player.GetComponent<PlayerInventory>();
        }
        else
        {
            Debug.LogWarning("Player with tag \"Player\" could not be found.");
        }

        // Find the image objects
        leftImage = GameObject.Find("LeftImage").GetComponent<Image>();
        rightImage = GameObject.Find("RightImage").GetComponent<Image>();
        oneImage = GameObject.Find("OneImage").GetComponent<Image>();

        // Find the text objects
        resultsText = GameObject.Find("ResultsText").GetComponent<TMP_Text>();
        instructions = GameObject.Find("InstructionText").GetComponent<TMP_Text>();

        // Hide everything initially
        resultsText.gameObject.SetActive(false);
        instructions.gameObject.SetActive(false);

        leftImage.gameObject.SetActive(false);
        rightImage.gameObject.SetActive(false);
        oneImage.gameObject.SetActive(false);

        // Load the images into the lists
        IntermediateAIImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Intermediate A.I")
        );

        IntermediateRealImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Intermediate Real")
        );

        Debug.Log("Intermediate A.I loaded " + IntermediateAIImages.Count);
        Debug.Log("Intermediate Real loaded " + IntermediateRealImages.Count);

        // Start the first round
        StartCoroutine(StartFirstRound());
    }


    // ---------------------------------------------------------
    // CAN CLICK
    // ---------------------------------------------------------

    public bool canBeClicked()
    {
        return canClick;
    }


    // ---------------------------------------------------------
    // FIRST ROUND
    // ---------------------------------------------------------

    private IEnumerator StartFirstRound()
    {
        canClick = false;

        // Choose the round and prepare everything,
        // BUT DO NOT SHOW THE IMAGES YET.
        PrepareSet();

        // Make absolutely sure images are hidden
        leftImage.gameObject.SetActive(false);
        rightImage.gameObject.SetActive(false);
        oneImage.gameObject.SetActive(false);

        // Show instruction FIRST
        instructions.gameObject.SetActive(true);

        // Wait 2 seconds
        yield return new WaitForSeconds(2.0f);

        // Hide instruction
        instructions.gameObject.SetActive(false);

        // NOW show the images
        ShowImages();

        // Allow player to click
        canClick = true;
    }


    // ---------------------------------------------------------
    // PREPARE SET
    // ---------------------------------------------------------

    private void PrepareSet()
    {
        // Hide the single image
        oneImage.gameObject.SetActive(false);

        // Decide which side is real
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

        /*
        Chooses what kind of round to give to the player

        1. Spot the real Image
        2. Spot the AI image
        3. Determine whether the single image is AI or Real
        */

        y = Random.Range(0, 3);

        realIndex = Random.Range(0, IntermediateRealImages.Count);
        aiIndex = Random.Range(0, IntermediateAIImages.Count);

        // Assign the images
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


        // -----------------------------------------------------
        // FIND REAL ROUND
        // -----------------------------------------------------

        if (y == 0)
        {
            Debug.Log("Find real");

            instructions.text = "Find the real image.";

            SetSimilarSize(leftImage);
            SetSimilarSize(rightImage);
        }


        // -----------------------------------------------------
        // FIND AI ROUND
        // -----------------------------------------------------

        else if (y == 1)
        {
            Debug.Log("Find AI");

            instructions.text = "Find the AI image.";

            SetSimilarSize(leftImage);
            SetSimilarSize(rightImage);
        }


        // -----------------------------------------------------
        // ONE IMAGE ROUND
        // -----------------------------------------------------

        else
        {
            instructions.text = "Is this real or AI image?";

            u = Random.Range(0, 2);

            if (u == 0)
            {
                // Real one image
                oneImage.sprite = IntermediateRealImages[realIndex];

                SetSimilarSize(oneImage);
            }
            else
            {
                // Fake one image
                oneImage.sprite = IntermediateAIImages[aiIndex];

                SetSimilarSize(oneImage);
            }
        }
    }


    // ---------------------------------------------------------
    // SHOW IMAGES
    // ---------------------------------------------------------

    private void ShowImages()
    {
        if (y == 0)
        {
            // Find Real
            leftImage.gameObject.SetActive(true);
            rightImage.gameObject.SetActive(true);
            oneImage.gameObject.SetActive(false);
        }
        else if (y == 1)
        {
            // Find AI
            leftImage.gameObject.SetActive(true);
            rightImage.gameObject.SetActive(true);
            oneImage.gameObject.SetActive(false);
        }
        else
        {
            // One Image Round
            leftImage.gameObject.SetActive(false);
            rightImage.gameObject.SetActive(false);
            oneImage.gameObject.SetActive(true);
        }
    }


    // ---------------------------------------------------------
    // CHECK ANSWER
    // ---------------------------------------------------------

    public bool CheckAnswer(bool clickedLeft)
    {
        // To identify the real image
        if (y == 0)
        {
            if (!canClick)
            {
                Debug.Log("Cannot be clicked.");
                return false;
            }

            bool correct = false;

            if (clickedLeft && leftReal)
            {
                Debug.Log("Left is real.");
                correct = true;
            }
            else if (!clickedLeft && rightReal)
            {
                Debug.Log("Right is real.");
                correct = true;
            }
            else
            {
                correct = false;
            }

            if (correct == true)
            {
                inventory.AddScore(1);

                if (inventory.score == 7)
                {
                    nextLevel.AdvancedStage();
                    inventory.Heal(5);
                }

                resultsText.text = "Correct!";

                StartCoroutine(delay());

                return correct;
            }
            else
            {
                resultsText.text = "Incorrect!";

                inventory.TakeDamage(1);

                StartCoroutine(delay());

                return correct;
            }
        }


        // To identify AI image
        else if (y == 1)
        {
            if (!canClick)
            {
                Debug.Log("Cannot be clicked.");
                return false;
            }

            bool correct = false;

            if (clickedLeft && !leftReal)
            {
                Debug.Log("Left is AI.");
                correct = true;
            }
            else if (!clickedLeft && !rightReal)
            {
                Debug.Log("Right is AI.");
                correct = true;
            }
            else
            {
                correct = false;
            }

            if (correct == true)
            {
                inventory.AddScore(1);

                if (inventory.score == 7)
                {
                    nextLevel.AdvancedStage();
                    inventory.Heal(5);
                }

                resultsText.text = "Correct!";

                StartCoroutine(delay());

                return correct;
            }
            else
            {
                resultsText.text = "Incorrect!";

                inventory.TakeDamage(1);

                StartCoroutine(delay());

                return correct;
            }
        }


        // One image round
        else
        {
            return false;
        }
    }


    // ---------------------------------------------------------
    // ROUND TRANSITION
    // ---------------------------------------------------------

    public IEnumerator delay()
    {
        // Disable clicking
        canClick = false;

        // Hide all images
        leftImage.gameObject.SetActive(false);
        rightImage.gameObject.SetActive(false);
        oneImage.gameObject.SetActive(false);

        // Make sure instruction is hidden
        instructions.gameObject.SetActive(false);

        // Show result
        resultsText.gameObject.SetActive(true);

        // Wait 2 seconds
        yield return new WaitForSeconds(2.0f);

        // Hide result
        resultsText.gameObject.SetActive(false);


        // -----------------------------------------------------
        // PREPARE NEXT ROUND
        // -----------------------------------------------------

        PrepareSet();

        // Make sure images are STILL hidden
        leftImage.gameObject.SetActive(false);
        rightImage.gameObject.SetActive(false);
        oneImage.gameObject.SetActive(false);

        // Show instruction
        instructions.gameObject.SetActive(true);

        // Wait 2 seconds
        yield return new WaitForSeconds(2.0f);

        // Hide instruction
        instructions.gameObject.SetActive(false);

        // NOW show the new images
        ShowImages();

        // Allow clicking
        canClick = true;
    }


    // ---------------------------------------------------------
    // IMAGE SIZE
    // ---------------------------------------------------------

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