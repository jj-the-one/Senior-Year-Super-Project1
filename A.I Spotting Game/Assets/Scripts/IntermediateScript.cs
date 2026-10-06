using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntermediateScript: MonoBehaviour
{
    private PlayerInventory inventory;

    [Header("Intermediate Images")]
    public Image leftImage;
    public Image rightImage;
    public Image oneImage;

    [Header("UI")]
    public TMP_Text resultsText;
    public TMP_Text instructions;

    [Header("Next Level")]
    public SceneController nextLevel;

    [Header("Advanced Image Prefabs")]
    public GameObject[] aiPrefabs;
    public GameObject[] realPrefabs;

    [Header("Advanced Buttons")]
    public Button realButton;
    public Button fakeButton;

    [Header("Intermediate Image Lists")]
    public List<Sprite> IntermediateRealImages = new List<Sprite>();
    public List<Sprite> IntermediateAIImages = new List<Sprite>();

    // Intermediate round variables
    public bool leftReal;
    public bool rightReal;
    public bool oneReal;

    private int realIndex;
    private int aiIndex;

    // 0 = Find Real
    // 1 = Find AI
    // 2 = One Image
    private int intermediateRoundType;

    // Advanced round variables
    // True = Real
    // False = AI
    private bool correctAnswer;

    private GameObject currentImage;

    private bool playerAnswered = false;
    private bool playerAnswer = false;

    // True = Advanced round
    // False = Intermediate round
    private bool advancedRound;

    private bool canClick = false;

    private int roundNumber = 1;

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

        // Find UI objects
        if (leftImage == null)
            leftImage = GameObject.Find("LeftImage").GetComponent<Image>();

        if (rightImage == null)
            rightImage = GameObject.Find("RightImage").GetComponent<Image>();

        if (oneImage == null)
            oneImage = GameObject.Find("OneImage").GetComponent<Image>();

        if (resultsText == null)
            resultsText = GameObject.Find("ResultsText").GetComponent<TMP_Text>();

        if (instructions == null)
            instructions = GameObject.Find("InstructionText").GetComponent<TMP_Text>();

        // Hide UI
        resultsText.gameObject.SetActive(false);
        instructions.gameObject.SetActive(false);

        leftImage.gameObject.SetActive(false);
        rightImage.gameObject.SetActive(false);
        oneImage.gameObject.SetActive(false);

        // Disable Advanced buttons
        if (realButton != null)
            realButton.interactable = false;

        if (fakeButton != null)
            fakeButton.interactable = false;

        // Load Intermediate images
        IntermediateAIImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Intermediate A.I")
        );

        IntermediateRealImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Intermediate Real")
        );

        Debug.Log("Intermediate A.I loaded: " + IntermediateAIImages.Count);
        Debug.Log("Intermediate Real loaded: " + IntermediateRealImages.Count);

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        canClick = false;

        instructions.gameObject.SetActive(true);
        instructions.text = "Intermediate Round";

        yield return new WaitForSeconds(2f);

        instructions.gameObject.SetActive(false);

        // Continue until player reaches 7 points
        // or loses all health
        while (inventory != null &&
               inventory.score < 7 &&
               inventory.health > 0)
        {
            yield return StartCoroutine(PlayRound());

            roundNumber++;
        }

        canClick = false;

        HideAllImages();
        DisableAdvancedButtons();

        resultsText.gameObject.SetActive(true);

        if (inventory != null && inventory.score >= 7)
        {
            resultsText.text = "Intermediate Complete!";
        }
        else
        {
            resultsText.text = "Game Over";
        }

        yield return new WaitForSeconds(2f);

        resultsText.gameObject.SetActive(false);

        // Move to Advanced level
        if (inventory != null &&
            inventory.score >= 7 &&
            inventory.health > 0)
        {
            if (nextLevel != null)
            {
                nextLevel.AdvancedStage();
                inventory.Heal(5);
            }
            else
            {
                Debug.LogWarning("Next Level SceneController is not assigned.");
            }
        }
        else
        {
            SceneController sceneController = FindObjectOfType<SceneController>();

            if (sceneController != null)
            {
                sceneController.ResultsPage();
            }
            else
            {
                Debug.LogWarning("SceneController could not be found.");
            }
        }
    }

    IEnumerator PlayRound()
    {
        canClick = false;

        HideAllImages();
        DisableAdvancedButtons();

        // 50% chance for Intermediate
        // 50% chance for Advanced
        advancedRound = Random.value > 0.5f;

        Debug.Log(
            "Round " + roundNumber +
            ": " +
            (advancedRound ? "Advanced" : "Intermediate")
        );

        if (advancedRound)
        {
            yield return StartCoroutine(PlayAdvancedRound());
        }
        else
        {
            yield return StartCoroutine(PlayIntermediateRound());
        }
    }

    IEnumerator PlayIntermediateRound()
    {
        instructions.gameObject.SetActive(true);
        instructions.text = "Round " + roundNumber;

        yield return new WaitForSeconds(2f);

        instructions.gameObject.SetActive(false);

        PrepareIntermediateRound();

        HideAllImages();

        instructions.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        instructions.gameObject.SetActive(false);

        ShowIntermediateImages();

        canClick = true;

        // Wait until the player answers
        yield return new WaitUntil(() => !canClick);
    }

    private void PrepareIntermediateRound()
    {
        // 0 = Find Real
        // 1 = Find AI
        // 2 = One Image
        intermediateRoundType = Random.Range(0, 3);

        realIndex = Random.Range(
            0,
            IntermediateRealImages.Count
        );

        aiIndex = Random.Range(
            0,
            IntermediateAIImages.Count
        );

        // Decide which side contains the real image
        int side = Random.Range(0, 2);

        if (side == 0)
        {
            leftReal = true;
            rightReal = false;
        }
        else
        {
            leftReal = false;
            rightReal = true;
        }

        // Assign left and right images
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

        // Find Real
        if (intermediateRoundType == 0)
        {
            instructions.text = "Find the real image.";

            SetSimilarSize(leftImage);
            SetSimilarSize(rightImage);
        }

        // Find AI
        else if (intermediateRoundType == 1)
        {
            instructions.text = "Find the AI image.";

            SetSimilarSize(leftImage);
            SetSimilarSize(rightImage);
        }

        // One Image
        else
        {
            instructions.text = "Is this real or AI?";

            int randomChoice = Random.Range(0, 2);

            if (randomChoice == 0)
            {
                oneReal = true;
                oneImage.sprite = IntermediateRealImages[realIndex];
            }
            else
            {
                oneReal = false;
                oneImage.sprite = IntermediateAIImages[aiIndex];
            }

            SetSimilarSize(oneImage);
        }
    }

    private void ShowIntermediateImages()
    {
        if (intermediateRoundType == 0)
        {
            // Find Real
            leftImage.gameObject.SetActive(true);
            rightImage.gameObject.SetActive(true);
            oneImage.gameObject.SetActive(false);
        }
        else if (intermediateRoundType == 1)
        {
            // Find AI
            leftImage.gameObject.SetActive(true);
            rightImage.gameObject.SetActive(true);
            oneImage.gameObject.SetActive(false);
        }
        else
        {
            // One Image
            leftImage.gameObject.SetActive(false);
            rightImage.gameObject.SetActive(false);
            oneImage.gameObject.SetActive(true);
        }
    }

    public bool CheckAnswer(bool clickedLeft)
    {
        if (!canClick)
        {
            Debug.Log("Cannot be clicked.");
            return false;
        }

        // This function is only for Intermediate
        // left/right image rounds
        if (advancedRound)
        {
            return false;
        }

        bool correct = false;

        // Find Real
        if (intermediateRoundType == 0)
        {
            if (clickedLeft && leftReal)
            {
                correct = true;
            }
            else if (!clickedLeft && rightReal)
            {
                correct = true;
            }
        }

        // Find AI
        else if (intermediateRoundType == 1)
        {
            if (clickedLeft && !leftReal)
            {
                correct = true;
            }
            else if (!clickedLeft && !rightReal)
            {
                correct = true;
            }
        }

        // One Image should use CheckOneImageAnswer()
        else
        {
            Debug.LogWarning(
                "Use CheckOneImageAnswer() for the One Image round."
            );

            return false;
        }

        FinishAnswer(correct);

        return correct;
    }

    public bool CheckOneImageAnswer(bool playerSaysReal)
    {
        if (!canClick)
        {
            Debug.Log("Cannot be clicked.");
            return false;
        }

        if (advancedRound || intermediateRoundType != 2)
        {
            return false;
        }

        bool correct = playerSaysReal == oneReal;

        FinishAnswer(correct);

        return correct;
    }

    private void FinishAnswer(bool correct)
    {
        canClick = false;

        HideAllImages();

        if (correct)
        {
            resultsText.text = "Correct!";

            if (inventory != null)
            {
                inventory.AddScore(1);
            }
        }
        else
        {
            resultsText.text = "Incorrect!";

            if (inventory != null)
            {
                inventory.TakeDamage(1);
            }
        }

        StartCoroutine(ShowResultAndNextRound());
    }

    IEnumerator PlayAdvancedRound()
    {
        instructions.gameObject.SetActive(true);
        instructions.text = "Round " + roundNumber;

        yield return new WaitForSeconds(2f);

        instructions.gameObject.SetActive(false);

        // Randomly choose Real or AI
        bool chooseReal = Random.value > 0.5f;

        GameObject selectedPrefab;

        if (chooseReal)
        {
            correctAnswer = true;

            if (realPrefabs == null || realPrefabs.Length == 0)
            {
                Debug.LogWarning("No Real prefabs assigned.");
                yield break;
            }

            int randomIndex = Random.Range(
                0,
                realPrefabs.Length
            );

            selectedPrefab = realPrefabs[randomIndex];
        }
        else
        {
            correctAnswer = false;

            if (aiPrefabs == null || aiPrefabs.Length == 0)
            {
                Debug.LogWarning("No AI prefabs assigned.");
                yield break;
            }

            int randomIndex = Random.Range(
                0,
                aiPrefabs.Length
            );

            selectedPrefab = aiPrefabs[randomIndex];
        }

        // Spawn the image
        currentImage = Instantiate(
            selectedPrefab,
            transform.position,
            transform.rotation
        );

        playerAnswered = false;
        playerAnswer = false;

        // Enable Real/Fake buttons
        if (realButton != null)
            realButton.interactable = true;

        if (fakeButton != null)
            fakeButton.interactable = true;

        // Wait for player answer
        yield return new WaitUntil(() => playerAnswered);

        DisableAdvancedButtons();

        bool correct = playerAnswer == correctAnswer;

        canClick = false;

        if (correct)
        {
            resultsText.text = "Correct!";

            if (inventory != null)
            {
                inventory.AddScore(1);
            }
        }
        else
        {
            resultsText.text = "Incorrect!";

            if (inventory != null)
            {
                inventory.TakeDamage(1);
            }
        }

        if (currentImage != null)
        {
            Destroy(currentImage);
            currentImage = null;
        }

        resultsText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        resultsText.gameObject.SetActive(false);
    }

    public void RealButton()
    {
        // If this is an Intermediate One Image round
        if (!advancedRound)
        {
            if (intermediateRoundType == 2)
            {
                CheckOneImageAnswer(true);
            }

            return;
        }

        // Advanced round
        if (!playerAnswered)
        {
            playerAnswer = true;
            playerAnswered = true;
        }
    }

    public void FakeButton()
    {
        // If this is an Intermediate One Image round
        if (!advancedRound)
        {
            if (intermediateRoundType == 2)
            {
                CheckOneImageAnswer(false);
            }

            return;
        }

        // Advanced round
        if (!playerAnswered)
        {
            playerAnswer = false;
            playerAnswered = true;
        }
    }

    IEnumerator ShowResultAndNextRound()
    {
        resultsText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        resultsText.gameObject.SetActive(false);

        if (inventory == null)
            yield break;

        if (inventory.score >= 7 ||
            inventory.health <= 0)
        {
            yield break;
        }

        StartCoroutine(PlayRound());
    }

    private void HideAllImages()
    {
        if (leftImage != null)
            leftImage.gameObject.SetActive(false);

        if (rightImage != null)
            rightImage.gameObject.SetActive(false);

        if (oneImage != null)
            oneImage.gameObject.SetActive(false);

        if (currentImage != null)
        {
            Destroy(currentImage);
            currentImage = null;
        }
    }

    private void DisableAdvancedButtons()
    {
        if (realButton != null)
            realButton.interactable = false;

        if (fakeButton != null)
            fakeButton.interactable = false;
    }

    private void SetSimilarSize(Image image)
    {
        if (image == null || image.sprite == null)
            return;

        float maxWidth = 850f;
        float maxHeight = 600f;

        float imageWidth = image.sprite.rect.width;
        float imageHeight = image.sprite.rect.height;

        float widthScale = maxWidth / imageWidth;
        float heightScale = maxHeight / imageHeight;

        float scale = Mathf.Min(
            widthScale,
            heightScale
        );

        image.SetNativeSize();

        image.rectTransform.localScale =
            new Vector3(scale, scale, 1f);
    }
}
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using UnityEngine.UI;

