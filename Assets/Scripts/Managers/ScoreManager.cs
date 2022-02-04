using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{

  
    
    CameraShake _cameraShake;

    public int Score = 0;
    public int PlacedShapeScore;

    public TMP_Text ScoreText;
    public TMP_Text HighScore;

    
    [SerializeField]
    private int IsComboAvailable = 0;
    private int _comboPoints = 50;

    // Start is called before the first frame update
    void Start()
    {
        
        _cameraShake = Camera.main.GetComponent<CameraShake>();


        HighScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        ScoreText.text = GetScore().ToString();
    }
    public void ScoreCounter(int score)
    {
        switch (score)
        {
            case 1:
                Score += 10;
                IsComboAvailable++;
               
                break;
            case 2:
                Score += 30;
              
                
                break;
            case 3:
                Score += 50;
               
                _cameraShake.PlayShake();
                break;
            case 4:
                Score += 70;
               
                _cameraShake.PlayShake();
                break;
            case 5:
                Score += 100;
              
                _cameraShake.PlayShake();
                break;
            default:
                Score += 0;
                IsComboAvailable = 0;
                break;

        }

        AddToScore(score - score);
       
    }
    public void ShapeScore(GameObject shape)
    {

        PlacedShapeScore = shape.transform.childCount;

        Score += PlacedShapeScore;


    }
    public int GetScore()
    {
        if (Score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", Score);
            HighScore.text = Score.ToString();
        }

        return Score;

    }
    private void AddToScore(int scoreValue)
    {
        Score += scoreValue;

        if (IsComboAvailable > 1)
        {
            Score += _comboPoints;
        }
        
    }

    
  
}
