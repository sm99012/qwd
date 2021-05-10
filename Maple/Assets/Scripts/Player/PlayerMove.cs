using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public bool isGround;
    public bool isLadder;
    public bool isLadderStay;
    public bool isjump;
    public bool ispJump; //점프가 중첩되는버그를 없애기위해서
    public bool isKey;
    public bool isMove; //몬스터에 데이고있는상황 false 면 이동불가
    public bool isRight;//플레이어가 오른쪽으로 이동했었음
    public bool isLeft;//플레이어가 왼쪽으로 이동했었음
    public bool isGetKeyLeftAlt;
    public bool isRemoveVelocity; //n단점프후 얻는 가속도 제거
    public bool isPutKey;
    public bool isEnding;

    public float Speed;
    public float JumpPower;
    public int JumpCount;//인스펙터창에서 설정가능

    public int JumpCountProgram;//Script에서만 설정가능 (실 기능)
    void Start()
    {
        isGround = true;
        isLadder = false;
        isLadderStay = false;
        isjump = false;
        ispJump = false;
        isMove = true;
        isGetKeyLeftAlt = false;

        isRight = true;
        isLeft = false;
        isKey = false;

        isEnding = false;
    }

    void Update()
    {
        nJump();
        if (Speed < 0)
        {
            Speed = 0.1f;
        }
    }

    public void FixedUpdate()
    {
        Move();
    }

    private void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "Bottom")//땅에데였을때
        {
            isGround = true;
            isjump = false;
            JumpCountProgram = JumpCount; // JumpCountProgram 에 JumpCount 를 되돌려줘라.
            //Debug.Log(c.gameObject + "Enter Bottom");
            if (isPutKey == false)
            {
                //Rigidbody2D r = this.gameObject.GetComponent<Rigidbody2D>();
                //r.velocity = new Vector3(0, 0, 0);
            }
        }
        if (c.gameObject.tag == "Bottom to Ladder")//땅에데였을때
        {
            isGround = true;
            isjump = false;
            JumpCountProgram = JumpCount; // JumpCountProgram 에 JumpCount 를 되돌려줘라.
            //Debug.Log(c.gameObject + "Enter Bottom");
            if (isPutKey == false)
            {
                //Rigidbody2D r = this.gameObject.GetComponent<Rigidbody2D>();
                //r.velocity = new Vector3(0, 0, 0);
            }
            //if (Input.GetKeyDown(KeyCode.DownArrow))
            //{
            //    this.gameObject.transform.position += new Vector3(0, 1f);
            //}
            //Debug.Log("Enter");
        }
    }

    private void OnCollisionStay2D(Collision2D c)
    {
        if (c.gameObject.tag == "Bottom")
        {
            isGround = true;
            isKey = false;
            isMove = true;
        }
        if (c.gameObject.tag == "Bottom to Ladder")
        {
            isGround = true;
            isKey = false;
            isMove = true;
            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.gameObject.transform.position -= new Vector3(0, 0.075f);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D c)
    {
        if (c.gameObject.tag == "Bottom")
        {
            isGround = false;
            //Debug.Log(c.gameObject + "Exit Bottom");
        }
        if (c.gameObject.tag == "Bottom to Ladder")
        {
            isGround = false;
            //Debug.Log(c.gameObject + "Exit Bottom");
        }
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag == "Ladder")//사다리무빙
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                Debug.Log("Ladder");
                isGround = false;
                isLadder = true;
                isLadderStay = true;
                isjump = false;
                isMove = true;
                GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                Debug.Log("Ladder");
                isGround = false;
                isLadder = true;
                isLadderStay = true;
                isjump = false;
                isMove = true;
                GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.name == "House") //집에 들어왔을때 엔딩보기
        {
            isEnding = true;
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (c.gameObject.tag == "Ladder" && isLadderStay == true) //+ 
        {
            Debug.Log("??");
            //isGround = true;
            isLadder = false;
            isLadderStay = false;
            isjump = true;
            GetComponent<Rigidbody2D>().gravityScale = 1;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            //Debug.Log(c.gameObject + "Exit Ladder");
        }
    }

    IEnumerator ProcessJump()
    {
        ispJump = true;
        JumpCountProgram--;
        Rigidbody2D r = GetComponent<Rigidbody2D>();
        r.AddForce(Vector3.up * JumpPower * 40f * Time.deltaTime);
        yield return new WaitForSeconds(0.3f);
        ispJump = false;
    }

    IEnumerator ProcessNJump(Vector3 rPos)
    {
        JumpCountProgram--;
        Rigidbody2D r = GetComponent<Rigidbody2D>();
        r.AddForce(rPos * JumpPower * 25f * Time.deltaTime);
        r.AddForce(Vector3.up * JumpPower * 25f * Time.deltaTime);
        yield return new WaitForSeconds(0.0000001f);
    }

    public void Move()
    {
        if (isRight == true && isLeft == false)
        {
            Transform t = this.gameObject.GetComponent<Transform>();
            t.localScale = new Vector3(+1, 1, 1);
        }
        else if (isRight == false && isLeft == true)
        {
            //SpriteRenderer r = this.gameObject.GetComponent<SpriteRenderer>();
            //r.flipX = true;
            Transform t = this.gameObject.GetComponent<Transform>();
            t.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetKey(KeyCode.LeftAlt) == true) //n단점프때문에 추가
        {
            isGetKeyLeftAlt = true;
            isPutKey = true;
        }
        else
        {
            isGetKeyLeftAlt = false;
            isPutKey = false;
        }
        if (isLadder == false && isMove == true)//평시
        {
            if (Input.GetKey(KeyCode.RightArrow)) //오른쪽이동
            {
                transform.position += Vector3.right * Speed * Time.deltaTime;
                isRight = true;
                isLeft = false;
                isPutKey = true;
            }
            else isPutKey = false;

            if (Input.GetKey(KeyCode.LeftArrow)) //왼쪽이동
            {
                transform.position += Vector3.left * Speed * Time.deltaTime;
                isRight = false;
                isLeft = true;
                isPutKey = true;
            }
            else isPutKey = false;

            if (JumpCountProgram > 0 && isGround == true && ispJump == false)//점프키 꾹누를때 점프 계속되는기능
            {
                if (Input.GetKey(KeyCode.LeftAlt))
                {
                    isKey = true;
                    StartCoroutine(ProcessJump());
                }
            }
        }//평시무빙

        if (isLadder == true && isMove == true)//사다리에있을시
        {
            
            if (Input.GetKey(KeyCode.RightArrow))
            {
                transform.position += Vector3.right * Time.deltaTime;
                //Debug.Log("Move Right");
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                transform.position += Vector3.left * Time.deltaTime;
                //Debug.Log("Move Left");
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                transform.position += Vector3.up * Time.deltaTime;
                //Debug.Log("Move Up");
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                transform.position += Vector3.down * Time.deltaTime;
                //Debug.Log("Move Down");
            }
        }//사다리무빙
    }

    public void nJump() //n단점프
    {
        if (JumpCountProgram > 0 && Input.GetKeyDown(KeyCode.LeftAlt) && isGround == false && isLadder == false)
        {
            Debug.Log("n단점프");
            if (isRight == true && isLeft == false)
            {
                Vector3 rPos = Vector3.right;//플레이어 진행방향.
                JumpCountProgram--;
                isMove = false;
                Rigidbody2D r = GetComponent<Rigidbody2D>();
                r.AddForce(rPos * JumpPower * 80f * Time.deltaTime);
                r.AddForce(Vector3.up * JumpPower * 40f * Time.deltaTime);
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    //Transform t = this.gameObject.GetComponent<Transform>();
                    //t.localScale = new Vector3(+1, 1, 1);
                    isRight = true;
                    isLeft = false;
                }
            }
            if (isRight == false && isLeft == true)
            {
                Vector3 rPos = Vector3.left;//플레이어 진행방향.
                JumpCountProgram--;
                isMove = false;
                Rigidbody2D r = GetComponent<Rigidbody2D>();
                r.AddForce(rPos * JumpPower * 80f * Time.deltaTime);
                r.AddForce(Vector3.up * JumpPower * 40f * Time.deltaTime);
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    //Transform t = this.gameObject.GetComponent<Transform>();
                    //t.localScale = new Vector3(-1, 1, 1);
                    isRight = false;
                    isLeft = true;
                }
            }
        }
    }
}