//public class IntermediateScript : MonoBehaviour
//{
//    private PlayerInventory inventory;

//    public Image leftImage;
//    public Image rightImage;
//    public Image oneImage;

//    public TMP_Text resultsText;
//    public TMP_Text instructions;

//    public SceneController nextLevel;

//    public bool leftReal;
//    public bool rightReal;
//    public bool oneReal;

//    private bool canClick = false;

//    public List<Sprite> IntermediateRealImages = new List<Sprite>();
//    public List<Sprite> IntermediateAIImages = new List<Sprite>();

//    private int currentSet;
//    private int realIndex;
//    private int aiIndex;
//    private int y;
//    private int u;
//    private int whatToCheckFor;


//    // ---------------------------------------------------------
//    // START
//    // ---------------------------------------------------------

//    void Start()
//    {
//        GameObject player = GameObject.FindGameObjectWithTag("Player");

//        if (player != null)
//        {
//            inventory = player.GetComponent<PlayerInventory>();
//        }
//        else
//        {
//            Debug.LogWarning("Player with tag \"Player\" could not be found.");
//        }

//        // Find the image objects
//        leftImage = GameObject.Find("LeftImage").GetComponent<Image>();
//        rightImage = GameObject.Find("RightImage").GetComponent<Image>();
//        oneImage = GameObject.Find("OneImage").GetComponent<Image>();

