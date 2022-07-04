using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : InteractAble
{
    [SerializeField]
    DebugDrawLine m_DebugDrawLine;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        m_DebugDrawLine = GetComponentInChildren<DebugDrawLine>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }

    public override void EquipInteractable(GameObject player, GameObject attachObject) {
        base.EquipInteractable(player, attachObject);

        if(m_PlayerReference != null) {
            PlayerInteractionScript playerInteractionScript = m_PlayerReference.GetComponent<PlayerInteractionScript>();
            //transform.SetParent(playerInteractionScript. m_RightHand.transform, false);
            transform.localPosition = playerInteractionScript.m_GunRightIdleOffset.localPosition;
            transform.localEulerAngles = playerInteractionScript.m_GunRightIdleOffset.localEulerAngles;
            //transform.root.FindChild("dsds").transform.localPosition.x;
        }

        Debug.Log("gun pickedup");
       

        //Gets the player interaction reference. 
    }

    public override void DropInteractable() {
        base.DropInteractable();
    }



    void EnableAim() {
        m_PlayerReference.GetComponent<HeadController>().m_IkActive = false;
        m_PlayerReference.GetComponent<WeaponIK>().m_WeaponIKEnabled = true;
    }
}
