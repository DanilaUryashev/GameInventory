using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class LoadedGame : MonoBehaviour
{
    private string saveFilePath;

    [SerializeField] private JsonManager jsonManager; // Ссылка на JsonManager
    [SerializeField] private GamePlay gamePlay;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject hatPlace;
    [SerializeField] private GameObject armor;
    private void Awake()
    {
        // Указываем путь к файлу
        saveFilePath = Path.Combine(Application.persistentDataPath, "LoadedGame.json");

        // Проверяем, существует ли файл
        if (!File.Exists(saveFilePath))
        {
            Debug.Log("Файл сохранения не найден. Создаём новый файл...");
            if (jsonManager != null)
            {
                jsonManager.CreateDefaultJsonFile();
                 // Создаём файл через JsonManager
            }
            else
            {
                Debug.LogError("JsonManager не настроен в инспекторе!");
            }
        }
        else
        {
            Debug.Log("Файл сохранения найден: " + saveFilePath);
        }
        Invoke(nameof(loadedInventory), 0.1f);
       
    }
    private void Start()
    {

    }
    public void loadedInventory()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            ItemData itemData = JsonUtility.FromJson<ItemData>(json);
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();
            foreach (var inventory in itemData.inventory)
            {
                GameObject slot = GameObject.Find("Slot " + inventory.placeindex);
                string itemAmount = inventory.itemAmount;
                GameObject prefabItemCell = Resources.Load<GameObject>($"UseItems/ObjectRef");

                if (!string.IsNullOrEmpty(inventory.name))
                {
                    GameObject instance = Instantiate(prefabItemCell);
                    instance.transform.SetParent(canvas.transform);
                    RectTransform rectTransform = instance.GetComponent<RectTransform>();
                    rectTransform.localScale = Vector3.one;
                    if (inventory.placeindex == "Hat")
                    {
                        instance.transform.SetParent(hatPlace.transform);

                    }
                    else if (inventory.placeindex == "Armor")
                    {
                        instance.transform.SetParent(armor.transform);
                        
                    }
                    else
                    {
                        instance.transform.SetParent(slot.transform);
                    }
                    
                    Transform itemImage = instance.transform.Find("ImageObject");
                    Image imageComponent = itemImage.GetComponent<Image>();
                    StackItem stackItem = instance.GetComponent<StackItem>();
                    Sprite itemsprite = Resources.Load<Sprite>($"UseItems/{inventory.name}");
                    imageComponent.sprite = itemsprite;
                    TextMeshProUGUI amountText = instance.transform.Find("ItemAmount").GetComponent<TextMeshProUGUI>();
                    foreach (var item in itemData.items)
                    {
                        if (item.name == inventory.name)
                        {
                            if (item.type == ItemType.Clothes)
                            {
                                Destroy(amountText.gameObject);
                            }
                            else
                            {
                                amountText.text = itemAmount;

                            }
                            stackItem.currentAmount = int.Parse(itemAmount);
                            stackItem.type = item.type;
                            stackItem.maxStack = int.Parse(item.maxStack);
                            stackItem.itemName = item.name;
                        }
                    }
                    CenterAndStretchItem(instance.transform, 0);
                    CenterAndStretchItem(itemImage.transform, 1);
                    CenterAndStretchItem(amountText.transform, 2);

                    DefenceClothes defenceClothes = hatPlace.GetComponent<DefenceClothes>();
                    defenceClothes.UpdateDef();

                    defenceClothes = armor.GetComponent<DefenceClothes>();
                    defenceClothes.UpdateDef();

                    

                }


            }
           
            //public string placeindex; //1-30 место в инвентореэ
            //public string name; //имя картинки в папке resource
            //public string itemAmount; // сколько предметов в ячейке
            foreach (var lvl in itemData.level)
            {
                gamePlay.currentHealthEnemy = int.Parse(lvl.hpEnemy);
                gamePlay.currentHealthPlayer = int.Parse(lvl.hpPlayer);
                Sprite backLvl = gamePlay.backgroundLevel.GetComponent<Sprite>();
                backLvl = Resources.Load<Sprite>($"Levels/{lvl.level}");
            }
        }
        

    }

    private void CenterAndStretchItem(Transform transform, int type)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        // В зависимости от типа применяем соответствующие отступы
        if (type == 0)
        {
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.sizeDelta = new Vector2(180, 190);
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        }
        else if (type == 1)
        {
            // Для отступов с учетом масштаба канваса
            rectTransform.offsetMin = new Vector2(15, 15);
            rectTransform.offsetMax = new Vector2(-15, -15);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
        }
        else
        {
            // Для других отступов с учетом масштаба канваса
            rectTransform.offsetMin = new Vector2(105, 0);
            rectTransform.offsetMax = new Vector2(-10, -150);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
        }


    }


    void OnApplicationQuit()
    {
        SaveGame();
    }

    void SaveGame()
    {
        if (File.Exists(saveFilePath))
        {
            // Загружаем текущие данные из JSON
            string json = File.ReadAllText(saveFilePath);
            ItemData saveData = JsonUtility.FromJson<ItemData>(json);

            // Обновляем инвентарь
            List<Inventory> saveInventorySlot = CheckInvent();
            saveData.inventory = saveInventorySlot.ToArray(); // Перезаписываем инвентарь

            // Обновляем данные уровня
            saveData.level = new Level[]
            {
            new Level
            {
                hpPlayer = gamePlay.currentHealthPlayer.ToString(),
                hpEnemy = gamePlay.currentHealthEnemy.ToString(),

                level = "Level1" // Можно менять динамически
            }
            };

            // Сериализуем обратно в JSON и сохраняем
            json = JsonUtility.ToJson(saveData, true);
            File.WriteAllText(saveFilePath, json);
            Debug.Log(saveData.inventory);
            Debug.Log(saveData.level);
            Debug.Log("Игра сохранена!");
        }
        else
        {
            Debug.LogError("Файл сохранения не найден!");
        }
    }
    private List<Inventory> CheckInvent()
    {
        List<Inventory> itemSlots = new List<Inventory>();

        foreach (Transform slot in inventory.transform) // Перебираем все слоты
        {
            if (slot.name.StartsWith("Slot") && slot.childCount > 0) // Проверяем, есть ли предмет в слоте
            {
                Transform item = slot.GetChild(0); // Получаем предмет в слоте

                StackItem stackItem = item.GetComponent<StackItem>();

                Inventory inventoryItem = new Inventory
                {
                    placeindex = slot.name.Replace("Slot ", ""), // Номер слота
                    name = stackItem.itemName, // Имя предмета
                    itemAmount = stackItem.currentAmount.ToString() // Можно добавить логику для подсчёта количества
                };

                itemSlots.Add(inventoryItem);
            }
        }
        if (hatPlace.transform.childCount > 0)
        {
            Transform hatItem = hatPlace.transform.GetChild(0);
            StackItem hatStackItem = hatItem.GetComponent<StackItem>();

            Inventory hatInventoryItem = new Inventory
            {
                placeindex = "Hat",
                name = hatStackItem.itemName,
                itemAmount = "1" // Одежда всегда в количестве 1
            };

            itemSlots.Add(hatInventoryItem);
        }

        // Проверяем armor
        if (armor.transform.childCount > 0)
        {
            Transform armorItem = armor.transform.GetChild(0);
            StackItem armorStackItem = armorItem.GetComponent<StackItem>();

            Inventory armorInventoryItem = new Inventory
            {
                placeindex = "Armor",
                name = armorStackItem.itemName,
                itemAmount = "1"
            };

            itemSlots.Add(armorInventoryItem);
        }

        return itemSlots;
    }
}
