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

    private bool canClick = true;

    public List<Sprite> IntermediateRealImages = new List<Sprite>();
    public List<Sprite> IntermediateAIImages = new List<Sprite>();

    private int currentSet;
    private int realIndex;
    private int aiIndex;

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
        leftImage = GameObject.Find("ImageLeft").GetComponent<Image>();
        rightImage = GameObject.Find("ImageRight").GetComponent<Image>();
        oneImage = GameObject.Find("OneImage").GetComponent<Image>();
        resultsText = GameObject.Find("Results Text(Correct)").GetComponent<TMP_Text>();
        //Sets 'Correct' 'Incorrect' messages to invisible.
        resultsText.gameObject.SetActive(false);

        //Loads the images into the lists from the folders
        IntermediateAIImages.AddRange(Resources.LoadAll<Sprite>("Photos/Intermediate A.I"));
        IntermediateRealImages.AddRange(Resources.LoadAll<Sprite>("Photos/Intermediate Real"));

        Debug.Log("Intermediate A.I loaded " + IntermediateAIImages.Count);
        Debug.Log("Intermediate Real loaded " + IntermediateRealImages.Count);

        int x = Random.Range(0, 2);
        if (x == 0) {
            leftReal = true;
            rightReal = false;
        }
        else {
            leftReal = false;
            rightReal = true;
        }
        int y = Random.Range(0, 3);
        if (y == 0) {
            Debug.Log("Find real");
        }
        else if (y == 1) {
            Debug.Log("Find AI");
        }
        else {
            Debug.Log("Only one");
        }

        leftImage.gameObject.SetActive(true);
        rightImage.gameObject.SetActive(true);
    }

    // Update is called once per frame
    public bool canBeClicked() {
        return canClick;
    }
    public bool CheckAnswer(bool clickedLeft) {
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
            return correct;
        }
        else {
            inventory.TakeDamage(1);
            return correct;
        }

    }
}
