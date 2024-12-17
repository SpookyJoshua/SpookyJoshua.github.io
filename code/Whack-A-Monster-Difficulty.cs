using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Difficulty : MonoBehaviour
{
    [SerializeField] private Timer timer;
    [SerializeField] private TMP_Text diffText;
    public enum difficulty
    {
        Easy,
        Medium,
        Hard
    }

    public difficulty currentDifficulty = new difficulty();

    private void Update()
    {
        diffText.text = currentDifficulty.ToString();
        if(currentDifficulty == difficulty.Easy)
        {
            timer.secondsToCountdownTo = 5;
        }
        if (currentDifficulty == difficulty.Medium)
        {
            timer.secondsToCountdownTo = 3;
        }
        if (currentDifficulty == difficulty.Hard)
        {
            timer.secondsToCountdownTo = 1;
        }
    }
}
