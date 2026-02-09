using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class UI_ResponsiveGrid : MonoBehaviour
{
    [Header("Option")]
    public int columnCount = 3;

    public float cellAspectRatio = 1.0f;

    private GridLayoutGroup _grid;
    private RectTransform _rectTransform;

    private void Awake()
    {
        _grid = GetComponent<GridLayoutGroup>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        UpdatCellSize();
    }

    public void UpdatCellSize()
    {
        if (_grid == null || _rectTransform == null) return;

        float totalWidth = _rectTransform.rect.width;

        float paddingHorizontal = _grid.padding.left + _grid.padding.right;

        float spacingTotla = _grid.spacing.x * (columnCount - 1);

        float availableWidth = totalWidth - paddingHorizontal - spacingTotla;

        float cellWidth = availableWidth / columnCount;

        float cellHeight = cellWidth / cellAspectRatio;

        _grid.cellSize = new Vector2(cellWidth, cellHeight);
    }
}