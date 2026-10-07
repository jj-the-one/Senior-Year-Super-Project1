//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using TMPro;
//using UnityEngine.UI;

//public class IntermediateScript : MonoBehaviour
//{
//    private PlayerInventory inventory;
//    [Header("Image Prefabs")]
//    public GameObject[] aiPrefabs;
//    public GameObject[] realPrefabs;

//    [Header("UI")]
//    public TMP_Text displayText;
//    public Button realButton;
//    public Button fakeButton;
//    private bool correctAnswer;
//    private GameObject currentImage;
//    private bool playerAnswered = false;
//    private bool playerAnswer = false;
//    private int turn = 1;

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
//            yield return StartCoroutine(PlayTurn());
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

//    IEnumerator PlayTurn()
//    {
//        // Show round number
//        displayText.text = "Round " + turn;

//        // Round text stays for 2 seconds
//        yield return new WaitForSeconds(2f);

//        // Hide TMP while player is looking at the image
//        displayText.text = "";

//        // Randomly decide whether to use an AI or Real image
//        bool chooseReal = Random.value > 0.5f;

//        GameObject selectedPrefab;

//        if (chooseReal)
//        {
//            // Real image
//            correctAnswer = true;

//            int randomIndex = Random.Range(0, realPrefabs.Length);
//            selectedPrefab = realPrefabs[randomIndex];
//        }
//        else
//        {
//            // AI image
//            correctAnswer = false;

//            int randomIndex = Random.Range(0, aiPrefabs.Length);
//            selectedPrefab = aiPrefabs[randomIndex];
//        }

//        // Spawn the image at the same position as this GameObject
//        currentImage = Instantiate(
//            selectedPrefab,
//            transform.position,
//            transform.rotation
//        );

//        // Reset the player's answer
//        playerAnswered = false;

//        // Enable the buttons
//        realButton.interactable = true;
//        fakeButton.interactable = true;

//        // Wait until the player clicks one of the buttons
//        yield return new WaitUntil(() => playerAnswered);

//        // Disable the buttons after answering
//        realButton.interactable = false;
//        fakeButton.interactable = false;

//        // Check the player's answer
//        if (playerAnswer == correctAnswer)
//        {
//            displayText.text = "Correct!";

//            if (inventory != null)
//            {
//                inventory.AddScore(1);
//            }
//        }
//        else
//        {
//            displayText.text = "Incorrect!";

//            if (inventory != null)
//            {
//                inventory.TakeDamage(1);
//            }
//        }

//        // Show Correct/Incorrect for 2 seconds
//        yield return new WaitForSeconds(2f);

//        // Destroy the current image
//        if (currentImage != null)
//        {
//            Destroy(currentImage);
//        }
//    }

//    // Called by the REAL button
//    public void RealButton()
//    {
//        playerAnswer = true;
//        playerAnswered = true;
//    }

