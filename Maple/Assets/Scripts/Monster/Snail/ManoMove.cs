using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManoMove : MonoBehaviour //딱히 공격안하는몹.
{
    public int RandomValue;
    public int RandomTime; //이동이 지속될 시간.
    public int AttackedCount; //몬스터가 맞은 횟수
    public int RandomAttackTime;
    public int RandomSkillValue;

    public float Speed; //몬스터 속도

    public bool isMove;
    public bool isSlowMove;
    public bool isFastMove;
    public bool isRandomValue; //true 일때만 랜덤값지정가능
    public bool isRandomTime; //true 일때만 랜덤값지정가능
    public bool isRight; //오브젝트가 우측을 바라봄
    public bool isLeft; //오브젝트가 좌측을 바라봄
    public bool isTimeChecked;
    public bool isUseSkill;
    public bool isAttackTime;


    public Vector3 vStartPos;
    public Vector3 vConstantPos;
    public Vector3 FindArea;
    Vector3 CallArea1 = new Vector3 (0.2f, 0, 0); //달팽이 소환 영역.
    Vector3 CallArea2 = new Vector3(0.4f, 0, 0); //달팽이 소환 영역.
    Vector3 CallArea3 = new Vector3(0.6f, 0, 0); //달팽이 소환 영역.
    Vector3 CallArea4 = new Vector3(-0.2f, 0, 0); //달팽이 소환 영역.
    Vector3 CallArea5 = new Vector3(-0.4f, 0, 0); //달팽이 소환 영역.
    Vector3 CallArea6 = new Vector3(-0.6f, 0, 0); //달팽이 소환 영역.

    public GameObject Target; //Target이 있다면 추적, 없으면 추적안함
    public int AttackTime;
    public GameObject Player;

    public SpawnSnailForManoSkill S1;
    public SpawnSnailForManoSkill S2;
    public SpawnSnailForManoSkill S3;
    public SpawnSnailForManoSkill S4;
    public SpawnSnailForManoSkill S5;
    public SpawnSnailForManoSkill S6;

    int i = 0; //디버프 한번만 적용하기위해서.
    public void Start()
    {
        isMove = true;
        isSlowMove = true;
        isFastMove = false;
        isRandomValue = true;
        isRandomTime = true;
        isRight = true;
        isLeft = false;
        isTimeChecked = false;
        isUseSkill = false;
        isAttackTime = false;

        vStartPos = this.gameObject.transform.position;
    }

    public void Update()
    {
        vConstantPos = this.transform.position;
        Attack();
        
        if (Target == null)
        {
            //AttackedCount = 0;
            //isTimeChecked = false;
            RandomAttackTime = 0;
            RandomSkillValue = 0;
            StopCoroutine(ProcessSetRandomAttackTime());
            StopCoroutine(ProcessSetUseSkill());
            if (isRandomValue == true)
            {
                StartCoroutine(ProcessSetRandomValue());
            }
            if (isRandomTime == true)
            {
                StartCoroutine(ProcessSetRandomTime());
            }
            Patrol();
        }
        if (Target != null)
        {
            RandomTime = 0;
            RandomValue = 0;
            StopCoroutine(ProcessSetRandomValue());
            StopCoroutine(ProcessSetRandomTime());
            AttackMove();
            UseSkill();
        }

        MonsterStatus m = this.gameObject.GetComponent<MonsterStatus>();
        Player = m.Player;
        this.AttackedCount = m.AttackedCount; //참조 하는거다
    }

    private void FixedUpdate()
    {
        //Find();
    }

    public void Patrol()
    {
        if (isMove == true)
        {
            SetDirection();
            if (RandomValue == 0) //Stop
            {
                this.transform.position += new Vector3(0, 0, 0);
            }
            if (RandomValue == 1) //우측이동
            {
                Vector3 vDir = Vector3.Normalize(Vector3.right);
                Move(vDir);
                isRight = true;
                isLeft = false;
            }
            if (RandomValue == 2) //좌측이동
            {
                Vector3 vDir = Vector3.Normalize(Vector3.left);
                Move(vDir);
                isRight = false;
                isLeft = true;
            }
        }
    }

    public void SetDirection()
    {
        if (isRight == true && isLeft == false) //
        {
            Transform t = this.gameObject.GetComponent<Transform>();
            t.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
            MonsterStatus MonsterStatus = this.gameObject.GetComponent<MonsterStatus>();
            MonsterStatus.isRight = true;
            MonsterStatus.isLeft = false;
        }
        else if (isRight == false && isLeft == true)
        {
            Transform t = this.gameObject.GetComponent<Transform>();
            t.localScale = new Vector3(+0.5f, 0.5f, 0.5f);
            MonsterStatus MonsterStatus = this.gameObject.GetComponent<MonsterStatus>();
            MonsterStatus.isRight = false;
            MonsterStatus.isLeft = true;
        }
    } //오브젝트 방향전환

    public void Move(Vector3 vDir)
    {
        if (isSlowMove == true && isFastMove == false)
        {
            this.transform.position += vDir * Speed * 0.25f * Time.deltaTime;
            StartCoroutine(ProcessSlowMove());
        }
        else if (isSlowMove == false && isFastMove == true)
        {
            this.transform.position += vDir * Speed * Time.deltaTime;
            StartCoroutine(ProcessFastMove());
        }
    }

    public void Find() //몬스터의 탐지범위
    {
        int nLayer = 1 << LayerMask.NameToLayer("Player");
        Collider2D c = Physics2D.OverlapBox(vConstantPos, FindArea, 0f, nLayer);
        MonsterStatus m = this.gameObject.GetComponent<MonsterStatus>();

        if (c) //탐지
        {
            if (AttackedCount == 0 && isTimeChecked == false && c.gameObject.tag == "Player")
            {
                Target = c.gameObject;
            }
        }
        else if (AttackedCount == 0 && isTimeChecked == false)
        {
            Target = null;
        }
    }

    public void Attack() //몬스터가 맞았을때
    {
        if (AttackedCount > 0 && isTimeChecked == false) //몬스터가 맞았을때 추적타임
        {
            Target = Player;
            StartCoroutine(ProcessAttack());
        }
        else if (AttackedCount == 0 && isTimeChecked == false)
        {
            Target = null;
        }
    }

    public void UseSkill() //마노 패턴
    {
        PlayerCondition P_Condition = Player.gameObject.GetComponent<PlayerCondition>();
        PlayerStatus s = Player.GetComponent<PlayerStatus>();
        PlayerMove m = Player.GetComponent<PlayerMove>();
        ManoCondition M_Condition = this.gameObject.GetComponent<ManoCondition>();
        if (isAttackTime == false)
        {
            StartCoroutine(ProcessSetRandomAttackTime());
        }
        if (isUseSkill == false)
        {
            StartCoroutine(ProcessSetUseSkill());
        }
        if (i == 0) 
        {
            if (RandomSkillValue == 0 || RandomSkillValue == 5 || RandomSkillValue == 6 || RandomSkillValue == 7)
            {
                i++;
                Debug.Log("마노는 당신을 지켜보고있다.");
            }
            if (RandomSkillValue == 1)
            {
                i++;
                P_Condition.GiveCondition3("Slow");
                Debug.Log("플레이어 이동속도 3단계 디버프");
            }
            if (RandomSkillValue == 2)
            {
                i++;
                P_Condition.GiveCondition1("Weakness2");
                Debug.Log("플레이어 공격력2 1단계 디버프");
            }
            if (RandomSkillValue == 3)
            {
                i++;
                M_Condition.GiveCondition3("Fast");
                Debug.Log("몬스터 이동속도 3단계 버프");
            }
            if (RandomSkillValue == 4)
            {
                CallSnail();
                i++;
                Debug.Log("달팽이 소환");
            }
        }
    }

    public void CallSnail()
    {
        int RandomValue = Random.Range(1, 4);
        if (RandomValue == 1)
        {
            S1.transform.position = this.transform.position + CallArea1;
            S2.transform.position = this.transform.position + CallArea2;
            S3.transform.position = this.transform.position + CallArea3;
            S4.transform.position = this.transform.position + CallArea4;
            S5.transform.position = this.transform.position + CallArea5;
            S6.transform.position = this.transform.position + CallArea6;
        }
        else if (RandomValue == 2)
        {
            S1.transform.position = this.transform.position + CallArea2;
            S2.transform.position = this.transform.position + CallArea3;
            S3.transform.position = this.transform.position + CallArea4;
            S4.transform.position = this.transform.position + CallArea5;
            S5.transform.position = this.transform.position + CallArea6;
            S6.transform.position = this.transform.position + CallArea1;
        }
        else if (RandomValue == 3)
        {
            S1.transform.position = this.transform.position + CallArea3;
            S2.transform.position = this.transform.position + CallArea4;
            S3.transform.position = this.transform.position + CallArea5;
            S4.transform.position = this.transform.position + CallArea6;
            S5.transform.position = this.transform.position + CallArea1;
            S6.transform.position = this.transform.position + CallArea2;
        }
        S1.obj_RespawnTarget();
        S2.obj_RespawnTarget();
        S3.obj_RespawnTarget();
        S4.obj_RespawnTarget();
        S5.obj_RespawnTarget();
        S6.obj_RespawnTarget();
    }

    public void AttackMove()
    {
        Vector3 vTargetPos = Player.gameObject.transform.position;
        Vector3 vDist = vTargetPos - vConstantPos;
        float vTargetPosX = vTargetPos.x;
        float vConstantPosX = vConstantPos.x;
        Vector3 vDir = vDist.normalized;
        if (vTargetPosX >= vConstantPosX) //몹 우측이동
        {
            isRight = true;
            isLeft = false;
            SetDirection();
            Move(Vector3.right);
        }
        else
        {
            isRight = false;
            isLeft = true;
            SetDirection();
            Move(Vector3.left);
        }

    }

    IEnumerator ProcessSetRandomValue()
    {
        isRandomValue = false;
        yield return new WaitForSeconds(RandomTime);
        RandomValue = Random.Range(0, 3);
        isRandomValue = true;
    } //랜덤하게 Patrol 값지정
    IEnumerator ProcessSetRandomTime()
    {
        isRandomTime = false;
        yield return new WaitForSeconds(RandomTime);
        RandomTime = Random.Range(3, 6);
        isRandomTime = true;
    } //랜덤하게 Patrol 이 지속될 시간지정
    IEnumerator ProcessSlowMove()
    {
        isSlowMove = true;
        isFastMove = false;
        yield return new WaitForSeconds(1f);
        isSlowMove = false;
        isFastMove = true;
    } //기본적인 달팽이의 무빙
    IEnumerator ProcessFastMove()
    {
        isSlowMove = false;
        isFastMove = true;
        yield return new WaitForSeconds(1f);
        isSlowMove = true;
        isFastMove = false;
    } //기본적인 달팽이의 무빙
    IEnumerator ProcessAttack()
    {
        isTimeChecked = true;
        yield return new WaitForSeconds(AttackTime);
        MonsterStatus m = this.gameObject.GetComponent<MonsterStatus>();
        if (m.AttackedCount > 0)
        {
            m.AttackedCount--;
        }
        isTimeChecked = false;
    } //맞으면 일정시간동안 공격모드
    IEnumerator ProcessSetUseSkill() //랜더하게 스킬값지정
    {
        isUseSkill = true;
        yield return new WaitForSeconds(RandomAttackTime);
        RandomSkillValue = Random.Range(0, 8);
        isUseSkill = false;
        i--;
    }
    IEnumerator ProcessSetRandomAttackTime() //랜덤하게 스킬을 사용할 시간텀 지정
    {
        isAttackTime = true;
        yield return new WaitForSeconds(RandomAttackTime);
        int r = Random.Range(1, 4);
        RandomAttackTime = r * 5;
        isAttackTime = false;
    }

    public void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawWireCube(this.gameObject.transform.position, CallArea);
        //Gizmos.DrawWireSphere(S1.transform.position, 0.1f);
        //Gizmos.DrawWireSphere(S2.transform.position, 0.1f);
        //Gizmos.DrawWireSphere(S3.transform.position, 0.1f);
        //Gizmos.DrawWireSphere(S4.transform.position, 0.1f);
        //Gizmos.DrawWireSphere(S5.transform.position, 0.1f);
        //Gizmos.DrawWireSphere(S6.transform.position, 0.1f);
    }
}