using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementHandler : MonoBehaviour
{
    public Transform[] AwardParents = new Transform[2];
    public int[] awardScores = new int[5];

    private bool[] streakUnlocks = new bool[5];

    private void ActivateAward(int parent, int ID)
    {
        AwardParents[parent].GetChild(ID).gameObject.SetActive(true);
    }

    public bool CheckForAward(int score, int streak)
    {
        bool either = false;
        for (int i = 0; i < awardScores.Length; i++)
        {
            if (score == awardScores[i])
            {
                ActivateAward(0, i);
                either = true;
            }

            if (streak == awardScores[i] && streakUnlocks[i] == false)
            {
                ActivateAward(1, i);
                streakUnlocks[i] = true;
                either = true;
            }
        }

        if (either)
        {
            return true;
        }

        return false;
    }
}