//        // Find the text objects
//        resultsText = GameObject.Find("ResultsText").GetComponent<TMP_Text>();
//        instructions = GameObject.Find("InstructionText").GetComponent<TMP_Text>();

//        // Hide everything initially
//        resultsText.gameObject.SetActive(false);
//        instructions.gameObject.SetActive(false);

//        leftImage.gameObject.SetActive(false);
//        rightImage.gameObject.SetActive(false);
//        oneImage.gameObject.SetActive(false);

//        // Load the images into the lists
//        IntermediateAIImages.AddRange(
//            Resources.LoadAll<Sprite>("Photos/Intermediate A.I")
//        );

//        IntermediateRealImages.AddRange(
//            Resources.LoadAll<Sprite>("Photos/Intermediate Real")
//        );

//        Debug.Log("Intermediate A.I loaded " + IntermediateAIImages.Count);
//        Debug.Log("Intermediate Real loaded " + IntermediateRealImages.Count);

//        // Start the first round
//        StartCoroutine(StartFirstRound());
//    }


//    // ---------------------------------------------------------
//    // CAN CLICK
//    // ---------------------------------------------------------

//    public bool canBeClicked()
//    {
//        return canClick;
//    }


//    // ---------------------------------------------------------
//    // FIRST ROUND
//    // ---------------------------------------------------------

