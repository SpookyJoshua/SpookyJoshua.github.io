using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUps_Handler : MonoBehaviour
{
    public int powerUpCount;
    [SerializeField] private PostProcessingColor postProcessing;

    [Header("Textures")]
    [SerializeField] private Texture powerUp1;
    [SerializeField] private Texture powerUp2; 
    [SerializeField] private Texture powerUp3;

    [Header("User Interface Components")]
    [SerializeField] private PU_UI ui1;
    [SerializeField] private PU_UI ui2;
    [SerializeField] private PU_UI ui3;

    private bool dumbellAssigned;
    private bool ideaAssigned;
    private bool puzzleAssigned;

    public Queue<string> powerUpQueue = new Queue<string>();

    private bool isRunningQueue;

    private float sat;

    private void Awake()
    {
        powerUpCount = 0;
        dumbellAssigned = false;
        ideaAssigned = false;
        puzzleAssigned = false;
        isRunningQueue = false;
        powerUpQueue.Clear();
    }

    private void AssignDumbellPowerUp()
    {
        if (!ui1.isUsed)
        {
            ui1.isUsed = true;
            ui1.GetComponent<RawImage>().texture = powerUp1;
            dumbellAssigned = true;
            sat = postProcessing.currentSat + 66.6666666668f;
            postProcessing.ChangeSaturation(postProcessing.currentSat + 66.6666666668f, 5f);
        }
        else
        {
            if (!ui2.isUsed)
            {
                ui2.isUsed = true;
                ui2.GetComponent<RawImage>().texture = powerUp1;
                dumbellAssigned = true;
                sat = postProcessing.currentSat + 66.6666666668f;
                postProcessing.ChangeSaturation(postProcessing.currentSat + 66.6666666668f, 5f);
            }
            else
            {
                if (!ui3.isUsed)
                {
                    ui3.isUsed = true;
                    ui3.GetComponent<RawImage>().texture = powerUp1;
                    dumbellAssigned = true;
                    sat = postProcessing.currentSat + 66.6666666668f;
                    postProcessing.ChangeSaturation(postProcessing.currentSat + 66.6666666668f, 5f);
                }
            }
        }
        powerUpCount++;
    }

    IEnumerator ApplyNextPowerUp()
    {
        if (powerUpQueue.Count > 0)
        {
            string nextPowerUp = powerUpQueue.Dequeue();
            Debug.Log("Power Up Issue");
            ApplyPowerUp(nextPowerUp);
            yield return new WaitForSeconds(5.1f);
            Debug.Log("Added " + nextPowerUp + " Successfully!");
        }
        isRunningQueue = false;
    }

    private void FixedUpdate()
    {
        if(isRunningQueue == false)
        {
            isRunningQueue = true;
            StartCoroutine(ApplyNextPowerUp());
        }
    }

    private void ApplyPowerUp(string powerUpName)
    {
        switch (powerUpName)
        {
            case "Dumbell":
                AssignDumbellPowerUp();
                break;
            case "Idea":
                AssignIdeaPowerUp();
                break;
            case "Puzzle":
                AssignPuzzlePowerUp();
                break;
            case "dumbell":
                AssignDumbellPowerUp();
                break;
            case "idea":
                AssignIdeaPowerUp();
                break;
            case "puzzle":
                AssignPuzzlePowerUp();
                break;
            default:
                Debug.LogWarning("Unknown power-up: " + powerUpName);
                break;
        }
    }


    public void PostProc()
    {
        postProcessing.ChangeSaturation(postProcessing.currentSat + 66.6666666668f, 5f);
    }

    private void AssignIdeaPowerUp()
    {
        if (!ui1.isUsed)
        {
            ui1.isUsed = true;
            ui1.GetComponent<RawImage>().texture = powerUp2;
            ideaAssigned = true;
            sat = postProcessing.currentSat + 66.6666666668f;
            postProcessing.ChangeSaturation(postProcessing.currentSat + 66.6666666668f, 5f);
        }
        else
        {
            if (!ui2.isUsed)
            {
                ui2.isUsed = true;
                ui2.GetComponent<RawImage>().texture = powerUp2;
                ideaAssigned = true;
                sat = postProcessing.currentSat + 66.6666666668f;
                postProcessing.ChangeSaturation(postProcessing.currentSat + 66.6666666668f, 5f);
            }
            else
            {
                if (!ui3.isUsed)
                {
                    ui3.isUsed = true;
                    ui3.GetComponent<RawImage>().texture = powerUp2;
                    ideaAssigned = true;
                    sat = postProcessing.currentSat + 66.6666666668f;
                    postProcessing.ChangeSaturation(postProcessing.currentSat + 66.6666666668f, 5f);
                }
            }
        }
        powerUpCount++;
    }

    private void AssignPuzzlePowerUp()
    {
        if (!ui1.isUsed)
        {
            ui1.isUsed = true;
            ui1.GetComponent<RawImage>().texture = powerUp3;
            puzzleAssigned = true;
            sat = postProcessing.currentSat + 66.6666666668f;
            postProcessing.ChangeSaturation(postProcessing.currentSat + 66.6666666668f, 5f);
        }
        else
        {
            if (!ui2.isUsed)
            {
                ui2.isUsed = true;
                ui2.GetComponent<RawImage>().texture = powerUp3; 
                puzzleAssigned = true;
                sat = postProcessing.currentSat + 66.6666666668f;
                postProcessing.ChangeSaturation(postProcessing.currentSat + 66.6666666668f, 5f);
            }
            else
            {
                if (!ui3.isUsed)
                {
                    ui3.isUsed = true;
                    ui3.GetComponent<RawImage>().texture = powerUp3;
                    puzzleAssigned = true;
                    sat = postProcessing.currentSat + 66.6666666668f;
                    postProcessing.ChangeSaturation(postProcessing.currentSat + 66.6666666668f, 5f);
                }
            }
        }
        powerUpCount++;
    }

    public void AssignRandomPowerUp()
    {
        int randNum = Random.Range(1, 4); // Change the upper limit to include 3

        // Use a switch statement for better readability and maintainability
        switch (randNum)
        {
            case 1:
                if (!ideaAssigned)
                {
                    AssignIdeaPowerUp();
                }
                else if (!dumbellAssigned)
                {
                    AssignDumbellPowerUp();
                }
                else if (!puzzleAssigned)
                {
                    AssignPuzzlePowerUp();
                }
                break;

            case 2:
                if (!dumbellAssigned)
                {
                    AssignDumbellPowerUp();
                }
                else if (!puzzleAssigned)
                {
                    AssignPuzzlePowerUp();
                }
                else if (!ideaAssigned)
                {
                    AssignIdeaPowerUp();
                }
                break;

            case 3:
                if (!puzzleAssigned)
                {
                    AssignPuzzlePowerUp();
                }
                else if (!ideaAssigned)
                {
                    AssignIdeaPowerUp();
                }
                else if (!dumbellAssigned)
                {
                    AssignDumbellPowerUp();
                }
                break;

            default:
                // Handle unexpected randNum value, if needed
                break;
        }
    }
}
