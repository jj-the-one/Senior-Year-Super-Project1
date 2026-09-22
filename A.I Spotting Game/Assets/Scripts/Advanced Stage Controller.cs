using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AdvancedStageController : MonoBehaviour
{
    private PlayerInventory inventory;

    [Header("Image Prefabs")]
    public GameObject[] aiPrefabs;
    public GameObject[] realPrefabs;

    [Header("UI")]
    public TMP_Text displayText;
    public Button realButton;
    public Button fakeButton;

    private int turn = 1;

    // True = Real
    // False = AI/Fake
    private bool correctAnswer;

    private GameObject currentImage;

    private bool playerAnswered = false;
    private bool playerAnswer = false;

    void Start()
    {
        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            inventory = player.GetComponent<PlayerInventory>();
        }
        else
        {
            Debug.LogWarning("Player with the 'Player' tag could not be found.");
        }

        // Disable buttons at the start
        realButton.interactable = false;
        fakeButton.interactable = false;

        StartCoroutine(StartGame());
    }

    IEnumerator StartGame()
    {
        // Starting message
        displayText.text = "Final Round";
        yield return new WaitForSeconds(2f);

        // Continue until the player reaches 21 points
        // OR loses all health
        while (inventory != null && inventory.score < 21 && inventory.health > 0)
        {
            yield return StartCoroutine(PlayTurn());

            turn++;
        }

        // Game over
        displayText.text = "Game Over";

        yield return new WaitForSeconds(2f);

        // Disable buttons
        realButton.interactable = false;
        fakeButton.interactable = false;

        // Go to the results page
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

    IEnumerator PlayTurn()
    {
        // Show round number
        displayText.text = "Round " + turn;

        // Round text stays for 2 seconds
        yield return new WaitForSeconds(2f);

        // Hide TMP while player is looking at the image
        displayText.text = "";

        // Randomly decide whether to use an AI or Real image
        bool chooseReal = Random.value > 0.5f;

        GameObject selectedPrefab;

        if (chooseReal)
        {
            // Real image
            correctAnswer = true;

            int randomIndex = Random.Range(0, realPrefabs.Length);
            selectedPrefab = realPrefabs[randomIndex];
        }
        else
        {
            // AI image
            correctAnswer = false;

            int randomIndex = Random.Range(0, aiPrefabs.Length);
            selectedPrefab = aiPrefabs[randomIndex];
        }

        // Spawn the image at the same position as this GameObject
        currentImage = Instantiate(
            selectedPrefab,
            transform.position,
            transform.rotation
        );

        // Reset the player's answer
        playerAnswered = false;

        // Enable the buttons
        realButton.interactable = true;
        fakeButton.interactable = true;

        // Wait until the player clicks one of the buttons
        yield return new WaitUntil(() => playerAnswered);

        // Disable the buttons after answering
        realButton.interactable = false;
        fakeButton.interactable = false;

        // Check the player's answer
        if (playerAnswer == correctAnswer)
        {
            displayText.text = "Correct!";

            if (inventory != null)
            {
                inventory.AddScore(1);
            }
        }
        else
        {
            displayText.text = "Incorrect!";

            if (inventory != null)
            {
                inventory.TakeDamage(1);
            }
        }

        // Show Correct/Incorrect for 2 seconds
        yield return new WaitForSeconds(2f);

        // Destroy the current image
        if (currentImage != null)
        {
            Destroy(currentImage);
        }
    }

    // Called by the REAL button
    public void RealButton()
    {
        playerAnswer = true;
        playerAnswered = true;
    }

    // Called by the FAKE/AI button
    public void FakeButton()
    {
        playerAnswer = false;
        playerAnswered = true;
    }
}
