
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform emptyCell;
    public int Height = 8;
    public int Width = 8;






    ParticleManager _particleManager;
    private Transform[,] _grid;
    
    public HashSet<Transform> _blocksInGrid = new HashSet<Transform>();

    private int _completedRows = 0;
    private int _completedColumns = 0;

    public int numberOfCompletedRowsAndColumns;

    public Transform[] StartPositions;

    // Start is called before the first frame update
    void Start()
    {
      
        _grid = new Transform[Height, Width];
        _particleManager = FindObjectOfType<ParticleManager>();
     

        DrawEpmtyCells();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void DrawEpmtyCells()
    {
        if (emptyCell != null)
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Transform cloneCell = Instantiate(emptyCell, new Vector3(i, j, 0.2f), Quaternion.identity);
                    cloneCell.transform.parent = transform;
                }
            }
        }
    }
    bool IsWhithinBoard(int x, int y)
    {
        return (x >= 0 && x < Width && y >= 0 && y < Height);
    }
    bool IsOccupied(int x, int y, GameObject shape)
    {
        return _grid[x, y] != null && _grid[x, y].parent != shape.transform;
    }
    public bool IsValidPosition(GameObject shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);

            if (!IsWhithinBoard((int)pos.x, (int)pos.y))
            {
                return false;
            }
            if (IsOccupied((int)pos.x, (int)pos.y, shape))
            {
                return false;
            }
        }
        return true;

    }
    public void StoreShapeInGrid(GameObject shape)
    {
        if (shape == null) return;

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            _grid[(int)pos.x, (int)pos.y] = child;

         
        }
    }
    private void StoreRowBlocks(int y)
    {
        for (int x = 0; x < Width; x++)
        {
            if (_grid[x, y] != null)
            {
                _blocksInGrid.Add(_grid[x, y]);
            }
        }
    }
    private void StoreColumnBlocks(int x)
    {
        for (int y = 0; y < Height; y++)
        {
            if (_grid[x, y] != null)
            {
                _blocksInGrid.Add(_grid[x, y]);
            }
        }
    }
    private bool IsRowFull(int y)
    {
        for (int x = 0; x < Width; ++x)
        {
            if (_grid[x, y] is null)
            {
                
                return false;
            }


        }
        
        return true;
    }
    private bool IsColumnFull(int x)
    {
        for (int y = 0; y < Height; ++y)
        {
            if (_grid[x, y] is null)
            {
               
                return false;
                
            }


        }
        
        return true;
    }
    private void ClearRow()
    {
        for (int x = 0; x < Width; x++)
        {
            if (IsRowFull(x))
            {
                
                StoreRowBlocks(x);
                _completedRows++;
                
            }
            
        }

    }
    private void ClearColumn()
    {
        for (int y = 0; y < Height; y++)
        {
            if (IsColumnFull(y))
            {
               
                StoreColumnBlocks(y);
                _completedColumns++;
                
            }
            
            
        }
    }
    
    
    public int ClearAllRowsAndColumns()
    {

        ClearColumn();
        ClearRow();

        
        

        numberOfCompletedRowsAndColumns = _completedColumns + _completedRows;

        if (_blocksInGrid.Count != 0)
        {

            foreach (var block in _blocksInGrid)
            {



                _particleManager.PlayGlowFxDestroy(block.gameObject);
                Destroy(block.gameObject, 0.6f);

                
                

                _grid[(int)Mathf.Round(block.position.x), (int)Mathf.Round(block.position.y)] = null;

            }
            _blocksInGrid = new HashSet<Transform>();
            
        }
        _completedColumns = 0;
        _completedRows = 0;

        
        return numberOfCompletedRowsAndColumns;

       
    }
   
    public bool CanShapeFitAnywhere(GameObject shape)
    {

        int shapeRotation = (int)shape.transform.eulerAngles.z;
        if (shapeRotation == 360)
        {
            shapeRotation = 0;
        }
        else if(shapeRotation == -90)
        {
            shapeRotation = 270;
        }
        else if (shapeRotation == -180)
        {
            shapeRotation = 180;
        }
        else if(shapeRotation == -270)
        {
            shapeRotation = 90;
        }

        if (shape == null) return false;

        if (shape.CompareTag("ShapeSQ"))
        {

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (_grid[x, y] == null)
                        return true;
                }
            }
            return false;
        }

        if (shape.CompareTag("ShapeI"))
        {
            if (shapeRotation == 0 || shapeRotation == 180)
            {

                for (int y = 0; y < Height - 3; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null
                            && _grid[x, y + 3] == null)
                            return true;
                    }
                }
                
            }
            else if (shapeRotation == 90 || shapeRotation == 270)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width -3; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null
                            && _grid[x + 3, y] == null)
                            return true;
                      
                    }
                }
            }
            return false;
        }
        if (shape.CompareTag("ShapeLine2"))
        {
            if (shapeRotation == 0 || shapeRotation == 180)
            {
                for (int y = 0; y < Height -1; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 90 || shapeRotation == 270)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width -1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null)
                            return true;
                    }
                }
            }
            return false;
        }
        if (shape.CompareTag("ShapeLine3"))
        {
            if (shapeRotation == 0 || shapeRotation == 180)
            {
                for (int y = 0; y < Height -2; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 90 || shapeRotation == 270)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width -2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null)
                            return true;
                    }
                }
            }
            return false;
        }
        if (shape.CompareTag("ShapeI5"))
        {
            if (shapeRotation == 0 || shapeRotation == 180)
            {

                for (int y = 0; y < Height - 4; y++)
                {
                    for (int x = 0; x < Width; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null
                            && _grid[x, y + 3] == null && _grid[x, y + 4] == null)
                            return true;
                    }
                }

            }
            else if (shapeRotation == 90 || shapeRotation == 270)
            {
                for (int y = 0; y < Height; y++)
                {
                    for (int x = 0; x < Width - 4; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null
                            && _grid[x + 3, y] == null && _grid[x + 4, y] == null)
                            return true;

                    }
                }
            }
            return false;
        }


        if (shape.CompareTag("ShapeJ"))
        {
            if (shapeRotation == 0)
            {

                for (int y = 0; y < Height - 1; y++)
                {
                    for (int x = 0; x < Width - 2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x + 1, y] == null
                            && _grid[x + 2, y] == null)
                            return true;
                    }
                }
                
                
            }
            else if (shapeRotation == 90)
            {
                for (int y = 0; y < Height -2; y++)
                {
                    for (int x = 0; x < Width -1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 1, y + 1] == null
                            && _grid[x + 1, y + 2] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 180)
            {
                for (int y = 1; y < Height; y++)
                {
                    for (int x = 0; x < Width -2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null
                            && _grid[x + 2, y - 1] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 270)
            {
                for (int y = 0; y < Height - 2; y++)
                {
                    for (int x = 0; x < Width - 1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null
                            && _grid[x + 1, y + 2] == null)
                            return true;
                    }
                }
            }
            return false;
        }
        
        if (shape.CompareTag("ShapeL"))
        {
            if (shapeRotation == 0)
            {

                for (int y = 0; y < Height - 2; y++)
                {
                    for (int x = 0; x < Width - 1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null
                            && _grid[x + 1, y] == null)
                            return true;
                    }

                }

            }
            else if (shapeRotation == 90)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    for (int x = 0; x < Width - 2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null
                            && _grid[x + 2, y + 1] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 180)
            {
                for (int y = 2; y < Height; y++)
                {
                    for (int x = 0; x < Width - 1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 1, y - 1] == null
                            && _grid[x + 1, y - 2] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 270)
            {
                for (int y = 0; y < Height -1; y++)
                {
                    for (int x = 0; x < Width -2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x + 1, y + 1] == null
                           && _grid[x + 2, y + 1] == null)
                            return true;
                    }
                }
            }
            return false;
        }
        if (shape.CompareTag("ShapeL2x2"))
        {
            if (shapeRotation == 0)
            {
                for (int y = 0; y < Height -1; y++)
                {
                    for (int x = 0; x < Width -1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x + 1, y] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 90)
            {
                for (int y = 0; y < Height -1; y++)
                {
                    for (int x = 0; x < Width -1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 1, y + 1] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 180)
            {
                for (int y = 1; y < Height; y++)
                {
                    for (int x = 1; x < Width; x++)
                    {
                        if (_grid[x, y] == null && _grid[x - 1, y] == null && _grid[x, y - 1] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 270)
            {
                for (int y = 0; y < Height -1; y++)
                {
                    for (int x = 0; x < Width -1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x + 1, y + 1] == null)
                            return true;
                    }
                }
            }
            return false;
        }

        if (shape.CompareTag("ShapeO"))
        {
            for (int y = 0; y < Height - 1; y++)
            {
                for (int x = 0; x < Width - 1; x++)
                {
                    if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x, y + 1] == null &&
                        _grid[x + 1, y + 1] == null)
                        return true;
                }
            }
            return false;
        }
        if (shape.CompareTag("ShapeO3x3"))
        {
            for (int y = 0; y < Height -2; y++)
            {
                for (int x = 0; x < Width -2; x++)
                {
                    if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null
                        && _grid[x + 1, y] == null && _grid[x + 1, y + 1] == null && _grid[x + 1, y + 2] == null
                        && _grid[x + 2, y] == null && _grid[x + 2, y + 1] == null && _grid[x + 2, y + 2] == null)
                        return true;
                }
            }
            return false;
        }
        if (shape.CompareTag("ShapeS"))
        {
            if (shapeRotation == 0 || shapeRotation == 180)
            {

                for (int y = 0; y < Height - 1; y++)
                {
                    for (int x = 0; x < Width - 2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 1, y + 1] == null
                             && _grid[x + 2, y + 1] == null)
                            return true;
                    }

                }

            }
            else if (shapeRotation == 90 || shapeRotation == 270)
            {
                for (int y = 2; y < Height; y++)
                {
                    for (int x = 0; x < Width -1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y - 1] == null && _grid[x + 1, y - 1] == null
                            && _grid[x + 1, y - 2] == null)
                            return true;
                    }
                }
            }
            return false;
            
            
            
        }
        
        if (shape.CompareTag("ShapeZ"))
        {
            if (shapeRotation == 0 || shapeRotation == 180)
            {

                for (int y = 0; y < Height - 1; y++)
                {
                    for (int x = 0; x < Width - 2; x++)
                    {
                        if (_grid[x, y + 1] == null && _grid[x + 1, y + 1] == null
                            && _grid[x + 1, y] == null && _grid[x + 2, y] == null)
                            return true;


                    }
                }
            }
            else if (shapeRotation == 90 || shapeRotation == 270)
            {
                for (int y = 0; y < Height - 2; y++)
                {
                    for (int x = 0; x < Width - 1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x + 1, y + 1] == null
                            && _grid[x + 1, y + 2] == null)
                            return true;
                    }
                }
            }
            return false;
        }
        if (shape.CompareTag("ShapeT"))
        {
            if (shapeRotation == 0)
            {
                for (int y = 0; y < Height - 1; y++)
                {
                    for (int x = 0; x < Width - 2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null
                            && _grid[x + 1, y + 1] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 90)
            {
                for (int y = 0; y < Height -2; y++)
                {
                    for (int x = 1; x < Width; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null
                            && _grid[x - 1, y + 1] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 180)
            {
                for (int y = 1; y < Height; y++)
                {
                    for (int x = 0; x < Width - 2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x + 1, y] == null && _grid[x + 2, y] == null
                            && _grid[x + 1, y - 1] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 270)
            {
                for (int y = 0; y < Height - 2; y++)
                {
                    for (int x = 0; x < Width - 1; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null
                            && _grid[x + 1, y + 1] == null)
                            return true;
                    }
                }
            }
            return false;
            
            
            
        }
        if (shape.CompareTag("ShapeL3x3"))
        {
            if (shapeRotation == 0)
            {
                for (int y = 0; y < Height -2; y++)
                {
                    for (int x = 0; x < Width -2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null
                            && _grid[x + 1, y] == null && _grid[x + 2, y] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 90)
            {
                for (int y = 0; y < Height -2; y++)
                {
                    for (int x = 0; x < Width -2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x+1, y] == null && _grid[x +2, y] == null
                            && _grid[x + 2, y + 1] == null && _grid[x + 2, y + 2] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 180)
            {
                for (int y = 2; y < Height; y++)
                {
                    for (int x = 2; x < Width; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y - 1] == null && _grid[x, y - 2] == null
                            && _grid[x - 1, y] == null && _grid[x - 2, y] == null)
                            return true;
                    }
                }
            }
            else if (shapeRotation == 270)
            {
                for (int y = 0; y < Height -2; y++)
                {
                    for (int x = 0; x < Width -2; x++)
                    {
                        if (_grid[x, y] == null && _grid[x, y + 1] == null && _grid[x, y + 2] == null
                            && _grid[x + 1, y + 2] == null && _grid[x + 2, y + 2] == null)
                            return true;
                    }
                }
            }
            return false;
        }
        return false;

        
        
    }
   
}
