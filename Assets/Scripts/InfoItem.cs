using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;
public class InfoItem : MonoBehaviour
{
    [SerializeField] private GameObject infoBoard;
    [SerializeField] private GameObject hatEquipPlace;
    [SerializeField] private GameObject armorEquipPlace;
    [SerializeField] private GamePlay gamePlay;
    public TextMeshProUGUI nameTex;
    public TextMeshProUGUI defenceText;
    public TextMeshProUGUI weightText;

    public TextMeshProUGUI hatdefEqpText;
    public TextMeshProUGUI armdefEqpText;

    public Image image;
    public Image defenceImage;
    private GameObject selectItem;
    [SerializeField] GameObject inventory;
    [Header("Кнопки")]
    [SerializeField] GameObject equipButton;
    [SerializeField] GameObject takeoffButton;
    [SerializeField] GameObject UseButton;
    [SerializeField] GameObject deleteButton;
    [SerializeField] GameObject ammoButton;
    private void Start()
    {
        infoBoard = gameObject;
    }
    public void SelectItem(GameObject select)
    {
        selectItem = select;
        if (selectItem.transform.parent == hatEquipPlace.transform || selectItem.transform.parent == armorEquipPlace.transform)
        {
            equipButton.SetActive(false);
            takeoffButton.SetActive(true);
            deleteButton.SetActive(true);
            ammoButton.SetActive(false);
            UseButton.SetActive(false);
        }
        else
        {
            StackItem stackItem = selectItem.GetComponent<StackItem>();
            if (stackItem.type == ItemType.Clothes)
            {
                equipButton.SetActive(true);
                takeoffButton.SetActive(false);
                deleteButton.SetActive(true);
                ammoButton.SetActive(false);
                UseButton.SetActive(false);
            }
            else if (stackItem.type == ItemType.MedKit)
            {
                equipButton.SetActive(false);
                takeoffButton.SetActive(false);
                deleteButton.SetActive(true);
                ammoButton.SetActive(false);
                UseButton.SetActive(true);
            }
            else
            {
                equipButton.SetActive(false);
                takeoffButton.SetActive(false);
                deleteButton.SetActive(false);
                ammoButton.SetActive(true);
                UseButton.SetActive(false);
            }
        }
    }
    public void DeleteButton()
    {
        infoBoard.SetActive(false);
        Destroy(selectItem);
    }
    public void EquipItem()
    {

        StackItem stackItem = selectItem.GetComponent<StackItem>();
        if (stackItem.itemName == "Hat" || stackItem.itemName == "Helmet")
        {
            if (hatEquipPlace.transform.childCount != 0)
            {
                Transform item = hatEquipPlace.transform.GetChild(0);
                item.SetParent(selectItem.transform.parent);
                CenterAndStretchItem(item.gameObject);
            }
            selectItem.transform.SetParent(hatEquipPlace.transform);
            CenterAndStretchItem(selectItem);
            if (stackItem.itemName == "Hat")
            {
                gamePlay.defenceHat = 3;
                hatdefEqpText.text = "3";
            }
            if (stackItem.itemName == "Helmet")
            {
                gamePlay.defenceHat = 10;
                hatdefEqpText.text = "10";
            }

        }
        if (stackItem.itemName == "Jacket" || stackItem.itemName == "Armor")
        {
            if (armorEquipPlace.transform.childCount != 0)
            {
                Transform item = hatEquipPlace.transform.GetChild(0);
                item.SetParent(selectItem.transform.parent);
                CenterAndStretchItem(item.gameObject);
            }
            selectItem.transform.SetParent(armorEquipPlace.transform);
            CenterAndStretchItem(selectItem);
            if (stackItem.itemName == "Jacket")
            {
                gamePlay.defenceHat = 3;
                armdefEqpText.text = "3";
            }
            if (stackItem.itemName == "Armor")
            {
                gamePlay.defenceHat = 10;
                armdefEqpText.text = "10";
            }
        }
        infoBoard.SetActive(false);
        DefenceClothes defenceClothes = hatEquipPlace.GetComponent<DefenceClothes>();
        defenceClothes.UpdateDef();

        defenceClothes = armorEquipPlace.GetComponent<DefenceClothes>();
        defenceClothes.UpdateDef();

    } 
        
    
    private void CenterAndStretchItem(GameObject item)
    {
        Debug.Log("SDafdsjn" + item);
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localScale = Vector3.one;
    }
    public void CloseInfoBoard()
    {
       
        infoBoard.SetActive(false);
    }


    public void ButtonTakeOff()
    {
        DefenceClothes defenceClothes = selectItem.transform.parent.GetComponent<DefenceClothes>();
        Transform nullSlot = CheckSlot();
        selectItem.transform.SetParent(nullSlot);
        CenterAndStretchItem(selectItem);
        defenceClothes.UpdateDef();
        infoBoard.SetActive(false);


    }
    public void ButtonUse()
    {
        gamePlay.Heal();
        StackItem stackItem = selectItem.GetComponent<StackItem>();
        stackItem.currentAmount -= 1;
        stackItem.UpdateCurrentAmount();
        infoBoard.SetActive(false);
    }
    public Transform CheckSlot()
    {
        foreach (Transform slot in inventory.transform) // Перебираем все слоты
        {
            if (slot.name.StartsWith("Slot") && slot.childCount == 0) // Если слот пустой
            {
                return slot; // Возвращаем первый найденный пустой слот
            }
        }
        return null; // Если пустых слотов нет, возвращаем null
    }
}
