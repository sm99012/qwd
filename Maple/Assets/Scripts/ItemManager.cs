using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    [TextArea]
    public string Discription;
    public PlayerRespawn PlayerRespawn;
    public GameObject Item;
    Item ItemScript;

    void Start()
    {
        ItemScript = Item.GetComponent<Item>();
        this.Discription = ItemScript.ItemDescription;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject obj_Player = PlayerRespawn.obj_Respawn;
        if (obj_Player)
        {
            ItemScript.PlayerRespawn = this.PlayerRespawn;
        }
    }
}
