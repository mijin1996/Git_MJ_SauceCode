using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour{
    public float speed = 8f;
    private Rigidbody bulletRigidbody;

void start(){
        bulletRigidbody = GetComponent<Rigidbody>();

    }
    void Start(){
        bulletRigidbody = GetComponent<Rigidbody>(); 
        bulletRigidbody.linearVelocity = transform.forward * speed;

        Destroy(gameObject, 3f); 
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "Player"){
            PlayerController playerController = other.GetComponent<PlayerController>();
        }

        if(playerController != null){
            playerController.TakeDamage(10);
        }
    }
}


/*
[📌📝MEMO]

< >
Rigidbody::⁉️ 물리 법칙(중력, 힘, 충돌 등)을 적용시켜주는 컴포넌트
inearVelocity::⁉️ 물체가 현재 이동하고 있는 속도(방향 + 빠르기)를 나타내는 프로퍼티

< >      
other :: ⁉️ 충돌한 상대방의 Collider를 의미
other.tag :: ⁉️ 충돌한 상대방의 태그를 의미


*/