using System.Numerics;
using UnityEngine;

// 발판을 생성하고 주기적으로 재배치하는 스크립트
public class PlatformSpawner : MonoBehaviour {
    public GameObject platformPrefab;
    public int cnt = 3; // 생성할 위 발판의 개수

    //배치 시간 간격 및 위치
    public float timeBetSpawnMin = 1.25f; 
    public float timeBetSpawnMax = 2.25f; 
    private float timeBetSpawn; 

    public float yMin = -3.5f;
    public float yMax = 1.5f;
    private float xPos = 20f;

    private GameObject[] platforms; // 미리 생성한 발판들
    private int currentIndex = 0; // 사용할 현재 순번의 발판

    private Vector2 poolPosition = new Vector2(0, -25); // 초반에 생성된 발판들을 화면 밖에 숨겨둘 위치
    private float lastSpawnTime; 


    void Start() {
        platforms = new GameObject[cnt];

        for(int i=0; i < cnt ; i++){
            platforms[i] = Instantiate(platformPrefab, poolPosition, Quaternion.Identity);
        }
        lastSpawnTime = 0f;
        timeBetSpawn = 0f;
    }

    void Update() {
        if(GameManager.instance.isGameover){
            return;
        }
        if(Time.time >= lastSpawnTime + timeBetSpawn){
            lastSpawnTime = Time.time;

            timeBetSpawn + Random.Range(timeBetspwanMin, timeBetSpawnMax);

            float yPos = Random.Range(yMin, YMax);

            platforms[currentIndex].SetActive(false);
            platforms[currentIndex].SetActive(ture);

            platforms[currentIndex].transform.positon = new Vector2(xPos, yPos);
            currentIndex++;

            if(currentIndex >= cnt){
                currentIndex = 0;
            }
        }
    }
}


/*
[📌📝MEMO] p.824


*/