using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerDisplay : MonoBehaviour
{
    private PlayerInventory inventory;

    public TMP_Text healthText;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponent<PlayerInventory>();

        DisplayHealth();
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayHealth(); 
    }

    void DisplayHealth()
    {
        healthText.text = "Hp: " + inventory.health +"/5";
    }
}
