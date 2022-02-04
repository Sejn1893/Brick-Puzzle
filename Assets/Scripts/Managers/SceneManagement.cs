using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManagement : MonoBehaviour
{
    


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");


    }
    public void RestartGame()
    {


        SceneManager.LoadScene("GameScene");
        
        

    }
    public void MenuScene()
    {

        SceneManager.LoadScene(0);
        
        

    }
    public void ExitGame()
    {
        Application.Quit();
    }
    
   

}
