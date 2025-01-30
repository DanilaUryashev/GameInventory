using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways] // Позволяет выполнять код в редакторе без запуска игры
[RequireComponent(typeof(GridLayoutGroup))]
public class DinamicGridInventory : MonoBehaviour
{
    public int columns = 6; // Количество столбцов
    public int rows = 5;    // Количество строк
    public float spacing = 10f; // Отступы между ячейками

    private RectTransform rectTransform;
    private GridLayoutGroup gridLayout;

    void OnEnable()
    {
        // Инициализация при включении объекта
        Initialize();
        UpdateCellSize();
    }

    void OnValidate()
    {
        // Обновление при изменении значений в инспекторе
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

        // Получаем текущие размеры объекта (контейнера)
        float containerWidth = rectTransform.rect.width;
        float containerHeight = rectTransform.rect.height;

        // Рассчитываем размеры ячеек, учитывая отступы
        float cellWidth = (containerWidth - (columns - 1) * spacing) / columns;
        float cellHeight = (containerHeight - (rows - 1) * spacing) / rows;

        // Устанавливаем размеры ячеек и отступы
        gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
        gridLayout.spacing = new Vector2(spacing, spacing);
    }

    void Update()
    {
        // Для обновления в реальном времени (в режиме игры)
        if (!Application.isPlaying)
        {
            UpdateCellSize();
        }
    }
}