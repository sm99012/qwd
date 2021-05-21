using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Dist; //총의 사거리설정
    public int BulletDamage; //Bullet 자체 데미지
    public int TotalDamage; //플레이어스탯, 무기공격력, 총알데미지가 합해진 값
    public float Back; //Bullet의 몬스터 넉백값
    Vector3 vStartPos;
    Vector3 vPos;

    void Start()
    {
        vStartPos = this.transform.position;
    }


    void Update()
    {
        vPos = this.transform.position;
    }

    private void FixedUpdate()
    {
        float fDist = Vector3.Distance(vStartPos, vPos);
        if (Dist < fDist)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Bottom") //총알이 바닥에부딪히면 사라짐
        {
            Destroy(this.gameObject);
        }
    }
}
