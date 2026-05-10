using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private Vector3 offsetPos;
    [SerializeField] private float hight;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;

    private Vector3 targetPos;
    private Quaternion targetRot;
    private Transform target;

    private void Update()
    {
        if (target == null) return;

        //计算目标位置
        targetPos = target.position;
        targetPos += target.forward * offsetPos.z;
        targetPos += Vector3.up * offsetPos.y;
        targetPos += target.right * offsetPos.x;
        transform.position = Vector3.Lerp(transform.position,targetPos,moveSpeed * Time.deltaTime);

        //计算目标旋转
        targetRot = Quaternion.LookRotation((target.position + Vector3.up * hight) - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
