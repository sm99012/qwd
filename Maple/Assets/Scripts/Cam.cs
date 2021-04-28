using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    public float Speed; //추적속도
    public GameObject Player; //카메라가 추적할 대상

    public void FixedUpdate()
    {
        if (Player != null)
        {
            Follow(this.transform.position, Player.transform.position);
        }
    }

    public void Follow(Vector3 vPos, Vector3 vTargetPos)
    {
        Vector3 vDist = vTargetPos - vPos;
        Vector3 vDir = vDist.normalized;
        float Dist = Vector3.Distance(vPos, vTargetPos);
        float fDist = Speed * Time.deltaTime;
        if (Dist > fDist)
        {
            this.transform.position += vDir * Speed * Time.deltaTime;
            this.transform.position = new Vector3(transform.position.x + 0.03f, transform.position.y + 0.02f, -5);
        }
    }
}
