using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Transform tutorialRoot;
    public ObjectSpawner spawner;
    public ControllerSelection.HoverSpot heldScript;
    public GameObject gameScreens;
    public progressManager progressor;
    public TextMesh DebugText;
    public int successTextID;
    public int incorrectTextID;

    private int currentScreen = 0;
    private GameObject successScreen;
    private AudioHandler audioScript;
    private bool firstClick = false;

    [HideInInspector] public bool firstObjectSorted = false;

    private void Start()
    {
        progressor.ChangeConveyorAnimators(0);
        audioScript = GameObject.Find("Audio").GetComponent<AudioHandler>();
    }

    public void NextScreen()
    {
        tutorialRoot.GetChild(currentScreen).gameObject.SetActive(false);
        currentScreen++;
        tutorialRoot.GetChild(currentScreen).gameObject.SetActive(true);
    }

    public void LastScreen()
    {
        tutorialRoot.GetChild(currentScreen).gameObject.SetActive(false);
        currentScreen--;
        tutorialRoot.GetChild(currentScreen).gameObject.SetActive(true);
    }

    public void PointAtSign()
    {
        if (!firstClick)
        {
            NextScreen();
            audioScript.PlaySFX(audioScript.Next);
            spawner.CreateNewObject();
            progressor.ChangeConveyorAnimators(progressor.minSpeedMultiplier);
            firstClick = true;
        }
    }

    public void firstPickup()
    {
        NextScreen();
        //DebugText.text = "First object picked up";
    }

    public void CorrectSort()
    {
        if (tutorialRoot.GetChild(currentScreen))
            tutorialRoot.GetChild(currentScreen).gameObject.SetActive(false);

        if (tutorialRoot.GetChild(successTextID))
            tutorialRoot.GetChild(successTextID).gameObject.SetActive(true);

        currentScreen = successTextID;
        ResetAllTutVariables();
        //DebugText.text = "Correct Sort - first try";
    }

    public void IncorrectSort()
    {
        tutorialRoot.GetChild(currentScreen).gameObject.SetActive(false);
        tutorialRoot.GetChild(incorrectTextID).gameObject.SetActive(true);
        currentScreen = incorrectTextID;
        //DebugText.text = "Incorrect Sort";
    }

    public void CorrectSortRetry()
    {
        //LastScreen();
        tutorialRoot.GetChild(currentScreen).gameObject.SetActive(false);
        tutorialRoot.GetChild(successTextID).gameObject.SetActive(true);
        currentScreen = successTextID;
        ResetAllTutVariables();
        //DebugText.text = "Correct Sort - repeat try";
    }

    private void ResetAllTutVariables()
    {
        if (spawner != null)
            spawner.startGame = true;

        if (progressor != null)
            progressor.ResetDifficulty();

        GameObject[] bins;
        bins = GameObject.FindGameObjectsWithTag("Bin");

        if (bins.Length != 0)
        {
            for (int i = 0; i < bins.Length; i++)
            {
                bins[i].GetComponentInChildren<Bin>().isTut = false;
            }
        }
    }

    public void StartGameScreen()
    {
        gameScreens.SetActive(true);
        gameObject.SetActive(false);
        //DebugText.text = "Show Game Screens";
    }
}
