using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonGen : MonoBehaviour
{
    [SerializeField] private List<GameObject> circles = new List<GameObject>();
    [SerializeField] private List<GameObject> monsterPrefabs = new List<GameObject>();
    [SerializeField] private Timer timer;
    private int activeMonsters = 0; // Tracks how many monsters are active
    [SerializeField] private Difficulty difficulty;
    public void GenMonster()
    {
        var randCircle = Random.Range(0, circles.Count); // Use 0-based index
        var randMonster = Random.Range(0, monsterPrefabs.Count);

        // Instantiate a monster at a random circle
        GameObject monster = Instantiate(monsterPrefabs[randMonster], circles[randCircle].transform);
        activeMonsters++; // Increment the active monster count
    }

    public void MonsterDestroyed()
    {
        activeMonsters--; // Decrement the active monster count
        if (activeMonsters <= 0)
        {
            Debug.Log("All monsters defeated!");
            timer.RestartTimer();
            if (difficulty.currentDifficulty == Difficulty.difficulty.Easy)
            {
                GenMonster(); // Spawn a new monster
                GenMonster(); // Spawn a new monster
            }
            if (difficulty.currentDifficulty == Difficulty.difficulty.Medium)
            {
                GenMonster(); // Spawn a new monster
                GenMonster(); // Spawn a new monster
                GenMonster(); // Spawn a new monster
                GenMonster(); // Spawn a new monster
            }
            if (difficulty.currentDifficulty == Difficulty.difficulty.Hard)
            {
                GenMonster(); // Spawn a new monster
                GenMonster(); // Spawn a new monster
                GenMonster(); // Spawn a new monster
                GenMonster(); // Spawn a new monster
                GenMonster(); // Spawn a new monster
                GenMonster(); // Spawn a new monster
                GenMonster(); // Spawn a new monster
                GenMonster(); // Spawn a new monster
            }
            // Handle what happens when all monsters are destroyed (e.g., stop the timer or trigger an event)
        }
    }

    private void Start()
    {
        GenMonster(); // Initial monster generation
    }
}
