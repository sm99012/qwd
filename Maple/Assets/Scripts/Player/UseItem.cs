using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseItem : MonoBehaviour
{
    public PlayerStatus PlayerStatus;
    public GetItem GetItem;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStatus = this.gameObject.GetComponent<PlayerStatus>();
        GetItem = this.gameObject.GetComponent<GetItem>();
        UseItemb();
    }

    void UseItemb()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            for (int i = 0; i < GetItem.Slotb.Length; i++)
            {
                if (GetItem.Slotb_Count[i] > 0)
                {
                    Item item = GetItem.Slotb[i].GetComponent<Item>();
                    item.ItembUse();
                    GetItem.Slotb_Count[i]--;
                    break;
                }
            }
        }
    }
}
