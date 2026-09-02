using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpwaner : MonoBehaviour{
    public GameObject bulletPrefab;
    public float spwanRateMin = 0.5f;
    public float spwanRateMax = 3f;

    private Transform target;
    private float spwanRate;
    private float timeAfterSpwan; //최근 생성 시점에서 경과한 시간

    void Start(){
        timeAfterSpwan = 0f;
        spwanRate = Random.Range(spwanRateMin, spwanRateMax);
        target = FindFirstObjectByType<PlayerController>().transform;
    }

    void Update(){
        timeAfterSpwan += Time.deltaTime;

        if(timeAfterSpwan >= spwanRate){
            timeAfterSpwan = 0f; //누적 시간 초기화

            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
            bullet.transform.LookAt(target); 
            spwanRate = Random.Range(spwanRateMin, spwanRateMax); // 재생성 주기 랜덤화
            
        }
    }
}
 

 /*
[📌📝MEMO]

< Quaternion과 짝인 아이들 >
LookAt::⁉️ 갑자기 바뀜
LookRotation::⁉️ 방향을 바라보기 위해 서서히 회전 = 자연스러움

< >
Time.deltaTime::⁉️ 이전 프레임에서 현재 프레임까지 걸린 perSec = 서로 다른 성능의 컴퓨터나 핸드폰에서 동일한 속도를 유지하기 위함

< >
<Type(컴퍼넌트)>


*/