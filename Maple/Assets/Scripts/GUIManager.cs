using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public PlayerRespawn PlayerRespawn;
    public PlayerStatus PlayerStatus;

    public bool isGameStart;

    public enum E_SCENE_STATE { START, SYSTEM, DEATH }
    public E_SCENE_STATE m_eCurState;
    public List<GameObject> m_listScenes;
    public Image M_HP;
    public Image HP;
    public Image M_MP;
    public Image MP;
    public Image M_EXP;
    public Image EXP;
    public Text Lv;
    void Start()
    {
        SetState(m_eCurState);
        StartCoroutine(ProcessStartGame());
        PlayerStatus = PlayerRespawn.obj_Respawn.GetComponent<PlayerStatus>();

        //RectTransform HP_RectTransform = HP.GetComponent<RectTransform>();
        //RectTransform M_HP_RectTransform = M_HP.GetComponent<RectTransform>();
        //Vector2 vSize = HP_RectTransform.sizeDelta;
        //float HPBarWidth = vSize.x;
        //float C_HP = HPBarWidth * PlayerStatus.HP / PlayerStatus.M_HP;
    }

    void Update()
    {
        UpdateState();
        HPBar();
        MPBar();
        EXPBar();
        LvText();
    }

    void FixedUpdate()
    {
        
    }


    public void ShowScene(E_SCENE_STATE state)
    {
        //for (int i = 0; i < m_listScenes.Count; i++)
        //{
        //    if (i == (int)state)
        //    {
        //        m_listScenes[i].SetActive(true);
        //    }
        //    else
        //    {
        //        m_listScenes[i].SetActive(false);
        //    }
        //}
        int i = (int)state;
        if (i == 0) //Start
        {
            m_listScenes[0].SetActive(true);
            m_listScenes[1].SetActive(false);
            m_listScenes[2].SetActive(false);
        }
        if (i == 1) //System
        {
            m_listScenes[0].SetActive(false);
            m_listScenes[1].SetActive(true);
            m_listScenes[2].SetActive(false);
        }
        if (i == 2) //Death
        {
            m_listScenes[0].SetActive(false);
            m_listScenes[1].SetActive(true);
            m_listScenes[2].SetActive(true);
        }
    }

    public void SetState(E_SCENE_STATE state)
    {
        switch (state)
        {
            case E_SCENE_STATE.START:
                Time.timeScale = 0;
                break;
            case E_SCENE_STATE.SYSTEM:
                Time.timeScale = 1;
                break;
            case E_SCENE_STATE.DEATH:
                Time.timeScale = 1;
                break;
        }
        ShowScene(state);
        m_eCurState = state;
    }

    public void UpdateState()
    {
        GameObject objPlayer = PlayerRespawn.obj_Respawn;
        switch (m_eCurState)
        {
            case E_SCENE_STATE.START:
                break;
            case E_SCENE_STATE.SYSTEM:
                {
                    if (isGameStart == true)
                    {
                        if (objPlayer)
                        {
                            PlayerStatus PlayerStatus = objPlayer.GetComponent<PlayerStatus>();
                            if (PlayerStatus.HP <= 0)
                            {
                                SetState(E_SCENE_STATE.DEATH);
                            }
                        }
                    }
                }
                break;
            case E_SCENE_STATE.DEATH:
                break;
        }
    }

    public void EventSceneChange(int sceneNumber)
    {
        SetState((E_SCENE_STATE)sceneNumber);
    }

    public void RespawnEventSceneChange()
    {
        PlayerRespawn.obj_RespawnTarget();
        StartCoroutine(ProcessRespawn());
    }

    
    public void LvText()
    {
        Text C_Lv = Lv.GetComponent<Text>();
        string ConstantLevel = PlayerStatus.Lv.ToString();
        C_Lv.text = ConstantLevel;
    }

    public void HPBar()
    {
        RectTransform HP_RectTransform = HP.GetComponent<RectTransform>();
        RectTransform M_HP_RectTransform = M_HP.GetComponent<RectTransform>();
        Vector2 vSize = M_HP_RectTransform.sizeDelta;
        //float HPBarWidth = vSize.x; //HPBarWidth값을 고정시켜주면 되지않는지???
        float M_HPBarWidth = vSize.x;
        float C_HP = M_HPBarWidth * PlayerStatus.HP / PlayerStatus.M_HP;
        HP_RectTransform.sizeDelta = new Vector2(C_HP, 20); 
    }

    public void MPBar()
    {
        RectTransform MP_RectTransform = MP.GetComponent<RectTransform>();
        RectTransform M_HP_RectTransform = M_MP.GetComponent<RectTransform>();
        Vector2 vSize = M_HP_RectTransform.sizeDelta;
        //float HPBarWidth = vSize.x; //HPBarWidth값을 고정시켜주면 되지않는지???
        float M_MPBarWidth = vSize.x;
        float C_MP = M_MPBarWidth * PlayerStatus.MP / PlayerStatus.M_MP;
        MP_RectTransform.sizeDelta = new Vector2(C_MP, 20);
    }

    public void EXPBar()
    {
        RectTransform EXP_RectTransform = EXP.GetComponent<RectTransform>();
        RectTransform M_EXP_RectTransform = M_EXP.GetComponent<RectTransform>();
        Vector2 vSize = M_EXP_RectTransform.sizeDelta;
        //float HPBarWidth = vSize.x; //HPBarWidth값을 고정시켜주면 되지않는지???
        float M_EXPBarWidth = vSize.x;
        float C_EXP = M_EXPBarWidth * PlayerStatus.EXP / PlayerStatus.M_EXP;
        EXP_RectTransform.sizeDelta = new Vector2(C_EXP, 20);
    }

    IEnumerator ProcessStartGame()
    {
        yield return new WaitForSeconds(0.1f); //0.1초후 정상구동
        isGameStart = true;
    }

    IEnumerator ProcessRespawn()
    {
        yield return new WaitForSeconds(0.1f);
        SetState(E_SCENE_STATE.SYSTEM);
    }
}
