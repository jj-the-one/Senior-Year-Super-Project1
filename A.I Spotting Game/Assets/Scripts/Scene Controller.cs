using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Beginner Stage");
    }

    public void TutorialPage()
    {
        SceneManager.LoadScene("Tutorial page");
    }

    public void ResultsPage()
    {
        SceneManager.LoadScene("Results Page");
    }

    public void IntermediateStage()
    {
        SceneManager.LoadScene("Intermediate Stage");
    }

    public void AdvancedStage()
    {
        SceneManager.LoadScene("Advanced Stage");
    }

    public void HomePage()
    {
        SceneManager.LoadScene("Home Page");
    }
}
