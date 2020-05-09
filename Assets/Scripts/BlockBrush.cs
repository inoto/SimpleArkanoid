using UnityEngine;

namespace SimpleArkanoid
{
    [CustomGridBrush(true, false, false, "Block Brush")]
    public class BlockBrush : GridBrushBase
    {
        [SerializeField] GameObject BlockPrefab;
        public override void Paint(GridLayout grid, GameObject brushTarget, Vector3Int position)
        {
            Vector2 halfCellSize = grid.cellSize / 2;
            GameObject go = Instantiate(BlockPrefab,
                (Vector2)grid.CellToWorld(position) + halfCellSize, Quaternion.identity);
            go.transform.parent = grid.transform;
        }
    }
}