//    // Called by the FAKE/AI button
//    public void FakeButton()
//    {
//        playerAnswer = false;
//        playerAnswered = true;
//    }
//}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class IntermediateScript : MonoBehaviour
{
    private PlayerInventory inventory;

    [Header("Image Prefabs")]
    public GameObject[] aiPrefabs;
    public GameObject[] realPrefabs;

    [Header("UI")]
    public TMP_Text displayText;
    public Button realButton;
    public Button fakeButton;

    [Header("Intermediate Images")]
    public Image leftImage;
    public Image rightImage;
    public Image oneImage;

    [Header("Text")]
    public TMP_Text resultsText;
    public TMP_Text instructions;

    [Header("Level Transition")]
    public SceneController nextLevel;

    [Header("Image Lists")]
    public List<Sprite> IntermediateRealImages = new List<Sprite>();
    public List<Sprite> IntermediateAIImages = new List<Sprite>();

    [Header("Round Information")]
    public bool leftReal;
    public bool rightReal;
    public bool oneReal;

    private bool canClick = false;

    private bool correctAnswer;
    private GameObject currentImage;

    private bool playerAnswered = false;
    private bool playerAnswer = false;

    private int turn = 1;

    // 0 = Find Real
    // 1 = Find AI
    // 2 = One Image
    private int y;


    // =========================================================
    // START
    // =========================================================

    void Start()
    {
        // -----------------------------------------------------
        // FIND PLAYER
        // -----------------------------------------------------

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            inventory = player.GetComponent<PlayerInventory>();

            if (inventory == null)
            {
                Debug.LogError("Player object does not have a PlayerInventory component.");
            }
        }
        else
        {
            Debug.LogWarning("Player with tag \"Player\" could not be found.");
        }


        // -----------------------------------------------------
        // FIND UI OBJECTS
        // -----------------------------------------------------

        if (leftImage == null)
        {
            GameObject leftObject = GameObject.Find("LeftImage");

            if (leftObject != null)
            {
                leftImage = leftObject.GetComponent<Image>();
            }
        }

        if (rightImage == null)
        {
            GameObject rightObject = GameObject.Find("RightImage");

            if (rightObject != null)
            {
                rightImage = rightObject.GetComponent<Image>();
            }
        }

        if (oneImage == null)
        {
            GameObject oneObject = GameObject.Find("OneImage");

            if (oneObject != null)
            {
                oneImage = oneObject.GetComponent<Image>();
            }
        }

        if (resultsText == null)
        {
            GameObject resultsObject = GameObject.Find("ResultsText");

            if (resultsObject != null)
            {
                resultsText = resultsObject.GetComponent<TMP_Text>();
            }
        }

        if (instructions == null)
        {
            GameObject instructionObject = GameObject.Find("InstructionText");

            if (instructionObject != null)
            {
                instructions = instructionObject.GetComponent<TMP_Text>();
            }
        }


        // -----------------------------------------------------
        // CHECK REQUIRED UI
        // -----------------------------------------------------

        if (leftImage == null)
        {
            Debug.LogError("LeftImage could not be found.");
        }

        if (rightImage == null)
        {
            Debug.LogError("RightImage could not be found.");
        }

        if (oneImage == null)
        {
            Debug.LogError("OneImage could not be found.");
        }

        if (resultsText == null)
        {
            Debug.LogError("ResultsText could not be found.");
        }

        if (instructions == null)
        {
            Debug.LogError("InstructionText could not be found.");
        }


        // -----------------------------------------------------
        // LOAD SPRITES FROM RESOURCES
        // -----------------------------------------------------


        IntermediateAIImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Intermediate A.I")
        );
        
        IntermediateRealImages.AddRange(
            Resources.LoadAll<Sprite>("Photos/Intermediate Real")
        );

        Debug.Log(
            "Intermediate A.I loaded: " +
            IntermediateAIImages.Count
        );

        Debug.Log(
            "Intermediate Real loaded: " +
            IntermediateRealImages.Count
        );


        // -----------------------------------------------------
        // HIDE UI
        // -----------------------------------------------------

        HideAllImages();

        if (resultsText != null)
        {
            resultsText.gameObject.SetActive(false);
        }

        if (instructions != null)
        {
            instructions.gameObject.SetActive(false);
        }

        if (displayText != null)
        {
            displayText.gameObject.SetActive(false);
        }

        DisableAnswerButtons();


        // -----------------------------------------------------
        // START FIRST ROUND
        // -----------------------------------------------------

        StartCoroutine(StartRound());
    }


    // =========================================================
    // CAN CLICK
    // =========================================================

    public bool canBeClicked()
    {
        return canClick;
    }


    // =========================================================
    // START ROUND
    // =========================================================

    private IEnumerator StartRound()
    {
        canClick = false;

        DisableAnswerButtons();

        HideAllImages();

        if (resultsText != null)
        {
            resultsText.gameObject.SetActive(false);
        }


        // -----------------------------------------------------
        // PREPARE ROUND
        // -----------------------------------------------------

        PrepareSet();


        // -----------------------------------------------------
        // ONE IMAGE ROUND
        // -----------------------------------------------------

        if (y == 2)
        {
            yield return StartCoroutine(PlayTurn());

            yield return StartCoroutine(FinishRound());

            yield break;
        }


        // -----------------------------------------------------
        // TWO IMAGE ROUND
        // -----------------------------------------------------

        if (instructions != null)
        {
            instructions.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        if (instructions != null)
        {
            instructions.gameObject.SetActive(false);
        }

        ShowImages();

        canClick = true;
    }


    // =========================================================
    // PREPARE SET
    // =========================================================

    private void PrepareSet()
    {
        HideAllImages();

        DisableAnswerButtons();


        // -----------------------------------------------------
        // RANDOMLY SELECT ROUND TYPE
        // -----------------------------------------------------

        y = Random.Range(0, 3);


        // -----------------------------------------------------
        // CHECK THAT IMAGE LISTS CONTAIN IMAGES
        // -----------------------------------------------------

        if (IntermediateRealImages.Count == 0)
        {
            Debug.LogError(
                "No Real images found in Resources/Photos/Intermediate Real"
            );

            return;
        }

        if (IntermediateAIImages.Count == 0)
        {
            Debug.LogError(
                "No AI images found in Resources/Photos/Intermediate A.I"
            );

            return;
        }


        // -----------------------------------------------------
        // FIND REAL / AI ROUND
        // -----------------------------------------------------

        if (y == 0 || y == 1)
        {
            int realIndex = Random.Range(
                0,
                IntermediateRealImages.Count
            );

            int aiIndex = Random.Range(
                0,
                IntermediateAIImages.Count
            );


            // Randomly determine which side contains the real image

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


            // -------------------------------------------------
            // ASSIGN SPRITES
            // -------------------------------------------------

            if (leftReal)
            {
                leftImage.sprite =
                    IntermediateRealImages[realIndex];

                rightImage.sprite =
                    IntermediateAIImages[aiIndex];
            }
            else
            {
                leftImage.sprite =
                    IntermediateAIImages[aiIndex];

                rightImage.sprite =
                    IntermediateRealImages[realIndex];
            }


            // -------------------------------------------------
            // FIND REAL
            // -------------------------------------------------

            if (y == 0)
            {
                Debug.Log("Intermediate Round: Find Real");

                instructions.text =
                    "Find the real image.";

                SetSimilarSize(leftImage);
                SetSimilarSize(rightImage);
            }


            // -------------------------------------------------
            // FIND AI
            // -------------------------------------------------

            else
            {
                Debug.Log("Intermediate Round: Find AI");

                instructions.text =
                    "Find the AI image.";

                SetSimilarSize(leftImage);
                SetSimilarSize(rightImage);
            }
        }


        // -----------------------------------------------------
        // ONE IMAGE ROUND
        // -----------------------------------------------------

        else
        {
            Debug.Log("Intermediate Round: Real or AI");

            instructions.text =
                "Is this image real or AI?";
        }
    }


    // =========================================================
    // SHOW IMAGES
    // =========================================================

    private void ShowImages()
    {
        HideAllImages();

        if (y == 0 || y == 1)
        {
            if (leftImage != null)
            {
                leftImage.gameObject.SetActive(true);
            }

            if (rightImage != null)
            {
                rightImage.gameObject.SetActive(true);
            }
        }
        else
        {
            if (oneImage != null)
            {
                oneImage.gameObject.SetActive(true);
            }
        }
    }


    // =========================================================
    // CHECK TWO-IMAGE ANSWER
    // =========================================================

    public bool CheckAnswer(bool clickedLeft)
    {
        if (!canClick)
        {
            Debug.Log("Cannot be clicked right now.");
            return false;
        }


        // -----------------------------------------------------
        // ONLY TWO-IMAGE ROUNDS USE THIS FUNCTION
        // -----------------------------------------------------

        if (y != 0 && y != 1)
        {
            Debug.LogWarning(
                "CheckAnswer() was called during a one-image round."
            );

            return false;
        }


        canClick = false;


        bool correct = false;


        // -----------------------------------------------------
        // FIND REAL
        // -----------------------------------------------------

        if (y == 0)
        {
            if (clickedLeft && leftReal)
            {
                Debug.Log("Player selected the correct REAL image.");
                correct = true;
            }
            else if (!clickedLeft && rightReal)
            {
                Debug.Log("Player selected the correct REAL image.");
                correct = true;
            }
        }


        // -----------------------------------------------------
        // FIND AI
        // -----------------------------------------------------

        else if (y == 1)
        {
            if (clickedLeft && !leftReal)
            {
                Debug.Log("Player selected the correct AI image.");
                correct = true;
            }
            else if (!clickedLeft && !rightReal)
            {
                Debug.Log("Player selected the correct AI image.");
                correct = true;
            }
        }


        // -----------------------------------------------------
        // CORRECT
        // -----------------------------------------------------

        if (correct)
        {
            if (inventory != null)
            {
                inventory.AddScore(1);
            }

            if (resultsText != null)
            {
                resultsText.text = "Correct!";
            }
        }


        // -----------------------------------------------------
        // INCORRECT
        // -----------------------------------------------------

        else
        {
            if (inventory != null)
            {
                inventory.TakeDamage(1);
            }

            if (resultsText != null)
            {
                resultsText.text = "Incorrect!";
            }
        }


        // -----------------------------------------------------
        // SHOW RESULT AND CONTINUE
        // -----------------------------------------------------

        StartCoroutine(FinishRound());

        return correct;
    }


    // =========================================================
    // ONE IMAGE ROUND
    // =========================================================

    private IEnumerator PlayTurn()
    {
        canClick = false;

        DisableAnswerButtons();

        HideAllImages();


        // -----------------------------------------------------
        // SHOW ROUND NUMBER
        // -----------------------------------------------------

        if (displayText != null)
        {
            displayText.gameObject.SetActive(true);
            displayText.text = "Round " + turn;
        }

        yield return new WaitForSeconds(2f);


        // -----------------------------------------------------
        // HIDE ROUND NUMBER
        // -----------------------------------------------------

        if (displayText != null)
        {
            displayText.text = "";
            displayText.gameObject.SetActive(false);
        }


        // -----------------------------------------------------
        // RANDOMLY CHOOSE REAL OR AI
        // -----------------------------------------------------

        bool chooseReal = Random.value > 0.5f;

        GameObject selectedPrefab;


        if (chooseReal)
        {
            correctAnswer = true;

            if (realPrefabs == null || realPrefabs.Length == 0)
            {
                Debug.LogError(
                    "Real Prefabs array is empty."
                );

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
                Debug.LogError(
                    "AI Prefabs array is empty."
                );

                yield break;
            }

            int randomIndex = Random.Range(
                0,
                aiPrefabs.Length
            );

            selectedPrefab = aiPrefabs[randomIndex];
        }


        // -----------------------------------------------------
        // SPAWN IMAGE
        // -----------------------------------------------------

        if (selectedPrefab == null)
        {
            Debug.LogError(
                "Selected image prefab is null."
            );

            yield break;
        }

        currentImage = Instantiate(
            selectedPrefab,
            transform.position,
            transform.rotation
        );


        // -----------------------------------------------------
        // SHOW INSTRUCTION
        // -----------------------------------------------------

        if (instructions != null)
        {
            instructions.gameObject.SetActive(true);
            instructions.text =
                "Is this image real or AI?";
        }


        // -----------------------------------------------------
        // RESET ANSWER
        // -----------------------------------------------------

        playerAnswered = false;
        playerAnswer = false;


        // -----------------------------------------------------
        // ENABLE BUTTONS
        // -----------------------------------------------------

        realButton.interactable = true;
        fakeButton.interactable = true;

        canClick = true;


        // -----------------------------------------------------
        // WAIT FOR PLAYER
        // -----------------------------------------------------

        yield return new WaitUntil(
            () => playerAnswered
        );


        // -----------------------------------------------------
        // DISABLE BUTTONS
        // -----------------------------------------------------

        canClick = false;

        DisableAnswerButtons();


        // -----------------------------------------------------
        // HIDE INSTRUCTION
        // -----------------------------------------------------

        if (instructions != null)
        {
            instructions.gameObject.SetActive(false);
        }


        // -----------------------------------------------------
        // CHECK ANSWER
        // -----------------------------------------------------

        bool correct = playerAnswer == correctAnswer;


        if (correct)
        {
            if (displayText != null)
            {
                displayText.gameObject.SetActive(true);
                displayText.text = "Correct!";
            }

            if (inventory != null)
            {
                inventory.AddScore(1);
            }
        }
        else
        {
            if (displayText != null)
            {
                displayText.gameObject.SetActive(true);
                displayText.text = "Incorrect!";
            }

            if (inventory != null)
            {
                inventory.TakeDamage(1);
            }
        }


        // -----------------------------------------------------
        // WAIT
        // -----------------------------------------------------

        yield return new WaitForSeconds(2f);


        // -----------------------------------------------------
        // DESTROY IMAGE
        // -----------------------------------------------------

        if (currentImage != null)
        {
            Destroy(currentImage);
            currentImage = null;
        }


        // -----------------------------------------------------
        // HIDE RESULT
        // -----------------------------------------------------

        if (displayText != null)
        {
            displayText.text = "";
            displayText.gameObject.SetActive(false);
        }
    }


    // =========================================================
    // REAL BUTTON
    // =========================================================

    public void RealButton()
    {
        if (!canClick)
        {
            return;
        }

        playerAnswer = true;
        playerAnswered = true;
    }


    // =========================================================
    // FAKE / AI BUTTON
    // =========================================================

    public void FakeButton()
    {
        if (!canClick)
        {
            return;
        }

        playerAnswer = false;
        playerAnswered = true;
    }


    // =========================================================
    // FINISH ROUND
    // =========================================================

    private IEnumerator FinishRound()
    {
        canClick = false;

        DisableAnswerButtons();

        HideAllImages();


        // -----------------------------------------------------
        // HIDE INSTRUCTIONS
        // -----------------------------------------------------

        if (instructions != null)
        {
            instructions.gameObject.SetActive(false);
        }


        // -----------------------------------------------------
        // SHOW RESULT
        // -----------------------------------------------------

        if (resultsText != null)
        {
            resultsText.gameObject.SetActive(true);
        }


        yield return new WaitForSeconds(2f);


        // -----------------------------------------------------
        // HIDE RESULT
        // -----------------------------------------------------

        if (resultsText != null)
        {
            resultsText.gameObject.SetActive(false);
        }


        // -----------------------------------------------------
        // CHECK PLAYER INVENTORY
        // -----------------------------------------------------

        if (inventory == null)
        {
            Debug.LogError(
                "PlayerInventory is missing."
            );

            yield break;
        }


        // -----------------------------------------------------
        // ADVANCE TO NEXT LEVEL AT 7
        // -----------------------------------------------------

        if (inventory.score >= 7)
        {
            Debug.Log(
                "Intermediate complete. Score: " +
                inventory.score
            );

            inventory.Heal(5);

            if (nextLevel != null)
            {
                nextLevel.AdvancedStage();
            }
            else
            {
                Debug.LogError(
                    "Next Level is not assigned in the Inspector."
                );
            }

            yield break;
        }


        // -----------------------------------------------------
        // CHECK HEALTH
        // -----------------------------------------------------

        if (inventory.health <= 0)
        {
            Debug.Log(
                "Player has no health remaining."
            );

            yield break;
        }


        // -----------------------------------------------------
        // NEXT ROUND
        // -----------------------------------------------------

        turn++;

        yield return StartCoroutine(StartRound());
    }


    // =========================================================
    // HIDE ALL IMAGES
    // =========================================================

    private void HideAllImages()
    {
        if (leftImage != null)
        {
            leftImage.gameObject.SetActive(false);
        }

        if (rightImage != null)
        {
            rightImage.gameObject.SetActive(false);
        }

        if (oneImage != null)
        {
            oneImage.gameObject.SetActive(false);
        }
    }


    // =========================================================
    // DISABLE BUTTONS
    // =========================================================

    private void DisableAnswerButtons()
    {
        if (realButton != null)
        {
            realButton.interactable = false;
        }

        if (fakeButton != null)
        {
            fakeButton.interactable = false;
        }
    }


    // =========================================================
    // IMAGE SIZE
    // =========================================================

    private void SetSimilarSize(Image image)
    {
        if (image == null)
        {
            return;
        }

        if (image.sprite == null)
        {
            Debug.LogWarning(
                "Cannot resize image because it has no sprite."
            );

            return;
        }

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
            new Vector3(
                scale,
                scale,
                1f
            );
    }
}