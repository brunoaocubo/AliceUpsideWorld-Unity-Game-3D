using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterIA : MonoBehaviour, IDataPersisting
{
    [SerializeField] private Transform target;
    [SerializeField] private Animator anim;
    [SerializeField] private float damage;
    [SerializeField] private float lastAttackTime;
    [SerializeField] private float attackCooldown;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float attackDistance;

    private NavMeshAgent enemy;
    public float healthMonster = 50;
    private float healthMonsterRegenerate = 2;

    private float distance;

    public float HealthMonster { get => healthMonster; set => healthMonster = value; }

    void Start()
    {
        enemy = GetComponent<NavMeshAgent>();
    }
    public void LoadData(GameData data)
    {

        this.healthMonster = data.healthMonsterData;
    }

    public void SaveData(ref GameData data)
    {
        data.healthMonsterData = this.healthMonster;
    }

    void Update()
    {

        if (healthMonster <= 0)
        {
            Destroy(gameObject);
        }
        distance = Vector3.Distance(transform.position, target.position);

        if (distance > stoppingDistance) 
        {
            enemy.isStopped = true;
            anim.SetBool("Walk", false);
            anim.SetBool("Attack", false);
        }
        else
        {
            enemy.SetDestination(target.position);
            enemy.isStopped = false;
            anim.SetBool("Walk", true);
        }

        if(distance <= attackDistance) 
        {
            if(Time.time - lastAttackTime >= attackCooldown) 
            {
                lastAttackTime = Time.time;
                anim.SetBool("Attack", true);
                anim.SetBool("Walk", false);
                PlayerController.Instance.TakeDamage(damage);
                PlayerController.Instance.IsTakeDamage = true;
            }
        }
        else 
        {
            anim.SetBool("Attack", false);
            PlayerController.Instance.IsTakeDamage = false;
        }

        /*
        if (enemy.remainingDistance < 3)
        {
            anim.SetBool("Walk", true);
            anim.SetBool("Attack", false);
            PlayerController.Instance.IsTakeDamage = false;

        }
        else 
        {
            if (Time.time - lastAttackTime >= attackCooldown) 
            {
                lastAttackTime = Time.time;
                anim.SetBool("Attack", true);
                anim.SetBool("Walk", false);
                PlayerController.Instance.TakeDamage(damage);
                PlayerController.Instance.IsTakeDamage = true;
            }
        }
        */
    }

    public void TakeDamage(float damage) 
    {
        healthMonster -= damage;
    }

    IEnumerator HealthRegenerateDelay()
    {
        yield return new WaitForSeconds(4f);
        healthMonster += healthMonsterRegenerate * Time.deltaTime;
    }
}
