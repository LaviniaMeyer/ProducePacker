using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class progressManager : MonoBehaviour
{
    [Header("Scene Objects")]
    public TextMesh scoreDisplay;
    public TextMesh streakDisplay;
    public GameObject confettiObject;
    public ObjectSpawner spawner;
    public AchievementHandler awards;
    public AudioHandler Audio;
    private GameObject[] conveyors;

    [Header("Game Settings")]
    public float speedIncreaseMultiplier = 0.2f;
    public float currentSpeedMultiplier = 0.2f;
    public float minSpeedMultiplier = 0.2f;
    public float maxSpeedMultiplier = 1;
    [Space]
    public int firstDifficultyIncrement = 3;
    public int difficultyIncrement = 7;
    public int currentDifficulty = 1;

    [HideInInspector] public int score = 0;
    [HideInInspector] public int streak = 0;

    private float lastSpeed;

    private bool noDrop = true;
    private bool isFirstRound = true;

    private void Start()
    {
        ResetDifficulty();
        UpdateScoreboard();
    }

    private void IncreaseDifficulty()
    {
        if (currentSpeedMultiplier < maxSpeedMultiplier)
        {
            currentSpeedMultiplier += speedIncreaseMultiplier;
            currentDifficulty++;
            UpdateSpeeds();
        }
    }

    private void DecreaseDifficulty()
    {
        if (currentDifficulty > 1)
        {
            currentSpeedMultiplier -= speedIncreaseMultiplier;
            currentDifficulty--;
            UpdateSpeeds();
        }
    }

    public void ResetDifficulty()
    {
        currentDifficulty = 1;
        currentSpeedMultiplier = minSpeedMultiplier;
        isFirstRound = true;
        UpdateSpeeds();
    }

    public void ScoreUp()
    {
        score++;
        streak++;
        noDrop = true;

        if (isFirstRound) //If the player has never increased difficulty the increment is lower
        {
            if (streak % firstDifficultyIncrement == 0)
            {
                IncreaseDifficulty();
                isFirstRound = false;
            }
        }
        else //Use the higher increment value
        {
            if (streak % difficultyIncrement == 0)
            {
                IncreaseDifficulty();
            }
        }

        UpdateScoreboard();
        CheckAwards();
    }

    public void WrongItem()
    {
        if (!noDrop)
        {
            DecreaseDifficulty();
            DecreaseDifficulty();
        }

        noDrop = false;
        streak = 0;
        UpdateScoreboard();
    }

    private void UpdateScoreboard()
    {
        scoreDisplay.text = score.ToString();
        streakDisplay.text = streak.ToString();
    }

    private void UpdateSpeeds()
    {
        spawner.minWait = spawner.defMinWait / currentSpeedMultiplier;
        spawner.maxWait = spawner.defMaxWait / currentSpeedMultiplier;

        ChangeConveyorAnimators(currentSpeedMultiplier);
    }

    public void ChangeConveyorAnimators(float speedMulti)
    {
        conveyors = GameObject.FindGameObjectsWithTag("Conveyor");
        for (int i = 0; i < conveyors.Length; i++)
        {
            conveyors[i].GetComponent<Animator>().speed = speedMulti;
        }
    }

    private void CheckAwards()
    {
        if (awards.CheckForAward(score, streak))
            ActivateAwardCelebration();
    }

    private void ActivateAwardCelebration()
    {
        //Call sound handler for sounds
        Audio.PlaySFX(Audio.partyHorn);
        Audio.PlaySFX(Audio.yay);
        Audio.PlaySFX(Audio.Confetti);
        //Activate confetti animation
        confettiObject.SetActive(true);
    }
}
