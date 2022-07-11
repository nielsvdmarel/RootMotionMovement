using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : InteractAble
{
    [SerializeField]
    GameObject m_FlashLightPL;
    [SerializeField]
    GameObject m_FlashLightSL;
    // Start is called before the first frame update

    [SerializeField]
    bool m_FlashLightEnabled = false;

    void Start() {
        base.Start();
        EnableFlashLight(m_FlashLightEnabled);
        
    }

    // Update is called once per frame
    void Update() {
        base.Update();
    }
    public override void EquipInteractable(GameObject player, GameObject attachObject) {
        base.EquipInteractable(player, attachObject);
        Debug.Log("FlashLight pickedup");
    }

    private void EnableFlashLight(bool enabled) {
        if (m_FlashLightPL != null) {
            m_FlashLightPL.SetActive(enabled);
        }
        if (m_FlashLightSL != null) {
            m_FlashLightSL.SetActive(enabled);
        }
    }

    public override void Interact() {
        base.Interact();
        EnableFlashLight(!m_FlashLightEnabled);
        m_FlashLightEnabled = !m_FlashLightEnabled;
    }
}
