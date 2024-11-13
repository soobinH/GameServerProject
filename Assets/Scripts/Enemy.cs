using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : LivingEntity
{
    public LayerMask targetMask;//추적대상 레이어
    private LivingEntity targetEntity;
    private NavMeshAgent pathFinder;

    public ParticleSystem hitEffect;
    public AudioClip deathSound;
    public AudioClip hitSound;

    private Animator enemyAnimator;
    private AudioSource enemyAudioPlayer;
    private Renderer enemyRenderder;

    public float damage = 20f;
    public float timeBetAttack = 0.5f;
    private float lastAttackTime;
    private bool hasTarget//추적대상이 존재하는지 확인하는 프로퍼티
    {
        get
        {
            if (targetEntity != null && !targetEntity.dead)
            {
                return true;
            }
            return false;
        }
    }

    private void Awake()
    {
        pathFinder = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponent<Animator>();
        enemyAudioPlayer = GetComponent<AudioSource>();
        enemyRenderder = GetComponentInChildren<Renderer>();
    }

    public void Setup(float setHealth, float setDamage, float setSpeed, Color skinColor)
    {
        startHealth = setHealth;
        health = setHealth;
        damage = setDamage;
        pathFinder.speed = setSpeed;
        enemyRenderder.material.color = skinColor;
    }
    private void Start()
    {
        StartCoroutine(UpdatePath());//AI추적 루틴 시작
    }
    private void Update()
    {
        enemyAnimator.SetBool("HasTarget", hasTarget);
    }

    public override void onDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!dead)
        {
            //공격받은 지점과 방향으로 이펙트 재생
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
            enemyAudioPlayer.PlayOneShot(hitSound);
        }
        base.onDamage(damage, hitPoint, hitNormal);
    }

    public override void Die()
    {
        base.Die();

        Collider[] enemyColliders = GetComponents<Collider>();
        for (int i = 0; i < enemyColliders.Length; i++)
        {
            enemyColliders[i].enabled = false;
        }
        pathFinder.isStopped = true;
        pathFinder.enabled = false;

        enemyAnimator.SetTrigger("Die");
        enemyAudioPlayer.PlayOneShot(deathSound);
    }
    private void OnTriggerStay(Collider other)
    {
        if (!dead && Time.time >= lastAttackTime + timeBetAttack)
        {
            LivingEntity livingEntity = other.GetComponent<LivingEntity>();
            //상대방의 LivingEntity가 추적대상이랑 동일하다면
            if (livingEntity != null && livingEntity == targetEntity)
            {
                lastAttackTime = Time.time;//최근공격 시간 갱신

                Vector3 hitPoint = other.ClosestPoint(transform.position);//근사값
                Vector3 hitNormal = transform.position - other.transform.position;

                livingEntity.onDamage(damage, hitPoint, hitNormal);
            }
        }
    }

    /// <summary>
    /// 추적할 대상의 위치를 주기적으로 찾아서 갱신하는 함수
    /// </summary>
    /// <returns></returns>
    private IEnumerator UpdatePath()
    {
        while (!dead)//살아있는동안
        {
            if (hasTarget)
            {
                pathFinder.isStopped = false;
                pathFinder.SetDestination(targetEntity.transform.position);
            }
            else
            {
                pathFinder.isStopped = true;

                //20f 반지름을 가진 가상의 구를 그렸을 때 구와 겹치는 모든 콜라이더를 가져옴
                //단 targetMask 레이러를 가진 콜라이더만 가져오도록 필터링
                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, targetMask);

                //모든 콜라이더를 순회하며 살아있는 LivingEntity를 찾는다
                for (int i = 0; i < colliders.Length; i++)
                {
                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();

                    //LivingEntity 컴포넌트가 존재하고 살아있는 상태라면
                    if (livingEntity != null && !livingEntity.dead)
                    {
                        //추적 대상을 갱신
                        targetEntity = livingEntity;
                        break;
                    }
                }
            }
            yield return new WaitForSeconds(0.25f);
        }
    }
}