using TriInspector;
using UnityEngine;


[ExecuteAlways]
public class GridLayout2D : MonoBehaviour
{
    private enum SortDirection
    {
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    private enum StartAxis
    {
        Horizontal,
        Vertical
    }
    
    [Title("Parameter")]
    [SerializeField] private StartAxis _sortAxis;
    [SerializeField] private SortDirection _sortDirection;
    [SerializeField, Min(1)] private int _columns = 1;
    [SerializeField] private Vector2 _spacing;


    void OnValidate()
    {
        SortGrid();
    }

    private void SortGrid()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            int row, col;
            
            
            if (_sortAxis == StartAxis.Horizontal)
            {
                row = i / _columns;
                col = i % _columns;
            }
            else
            {
                col = i / _columns;
                row = i % _columns;
            }

            float xMultiplier = 1;
            float yMultiplier = 1;
            
            Vector2 mult = Vector2.one;
            
            switch (_sortDirection)
            {
                case SortDirection.TopRight:
                    mult = new(1, 1);
                    break;
                
               case SortDirection.BottomRight:
                    mult = new Vector2(1, -1);
                    break;
                case SortDirection.TopLeft:
                    mult = new Vector2(-1, 1);
                    break;
                
                case SortDirection.BottomLeft:
                    mult = new Vector2(-1, -1);
                    break;
            }

            Vector2 position = new Vector2(col * _spacing.x, row * _spacing.y) * mult;
            
            transform.GetChild(i).localPosition = position;
        }
    }
}
