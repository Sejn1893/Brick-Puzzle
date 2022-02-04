using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
   

   
    public GameObject PauseMenuPanel;
   
    public GameObject GameOverPanel;
   
    Spawner _spawner;
    // Start is called before the first frame update
    void Start()
    {
        _spawner = FindObjectOfType<Spawner>();
       

        PauseMenuPanel.SetActive(false);
        
        GameOverPanel.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OpenPauseMenu()
    {
        PauseMenuPanel.SetActive(true);

        

        foreach (GameObject item in _spawner.SpawnedShapes)
        {
            item.GetComponent<Collider>().enabled = false;
        }
        

    }
    public void ClosePauseMenu()
    {

        PauseMenuPanel.SetActive(false);

       

        foreach (GameObject item in _spawner.SpawnedShapes)
        {
            item.GetComponent<Collider>().enabled = true;
        }
    }
   
    
    public GameObject GetGameOverPanel()
    {
        return GameOverPanel;
    }
}
