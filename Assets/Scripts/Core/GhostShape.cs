using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostShape : MonoBehaviour
{

    private GameObject _ghostShape = null;
    public Color GhostColor = new Color(1f, 1f, 1f, 0.2f);

    public Sprite GhostSprite;
    
   // private bool _isGhostAvailable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GhostCreator(GameObject activeShape, Board board)
    {
        if(!_ghostShape)
        {

            _ghostShape = Instantiate(activeShape, activeShape.transform.position, activeShape.transform.rotation);
            _ghostShape.GetComponent<Collider>().enabled = false;
      
            //Ghost shape color change
            SpriteRenderer[] allRenderers = _ghostShape.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer item in allRenderers)
            {
                item.sprite = GhostSprite;
                item.color = GhostColor;
            }

        }

        // Ghost shape updated position
        if(board.IsValidPosition(activeShape))
        {
            _ghostShape.transform.position = Vectorf.Round(activeShape.transform.position);
            
        }
       
    }
    public void GhostShapeReset()
    {
        if(_ghostShape)
            Destroy(_ghostShape.gameObject);

    }
}
