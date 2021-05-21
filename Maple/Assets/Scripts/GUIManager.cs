using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public PlayerRespawn PlayerRespawn;
    public PlayerStatus PlayerStatus;
    public PlayerEquip PlayerEquip;
    public GetItem GetItem;

    public bool isGameStart;
    public bool isInventory; //true : 인벤토리가 켜져있음
    public bool isSlota;
    public bool isSlotb;
    public bool isSlotc;
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

    public List<GameObject> Inventory;
    public List<Button> InventorySlotButton;
    public List<Image> Sa;
    public List<Text> Sa_C;
    public List<Image> Sb;
    public List<Text> Sb_C;
    public List<Image> Sc;
    public List<Text> Sc_C;

    //public Image Sc1; //아이템슬롯
    //public Image Sc2;
    //public Image Sc3;
    //public Image Sc4;
    //public Image Sc5;
    //public Image Sc6;
    //public Image Sc7;
    //public Image Sc8;
    //public Image Sc9;
    //public Image Sc10;
    //public Image Sc11;
    //public Image Sc12;
    //public Image Sc13;
    //public Image Sc14;
    //public Image Sc15;

    public Text HoldGold; //가지고있는 골드의 보유량.
    void Start()
    {
        SetState(m_eCurState);
        StartCoroutine(ProcessStartGame());

        isGameStart = false;
        isInventory = false;
        isSlota = true;
        isSlotb = false;
        isSlotc = false;
    }


    void Update()
    {
        if (PlayerRespawn.obj_Respawn)
        {
            PlayerStatus = PlayerRespawn.obj_Respawn.GetComponent<PlayerStatus>();
            PlayerEquip = PlayerRespawn.obj_Respawn.GetComponent<PlayerEquip>();
            GetItem = PlayerRespawn.obj_Respawn.GetComponent<GetItem>();
            GetItem.GUIManager = this;
            PlayerEquip.GUIManager = this;
        }
        string S_Gold = GetItem.I_Gold.ToString(); //보유 골드 현황
        HoldGold.text = S_Gold;

        UpdateState();
        HPBar();
        MPBar();
        EXPBar();
        LvText();
        EquipmentWindow();
        SaveItem(); //아이템창
        SlotWindow();
    }
    
    void FixedUpdate()
    {

    }

    public void ShowScene(E_SCENE_STATE state)
    {
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
        if (i == 3) //InventorySlota
        {

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
                            PlayerStatus = objPlayer.GetComponent<PlayerStatus>();
                            if (PlayerStatus.HP <= 0)
                            {
                                SetState(E_SCENE_STATE.DEATH);
                            }
                            if (Input.GetKeyDown(KeyCode.I))
                            {
                                if (isInventory == true)
                                {
                                    isInventory = false;
                                }
                                else if (isInventory == false)
                                {
                                    isInventory = true;
                                }
                            }
                            if (isInventory == true)
                            {
                                if (Input.GetKeyDown(KeyCode.Escape))
                                {
                                    isInventory = false;
                                }
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
    public void EquipmentWindow()
    {
        if (isInventory == true)
        {
            m_listScenes[3].SetActive(true);
        }
        if (isInventory == false)
        {
            m_listScenes[3].SetActive(false);
        }
    }
    public void EquipmentWindowClose()
    {
        isInventory = false;
    }

    public void SaveItem() //획득아이템 GUI연동
    {
        for (int i = 0; i < GetItem.Slota.Length; i++)
        {
            if (GetItem.Slota[i] != null)
            {
                GameObject s = Resources.Load("Prefabs/" + GetItem.Slota[i].name + "Sprite") as GameObject;
                SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
                Sa[i].sprite = ss.sprite;
                string t = GetItem.Slota_Count[i].ToString();
                Sa_C[i].text = t;
            }
            if (GetItem.Slota[i] == null)
            {
                GameObject s = Resources.Load("Prefabs/ItemBackGroundSprite") as GameObject;
                SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
                Sa[i].sprite = ss.sprite;
                string t = "";
                Sa_C[i].text = t;
            }
        }
        for (int i = 0; i < GetItem.Slotb.Length; i++)
        {
            if (GetItem.Slotb[i] != null)
            {
                GameObject s = Resources.Load("Prefabs/" + GetItem.Slotb[i].name + "Sprite") as GameObject;
                SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
                Sb[i].sprite = ss.sprite;
                string t = GetItem.Slotb_Count[i].ToString();
                Sb_C[i].text = t;
            }
            if (GetItem.Slotb[i] == null)
            {
                GameObject s = Resources.Load("Prefabs/ItemBackGroundSprite") as GameObject;
                SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
                Sb[i].sprite = ss.sprite;
                string t = "";
                Sb_C[i].text = t;
            }
        }
        for (int i = 0; i < GetItem.Slotc.Length; i++)
        {
            if (GetItem.Slotc[i] != null)
            {
                GameObject s = Resources.Load("Prefabs/" + GetItem.Slotc[i].name + "Sprite") as GameObject;
                SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
                Sc[i].sprite = ss.sprite;
                string t = GetItem.Slotc_Count[i].ToString();
                Sc_C[i].text = t;
            }
            if (GetItem.Slotc[i] == null)
            {
                GameObject s = Resources.Load("Prefabs/ItemBackGroundSprite") as GameObject;
                SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
                Sc[i].sprite = ss.sprite;
                string t = "";
                Sc_C[i].text = t;
            }
        }
        //if (GetItem.Slotc[0] != "")
        //{
        //    GameObject s = Resources.Load("Prefabs/" + GetItem.Slotc[0] + "Sprite") as GameObject;
        //    SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
        //    Sc1.sprite = ss.sprite;
        //}
        //if (GetItem.Slotc[1] != "")
        //{
        //    GameObject s = Resources.Load("Prefabs/" + GetItem.Slotc[0] + "Sprite") as GameObject;
        //    SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
        //    Sc2.sprite = ss.sprite;
        //}
        //if (GetItem.Slotc[2] != "")
        //{
        //    GameObject s = Resources.Load("Prefabs/" + GetItem.Slotc[0] + "Sprite") as GameObject;
        //    SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
        //    Sc3.sprite = ss.sprite;
        //}
        //if (GetItem.Slotc[3] != "")
        //{
        //    GameObject s = Resources.Load("Prefabs/" + GetItem.Slotc[0] + "Sprite") as GameObject;
        //    SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
        //    Sc4.sprite = ss.sprite;
        //}
        //if (GetItem.Slotc[4] != "")
        //{
        //    GameObject s = Resources.Load("Prefabs/" + GetItem.Slotc[0] + "Sprite") as GameObject;
        //    SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
        //    Sc5.sprite = ss.sprite;
        //}
        //if (GetItem.Slotc[5] != "")
        //{
        //    GameObject s = Resources.Load("Prefabs/" + GetItem.Slotc[0] + "Sprite") as GameObject;
        //    SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
        //    Sc6.sprite = ss.sprite;
        //}
        //if (GetItem.Slotc[6] != "")
        //{
        //    GameObject s = Resources.Load("Prefabs/" + GetItem.Slotc[0] + "Sprite") as GameObject;
        //    SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
        //    Sc7.sprite = ss.sprite;
        //}
        //if (GetItem.Slotc[7] != "")
        //{
        //    GameObject s = Resources.Load("Prefabs/" + GetItem.Slotc[0] + "Sprite") as GameObject;
        //    SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
        //    Sc8.sprite = ss.sprite;
        //}
        //if (GetItem.Slotc[8] != "")
        //{
        //    GameObject s = Resources.Load("Prefabs/" + GetItem.Slotc[0] + "Sprite") as GameObject;
        //    SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
        //    Sc9.sprite = ss.sprite;
        //}
        //if (GetItem.Slotc[9] != "")
        //{
        //    GameObject s = Resources.Load("Prefabs/" + GetItem.Slotc[0] + "Sprite") as GameObject;
        //    SpriteRenderer ss = s.gameObject.GetComponent<SpriteRenderer>();
        //    Sc10.sprite = ss.sprite;
        //}
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

    public void SlotWindow() //인벤토리 장비, 소비, 기타창
    {
        if (isSlota == true && isSlotb == false && isSlotc == false)
        {
            if (isInventory == true) //장비
            {
                InventorySlotButton[0].image.color = new Color(255, 255, 255, 0.7f);
                InventorySlotButton[1].image.color = new Color(255, 255, 255, 0.1f);
                InventorySlotButton[2].image.color = new Color(255, 255, 255, 0.1f);
                Inventory[0].SetActive(true);
                Inventory[1].SetActive(false);
                Inventory[2].SetActive(false);
            }
        }
        if (isSlota == false && isSlotb == true && isSlotc == false)
        {
            if (isInventory == true) //소비
            {
                InventorySlotButton[0].image.color = new Color(255, 255, 255, 0.1f);
                InventorySlotButton[1].image.color = new Color(255, 255, 255, 0.7f);
                InventorySlotButton[2].image.color = new Color(255, 255, 255, 0.1f);
                Inventory[0].SetActive(false);
                Inventory[1].SetActive(true);
                Inventory[2].SetActive(false);
            }
        }
        if (isSlota == false && isSlotb == false && isSlotc == true)
        {
            if (isInventory == true) //기타
            {
                InventorySlotButton[0].image.color = new Color(255, 255, 255, 0.1f);
                InventorySlotButton[1].image.color = new Color(255, 255, 255, 0.1f);
                InventorySlotButton[2].image.color = new Color(255, 255, 255, 0.7f);
                Inventory[0].SetActive(false);
                Inventory[1].SetActive(false);
                Inventory[2].SetActive(true);
            }
        }
    }

    public void SetSlota()
    {
        isSlota = true;
        isSlotb = false;
        isSlotc = false;
    }
    public void SetSlotb()
    {
        isSlota = false;
        isSlotb = true;
        isSlotc = false;
    }
    public void SetSlotc()
    {
        isSlota = false;
        isSlotb = false;
        isSlotc = true;
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
