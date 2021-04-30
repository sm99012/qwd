using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSuper : MonoBehaviour
{
    public bool isPower; //true : 피격X
    public float PowerTime; //피격시 무적시간

    void Start()
    {
        isPower = false;
    }

    public void P_Power()
    {
        StartCoroutine(ProcessHit());
        StartCoroutine(ProcessRight());
    }

    IEnumerator ProcessHit()
    {
        isPower = true;
        yield return new WaitForSeconds(PowerTime);
        isPower = false;
    } //피격시무적

    IEnumerator ProcessRight() //피격시 반짝임 효과
    {
        SpriteRenderer s = this.GetComponent<SpriteRenderer>();
        Color color = s.color;
        color.a = 0;
        s.color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1;
        s.color = color;
        yield return new WaitForSeconds(0.2f);
        color.a = 0;
        s.color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1;
        s.color = color;
        yield return new WaitForSeconds(0.2f);
        color.a = 0;
        s.color = color;
        yield return new WaitForSeconds(0.1f);
        color.a = 1;
        s.color = color;
    }
}