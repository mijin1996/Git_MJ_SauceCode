using System.Collections;
using UnityEngine;

// 총을 구현
public class Gun : MonoBehaviour {
    // 총의 상태를 표현하는 데 사용할 타입을 선언
    public enum State {
        Ready, // 발사 준비됨
        Empty, // 탄알집이 빔
        Reloading // 재장전 중
    }

    public State state { get; private set; } // 현재 총의 상태

    public Transform fireTransform; // 탄알이 발사될 위치

    public ParticleSystem muzzleFlashEffect; // 총구 화염 효과
    public ParticleSystem shellEjectEffect; // 탄피 배출 효과

    private LineRenderer bulletLineRenderer; // 탄알 궤적을 그리기 위한 렌더러

    private AudioSource gunAudioPlayer; // 총 소리 재생기

    public GunData gunData; // 총의 현재 데이터

    private float fireDistance = 50f; // 사정거리

    public int ammoRemain = 100; // 남은 전체 탄알
    public int magAmmo; // 현재 탄알집에 남아 있는 탄알

    private float lastFireTime; // 총을 마지막으로 발사한 시점

    private void Awake() {
        // 사용할 컴포넌트의 참조 가져오기
        gunAudioPlayer = GetComponent<AudioSource>();
        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2; // 라인렌더러는 두 점을 연결하는 선이므로 점을 2개로 설정
        bulletLineRenderer.enabled = false; // 처음에는 비활성화
    }

    private void OnEnable() {
        // 총 상태 초기화
        aammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;

        state = State.Ready; // 총의 현재 상태를 발사 준비된 상태로 변경
        lastFireTime = 0; // 마지막 발사 시점 초기화
    }

    // 발사 시도
    public void Fire() {
        if(state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire) {
            lastFireTime = Time.time; // 마지막 발사 시점 갱신
            Shot(); // 실제 발사 처리
        }

    }

    // 실제 발사 처리
    private void Shot() {
        RaycastHit hit; // 레이캐스트 충돌 정보를 저장할 변수
        Vector3 hitPosition = Vector3.zero; // 충돌 위치
        
        if(Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance)) {
            // 레이캐스트가 충돌한 경우
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            // 충돌한 대상이 IDamageable 인터페이스를 구현하고 있다면 데미지 적용
            if(target != null) {
                target.OnDamage(gunData.damage, hit.point, hit.nrmal); 
            }

            // 충돌 위치를 레이캐스트 충돌 지점으로 설정
            hitPosition = hit.point;
            
        } else {
            // 레이캐스트가 충돌하지 않은 경우
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        StartCoroutine(ShotEffect(hitPosition)); // 발사 이펙트 재생 코루틴 실행

        magAmmo--; // 현재 탄알집에 남아 있는 탄알 수 감소
        if(magAmmo <= 0) {
            state = State.Empty; // 탄창이 빔 상태로 전환
        }
      
    }

    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition) {
        muzzleFlashEffect.Play(); // 총구 화염 효과 재생
        shellEjectEffect.Play(); // 탄피 배출 효과 재생

        gunAudioPlayer.PlayOneShot(gunData.shotClip); // 발사 소리 재생

        bulletLineRenderer.SetPosition(0, fireTransform.position); // 라인 렌더러의 시작점은 총구 위치
        bulletLineRenderer.SetPosition(1, hitPosition); // 
        // 라인 렌더러를 활성화하여 탄알 궤적을 그림
        bulletLineRenderer.enabled = true;

        // 0.03초 동안 잠시 처리를 대기
        yield return new WaitForSeconds(0.03f);

        // 라인 렌더러를 비활성화하여 탄알 궤적을 지움
        bulletLineRenderer.enabled = false;
    }

    // 재장전 시도
    public bool Reload() {
        if(state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            // 이미 재장전 중이거나 남은 전체 탄알이 없거나 탄창이 이미 가득 찬 경우 재장전 불가
            return false;
        }
        
        StartCoroutine(ShotEffect(hitPosition));
        return true;


    }

    // 실제 재장전 처리를 진행
    private IEnumerator ReloadRoutine() {
        // 현재 상태를 재장전 중 상태로 전환
        state = State.Reloading;
      
        // 재장전 소요 시간 만큼 처리 쉬기
        yield return new WaitForSeconds(gunData.reloadTime);

        // 총의 현재 상태를 발사 준비된 상태로 변경
        state = State.Ready;
        lastFireTime = 0; // 마지막 발사 시점 초기화 
    }

    // 발사를 하되 발사 상태일때만 실행
    public void Fire()
    {
        if(state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;
            Shot();
        }
    }

    private void Shot()
    {
        RaycastHit hit; // 레이캐스트 충돌 정보를 저장할 변수
        Vector3 hitPosition = Vector3.zero; // 충돌 위치

        if(Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
                {
                    // 레이캐스트가 충돌한 경우
                    IDamageable target = hit.collider.GetComponent<IDamageable>();
                    if(target != null)
                    {
                        target.OnDamage(gunData.damage, hit);
                    }
                    hitPosition = hit.point;
                }
                else
                {
                    // 레이캐스트가 충돌하지 않은 경우
                    hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
                }


        // 남은 탄알 수
        magAmmo --;
        if(magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

}