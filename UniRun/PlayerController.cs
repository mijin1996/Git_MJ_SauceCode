using UnityEngine;

public class PlayerController : MonoBehaviour{
    public AuidoClip DeathClip;
    public float jumpForce = 700f;

    private int jumpCnt = 0;
    private bool isGrounded = false;
    private bool isDead = false;

    private Rigidbody2D playerRb;
    private Animator ani;
    private AudioSource playerAudio;

    private void Start(){
        playerRb = GetComponent<Rigidbody2D>();
        ani = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
    }

    private void Update(){
        if(isDead) return;

        // 점프 로직
        if(Input.GetButtonDown("Jump") && jumpCnt < 2){
            jumpCnt++;
            playerRb.linearVelocity = Vector2.zero;
            playerRb.AddForce(new Vector2(0f, jumpForce));
            playerAudio.Play();
        }
        else if(Input.GetButtonUp(0) && playerRb.linearVelocity.y > 0){
            // 점프 버튼을 떼었을 때, 상승 중이면 속도를 절반으로 줄임
            playerRb.linearVelocity = playerRb.linearVelocity * 0.5f;
        }
        ani.SetBool("Grounded", isGrounded);
    }

    private void Die(){
        ani.SetTrigger("Die");
        playerAudio.clip = DeathClip;
        playerAudio.Play();
        playerRb.linearVelocity = Vector2.zero;
        isDead = true;
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.tag == "Dead" && !isDead){
            Die();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if(collision.contacts[0].normal.y > 0.7f){
            isGrounded = true;
            jumpCnt = 0;
        }
    }

    private void OnCollisionExit2D(Collision2D collision){
        isGrounded = false;
    }
}


/*
[📌📝MEMO] p.538

linearVelocity::⁉️ 물체가 현재 이동하고 있는 속도(방향 + 빠르기)를 나타내는 프로퍼티

OnCollisionEnter2D::⁉️  물리적 충돌(부딪힘)
OnTriggerEnter2D::⁉️ 그냥 통과(감지만 함)

other ::⁉️ 누군가
other.tag ::⁉️ 특정 누군가


*/