using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManoCondition : MonoBehaviour //버프, 디버프 관련 스크립트 시간중첩가능(최대 3번)
{
    public GameObject Monster; //버프, 디버프가 적용될 오브젝트
    string Fast;
    string Slow;
    string Strong1; //공격력 버프, 디버프 +-
    string Strong2; //공격력 버프, 디버프 */
                    //곱적용은 또 플레이어의 STR에만 적용할지, T_STR에 적용할지 생각.
    string Weakness1;
    string Weakness2;
    string Darkness;

    void Start()
    {

    }

    void Update()
    {
        Monster = this.gameObject;
    }

    public void GiveCondition1(string ConditionList)
    {
        switch (ConditionList)
        {
            case "Fast":
                StartCoroutine(ProcessContinueCondition_Speed(1.25f, 10));
                break;
            case "Slow":
                StartCoroutine(ProcessContinueCondition_Speed(0.75f, 10));
                break;
            case "Strong1":
                StartCoroutine(ProcessContinueCondition_Strong1(5, 10));
                break;
            case "Strong2":
                StartCoroutine(ProcessContinueCondition_Strong2(1.25f, 10)); //%계산필요 //총뎀1.25배
                break;
            case "Weakness1":
                StartCoroutine(ProcessContinueCondition_Strong1(-5, 10));
                break;
            case "Weakness2":
                StartCoroutine(ProcessContinueCondition_Strong2(0.75f, 10)); //총뎀 0.75배
                break;
                //case "Darkness":
                //    StartCoroutine(ProcessContinueCondition_Darkness(10, 10));
                //    break;

        }
    }
    public void GiveCondition3(string ConditionList)
    {
        switch (ConditionList)
        {
            case "Fast":
                StartCoroutine(ProcessContinueCondition_Speed(3f, 10));
                break;
        }
    }

    IEnumerator ProcessContinueCondition_Speed(float AddSpeed, int ContinueTime) //이속버프, 디버프
    {
        MonsterStatus MonsterStatus = this.gameObject.GetComponent<MonsterStatus>();
        ManoMove ManoMove = this.gameObject.GetComponent<ManoMove>();
        ManoMove.Speed = (float)(ManoMove.Speed * AddSpeed);
        yield return new WaitForSeconds(ContinueTime);
        ManoMove.Speed = (float)(ManoMove.Speed * 1 / AddSpeed);
    }
    IEnumerator ProcessContinueCondition_Strong1(int AddStrong, int ContinueTime) //공격력+-디버프
    {
        MonsterStatus MonsterStatus = this.gameObject.GetComponent<MonsterStatus>();
        ManoMove ManoMove = this.gameObject.GetComponent<ManoMove>();
        MonsterStatus.STR += AddStrong;
        yield return new WaitForSeconds(ContinueTime);
        MonsterStatus.STR -= AddStrong;
    }
    IEnumerator ProcessContinueCondition_Strong2(float AddStrong, int ContinueTime) //공격력*/디버프
    {
        MonsterStatus MonsterStatus = this.gameObject.GetComponent<MonsterStatus>();
        ManoMove ManoMove = this.gameObject.GetComponent<ManoMove>();
        MonsterStatus.STR = (int)(MonsterStatus.STR * AddStrong);
        yield return new WaitForSeconds(ContinueTime);
        MonsterStatus.STR = (int)(MonsterStatus.STR * 1 / AddStrong);
    }
}
