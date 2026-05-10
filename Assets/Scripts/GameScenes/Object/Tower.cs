using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Transform head;
    [SerializeField]private Transform firePoint;

    private float roundSpeed = 20f;

    private Zombie target;
    private float atkTimer = 0;

    private float atkRange;
    private float offsetTime;

    TowerData data;

    Vector3 monsterPos;

    public void Init(TowerData data)
    { 
        this.data = data;   
    }

    private void Update()
    {
        atkTimer += Time.deltaTime;
        if (data.type == 1)
        {
            if (target == null || target.isDead
                || Vector3.Distance(target.transform.position,this.transform.position) > atkRange)
            {
                target = GameManager.Instance.FindZombie(this.transform.position, data.atkRange);
            }
            if (target == null) return;
            monsterPos = target.transform.position;
            monsterPos.y = head.position.y;
            Vector3 dir = monsterPos - head.position;
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                Quaternion.LookRotation(dir),
                Time.deltaTime * roundSpeed);
            if (atkTimer >= data.offsetTime && Vector3.Angle(head.forward, dir) < 5f)
            {
                atkTimer = 0;
                GameObject effect = Instantiate(Resources.Load<GameObject>("Effects/Fire1"), firePoint.position, firePoint.rotation);
                Destroy(effect, 0.2f);
                GameDataMgr.Instance.PlaySound("Tower", firePoint.position);
                target.Wound(data.atk);
            }
        }
        //else 
        //{ 
        //    List<Zombie> zombies = GameManager.Instance.FindZombies(this.transform.position, data.atkRange);
        //    if (zombies.Count > 0 && atkTimer > data.offsetTime)
        //    { 
        //        foreach (var zombie in zombies)
        //        {
        //            GameObject effect = Instantiate(Resources.Load<GameObject>("Effects/Fire2"), zombie.transform.position, Quaternion.identity);
        //            Destroy(effect, 0.2f);
        //            GameDataMgr.Instance.PlaySound("Tower", zombie.transform.position);
        //            zombie.Wound(data.atk);
        //        }
        //    }
        //}
    }
}
