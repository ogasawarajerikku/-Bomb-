using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDirector : MonoBehaviour
{
    [SerializeField, Tooltip("スコア表示用のテキストオブジェクト")]
    GameObject scoreTextObject = null;
    //スコア
    int scoreValue = 0;
    //表示用テキスト
    Text scoreText = null;
    void Start()
    {
        scoreText = scoreTextObject.GetComponent<Text>();
    }

    void Update()
    {
        scoreText.text = "" + scoreValue;
    }
    public void AddScore(int newScore)
    {
        scoreValue += newScore;
    }
}
