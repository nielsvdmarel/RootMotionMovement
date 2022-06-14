using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ActiveTriggeredInteractable;

    [SerializeField]
    private GameObject m_HoldingInteractable;

    public InputAction m_RightHandPickupKey;
    public InputAction m_LeftHandPickupKey;
    public InputAction m_HeadPickupKey;

    [SerializeField]
    private Vector3 m_WeaponTransformOffsetPositionR;
    [SerializeField]
    private Vector3 m_WeaponTransformOffsetRotationR;

    [SerializeField]
    private Transform m_GunnOffset;

    [SerializeField]
    private GameObject m_LeftHand;
    [SerializeField]
    private GameObject m_RightHand;
    [SerializeField]
    private GameObject m_Head;

    public MeshSockets sockets;

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

    private void Start() {
        sockets = this.GetComponent<MeshSockets>();
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
        m_HoldingInteractable = m_ActiveTriggeredInteractable;
        m_HoldingInteractable.transform.SetParent(m_RightHand.transform, false);
        m_HoldingInteractable.transform.localPosition = m_GunnOffset.localPosition;
        m_HoldingInteractable.transform.localEulerAngles = m_GunnOffset.localEulerAngles;
        this.GetComponent<HeadController>().m_IkActive = false;
        //m_HoldingInteractable.transform.localPosition = m_WeaponTransformOffsetPositionR;
        //m_HoldingInteractable.transform.localEulerAngles = m_WeaponTransformOffsetRotationR;
        this.GetComponent<WeaponIK>().m_WeaponIKEnabled = true;
        //Sockets logic
        //sockets.Attach(m_HoldingInteractable.transform, MeshSockets.SocketId.Spine);
        //sockets.Attach(m_HoldingInteractable.transform, MeshSockets.SocketId.RightHand);
    }

    void DropItem() {
        m_HoldingInteractable.transform.SetParent(null);
        //Other stuff such as rigidbody etc
        //m_HoldingInteractable.gameObject.
    }
}
