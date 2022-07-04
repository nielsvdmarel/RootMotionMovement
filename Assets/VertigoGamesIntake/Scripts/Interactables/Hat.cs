using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hat : InteractAble
{
    // Start is called before the first frame update
    void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        base.Update();
    }
    public override void EquipInteractable(GameObject player, GameObject attachObject) {
        base.EquipInteractable(player, attachObject);
        if (m_PlayerReference != null) {
            PlayerInteractionScript playerInteractionScript = m_PlayerReference.GetComponent<PlayerInteractionScript>();
            transform.localPosition = playerInteractionScript.m_hatOffset.localPosition;
            transform.localEulerAngles = playerInteractionScript.m_hatOffset.localEulerAngles;
        }
    }
}
