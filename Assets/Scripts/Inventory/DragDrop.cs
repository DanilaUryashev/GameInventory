using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.IO;
using TMPro;
using UnityEngine.TextCore.Text;
public class DragDrop : MonoBehaviour,IPointerDownHandler, IPointerUpHandler,  IBeginDragHandler, IEndDragHandler, IDragHandler
{

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Vector3 originalPosition;
    public Transform originalParent;
    private string saveFilePath;


    [SerializeField] private Transform infoBoard;
    private TextMeshProUGUI NameText;

    private float timeHeld = 0f; // Время удержания в данный момент
    [SerializeField]private bool isMouseHeldDown = false; // Флаг для отслеживания удержания
    [SerializeField] private bool isDrag = false; // Флаг для отслеживания перенос

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originalPosition = transform.localPosition;
        originalParent = transform.parent;
        saveFilePath = Path.Combine(Application.persistentDataPath, "LoadedGame.json");
        Transform parentTransform = GameObject.Find("Canvas").transform; // Найти родительский объект
        infoBoard = parentTransform.Find("InfoBoard");

    }
    private void Start()
    {
        //saveFilePath = Path.Combine(Application.persistentDataPath, "LoadedPrefabs.json");
        if (canvas == null)
        {
            canvas = GetComponentInParent<Canvas>();
        }

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        timeHeld = 0f; 
        isMouseHeldDown = true;

    }
    public void OnPointerUp(PointerEventData eventData)
    {

        isMouseHeldDown = false;
        
        if (timeHeld<=0.2f&& !isDrag) ViewInfo(eventData.pointerEnter.gameObject);
        timeHeld = 0f;
       
    }

    private void Update()
    {
        if (isMouseHeldDown)
        {
            timeHeld += Time.deltaTime; // Увеличиваем время удержания
            
        }

    }

    public void OnDrag(PointerEventData eventData)
    {

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        isDrag=true;
        originalPosition = transform.localPosition; // Запоминаем положение на старте
        originalParent = transform.parent; // Запоминаем родителя   
                                           // transform.SetParent(transform.root);
        canvasGroup.alpha = .7f;
        canvasGroup.blocksRaycasts = false;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag=false;
        if (eventData.pointerEnter != originalParent)
        {
            if (!eventData.pointerEnter.CompareTag("Cell"))
            {
                if (!eventData.pointerEnter.CompareTag("Item"))
                {
                    Debug.Log("<Color=red> ReturnToOriginalPosition " + eventData.pointerEnter);
                    ReturnToOriginalPosition(); // Возврат на исходную позицию, если не в зоне
                }
            }

        }
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        Transform currentParent;
        Transform areaCard = eventData.pointerEnter.transform;
        currentParent = areaCard;
        currentParent = CheckParent(currentParent);
        if (currentParent == originalParent) 
        {
            Debug.Log("Тотт же родитель");
        }
        Debug.Log(currentParent.name);
        

    }
    public void ReturnToOriginalPosition()
    {
        transform.SetParent(originalParent); // Возвращаем родителю
        transform.localPosition = originalPosition; // Возвращаем на исходную позицию
        canvasGroup.alpha = 1f; // Сбрасываем прозрачность
        canvasGroup.blocksRaycasts = true; // Делаем объект снова доступным для raycast
    }

    public Transform CheckParent(Transform currentParent)
    {
        while (currentParent != null)
        {
            if (currentParent.CompareTag("Cell"))
            {
                if (currentParent == originalParent) return null;
                // Условие истинно, если текущий родитель имеет тег "Cell"
                Debug.Log("ROD123ITEl " + currentParent);
                return currentParent; // Выход из цикла, если найдено совпадение
            }
            currentParent = currentParent.parent; // Переход к следующему родителю
        }
        return null;
    }

    public void SearchComponentInfo()
    {
        //PrevUnit = GameObject.Find("PreviewImage").transform;

        //Transform name = GameObject.Find("Name").transform;
        //NameText = name.GetComponent<TextMeshProUGUI>();

        //Transform description = GameObject.Find("Description").transform;
        //DescriptionText = description.GetComponent<TextMeshProUGUI>();

        //Transform health = GameObject.Find("HealthInfoUnit").transform;
        //HealthText = health.GetComponent<TextMeshProUGUI>();

        //Transform spell = GameObject.Find("SpellDescription").transform;
        //SpellText = spell.GetComponent<TextMeshProUGUI>();

        //Transform damage = GameObject.Find("DamageInfoUnit").transform;
        //DamageText = damage.GetComponent<TextMeshProUGUI>();

        //Transform defence = GameObject.Find("DefenceInfoUnit").transform;
        //DefenceText = defence.GetComponent<TextMeshProUGUI>();

        //Transform price = GameObject.Find("PriceInfoUnit").transform;
        //PriceText = price.GetComponent<TextMeshProUGUI>();

        //Transform upgradePrice = GameObject.Find("UpgradePrice").transform;
        //UpgradePriceText = upgradePrice.GetComponent<TextMeshProUGUI>();

        //Transform LVL = GameObject.Find("LvlText").transform;
        //LVLText = LVL.GetComponent<TextMeshProUGUI>();

    }

    public void ViewInfo(GameObject item)
    {
        infoBoard.gameObject.SetActive(true);
        
        InfoItem infoBoardscrp= infoBoard.GetComponent<InfoItem>();
        TextMeshProUGUI defenceText = infoBoardscrp.defenceText;
        TextMeshProUGUI weightText = infoBoardscrp.weightText;
        TextMeshProUGUI name = infoBoardscrp.nameTex;
        Image image = infoBoardscrp.image;
        Image defenceImage = infoBoardscrp.defenceImage;
        StackItem stackItem = item.GetComponent<StackItem>();
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            ItemData itemData = JsonUtility.FromJson<ItemData>(json);

            foreach (var items in itemData.items)
            {
            
            if (stackItem.itemName == items.name)
                {
                if (stackItem.type != ItemType.Clothes)
                {
                    defenceImage.gameObject.SetActive(false);
                    defenceText.gameObject.SetActive(false);
                }
                else
                {
                    defenceText.text = "+" + items.defence;
                   
                }
                weightText.text = items.weight;
                name.text = items.name;
                Sprite itemsprite = Resources.Load<Sprite>($"UseItems/{items.name}");
                image.sprite = itemsprite;
            }
            }
        infoBoardscrp.SelectItem(item);
        }
        else Debug.Log("нет сохранения");
    }

}


