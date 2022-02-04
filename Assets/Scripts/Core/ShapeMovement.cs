using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeMovement : MonoBehaviour
{
    private GameObject _activeShape;
   // public List<GameObject> _placedShapes = new List<GameObject>();

    Camera _cam;
    Board _board;
    Spawner _spawner;
    ScoreManager _scoreManager;
    GhostShape _ghostShape;
    ParticleManager _particleManager;
    UIManager _uiManager;
    

    private readonly Color _unfitColor = new Color(0.6f, 0.6f, 0.6f, 1);
    
    private int _index; // spawned shape index in list
    public float offset = 5f; // camera offset
    private bool _isShapePicked = true;
    public float Speed = 2; //Shape movement lerp speed

    public Color32 _initialColor;
    public float ColorLerpSpeed = 1f;
    
    
    private int _numberOfShapesInPlay;
    // Start is called before the first frame update
    void Start()
    {
        _board = FindObjectOfType<Board>();
        _cam = Camera.main;
        _scoreManager = FindObjectOfType<ScoreManager>();
        _spawner = FindObjectOfType<Spawner>();
        _ghostShape = FindObjectOfType<GhostShape>();
        _particleManager = FindObjectOfType<ParticleManager>();
        _uiManager = FindObjectOfType<UIManager>();
        
        _numberOfShapesInPlay = _spawner.SpawnedShapes.Count;
        
        

    }

#if UNITY_ANDROID   // Update is called once per frame
    void Update()
    {
        //TouchControls();
        MouseControls();
        
    }
    void LateUpdate()
    {
        GhostShapeControl();

    }

    private void GhostShapeControl()
    {
        if (_activeShape && _board.IsValidPosition(_activeShape))
        {
            _ghostShape.GhostCreator(_activeShape, _board);
        }
        else if (_activeShape && !_board.IsValidPosition(_activeShape))
        {
            _ghostShape.GhostShapeReset();
        }
    }

    private void MouseControls()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, offset));
            RaycastHit hit;

            if (Physics.Raycast(_cam.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, out hit))
            {

                if ( hit.collider.gameObject == null) return;
                if (hit.collider.gameObject.GetComponent<Shape>() != null && _isShapePicked)
                {

                    _activeShape = hit.collider.gameObject;

                    //Return shapes scale to 1
                    _activeShape.transform.localScale = new Vector3(1, 1, 1);

                   
                    _isShapePicked = false;

                }

            }

            if (_activeShape == null) return;
            _activeShape.transform.position = Vector3.Lerp(_activeShape.transform.position, new Vector3(mousePos.x, mousePos.y + 3f, -0.1f), Speed * Time.deltaTime);
          //  _activeShape.transform.position = new Vector3(mousePos.x, mousePos.y + 3f, -0.1f);

            
          
  
            // Get spawned shape index for GameOver check
            foreach (var item in _spawner.SpawnedShapes)
            {
                if(item != null)
                {
                    _index = _spawner.SpawnedShapes.IndexOf(_activeShape);
                    
                }
         
            }

            //_activeShape.transform.position = Vector3.Lerp(_activeShape.transform.position, mousePos, Speed * Time.deltaTime);
        }

        if (Input.GetMouseButtonUp(0) && _activeShape != null)
        {
            _ghostShape.GhostShapeReset();
            _isShapePicked = true;

           // _ghostShape.GhostShapeReset();
  
            if(_board.IsValidPosition(_activeShape))
            {

                
                _activeShape.GetComponent<Collider>().enabled = false;
                _activeShape.transform.position = Vectorf.Round(_activeShape.transform.position);

                _particleManager.PlayGlowFx(_activeShape);
                _board.StoreShapeInGrid(_activeShape);

                _scoreManager.ShapeScore(_activeShape);
                
                
                //Destroy shapes parent
                DestroyParentObj(_activeShape);
                

                _scoreManager.ScoreCounter(_board.ClearAllRowsAndColumns());

               
                //Remove placed shape from active shape list
                _spawner.SpawnedShapes.RemoveAt(_index);
                _numberOfShapesInPlay--;
                
                RespawnShapes();
                
                GameOver();
         

            }
            else
            {
                _activeShape.transform.position = _activeShape.transform.parent.position;
                _activeShape.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                _activeShape = null;
               
            }
            

        }

    }

    // ---------------------------------------TOUCH CONTROLS--------------------------------------------- //
    private void TouchControls()
    {
        Touch touch = Input.GetTouch(0);


        if (touch.phase == TouchPhase.Began)
        {
            
            RaycastHit hit;

            if (Physics.Raycast(_cam.ScreenToWorldPoint(Input.mousePosition), Vector3.forward, out hit))
            {

                if (hit.collider.gameObject == null) return;
                if (hit.collider.gameObject.GetComponent<Shape>() != null && _isShapePicked)
                {

                    _activeShape = hit.collider.gameObject;

                    //Return shapes scale to 1
                    _activeShape.transform.localScale = new Vector3(1, 1, 1);

                    

                    _isShapePicked = false;

                }

            }

            // Get spawned shape index for GameOver check
            foreach (var item in _spawner.SpawnedShapes)
            {
                if (item != null)
                {
                    _index = _spawner.SpawnedShapes.IndexOf(_activeShape);

                }

            }
        }
        else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
        {

            Vector3 touchPos = _cam.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y + 3f, offset));

            if (_activeShape == null) return;
            _activeShape.transform.position = Vector3.Lerp(_activeShape.transform.position, new Vector3(touchPos.x, touchPos.y + 3.8f, -0.1f), Speed * Time.deltaTime);
            //_activeShape.transform.position = new Vector3(touchPos.x, touchPos.y + 2.8f, -0.1f);

        }
        else if (touch.phase == TouchPhase.Ended)
        {
            _ghostShape.GhostShapeReset();
            _isShapePicked = true;

            // _ghostShape.GhostShapeReset();

            if (_board.IsValidPosition(_activeShape))
            {


                _activeShape.GetComponent<Collider>().enabled = false;
                _activeShape.transform.position = Vectorf.Round(_activeShape.transform.position);

               

                _board.StoreShapeInGrid(_activeShape);
                _particleManager.PlayGlowFx(_activeShape);

                _scoreManager.ShapeScore(_activeShape);

                //Destroy shapes parent
                DestroyParentObj(_activeShape);

                _scoreManager.ScoreCounter(_board.ClearAllRowsAndColumns());


                //Remove placed shape from active shape list
                _spawner.SpawnedShapes.RemoveAt(_index);
                

                _numberOfShapesInPlay--;

                RespawnShapes();

                GameOver();

            }
            else
            {
                _activeShape.transform.position = _activeShape.transform.parent.position;
                _activeShape.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                _activeShape = null;
            }

        }
    }

    

    private void RespawnShapes()
    {
        if (_spawner.SpawnedShapes.Count <=0)
        {
            _spawner.SpawnShape();
            _numberOfShapesInPlay = _spawner.SpawnedShapes.Count;
        }
        
    }
    public void GameOver()
    {
        int numberOfUnfitShapes = 0;
 
        foreach (GameObject shape in _spawner.SpawnedShapes)
        {
            SpriteRenderer[] blocksColor = shape.GetComponentsInChildren<SpriteRenderer>();
            

            //Debug.Log(shape + "  " + _board.CanShapeFitAnywhere(shape));

            if (shape != null)
            {
               // _initialColor = shape.GetComponent<SpriteRenderer>().color;


                if (_board.CanShapeFitAnywhere(shape) == false)
                {

                    foreach (var item in blocksColor)
                    {
                        item.color = _unfitColor;

                    }
                    GetShapeCollider(shape).enabled = false;
                   
                    numberOfUnfitShapes++;
                 
                    if (_numberOfShapesInPlay > 0)
                    {
                        
                        if (_numberOfShapesInPlay == numberOfUnfitShapes)
                        {

                            
                           

                            _uiManager.GetGameOverPanel().SetActive(true);
                           
                        }
                    }
                }
                else
                {
                    foreach (var item in blocksColor)
                    {
                        item.color = _initialColor;
                    }
                    if (_initialColor == _unfitColor)
                    {
                        _initialColor = new Color(1, 1, 1, 1);
                    }

                    GetShapeCollider(shape).enabled = true;
                    
                }
                
            }
            
        }

    }
    private Collider GetShapeCollider(GameObject shape)
    {
        return shape.GetComponent<Collider>();
    }
    private void DestroyParentObj(GameObject shape)
    {
        if (shape.transform.childCount > 0)
        {
            shape.transform.DetachChildren();

        }
        Destroy(shape);
    }
    public Transform GetRandomBlockInActiveShape()
    {

        

        if (_activeShape == null)
        {
            return null;
        }
        else
        {
            return _activeShape.transform;
        }
            
    }
    



#endif   

}
