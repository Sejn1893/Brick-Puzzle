using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    Board _board;
    ScoreManager _scoreManager;
   
    public Shape[] Shapes;
    public List<GameObject> SpawnedShapes;
    private Vector3 _blockScale = new Vector3(0.4f, 0.4f, 0.4f);

    public GameObject firstPos;
    public GameObject secondPos;
    public GameObject thirdPos;

   // public List<Sprite> ShapeSprites;
    
    public int[] _randomRotation;
    // Start is called before the first frame update
    void Start()
    {

        _board = FindObjectOfType<Board>();
        _scoreManager = FindObjectOfType<ScoreManager>();
        

        SpawnShape();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnShape()
    {

        GameObject firstShape = Instantiate(GetRandomBasicShape());
        // GetRandomBlock(firstShape);

        // firstShape.transform.rotation = Quaternion.Euler(0, 0, GetRandomRotation(firstShape));

        firstShape.transform.eulerAngles = new Vector3(0, 0, GetRandomRotation(firstShape));
       // ReverseBlockRotation(firstShape);

        firstShape.transform.position = firstPos.transform.position;
        firstShape.transform.localScale = _blockScale;
        
       

        GetStartingPosition(firstShape);
        
        SpawnedShapes.Add(firstShape);
        

        GameObject secondShape = Instantiate(GetRandomBasicShape(), gameObject.transform.position, Quaternion.identity);
        // GetRandomBlock(secondShape);

        //secondShape.transform.localRotation = Quaternion.Euler(0, 0, GetRandomRotation(secondShape));

        secondShape.transform.eulerAngles = new Vector3(0, 0, GetRandomRotation(secondShape));
       // ReverseBlockRotation(secondShape);

        secondShape.transform.position = secondPos.transform.position;
        secondShape.transform.localScale = _blockScale;
        

        GetStartingPosition(secondShape);
        
        SpawnedShapes.Add(secondShape);
        

        GameObject thirdShape = Instantiate(GetRandomShape(), gameObject.transform.position, Quaternion.identity);

        // GetRandomBlock(thirdShape);
        //thirdShape.transform.rotation = Quaternion.Euler(0, 0, GetRandomRotation(thirdShape));

        thirdShape.transform.eulerAngles = new Vector3(0, 0, GetRandomRotation(thirdShape));
        //ReverseBlockRotation(thirdShape);

        thirdShape.transform.position = thirdPos.transform.position;
        thirdShape.transform.localScale = _blockScale;
        
       

        GetStartingPosition(thirdShape);
        

        SpawnedShapes.Add(thirdShape);

       /* Debug.Log(firstShape.transform.eulerAngles);
        Debug.Log(secondShape.transform.eulerAngles);
        Debug.Log(thirdShape.transform.eulerAngles);*/


        // Debug.Log(SpawnedShapes.Count);
    }
    private int GetRandomRotation(GameObject shape)
    {
        int AllRotations = Random.Range(0, _randomRotation.Length);
        int BasicRotations = Random.Range(0, 2);
        int IntermediateRotations = Random.Range(0, 3);

        if (shape.CompareTag("ShapeT") || shape.CompareTag("ShapeS") || shape.CompareTag("ShapeZ")
            || shape.CompareTag("ShapeL") || shape.CompareTag("ShapeJ"))
        {
            if (_scoreManager.Score <= 1500)
            {
                return _randomRotation[0];
            }
            else if (_scoreManager.Score > 1500)
            {
                return _randomRotation[BasicRotations];
            }
            else if (_scoreManager.Score > 2800)
            {
                return _randomRotation[IntermediateRotations];
            }
            else
            {
                return _randomRotation[AllRotations];
            }

        }
        else if (shape.CompareTag("ShapeO") || shape.CompareTag("ShapeSQ") || shape.CompareTag("ShapeO3x3"))
        {
            return _randomRotation[0];
        }
        else
        {
            return _randomRotation[AllRotations];
        }


    }
    private GameObject GetRandomShape()
    {
        int randomAllShapes = Random.Range(0, Shapes.Length);

        if (Shapes[randomAllShapes])
        {
            return Shapes[randomAllShapes].gameObject;
        }
        else
        {
            return null;
        }
    }
    GameObject GetRandomBasicShape()
    {
        int randomBasicShapes = Random.Range(0, Shapes.Length - 5);

        if (_scoreManager.Score < 1200)
        {
            return Shapes[randomBasicShapes].gameObject;
        }
        else
        {
            return GetRandomShape();

        }

    }
    private void GetStartingPosition(GameObject shape)
    {
        for (int i = 0; i < _board.StartPositions.Length; i++)
        {
            if (Vector3.Distance(shape.transform.position, _board.StartPositions[i].position) < 1f)
            {
                shape.transform.parent = _board.StartPositions[i];
            }
        }
    }
   /* public void GetRandomBlock(GameObject shape)
    {
      
        int randomSprite = Random.Range(0, ShapeSprites.Count);

        SpriteRenderer[] blocksInShape = shape.GetComponentsInChildren<SpriteRenderer>();
        //blocksInShape[0] = null;
       

        foreach (var item in blocksInShape)
        {
            if (item.GetComponent<SpriteRenderer>() == null) return;
            item.sprite = ShapeSprites[randomSprite];
            
        }

    }*/
    /*private void ReverseBlockRotation(GameObject shape)
    {
        List<Transform> children = new List<Transform>(shape.GetComponentsInChildren<Transform>());

        foreach (var item in children)
        {
            if (shape.transform.eulerAngles.z == 90)
            {
                item.transform.localEulerAngles = new Vector3(0, 0, 270);
            }
            else if (shape.transform.eulerAngles.z == 180)
            {
                item.transform.localEulerAngles = new Vector3(0, 0, 180);
            }
            else if (shape.transform.eulerAngles.z == 270)
            {
                item.transform.localEulerAngles = new Vector3(0, 0, 90);
            }
            

        }

    }*/
    
    
    
    
   
}
