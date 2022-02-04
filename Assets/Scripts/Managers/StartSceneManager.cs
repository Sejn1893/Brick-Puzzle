using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSceneManager : MonoBehaviour
{
    public GameObject YesButton;
    public GameObject NoButton;
    public GameObject ExitMenu;
   

   

    
    // Start is called before the first frame update
    void Start()
    {
        ExitMenu.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        if(Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OpenExitMenu();
            }
        }

       
        
    }
    public void CloseExitMenu()
    {
        ExitMenu.SetActive(false);
    }
    public void OpenExitMenu()
    {
        ExitMenu.SetActive(true);
    }
    
   
   
    public void ExitGame()
    {
        Application.Quit(); 
    }
}
