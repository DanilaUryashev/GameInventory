using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Timeline.Actions.MenuPriority;

public class ViewAddItem : MonoBehaviour
{
    [SerializeField] Image ItemView;
    private GameObject addItem;
    public void ViewItem(GameObject item)
    {
        addItem = Instantiate(item);
        addItem.transform.SetParent(ItemView.transform);
        CenterAndStretchItem(addItem);
    }
    private void CenterAndStretchItem(GameObject item)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        rectTransform.localPosition = Vector3.zero;
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.one;
        rectTransform.offsetMin = Vector2.zero;
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.localScale = Vector3.one;
    }
    public void CloseView()
    {
        Destroy(addItem);
        gameObject.SetActive(false);
    }
}
