using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvancedStageController : MonoBehaviour
{
    private PlayerInventory inventory;
    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        inventory = player.GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
/*
AI Generated Images Prefab Array size 20
Real Images Prefab Array size 20
turn = 1
print for 2 seconds final round using a TMP
while health >= 0 && turn < 8{
    print for 3 seconds phase (turn) using the same TMP
    randomly select which array to use
    A.I list holds a value of false
    Real List has a value of true
    from there randomly select an image from the selected prefab list
    spawn it in from the same spot the onbject attached to the script is at
    print for 2 seconds determine whether this image is A.I or real
    if user clicks one of two buttons
    2 functions each attached to a seperate button
    1. real
        returns true
    2. fake
        returns false
    check if the value of the clicked button is same as the value of the list selected
    true: run player inventory method AddScore(1);
    false: run player inventory method TakeDamage(1);
    turn ++;
}
From scenemanager script from a prefab, run the method ResultsPage()
 */