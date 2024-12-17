using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreHandler : MonoBehaviour
{
    public int score;
    [SerializeField] private Difficulty difficulty;
    public void AddScore(int scoreToAddTo)
    {
        score += scoreToAddTo;
    }

    private void Update()
    {
        if (score >= 0)
        {
            difficulty.currentDifficulty = Difficulty.difficulty.Easy;
            if (score >= 15)
            {
                difficulty.currentDifficulty = Difficulty.difficulty.Medium;
                if (score >= 30)
                {
                    difficulty.currentDifficulty = Difficulty.difficulty.Hard;
                }
            }
        }
    }
}