using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DefenceClothes : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtdef;
    private string saveFilePath;
    [SerializeField] private GamePlay gamePlay;
    [SerializeField] private StackItem itemStack;
    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "LoadedGame.json");
    }
    private void Start()
    {
        
    }
    public void UpdateDef()
    {

        if (gameObject.transform.childCount > 0)
        {
            
            Transform item = gameObject.transform.GetChild(0);
            Debug.Log(item);
            string json = File.ReadAllText(saveFilePath);
            itemStack = item.gameObject.GetComponent<StackItem>();
            Debug.Log(itemStack.itemName);
            ItemData itemData = JsonUtility.FromJson<ItemData>(json);
            foreach (var Item in itemData.items)
            {
                
                if (Item.name == itemStack.itemName)
                {
                    
                    if (itemStack.itemName == "Hat")
                    {
                        //Debug.Log("Hat");
                        gamePlay.defenceHat = 3;
                        txtdef.text = "3";
                    }
                    if (itemStack.itemName == "Helmet")
                    {
                        gamePlay.defenceHat = 10;
                        txtdef.text = "10";
                        //Debug.Log("Helmet");
                    }
                    if (itemStack.itemName == "Jacket")
                    {
                        gamePlay.defenceArmor = 3;
                        txtdef.text = "3";
                        //Debug.Log("Jacket");
                    }
                    if (itemStack.itemName == "Armor")
                    {
                        gamePlay.defenceArmor = 10;
                        txtdef.text = "10";
                        //Debug.Log("Armor");
                    }


                }
            }
        }
        else          
        {
            //Debug.Log("ytneArmor");
            txtdef.text = "0";
        }
    }
    
}
