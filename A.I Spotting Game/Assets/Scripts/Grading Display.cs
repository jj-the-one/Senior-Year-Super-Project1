using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GradingDisplay : MonoBehaviour
{
    private PlayerInventory inventory;

    public TMP_Text scoreText;
    public TMP_Text gradeText;
    public TMP_Text explanationText;

    void Start()
    {
        // Find the Player using the "Player" tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player with the 'Player' tag could not be found!");
            return;
        }

        // Get the PlayerInventory component from the Player
        inventory = player.GetComponent<PlayerInventory>();

        if (inventory == null)
        {
            Debug.LogError("PlayerInventory component could not be found on the Player!");
            return;
        }

        DisplayGrade();
    }

    void DisplayGrade()
    {
        int score = inventory.score;

        scoreText.text = "Your Score: " + score;

        if (score <= 3)
        {
            gradeText.text = "Grade: G";
            explanationText.text =
                "Getting a grade of G indicates an extremely high need for improvement. " +
                "Your ability to distinguish AI and real imagery with a reference is lacking.";
        }
        else if (score <= 5)
        {
            gradeText.text = "Grade: F";
            explanationText.text =
                "Getting a grade of F indicates a very high need for improvement. " +
                "Your ability to distinguish AI and real imagery with a reference is lacking.";
        }
        else if (score <= 7)
        {
            gradeText.text = "Grade: E";
            explanationText.text =
                "Getting a grade of E indicates a high need for improvement. " +
                "Your ability to distinguish AI and real imagery with a reference is lacking.";
        }
        else if (score <= 10)
        {
            gradeText.text = "Grade: D";
            explanationText.text =
                "Getting a grade of D indicates room for improvement. " +
                "You have shown that you can recognize some basic differences between AI-generated images and real images.";
        }
        else if (score <= 12)
        {
            gradeText.text = "Grade: C";
            explanationText.text =
                "Getting a grade of C indicates a developing ability to distinguish AI-generated images from real images.";
        }
        else if (score <= 14)
        {
            gradeText.text = "Grade: B";
            explanationText.text =
                "Getting a grade of B indicates a good ability to distinguish AI-generated images from real images.";
        }
        else if (score <= 17)
        {
            gradeText.text = "Grade: A";
            explanationText.text =
                "Getting a grade of A indicates a very strong ability to distinguish AI-generated images from real images.";
        }
        else if (score <= 19)
        {
            gradeText.text = "Grade: S";
            explanationText.text =
                "Getting a grade of S indicates an exceptional ability to distinguish AI-generated images from real images.";
        }
        else
        {
            gradeText.text = "Grade: Z";
            explanationText.text =
                "Getting a grade of Z indicates an outstanding performance. " +
                "You demonstrated an extremely strong ability to distinguish AI-generated images from real images.";
        }
    }
}