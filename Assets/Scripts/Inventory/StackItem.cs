using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class StackItem : MonoBehaviour
{
    public TextMeshProUGUI amountText;
    public int currentAmount;
    public ItemType type;
    public int maxStack;
    public string itemName;
    public void UpdateCurrentAmount()
    {
        amountText.text= currentAmount.ToString();
    }

}
