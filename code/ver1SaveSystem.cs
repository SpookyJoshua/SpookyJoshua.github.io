using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections;

public class SaveSystemAdvanced : MonoBehaviour
{
    public List<GameObject> GameObjects = new List<GameObject>();
    public static List<BuildAdvanced> Items = new List<BuildAdvanced>();
    string Item_SUB;
    string Item_Count_SUB;
    string ScenePath;
    public bool autoSave = true;
    public float autoSaveInterval = 60f;

    private void Awake()
    {
        ScenePath = PlayerPrefs.GetString("Folder") + "/";
        Item_SUB = "/saveSystem/" + ScenePath +  SceneManager.GetActiveScene().name + "/items";
        Item_Count_SUB = "/saveSystem/" + ScenePath +  SceneManager.GetActiveScene().name + "/items.count";

        if (!Directory.Exists(Application.persistentDataPath + "/saveSystem/" + ScenePath +  SceneManager.GetActiveScene().name))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/saveSystem/" + ScenePath +  SceneManager.GetActiveScene().name);
        }
        LoadItems();
    }
    private void OnApplicationQuit()
    {
        SaveItems();
    }

    public void Exit()
    {
        SaveItems();
    }
    public void SaveItems()
    {

        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + Item_SUB;
        string Countpath = Application.persistentDataPath + Item_Count_SUB;
        FileStream countStream = new FileStream(Countpath, FileMode.Create);
        formatter.Serialize(countStream, Items.Count);
        countStream.Close();

        for (int i = 0; i < Items.Count; i++)
        {
            FileStream stream = new FileStream(path + i, FileMode.Create);
            BuildDataAdvanced data = new BuildDataAdvanced(Items[i]);
            formatter.Serialize(stream, data);
            stream.Close();
        }
    }

    void LoadItems()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + Item_SUB;
        string Countpath = Application.persistentDataPath + Item_Count_SUB;
        int itemCount = 0;
        if (File.Exists(Countpath))
        {
            FileStream countStream = new FileStream(Countpath, FileMode.Open);
            itemCount = (int)formatter.Deserialize(countStream);
            countStream.Close();
        }

        for (int i = 0; i < itemCount; i++)
        {
            if (File.Exists(path + i))
            {
                FileStream stream = new FileStream(path + i, FileMode.Open);
                BuildDataAdvanced data = formatter.Deserialize(stream) as BuildDataAdvanced;
                stream.Close();
                string itemName = data.Name;
                Vector3 position = new Vector3(data.position[0], data.position[1], data.position[2]);
                Quaternion rotation = new Quaternion(data.rot[0], data.rot[1], data.rot[2], data.rot[3]);
                char[] myChars = { '(' };
                string myText = data.Name;
                string[] name = myText.Split(myChars);
                itemName = name[0];
                GameObject buildItem = GameObjects.Find((x) => x.name == itemName);
                GameObject item = Instantiate(buildItem, position, rotation);
                item.name = itemName;
            }
        }
    }


    private void Update()
    {
        if (autoSave)
        {
            autoSave = false;
            StartCoroutine(autoDelay());
        }
    }

    IEnumerator autoDelay()
    {
        yield return new WaitForSeconds(autoSaveInterval);
        SaveItems();
        autoSave = true;
    }
}
