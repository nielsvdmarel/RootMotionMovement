using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ActiveTriggeredInteractable;

    public InputAction m_RightHandPickupKey;
    public InputAction m_LeftHandPickupKey;
    public InputAction m_HeadPickupKey;

    [SerializeField]
    bool m_InRangeOfInteractable = false;

    private void OnEnable() {
        m_RightHandPickupKey.Enable();
        m_LeftHandPickupKey.Enable();
        m_HeadPickupKey.Enable();
    }

    private void OnDisable() {
        m_RightHandPickupKey.Disable();
        m_LeftHandPickupKey.Disable();
        m_HeadPickupKey.Disable();
    }

    void Update() {
        if (m_RightHandPickupKey.WasPressedThisFrame()) {
            if (m_ActiveTriggeredInteractable != null) {
                MainItemPickup();
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<InteractAble>()) {
            m_ActiveTriggeredInteractable = other.gameObject;
            Debug.Log("entered trigger for interactable");
            m_InRangeOfInteractable = true;
            Debug.Log(other.gameObject.GetComponent<InteractAble>().GetType());
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject == m_ActiveTriggeredInteractable) {
            m_ActiveTriggeredInteractable = null;
            m_InRangeOfInteractable = false;
           
        }
    }

    void MainItemPickup() {
        m_ActiveTriggeredInteractable.GetComponent<InteractAble>().EquipInteractable(this.gameObject);
    }
}
