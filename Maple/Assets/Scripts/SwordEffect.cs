using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEffect : MonoBehaviour
{
    public float Dist; //사거리설정
    public int TotalDamage; //플레이어스탯, 무기공격력이 합해진 값
    public float Back; //검의 몬스터 넉백값
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
            StartCoroutine(ProcessDestroy());
            Rigidbody2D r = this.GetComponent<Rigidbody2D>();
            r.velocity = new Vector3(0, 0, 0);
        }
    }

    public void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Bottom") //검기가 바닥에부딪히면 사라짐
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator ProcessDestroy()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }
}