using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryCells : MonoBehaviour, IDropHandler
{
    private Transform currentParent;
    [SerializeField] private Transform hatEquipPlace;
    [SerializeField] private Transform armorEquipPlace;
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null) return;

        Transform draggedItem = eventData.pointerDrag.transform; // ��������������� �������
        Transform targetCell = eventData.pointerEnter?.GetComponentInParent<InventoryCells>()?.transform;

        // ���� ���� �� ������, �������
        if (targetCell == null || !targetCell.CompareTag("Cell")) return;
        DragDrop dragdrop= draggedItem.GetComponent<DragDrop>();
        Transform draggedItemParent = dragdrop.originalParent; // �������� ���������������� ��������
        DefenceClothes defenceClothes = draggedItemParent.GetComponent<DefenceClothes>();
        // ��������, ����� �� ���������� ������� � �� �� ������
        if (draggedItemParent == targetCell)
        {
            DragDrop dragDrop = draggedItem.GetComponent<DragDrop>();
            dragDrop.ReturnToOriginalPosition();
            return;
        }
        
        // ���� � ������� ������ ���� �������, ������ �� �������
        Transform targetItem = targetCell.childCount > 0 ? targetCell.GetChild(0) : null;
        if (targetItem)
        {
            Debug.Log("<color=black>"+ dragdrop.originalParent);
            if (dragdrop.originalParent == armorEquipPlace || dragdrop.originalParent == hatEquipPlace)
            {
                DragDrop dragDrop = draggedItem.GetComponent<DragDrop>();
                dragDrop.ReturnToOriginalPosition();
                return;
            }
            else
            {
               
                bool success = �ombiningItem(draggedItem, targetItem);
                if (success)
                {
                    return;
                }
            }
            
            
        }
        
        Debug.Log("<color=red> ����������");

        if (targetItem != null)
        {
            targetItem.SetParent(draggedItemParent);
            targetItem.localPosition = Vector3.zero;
        }
        
        // ���������� ��������������� ������� � ������� ������
        draggedItem.SetParent(targetCell);
        draggedItem.localPosition = Vector3.zero;
        
        // ���������� � ����������� �������
        RectTransform rectTransform = draggedItem.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
           // CenterAndStretchItem(rectTransform);
        }

        Debug.Log($"��������� ������� {draggedItem.name} � ������ {targetCell.name}");
        if (draggedItemParent == armorEquipPlace)
        {
            defenceClothes.UpdateDef();
        }
        if (draggedItemParent == hatEquipPlace)
        {
            defenceClothes.UpdateDef();
        }



    }
    private bool �ombiningItem(Transform draggedItem, Transform targetItem) 
    {
        if (draggedItem == targetItem) return false;
        
        StackItem itemDrag = draggedItem.GetComponent<StackItem>();
        StackItem itemTarget = targetItem.GetComponent<StackItem>();
        if (itemDrag.type == ItemType.Clothes || itemTarget.type == ItemType.Clothes)
        {
            Debug.Log("<color=red> �� ����������");
            return false;
        }
        TextMeshProUGUI amountTextTarget = targetItem.transform.Find("ItemAmount").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI amountTextDrag = draggedItem.transform.Find("ItemAmount").GetComponent<TextMeshProUGUI>();
        
        if (itemDrag.type == itemTarget.type)
        {
            Debug.Log("<color=red> ����������");
            if ((itemTarget.currentAmount + itemDrag.currentAmount) <= itemTarget.maxStack)
            {
                Debug.Log(itemTarget.name + " � ����� " + itemTarget.currentAmount);
                itemTarget.currentAmount += itemDrag.currentAmount;
                Destroy(draggedItem.gameObject);
                amountTextTarget.text = itemTarget.currentAmount.ToString();
            }
            else if ((itemTarget.currentAmount + itemDrag.currentAmount)> itemTarget.maxStack)
            {
                itemDrag.currentAmount -= itemTarget.maxStack - itemTarget.currentAmount;
                itemTarget.currentAmount = itemTarget.maxStack;
                amountTextTarget.text = itemTarget.currentAmount.ToString();
                amountTextDrag.text = itemDrag.currentAmount.ToString();
                if (itemDrag.currentAmount <= 0)
                {
                    Destroy(draggedItem.gameObject); // ���������� ������, ���� �� ����
                }
                else
                {
                    DragDrop dragDrop = draggedItem.GetComponent<DragDrop>();
                    dragDrop.ReturnToOriginalPosition();
                }
            }
            return true;
        }
        else return false;
    }
    private void CenterAndStretchItem(RectTransform rectTransform)
    {
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localScale = Vector3.one;
    }
    public Transform CheckParent(Transform currentParent, Transform cardTransform)
    {
        while (currentParent != null)
        {
            if (currentParent.CompareTag("Cell"))
            {
                if (currentParent == cardTransform.parent) return null;
                // ������� �������, ���� ������� �������� ����� ��� "Cell"
                Debug.Log("RODITEl " + currentParent);
                return currentParent; // ����� �� �����, ���� ������� ����������
            }
            currentParent = currentParent.parent; // ������� � ���������� ��������
        }
        return null;
    }
}
