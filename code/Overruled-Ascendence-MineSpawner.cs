using UnityEngine;

public class RandomSpawnerWithBounds : MonoBehaviour
{
    // The prefab to spawn
    [SerializeField] private GameObject objectToSpawn;

    [SerializeField] private GameObject nodeParent;

    // The GameObject that defines the spawn area
    [SerializeField] private GameObject spawnArea;

    // Range for the number of objects to spawn
    [SerializeField] private int minObjects = 5;
    [SerializeField] private int maxObjects = 15;

    // Toggle for each side of the bounding box
    [SerializeField] private bool spawnOnLeft = true;
    [SerializeField] private bool spawnOnRight = true;
    [SerializeField] private bool spawnOnBottom = true;
    [SerializeField] private bool spawnOnTop = true;
    [SerializeField] private bool spawnOnFront = true;
    [SerializeField] private bool spawnOnBack = true;

    void Start()
    {
        if (spawnArea == null)
        {
            Debug.LogError("Spawn Area GameObject is not assigned.");
            return;
        }

        // Randomize the number of objects to spawn within the range
        int numberOfObjects = Random.Range(minObjects, maxObjects + 1);
        SpawnObjectsOnSelectedSides(numberOfObjects);
    }

    // Method to spawn objects on selected sides of the spawnArea
    void SpawnObjectsOnSelectedSides(int numberOfObjects)
    {
        // Calculate the bounds of the spawn area
        Bounds areaBounds = spawnArea.GetComponent<Renderer>().bounds;

        // Create a list of enabled sides
        var enabledSides = GetEnabledSides();

        if (enabledSides.Count == 0)
        {
            Debug.LogWarning("No sides selected for spawning. Enable at least one side.");
            return;
        }

        for (int i = 0; i < numberOfObjects; i++)
        {
            // Randomly select one of the enabled sides
            int side = enabledSides[Random.Range(0, enabledSides.Count)];
            Vector3 spawnPosition = GetRandomPositionOnSide(areaBounds, side);

            // Instantiate the object at the calculated position
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity, nodeParent.transform);
        }
    }

    // Generates a random position on a specific side of the bounds
    Vector3 GetRandomPositionOnSide(Bounds bounds, int side)
    {
        Vector3 position = Vector3.zero;

        switch (side)
        {
            case 0: // Left face
                position = new Vector3(bounds.min.x,
                                       Random.Range(bounds.min.y, bounds.max.y),
                                       Random.Range(bounds.min.z, bounds.max.z));
                break;
            case 1: // Right face
                position = new Vector3(bounds.max.x,
                                       Random.Range(bounds.min.y, bounds.max.y),
                                       Random.Range(bounds.min.z, bounds.max.z));
                break;
            case 2: // Bottom face
                position = new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                                       bounds.min.y,
                                       Random.Range(bounds.min.z, bounds.max.z));
                break;
            case 3: // Top face
                position = new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                                       bounds.max.y,
                                       Random.Range(bounds.min.z, bounds.max.z));
                break;
            case 4: // Front face
                position = new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                                       Random.Range(bounds.min.y, bounds.max.y),
                                       bounds.min.z);
                break;
            case 5: // Back face
                position = new Vector3(Random.Range(bounds.min.x, bounds.max.x),
                                       Random.Range(bounds.min.y, bounds.max.y),
                                       bounds.max.z);
                break;
        }

        return position;
    }

    // Returns a list of enabled sides based on user selection
    System.Collections.Generic.List<int> GetEnabledSides()
    {
        var enabledSides = new System.Collections.Generic.List<int>();

        if (spawnOnLeft) enabledSides.Add(0);
        if (spawnOnRight) enabledSides.Add(1);
        if (spawnOnBottom) enabledSides.Add(2);
        if (spawnOnTop) enabledSides.Add(3);
        if (spawnOnFront) enabledSides.Add(4);
        if (spawnOnBack) enabledSides.Add(5);

        return enabledSides;
    }
}