using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombie : MonoBehaviour
{
    private Animator animator;
    private NavMeshAgent agent;

    private ZombieInfo info;

    private int hp;
    private int attack;
    private float atkOffset;
    private float attackTime;
    private float distance;
    private int deadMoney;
    private Transform target;


    public bool isDead;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        if (isDead) return;
        attackTime += Time.deltaTime;
        Move();
        Attack();

    }

    private void Move()
    {
        animator.SetBool("isRunning",agent.velocity != Vector3.zero);      
    }

    public void Attack()
    {
        float distance = Vector3.Distance(this.transform.position, target.position);
        if (distance < this.distance && attackTime > atkOffset)
        {
            attackTime = 0f;
            animator.SetTrigger("attack");
            GameDataMgr.Instance.PlaySound("Eat", this.transform.position);
        }
    }

    public void Wound(int damage)
    { 
        if(isDead) return;

        hp -= damage;
        if (hp <= 0)
        {
            Dead();
        }
        else 
        { 
            animator.SetTrigger("heart");
            //TODO: ‹…À“Ù–ß 
            GameDataMgr.Instance.PlaySound("Wound", this.transform.position);
        }
    }

    public void BornOver()
    {
        agent.SetDestination(target.position);
    }

    public void AtkEvent()
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position + this.transform.forward,2f,1 << LayerMask.NameToLayer("MainTower"));
        if (colliders.Length > 0)
        {
            MainTower.Instance.UpdateHp(attack);
        }
    }

    public void DeadEvent()
    {
        //TODO:À¿Õˆœ˙ªŸ
        GameManager.Instance.RemoveZombie(this);
        if(GameManager.Instance.CheckOver())
        {
            GameManager.Instance.IsGameOver = true;
            OverPanel overPanel = UIManager.Instance.ShowPanel<OverPanel>();
            overPanel.InitInfo(GameManager.Instance.player.GetMoney(),true);
        }
        Destroy(this.gameObject);
    }

    public void Dead()
    { 
        isDead = true;
        agent.isStopped = isDead;
        animator.SetBool("isDead",isDead);
        //TODO:ÕÊº“µ√«Æ
        GameManager.Instance.PlayerAddMoney(deadMoney);
        //TODO:À¿Õˆ“Ù–ß
        GameDataMgr.Instance.PlaySound("dead",this.transform.position);
    }


    public void InitInfo(ZombieInfo info)
    { 
        this.info = info;
        this.hp = info.hp;
        this.attack = info.atk;
        this.atkOffset = info.atkOffset;
        this.deadMoney = info.money;
        animator.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(info.animator);
        agent.speed = agent.acceleration = info.moveSpeed;
        agent.angularSpeed = info.roundSpeed;
        agent.stoppingDistance = 5f;
        this.distance = agent.stoppingDistance;
        target = MainTower.Instance.transform;
    }
}
