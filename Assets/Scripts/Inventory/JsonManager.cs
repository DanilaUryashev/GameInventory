using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class JsonManager : MonoBehaviour
{
    private string saveFilePath;
    public DefaultItemsList defaultItemsList;
    //void Start()
    //{
    //    saveFilePath = Path.Combine(Application.persistentDataPath, "LoadedGame.json");

    //    // Проверяем, существует ли файл
    //    if (!File.Exists(saveFilePath))
    //    {
    //        CreateDefaultJsonFile();
    //    }
    //    else
    //    {
    //        LoadJsonFile();
    //    }
    //}
    private IEnumerator Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "LoadedGame.json");
        Debug.Log("Путь к файлу: " + saveFilePath);

        yield return new WaitUntil(() => DefaultItemsList.Instance != null);

        if (!File.Exists(saveFilePath))
        {
            CreateDefaultJsonFile();
        }
        else
        {
            LoadJsonFile();
        }
    }
    public void CreateDefaultJsonFile()
    {
        if (DefaultItemsList.Instance == null || DefaultItemsList.Instance.defaultItems == null)
        {
            Debug.LogError("DefaultItemsList.Instance пуст!");
            return;
        }

        ItemData defaultData = new ItemData
        {
            inventory = DefaultItemsList.Instance.defaultItems.ConvertAll(item => new Inventory
            {
                placeindex = item.placeindex.ToString(),
                name = item.itemName,  // name теперь будет корректным
                itemAmount = item.itemAmount.ToString()
            }).ToArray(),
            items = new Item[]
            {
                    new Item { name = "Ammo556", type = ItemType.Ammo556, maxStack = "100", weight= "0.03", defence=""},
                    new Item { name = "Ammo918", type = ItemType.Ammo918, maxStack = "50", weight= "0.01", defence="" },
                    new Item { name = "MedKit", type = ItemType.MedKit, maxStack = "6", weight= "1", defence=""},
                    new Item { name = "Hat", type = ItemType.Clothes, maxStack = "1", weight= "0.1", defence="3"},
                    new Item { name = "Helmet", type = ItemType.Clothes, maxStack = "1", weight= "1",defence="10" },
                    new Item { name = "Jacket", type = ItemType.Clothes, maxStack = "1", weight= "1",defence="3" },
                    new Item { name = "Armor", type = ItemType.Clothes, maxStack = "1", weight= "10",defence="10" }
            },
            level = new Level[]
            {
                new Level { hpPlayer = "100", hpEnemy = "100", level = "Level1" }
            }
        };

        
        string json = JsonUtility.ToJson(defaultData, true);
        Debug.Log(json);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("JSON сохранен в: " + saveFilePath);

        //ItemData defaultData = new ItemData
        //{
        //    items = new Item[]
        //    {
        //        new Item { name = "Ammo556", type = ItemType.Ammo556, maxStack = "100", weight= "0.03", defence=""},
        //        new Item { name = "Ammo918", type = ItemType.Ammo918, maxStack = "50", weight= "0.01", defence="" },
        //        new Item { name = "MedKit", type = ItemType.MedKit, maxStack = "6", weight= "1", defence=""},
        //        new Item { name = "Hat", type = ItemType.Clothes, maxStack = "1", weight= "0.1", defence="3"},
        //        new Item { name = "Helmet", type = ItemType.Clothes, maxStack = "1", weight= "1",defence="10" },
        //        new Item { name = "Jacket", type = ItemType.Clothes, maxStack = "1", weight= "1",defence="3" },
        //        new Item { name = "Armor", type = ItemType.Clothes, maxStack = "1", weight= "10",defence="10" }
        //    },
        //    inventory = new Inventory[]
        //    {
        //        new Inventory { placeindex = "1", name = "Ammo918", itemAmount = "10" },
        //        new Inventory { placeindex = "2", name = "MedKit", itemAmount = "2" },
        //        new Inventory { placeindex = "3", name = "Jacket", itemAmount = "1" },
        //        new Inventory { placeindex = "4", name = "Hat", itemAmount = "1" },
        //        new Inventory { placeindex = "5", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "6", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "7", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "8", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "9", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "10", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "11", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "12", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "13", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "14", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "15", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "16", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "17", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "18", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "19", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "20", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "21", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "22", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "23", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "24", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "25", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "26", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "27", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "28", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "29", name = "", itemAmount = "" },
        //        new Inventory { placeindex = "30", name = "", itemAmount = "" },

        //    },
        //    level = new Level[]
        //    {
        //        new Level { hpPlayer = "100", hpEnemy = "100", level = "Level1" }
        //    }
        //};

        //string json = JsonUtility.ToJson(defaultData, true);
        //File.WriteAllText(saveFilePath, json);
    }

    private void LoadJsonFile()
    {
        string json = File.ReadAllText(saveFilePath);
        ItemData characterData = JsonUtility.FromJson<ItemData>(json);
    }
}

[System.Serializable]
public class ItemData
{
    public Inventory[] inventory;
    public Item[] items;
    public Level[] level;
}

[System.Serializable]
public class Item
{
    public string name; // Название предмета и ссылка на префаб
    public ItemType type; // Тип предмета
    public string maxStack; // Максимальный размер стака
    public string weight;
    public string defence;
}
public enum ItemType
{
    Ammo556,  // Патроны 5.56
    Ammo918,  // Патроны 9.18
    MedKit,   // Аптечка
    Clothes   // Одежда
}
[System.Serializable]
public class Inventory
{
    public string placeindex; //1-30 место в инвентореэ
    public string name; //имя картинки в папке resource
    public string itemAmount; // сколько предметов в ячейке
}
[System.Serializable]
public class Level
{
    public string hpPlayer;
    public string hpEnemy;
    public string level; // картирнка уровеня
}

