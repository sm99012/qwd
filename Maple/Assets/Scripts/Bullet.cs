using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Dist; //총의 사거리설정
    public int Damage; //Bullet 자체 데미지
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

        TotalDamage = this.Damage; // + 무기데미지 + 플레이어 스탯
    }

    private void FixedUpdate()
    {
        float fDist = Vector3.Distance(vStartPos, vPos);
        if (Dist < fDist)
        {
            Destroy(this.gameObject);
        }
    }
}
