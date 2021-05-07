using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSuper : MonoBehaviour //플레이어 무적 스크립트
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
        s.color = new Color(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        s.color = new Color(1f, 1f, 1f);
        yield return new WaitForSeconds(0.2f);
        s.color = new Color(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        s.color = new Color(1f, 1f, 1f);
        yield return new WaitForSeconds(0.2f);
        s.color = new Color(0.5f, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        s.color = new Color(1f, 1f, 1f);
    }
}
