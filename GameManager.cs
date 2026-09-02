using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour{
    public GameObject gameOverUI;
    public Text timeText;
    public Text recordText;

    private float survivetime;
    private bool isGameover;

    void Start(){
        survivetime = 0f;
        isGameover = false;
    }

    void Update(){
        if(!isGameover){
            survivetime += Time.deltaTime;
            timeText.text = "Time: " + (int)survivetime; 
        }
        else{
            if(Input.GetKeyDown(KeyCode.R)){
                SceneManager.LoadScene("SampleScene");
            }
        }
    }

    public void EndGame(){
        isGameover = true;
        gameOverTest.SetActive(true);

        float bestTime = PlayerPrefs.GetFloat("BestTime");

        if(survivetime > bestTime){
            bestTime = surviveTime; // 최고 기록값을 bestTime에 저장
            PlayerPrefs.SetFloat("BestTime", bestTime);
        }
   }

}


/*
[📌📝MEMO]

< inspector에서 index로 LoadScene하는 쉬운 소스코드 >
public int sceneIndex = 0; // 인스펙터에서 숫자로 수정 가능
void Update(){
    if (Input.GetKeyDown(KeyCode.R)){
        SceneManager.LoadScene(sceneIndex);
    }
}


*/