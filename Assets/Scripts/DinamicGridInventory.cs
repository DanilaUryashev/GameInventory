using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // ��������� ��������� ��� � ��������� ��� ������� ����
[RequireComponent(typeof(GridLayoutGroup))]
public class DinamicGridInventory : MonoBehaviour
{
    public int columns = 6; // ���������� ��������
    public int rows = 5;    // ���������� �����
    public float spacing = 10f; // ������� ����� ��������

    private RectTransform rectTransform;
    private GridLayoutGroup gridLayout;

    void OnEnable()
    {
        // ������������� ��� ��������� �������
        Initialize();
        UpdateCellSize();
    }

    void OnValidate()
    {
        // ���������� ��� ��������� �������� � ����������
        Initialize();
        UpdateCellSize();
    }

    void Initialize()
    {
        if (rectTransform == null)
            rectTransform = GetComponent<RectTransform>();

        if (gridLayout == null)
            gridLayout = GetComponent<GridLayoutGroup>();
    }

    void UpdateCellSize()
    {
        if (rectTransform == null || gridLayout == null)
            return;

        // �������� ������� ������� ������� (����������)
        float containerWidth = rectTransform.rect.width;
        float containerHeight = rectTransform.rect.height;

        // ������������ ������� �����, �������� �������
        float cellWidth = (containerWidth - (columns - 1) * spacing) / columns;
        float cellHeight = (containerHeight - (rows - 1) * spacing) / rows;

        // ������������� ������� ����� � �������
        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
        gridLayout.spacing = new Vector2(spacing, spacing);
    }

    void Update()
    {
        // ��� ���������� � �������� ������� (� ������ ����)
        if (!Application.isPlaying)
        {
            UpdateCellSize();
        }
    }
}