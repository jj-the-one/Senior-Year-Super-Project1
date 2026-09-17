using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int score = 0;
    public int health = 5;
    private static PlayerInventory instance;

    void Awake()
    {
        if (instance != null && instance != this) {
            Destroy(gameObject); 
            return; 
        }
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            //access Scene Controller Script and call function ResultsPage()
            SceneController sceneController = FindObjectOfType<SceneController>();
            if (sceneController != null){
                sceneController.ResultsPage();
            }
            else{
                Debug.LogError("SceneController could not be found!");
            }
        }
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > 5)
        {
            health = 5;
        }
    }
}
