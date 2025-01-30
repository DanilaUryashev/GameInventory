using NUnit.Framework.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour
{
    [Header("������� ���������")]
    public float maxHealthPlayer = 100f;
    public float maxHealthEnemy = 100f;
    public float currentHealthPlayer;
    public float currentHealthEnemy;
    [SerializeField] private float damageAssault;
    [SerializeField] private float damagePistol;
    [SerializeField]private float damage;
    public float defenceHat;
    public float defenceArmor;
    private float healthIFAK = 50f;
   
    [Header("�� ����������")]
    [SerializeField] private Image healthBarFillEnemy;
    [SerializeField] private Image healthBarFillPlayer;
    [SerializeField] private TextMeshProUGUI enemyHP;
    [SerializeField] private TextMeshProUGUI playerHP;
    

    [SerializeField] private GameObject inventory;
    private string saveFilePath;
    public Image backgroundLevel;
    private int lvlnumber=1;
    [SerializeField] private GameObject addItemBoard;

    [SerializeField] GameObject gameOver;
    [Header("���� � ���������")]
    [SerializeField] private GameObject min918; // ���� � ����������� ������������ 918
    [SerializeField] private GameObject min556; // ���� � ����������� ������������ 556

    [SerializeField] private Transform prev918;
    [SerializeField] private Transform prev556;
    
    [Header("���������� ������")]
    [SerializeField] private int bullet918 = 0;
    [SerializeField] private int bullet556 = 0;
    [SerializeField] private bool eqpWeapon = false;
    
    [Header("������ �����")]
    [SerializeField] private Image selectRifle;
    [SerializeField] private Image selectPistol;
   
    [Header("������� ���������")]
    [SerializeField] private GameObject emptyPistolImage;
    [SerializeField] private GameObject emptyAssautImage;
    [SerializeField] private Button attackPistol;
    [SerializeField] private Button attackAssaut;

    [Header("������� ������� ������ ���� ��� �������� �����")]
    [SerializeField] private int minIssuedBullet918=5;
    [SerializeField] private int minIssuedBullet556=10;

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "LoadedGame.json");
        currentHealthEnemy = maxHealthEnemy;
        currentHealthPlayer = maxHealthPlayer;
        enemyHP.text = currentHealthEnemy.ToString();
        playerHP.text = currentHealthPlayer.ToString();
        SwitchWeapon(eqpWeapon);
        //TotalBullets();
        Invoke(nameof(FindAndCalculateAmmo), 0.1f); // �������� ����� ������ ���������� ��������
    }

    public void Attack()
    {
        
       // Debug.Log("2");
        

        if (!eqpWeapon)
        {
            if (bullet918 <= 0) return;
            bullet918 -= 1;
            min918.GetComponent<StackItem>().currentAmount -= 1;
            min918.GetComponent<StackItem>().UpdateCurrentAmount();
            if (min918.GetComponent<StackItem>().currentAmount <= 0)
            {
                prev918 = min918.transform.parent;
                Destroy(min918);
                min918 = null;
                bullet918 = 0;
                Debug.Log("<color=red>" + prev918);
                FindAndCalculateAmmo();
            }

        }
        else
        {
            if (bullet556 <= 0) return;
            bullet556 -= 3;
            min556.GetComponent<StackItem>().currentAmount -= 3;
            min556.GetComponent<StackItem>().UpdateCurrentAmount();
            if (min556.GetComponent<StackItem>().currentAmount <= 0)
            {
                prev556 = min556.transform.parent;
                Destroy(min556);
                min556 = null;
                bullet556 = 0;
                FindAndCalculateAmmo();
            }

        }
        currentHealthEnemy -= damage;
        float fillAmount = currentHealthEnemy / maxHealthEnemy;
        healthBarFillEnemy.fillAmount = fillAmount;
        enemyHP.text = currentHealthEnemy.ToString();
        if (currentHealthEnemy <= 0)
        {
            DieEnemy();
            RandomTakeItem();
        }else TakeDamage();

        CheckBullet();
       
    }

    private bool head = false;
    [SerializeField] private float damageEnemy = 15f;
    public void TakeDamage()
    {
        Debug.Log("das");
        //�������� ��� �������� ���� �� ����� ���� � ����
        if (head)
        {
            currentHealthPlayer -= damageEnemy - defenceHat;
            float fillAmount = currentHealthPlayer / maxHealthPlayer;
            healthBarFillPlayer.fillAmount = fillAmount;
            playerHP.text = currentHealthPlayer.ToString();
            head = false;
        }
        else
        {
            currentHealthPlayer -= damageEnemy - defenceArmor;
            float fillAmount = currentHealthPlayer / maxHealthPlayer;
            healthBarFillPlayer.fillAmount = fillAmount;
            playerHP.text = currentHealthPlayer.ToString();

            head = true;
        }
        

    }

    public void Heal()
    {
        currentHealthPlayer += healthIFAK;
    }

    private void DiePlayer()
    {
        gameOver.SetActive(true);
    }
    public void NewGame()
    {
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("���� ���������� �����!");
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void QuitGame()
    {
        Debug.Log("���� ��������� ����� 0.2 ������...");
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("���� ���������� �����!");
        }
        Invoke(nameof(ExitGame), 0.2f);
    }
    public void ExitGame()
    {
        Debug.Log("���� �����������...");
        #if UNITY_EDITOR
        Debug.Log("����� �� ������ ���� � ���������...");
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
        // ��� ��������� Unity

    }
    private void DieEnemy()
    {
        if(lvlnumber==3) lvlnumber = 0;
        lvlnumber += 1;
        Sprite itemsprite = Resources.Load<Sprite>($"Levels/Level {lvlnumber}");
        backgroundLevel.sprite= itemsprite;
        currentHealthEnemy=maxHealthEnemy;
        float fillAmount = currentHealthEnemy / currentHealthEnemy;
        healthBarFillEnemy.fillAmount = fillAmount;
        enemyHP.text = currentHealthEnemy.ToString();
    }
    public void SwitchWeapon(bool weapon)
    {
        if (weapon)
        {
            damage = damageAssault;
            selectRifle.color = Color.green;
            selectPistol.color = Color.gray;

        }
        else
        {
            damage = damagePistol;
            selectPistol.color = Color.green;
            selectRifle.color = Color.gray;
        }
        eqpWeapon = weapon;
    }
    public void FindAndCalculateAmmo()
    {
        Debug.Log("�������� ������� �������");
        bullet918 = 0; // �������� ������� ��������
        min918 = null; // ���������� ������ �� ����������� ����
        foreach (Transform slot in inventory.transform) // ���������� ��� �����
        {
            if (slot.name.StartsWith("Slot") && slot.childCount > 0) // ���������, ���� �� ������� � �����
            {
                Transform item = slot.GetChild(0); // �������� ������� � �����
                StackItem stackItem = item.GetComponent<StackItem>();

                if (stackItem != null && stackItem.type == ItemType.Ammo918 && slot != prev918) // ���������, ��� ��� ������� 918
                {
                    bullet918 += stackItem.currentAmount; // ��������� ���������� �������� � ����� ����

                    // ���������, �������� �� ����� ������ ������ ��������, ��� ��� ��������� ����������� ����
                    if (min918 == null || !min918 || min918.GetComponent<StackItem>().currentAmount > stackItem.currentAmount)
                    {
                        min918 = item.gameObject;
                    }
                }
                if (stackItem != null && stackItem.type == ItemType.Ammo556 && slot != prev556) // ���������, ��� ��� ������� 918
                {
                    bullet556 += stackItem.currentAmount; // ��������� ���������� �������� � ����� ����

                    // ���������, �������� �� ����� ������ ������ ��������, ��� ��� ��������� ����������� ����
                    if (min556 == null || !min556 || min556.GetComponent<StackItem>().currentAmount > stackItem.currentAmount)
                    {
                        min556 = item.gameObject;
                    }
                }
            }
        }
     
        CheckBullet();
    }
    public void CheckBullet()
    {
        if (bullet556 == 0)
        {
            emptyAssautImage.SetActive(true);
            attackAssaut.interactable = false;
        }
        else
        {
            emptyAssautImage.SetActive(false);
            attackAssaut.interactable = true;
        }


        if (bullet918 == 0)
        {
            emptyPistolImage.SetActive(true);
            attackPistol.interactable = false;
        }
        else
        {
            emptyPistolImage.SetActive(false);
            attackPistol.interactable = true;
        }

    }
    private void RandomTakeItem()
    {
        ItemType randomType = GetRandomItemType();
        Transform slot = CheckSlot();
        if (slot != null)
        {
            if (File.Exists(saveFilePath))
            {
                string json = File.ReadAllText(saveFilePath);
                ItemData itemData = JsonUtility.FromJson<ItemData>(json);
                Canvas canvas = UnityEngine.Object.FindFirstObjectByType<Canvas>();

                List<Item> possibleItems = itemData.items
                    .Where(item => item.type == randomType)
                    .ToList();

                // ���� ������� ��������� ��������� (��������� ������ ��� ������), �������� ����
                if (possibleItems.Count == 0)
                    return;

                Item selectedItem = possibleItems[UnityEngine.Random.Range(0, possibleItems.Count)];

                GameObject prefabItemCell = Resources.Load<GameObject>($"UseItems/ObjectRef");
                GameObject instance = Instantiate(prefabItemCell);
                instance.transform.SetParent(canvas.transform);
                RectTransform rectTransform = instance.GetComponent<RectTransform>();
                rectTransform.localScale = Vector3.one;
                instance.transform.SetParent(slot.transform);

                Transform itemImage = instance.transform.Find("ImageObject");
                Image imageComponent = itemImage.GetComponent<Image>();
                StackItem stackItem = instance.GetComponent<StackItem>();
                Sprite itemsprite = Resources.Load<Sprite>($"UseItems/{selectedItem.name}");

                imageComponent.sprite = itemsprite;
                TextMeshProUGUI amountText = instance.transform.Find("ItemAmount").GetComponent<TextMeshProUGUI>();

                string itemAmount = null;

                if (selectedItem.type == ItemType.Clothes)
                {
                    Destroy(amountText.gameObject);
                    itemAmount = "1";
                    
                }
                else
                {
                    int randomInt = UnityEngine.Random.Range(
                        (selectedItem.type == ItemType.Ammo556) ? minIssuedBullet556 :
                        (selectedItem.type == ItemType.Ammo918) ? minIssuedBullet918 :
                        (selectedItem.type == ItemType.MedKit) ? 5 :
                        1, int.Parse(selectedItem.maxStack) + 1
                    );
                    itemAmount = randomInt.ToString();
                    amountText.text = itemAmount;
                   
                }
                stackItem.itemName = selectedItem.name;
                stackItem.currentAmount = int.Parse(itemAmount);
                stackItem.type = selectedItem.type;
                stackItem.maxStack = int.Parse(selectedItem.maxStack);

                CenterAndStretchItem(instance.transform, 0);
                CenterAndStretchItem(itemImage.transform, 1);
                CenterAndStretchItem(amountText.transform, 2);
                ViewAddItem viewAddItem = addItemBoard.GetComponent<ViewAddItem>();
                viewAddItem.ViewItem(instance);
                addItemBoard.SetActive(true);
            }
        }
    }
    public static ItemType GetRandomItemType()
    {
        Array values = Enum.GetValues(typeof(ItemType));
        return (ItemType)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }
    public Transform CheckSlot()
    {
        foreach (Transform slot in inventory.transform) // ���������� ��� �����
        {
            if (slot.name.StartsWith("Slot") && slot.childCount == 0) // ���� ���� ������
            {
                return slot; // ���������� ������ ��������� ������ ����
            }
        }
        return null; // ���� ������ ������ ���, ���������� null
    }
    private void CenterAndStretchItem(Transform transform, int type)
    {
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        // � ����������� �� ���� ��������� ��������������� �������
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
            // ��� �������� � ������ �������� �������
            rectTransform.offsetMin = new Vector2(15, 15);
            rectTransform.offsetMax = new Vector2(-15, -15);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
        }
        else
        {
            // ��� ������ �������� � ������ �������� �������
            rectTransform.offsetMin = new Vector2(105, 0);
            rectTransform.offsetMax = new Vector2(-10, -150);
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
        }
    }

}
