using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoClip : InteractAble 
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
        transform.localPosition = Vector3.zero;
        Debug.Log("Ammo clip pickedup");
        //Gets the player interaction reference. 
    }

    public override void DropInteractable() {
        base.DropInteractable();
    }
}