//    private IEnumerator StartFirstRound()
//    {
//        canClick = false;

//        // Choose the round and prepare everything,
//        // BUT DO NOT SHOW THE IMAGES YET.
//        PrepareSet();

//        // Make absolutely sure images are hidden
//        leftImage.gameObject.SetActive(false);
//        rightImage.gameObject.SetActive(false);
//        oneImage.gameObject.SetActive(false);

//        // Show instruction FIRST
//        instructions.gameObject.SetActive(true);

//        // Wait 2 seconds
//        yield return new WaitForSeconds(2.0f);

//        // Hide instruction
//        instructions.gameObject.SetActive(false);

//        // NOW show the images
//        ShowImages();

//        // Allow player to click
//        canClick = true;
//    }


//    // ---------------------------------------------------------
//    // PREPARE SET
//    // ---------------------------------------------------------

//    private void PrepareSet()
//    {
//        // Hide the single image
//        oneImage.gameObject.SetActive(false);

//        // Decide which side is real
//        int x = Random.Range(0, 2);

//        if (x == 0)
//        {
//            leftReal = true;
//            rightReal = false;
//        }
//        else
//        {
//            leftReal = false;
//            rightReal = true;
//        }

//        /*
//        Chooses what kind of round to give to the player

//        1. Spot the real Image
//        2. Spot the AI image
//        3. Determine whether the single image is AI or Real
//        */

