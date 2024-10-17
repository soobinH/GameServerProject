using System;
using UnityEngine;

//생명체로 동작할 오브젝트들은 모두 이 클래스를 상속받아야 함
//체력, 데미지 적용, 사망, 사망 이벤트등의 기능을 제공함
public class LivingEntity : MonoBehaviour, IDamageable
{
    public float startHealth = 100f;
    public float health { get; protected set; }//체력 프로퍼티
    public bool dead { get; protected set; }//사망관련 프로퍼티
    public event Action onDeath;

    protected virtual void OnEnable()
    {
        dead = false;
        health = startHealth;
    }

    //IDamageable 인터페이스를 상속해서 반드시 이 함수를 구현해줘야함
    public virtual void onDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    public virtual void RestoreHealth(float plusHealth)
    {
        if (dead) return;
        health += plusHealth;
    }

    public virtual void Die()
    {
        //onDeath에 등록된 이벤트가 없다면
        if (onDeath != null) onDeath();
        dead = true;
    }
}