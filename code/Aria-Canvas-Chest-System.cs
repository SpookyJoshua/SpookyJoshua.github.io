using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
public class ChestScript : MonoBehaviour
{
    [SerializeField] private Tile closedChest;
    [SerializeField] private Tile closedChest2;
    [SerializeField] private Tile openChest;

    [SerializeField] private Tilemap tilemap;

    [SerializeField] private Transform player;
    [SerializeField] private TMP_Text hint_TXT;
    [SerializeField] private Transform hint_Panel;
    [SerializeField] private int activationDistance = 3; // Distance within which the tile changes
    [SerializeField] private QTE_Handler qte_Handler;

    [SerializeField] private Lockpick_Handler lock_handler;

    [SerializeField] private string hintText;

    [SerializeField] private XP_Script xp_Handler;

    [SerializeField] private PuzzleHandler puzzleHandler;

    private bool isAllowed;

    private bool overwriteHint;

    public bool isChestOpen;

    private void Start()
    {
        isAllowed = true;
        lock_handler.chest_Handler = gameObject.GetComponent<ChestScript>();
        qte_Handler.chestHandler = gameObject.GetComponent<ChestScript>();
    }

    public void cReset(Vector3Int cellPosition, string chestType)
    {
        if (chestType == "closed1")
        {
            StartCoroutine(chestReset(cellPosition, closedChest));
        }
        if (chestType == "closed2")
        {
            StartCoroutine(chestReset(cellPosition, closedChest2));
        }
    }

    private void OpenChest(Vector3Int cellPos)
    {
        if (!isChestOpen)
        {
            isChestOpen = true;
            // Convert the chest's world position to a cell position in the tilemap
            Vector3Int cellPosition = tilemap.WorldToCell(cellPos);

            if (tilemap.GetTile(cellPosition) == closedChest) // Check if the chest tile is closed
            {
                int randNum = Random.Range(1, 4);
                if (randNum == 1)
                {
                    lock_handler.ChestAttempt(0, 100, false, cellPosition, "closed1");
                }
                else if (randNum == 2)
                {
                    qte_Handler.ChestAttempt(0, 100, false, cellPosition, "closed1");
                }
                else if (randNum == 3)
                {
                    puzzleHandler.ChestAttempt(0, 100, false, cellPosition, "closed1");
                }
                else if (randNum == 4)
                {
                    lock_handler.ChestAttempt(200, 400, true, cellPosition, "closed1");
                }

                overwriteHint = true;
                tilemap.SetTile(cellPosition, openChest); // Change the tile to open chest
                hint_Panel.gameObject.SetActive(false);
                hint_TXT.text = "";
                isAllowed = false;
                hint_Panel.gameObject.SetActive(false);
            }
            else
            {
                if (tilemap.GetTile(cellPosition) == closedChest2) // Check if the chest tile is closed
                {
                    overwriteHint = true;
                    int randNum = Random.Range(1, 4);
                    if (randNum == 1)
                    {
                        lock_handler.ChestAttempt(200, 400, true, cellPosition, "closed2");
                    }
                    else if (randNum == 2)
                    {
                        qte_Handler.ChestAttempt(200, 400, true, cellPosition, "closed2");
                    }
                    else if (randNum == 3)
                    {
                        puzzleHandler.ChestAttempt(200, 400, true, cellPosition, "closed2");
                    }
                    else if (randNum == 4)
                    {
                        lock_handler.ChestAttempt(200, 400, true, cellPosition, "closed2");
                    }
                    tilemap.SetTile(cellPosition, openChest); // Change the tile to open chest
                    hint_TXT.text = "";
                    isAllowed = false;
                    hint_Panel.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("There is an issue here!");
                }
            }
        }
    }

    public void GrantXP(int xpR1, int xpR2)
    {
        int xpAmnt = Random.Range(xpR1, xpR2);
        xp_Handler.AddXP(xpAmnt);
    }

    private IEnumerator chestReset(Vector3Int cellPosition, Tile tileToChangeTo)
    {
        yield return new WaitForSeconds(1);
        isAllowed = true;
        tilemap.SetTile(tilemap.WorldToCell(cellPosition), null);
        isChestOpen = false;
        tileToChangeTo = null;
        overwriteHint = false;
    }

    void Update()
    {
        Vector3Int playerCell = tilemap.WorldToCell(player.position);
        float closestDistance = activationDistance;
        Vector3Int closestTilePos = Vector3Int.zero;
        bool foundClosestTile = false;

        for (int x = -activationDistance; x <= activationDistance; x++)
        {
            for (int y = -activationDistance; y <= activationDistance; y++)
            {
                Vector3Int checkPos = playerCell + new Vector3Int(x, y, 0);
                if (!tilemap.HasTile(checkPos)) continue;

                Vector3 tileWorldPosition = tilemap.CellToWorld(checkPos);
                float distance = Vector3.Distance(player.position, tileWorldPosition);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTilePos = checkPos;
                    foundClosestTile = true;
                }
            }
        }

        if (foundClosestTile)
        {
            if (!overwriteHint)
            {
                hint_Panel.gameObject.SetActive(true);
                hint_Panel.gameObject.transform.position = tilemap.CellToWorld(closestTilePos) + new Vector3(0, 1, 0);
                hint_TXT.text = hintText;
                if (Input.GetKeyDown(KeyCode.E) && isAllowed)
                {
                    hint_Panel.gameObject.SetActive(false);
                    hint_TXT.text = "";
                    OpenChest(closestTilePos);
                }
            }
        }
        else
        {
            if (!overwriteHint)
            {
                hint_Panel.position = Vector3.zero;
                hint_Panel.gameObject.SetActive(false);
                hint_TXT.text = "";
            }
        }
    }
}