//        y = Random.Range(0, 3);

//        realIndex = Random.Range(0, IntermediateRealImages.Count);
//        aiIndex = Random.Range(0, IntermediateAIImages.Count);

//        // Assign the images
//        if (leftReal)
//        {
//            leftImage.sprite = IntermediateRealImages[realIndex];
//            rightImage.sprite = IntermediateAIImages[aiIndex];
//        }
//        else
//        {
//            leftImage.sprite = IntermediateAIImages[aiIndex];
//            rightImage.sprite = IntermediateRealImages[realIndex];
//        }


//        // -----------------------------------------------------
//        // FIND REAL ROUND
//        // -----------------------------------------------------

//        if (y == 0)
//        {
//            Debug.Log("Find real");

//            instructions.text = "Find the real image.";

//            SetSimilarSize(leftImage);
//            SetSimilarSize(rightImage);
//        }


//        // -----------------------------------------------------
//        // FIND AI ROUND
//        // -----------------------------------------------------

//        else if (y == 1)
//        {
//            Debug.Log("Find AI");

//            instructions.text = "Find the AI image.";

//            SetSimilarSize(leftImage);
//            SetSimilarSize(rightImage);
//        }


//        // -----------------------------------------------------
//        // ONE IMAGE ROUND
//        // -----------------------------------------------------

//        else
//        {
//            instructions.text = "Is this real or AI image?";

//            u = Random.Range(0, 2);

//            if (u == 0)
//            {
//                // Real one image
//                oneImage.sprite = IntermediateRealImages[realIndex];

//                SetSimilarSize(oneImage);
//            }
//            else
//            {
//                // Fake one image
//                oneImage.sprite = IntermediateAIImages[aiIndex];

//                SetSimilarSize(oneImage);
//            }
//        }
//    }


//    // ---------------------------------------------------------
//    // SHOW IMAGES
//    // ---------------------------------------------------------

//    private void ShowImages()
//    {
//        if (y == 0)
//        {
//            // Find Real
//            leftImage.gameObject.SetActive(true);
//            rightImage.gameObject.SetActive(true);
//            oneImage.gameObject.SetActive(false);
//        }
//        else if (y == 1)
//        {
//            // Find AI
//            leftImage.gameObject.SetActive(true);
//            rightImage.gameObject.SetActive(true);
//            oneImage.gameObject.SetActive(false);
//        }
//        else
//        {
//            // One Image Round
//            leftImage.gameObject.SetActive(false);
//            rightImage.gameObject.SetActive(false);
//            oneImage.gameObject.SetActive(true);
//        }
//    }


//    // ---------------------------------------------------------
//    // CHECK ANSWER
//    // ---------------------------------------------------------

