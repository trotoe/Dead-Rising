using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Transform firePoint; 
    [SerializeField] private float shootDis = 10f;

    private Animator animator;
    private int attack;
    private int money;
    private int hp;
    private float rotateSpeed = 50f;

    private void Start()
    {
        animator = GetComponent<Animator>();
        Camera.main.GetComponent<CameraMove>().SetTarget(transform);
    }
    private void Update()
    {
        if (Vector3.Distance(this.transform.position, MainTower.Instance.transform.position) > 60f)
        { 
            this.transform.position = MainTower.Instance.transform.position + Vector3.forward * 5f;
        }

        animator.SetFloat("xSpeed", Input.GetAxis("Horizontal"));
        animator.SetFloat("ySpeed", Input.GetAxis("Vertical"));
        transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime);

        //if (Input.GetKey(KeyCode.LeftShift))
        //{ 
        
        //}

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetLayerWeight(1, 1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        { 
            animator.SetLayerWeight(1, 0);
        }

        //开火
        if (Input.GetMouseButtonDown(0))
        { 
            animator.SetTrigger("attack");
        }
        //翻滚
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("roll");
        }
    }

    public void InitPlayerInfo(RoleInfo roleInfo)
    {
        //获取角色信息
        attack = roleInfo.damage;
        money = roleInfo.money;
        MainTower.Instance.Init(roleInfo);
        UpdateMoney();
    }

    private void UpdateMoney()
    { 
        UIManager.Instance.GetPanel<GamePanel>().UpdateMoney(money);

    }

    public void AddMoney(int money)
    { 
        this.money += money;
        UpdateMoney();
    }

    public void AttackEvent()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward + transform.up,1f,1 << LayerMask.NameToLayer("Monster"));
        //TODO:伤害怪物
        for (int i = 0; i < colliders.Length; i++)
        {
            Zombie zombie = colliders[i].GetComponent<Zombie>();
            if (zombie != null && !zombie.isDead)
            {
                zombie.Wound(attack);
                GameDataMgr.Instance.PlaySound("Knife", colliders[i].transform.position);
                break;
            }
        }
    }

    public void FireEvent()
    {
        RaycastHit[] raycastHits = Physics.RaycastAll(new Ray(firePoint.position,transform.forward),shootDis,1 << LayerMask.NameToLayer("Monster"));
        for (int i = 0; i < raycastHits.Length; i++)
        {
            Zombie zombie = raycastHits[i].collider.GetComponent<Zombie>();
            if (zombie != null && !zombie.isDead)
            {
                zombie.Wound(attack);
                GameObject effect = Instantiate(Resources.Load<GameObject>("Effects/FireImpactMega"),
                   raycastHits[i].transform.position, Quaternion.LookRotation(raycastHits[i].normal));
                GameDataMgr.Instance.PlaySound("Gun", raycastHits[i].transform.position);
                break;
            }
        }
    }

    public int GetMoney()
    { 
        return money;
    }

}
