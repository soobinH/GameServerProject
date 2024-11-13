using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,//발사 준비
        Empty,//탄창이 비어있음
        Reloading//재장전
    }

    public State state { get; private set; }//상태 프로퍼티

    public Transform firePosition;

    public ParticleSystem muzzleFlashEffect;//총구 폭발
    public ParticleSystem shellEjectEffect;//탄피 배출

    private LineRenderer bulletLienRenderer;//탄알 궤적

    private AudioSource gunAudioSource;
    public  AudioClip shotClip;
    public  AudioClip reloadClip;

    public  float damage = 25;
    private float fireDistance = 50f;

    public int ammoRemain = 100;
    public int magCapacity = 25;
    public int magAmmo;//현재 총알

    public  float timeBetFire = 0.12f;
    public  float reloadTime = 1.8f;
    private float lastFireTime;

    #region 코루틴 함수 모음
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        gunAudioSource.PlayOneShot(shotClip);

        bulletLienRenderer.SetPosition(0, firePosition.position);
        bulletLienRenderer.SetPosition(1, hitPosition);

        bulletLienRenderer.enabled = true;
        yield return new WaitForSeconds(0.03f);
        bulletLienRenderer.enabled = false;
    }
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading;
        gunAudioSource.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(reloadTime);

        //채워야 할 탄창 = 총 총알량 - 현재 총알
        int ammoToFill = magCapacity - magAmmo;

        //현재 남은 총알이 남은 탄알 보다 적으면
        if(ammoRemain < ammoToFill)
        {
            //채워야 할 탄창은 = 남은 탄창
            ammoToFill = ammoRemain;
        }
        magAmmo += ammoToFill;
        ammoRemain -= ammoToFill;
        state = State.Ready;
    }
    #endregion

    private void Awake()
    {
        gunAudioSource = GetComponent<AudioSource>();
        bulletLienRenderer = GetComponent<LineRenderer>();

        bulletLienRenderer.positionCount = 2;//사용할 점을 2개로 설정
        bulletLienRenderer.enabled = false;
    }

    private void OnEnable()
    {
        magAmmo = magCapacity;
        state = State.Ready;
        lastFireTime = 0;
    }

    public void Fire()
    {
        //발가 사능 상태 and 마지막 총 발사 시점에서 timeBetFire 이상의 시간이 지나면
        if(state == State.Ready && Time.time >= lastFireTime + timeBetFire)
        {
            lastFireTime = Time.time;//발사 시점 갱신
            Shot();//발사
        }
    }

    private void Shot()
    {
        RaycastHit hit;//충돌 정보를 저장하기 위한 레이케스트
        Vector3 hitPosition = Vector3.zero;//탄알이 맞은 곳

        if(Physics.Raycast(firePosition.position, firePosition.forward, out hit, fireDistance))
        {

            //충돌한 상태방으로부터 인터페이스를 가져옴
            IDamageable target = hit.collider.GetComponent<IDamageable>();
            if(target != null)//인터페이스를 가져오는데 성공했다면
            {   //타겟의 onDamage를 실행시켜 데미지 적중)
                target.onDamage(damage, hit.point, hit.normal);
            }
            hitPosition = hit.point;//ray가 충돌한 위치 저장
        }
        else//충돌 안했을시
        {
            //최대 사정거리 까지 날아갔을때의 위치를 충돌 위치로서 사용
            hitPosition = firePosition.position + firePosition.forward * fireDistance;
        }
        StartCoroutine(ShotEffect(hitPosition));
        magAmmo--;
        if(magAmmo <= 0)
        {
            state = State.Empty;
        }
    }

    public bool Reload()
    {
        if(state == State.Reloading || ammoRemain <= 0 || magAmmo >= magCapacity)
        {
            return false;
        }
        StartCoroutine(ReloadRoutine());
        return true;
    }
}
