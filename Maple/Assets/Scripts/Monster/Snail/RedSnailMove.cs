using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedSnailMove : MonoBehaviour //딱히 공격안하는몹.
{
    public int RandomValue;
    public int RandomTime; //이동이 지속될 시간.
    public int AttackedCount; //몬스터가 맞은 횟수

    public float Speed; //몬스터 속도

    public bool isMove;
    public bool isSlowMove;
    public bool isFastMove;
    public bool isRandomValue; //true 일때만 랜덤값지정가능
    public bool isRandomTime; //true 일때만 랜덤값지정가능
    public bool isRight; //오브젝트가 우측을 바라봄
    public bool isLeft; //오브젝트가 좌측을 바라봄
    public bool isTimeChecked;

    public Vector3 vStartPos;
    public Vector3 vConstantPos;
    public Vector3 FindArea;

    public GameObject Target; //Target이 있다면 추적, 없으면 추적안함
    public int AttackTime;
    public GameObject Player;

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

        vStartPos = this.gameObject.transform.position;
    }

    public void Update()
    {
        vConstantPos = this.transform.position;
        if (Target == null && AttackedCount == 0 && isTimeChecked == false)
        {
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
        }

        MonsterStatus m = this.gameObject.GetComponent<MonsterStatus>();
        Player = m.Player;
        this.AttackedCount = m.AttackedCount; //참조 하는거다
    }

    private void FixedUpdate()
    {
        Find();
        Attack();
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
                Move(Vector3.right);
                isRight = true;
                isLeft = false;
            }
            if (RandomValue == 2) //좌측이동
            {
                Move(Vector3.left);
                isRight = false;
                isLeft = true;
            }
        }
    }

    public void SetDirection()
    {
        if (isRight == true && isLeft == false)
        {
            Transform t = this.gameObject.GetComponent<Transform>();
            t.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
        }
        else if (isRight == false && isLeft == true)
        {
            Transform t = this.gameObject.GetComponent<Transform>();
            t.localScale = new Vector3(+0.5f, 0.5f, 0.5f);
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
    }

    public void AttackMove()
    {
        if (Player != null)
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
                Move(vDir);
            }
            else
            {
                isRight = false;
                isLeft = true;
                SetDirection();
                Move(vDir);
            }
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
        m.AttackedCount--;
        //if (m.AttackedCount == 0)
        //{
        //    Target = null;
        //}
        Debug.Log(m.AttackedCount);
        isTimeChecked = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.gameObject.transform.position, FindArea);
    }
}