//    public bool CheckAnswer(bool clickedLeft)
//    {
//        // To identify the real image
//        if (y == 0)
//        {
//            if (!canClick)
//            {
//                Debug.Log("Cannot be clicked.");
//                return false;
//            }

//            bool correct = false;

//            if (clickedLeft && leftReal)
//            {
//                Debug.Log("Left is real.");
//                correct = true;
//            }
//            else if (!clickedLeft && rightReal)
//            {
//                Debug.Log("Right is real.");
//                correct = true;
//            }
//            else
//            {
//                correct = false;
//            }

//            if (correct == true)
//            {
//                inventory.AddScore(1);

//                if (inventory.score == 7)
//                {
//                    nextLevel.AdvancedStage();
//                    inventory.Heal(5);
//                }

//                resultsText.text = "Correct!";

//                StartCoroutine(delay());

//                return correct;
//            }
//            else
//            {
//                resultsText.text = "Incorrect!";

//                inventory.TakeDamage(1);

//                StartCoroutine(delay());

//                return correct;
//            }
//        }


//        // To identify AI image
//        else if (y == 1)
//        {
//            if (!canClick)
//            {
//                Debug.Log("Cannot be clicked.");
//                return false;
//            }

//            bool correct = false;

//            if (clickedLeft && !leftReal)
//            {
//                Debug.Log("Left is AI.");
//                correct = true;
//            }
//            else if (!clickedLeft && !rightReal)
//            {
//                Debug.Log("Right is AI.");
//                correct = true;
//            }
//            else
//            {
//                correct = false;
//            }

//            if (correct == true)
//            {
//                inventory.AddScore(1);

//                if (inventory.score == 7)
//                {
//                    nextLevel.AdvancedStage();
//                    inventory.Heal(5);
//                }

//                resultsText.text = "Correct!";

//                StartCoroutine(delay());

//                return correct;
//            }
//            else
//            {
//                resultsText.text = "Incorrect!";

//                inventory.TakeDamage(1);

//                StartCoroutine(delay());

//                return correct;
//            }
//        }


//        // One image round
//        else
//        {
//            return false;
//        }
//    }


//    // ---------------------------------------------------------
//    // ROUND TRANSITION
//    // ---------------------------------------------------------

//    public IEnumerator delay()
//    {
//        // Disable clicking
//        canClick = false;

//        // Hide all images
//        leftImage.gameObject.SetActive(false);
//        rightImage.gameObject.SetActive(false);
//        oneImage.gameObject.SetActive(false);

//        // Make sure instruction is hidden
//        instructions.gameObject.SetActive(false);

//        // Show result
//        resultsText.gameObject.SetActive(true);

//        // Wait 2 seconds
//        yield return new WaitForSeconds(2.0f);

//        // Hide result
//        resultsText.gameObject.SetActive(false);


//        // -----------------------------------------------------
//        // PREPARE NEXT ROUND
//        // -----------------------------------------------------

//        PrepareSet();

//        // Make sure images are STILL hidden
//        leftImage.gameObject.SetActive(false);
//        rightImage.gameObject.SetActive(false);
//        oneImage.gameObject.SetActive(false);

//        // Show instruction
//        instructions.gameObject.SetActive(true);

//        // Wait 2 seconds
//        yield return new WaitForSeconds(2.0f);

//        // Hide instruction
//        instructions.gameObject.SetActive(false);

//        // NOW show the new images
//        ShowImages();

//        // Allow clicking
//        canClick = true;
//    }


//    // ---------------------------------------------------------
//    // IMAGE SIZE
//    // ---------------------------------------------------------

//    private void SetSimilarSize(Image image)
//    {
//        float maxWidth = 850f;
//        float maxHeight = 600f;

//        float imageWidth = image.sprite.rect.width;
//        float imageHeight = image.sprite.rect.height;

//        float widthScale = maxWidth / imageWidth;
//        float heightScale = maxHeight / imageHeight;

//        float scale = Mathf.Min(widthScale, heightScale);

//        image.SetNativeSize();

//        image.rectTransform.localScale =
//            new Vector3(scale, scale, 1f);
//    }
//}