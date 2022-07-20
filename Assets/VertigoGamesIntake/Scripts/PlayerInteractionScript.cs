using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    [Header("Interaction UI elements")]
    [SerializeField]
    RectTransform m_LeftHandInteractionRect;
    [SerializeField]
    RectTransform m_RightHandInteractionRect;
    [SerializeField]
    RectTransform m_HeadInteractionRect;
    
    [Header("Interaction variables")]
    [SerializeField]
    private GameObject m_ActiveTriggeredInteractable;

    [SerializeField]
    private GameObject m_LeftHandInteractable;
    [SerializeField]
    private GameObject m_RightHandInteractable;
    [SerializeField]
    private GameObject m_HeadInteractable;

    [Header("Pikcup Input actions")]

    public InputAction m_RightHandPickupKey;
    public InputAction m_LeftHandPickupKey;
    public InputAction m_HeadPickupKey;
    public InputAction m_LeftMouseButtonKey;
    public InputAction m_RightMouseButtonKey;

    [Header("Interactable placement objects")]
    public GameObject m_LeftHand;
    public GameObject m_RightHand;
    public GameObject m_Head;

    [Header("Booleans indicating pickupable locations")]
    [SerializeField]
    private bool m_CanPickupWithLHand;
    [SerializeField]
    private bool m_CanPickupWithRHand;
    [SerializeField]
    private bool m_CanPickupWithHead;

    [SerializeField]
    bool m_InRangeOfInteractable = false;

    //Needed set active pickup variables, used when animation event is triggered
    private GameObject m_ToBePickedUpInteractable = null;
    private GameObject m_ToBeUsedAttachObject = null;

    private bool m_IsAiming = false;

    private enum InteractableCombos {
        NoCombo,
        WeaponAmmoClip,
        AmmoClipBullets
    }

    private void OnEnable() {
        m_RightHandPickupKey.Enable();
        m_LeftHandPickupKey.Enable();
        m_HeadPickupKey.Enable();
        m_LeftMouseButtonKey.Enable();
        m_RightMouseButtonKey.Enable();
    }

    private void OnDisable() {
        m_RightHandPickupKey.Disable();
        m_LeftHandPickupKey.Disable();
        m_HeadPickupKey.Disable();
        m_LeftMouseButtonKey.Disable();
        m_RightMouseButtonKey.Disable();
    }

    private void Awake() {
        //Resetting the status of the pikcup availability indicators
        UpdatePikcupStatus(null);
    }

    private void Start() {
     
    }

    void Update() {

        //Left hand pickup logic
        if (m_LeftHandPickupKey.WasPressedThisFrame()) {
            CheckPickUpDrop(ref m_LeftHandInteractable, m_LeftHand, m_CanPickupWithLHand);
        }

        //Right hand pickup logic
        if (m_RightHandPickupKey.WasPressedThisFrame()) {
            CheckPickUpDrop(ref m_RightHandInteractable, m_RightHand, m_CanPickupWithRHand);
        }

        //Head pikcup logic
        if (m_HeadPickupKey.WasPressedThisFrame()) {
            CheckPickUpDrop(ref m_HeadInteractable, m_Head, m_CanPickupWithHead);
        }

        if (m_LeftMouseButtonKey.WasPressedThisFrame()) {
            //Interact with active interactables
            CheckInteraction();
        }

        if (m_RightMouseButtonKey.WasPressedThisFrame()) {
            //Possibly pre interact for active interacbles

        }
        UpdateUIElementsPosition();
    }

    void CheckPickUpDrop(ref GameObject activeInteractableInSlot, GameObject attachObject, bool canPikcup) {
        //first we check the logic for dropping Interactables
        if (activeInteractableInSlot != null) {
            DropInteractable(activeInteractableInSlot.gameObject);
            activeInteractableInSlot = null;
            return;
        }
        //If we don't have a active holding interactable, we check if we want/can pickup.
        //We first check if the active interactable has triggered the left hand availability to pick something up
        if (canPikcup) {
            //If we have a triggered interactable in range
            if (m_ActiveTriggeredInteractable != null) {
                if (!m_ActiveTriggeredInteractable.GetComponent<InteractAble>().m_Equiped) {
                    //Pickup item
                    PickupInteractable(m_ActiveTriggeredInteractable, attachObject);
                    activeInteractableInSlot = m_ActiveTriggeredInteractable;
                }
            }
        }
    }

    void PickupInteractable(GameObject interactAble, GameObject attachObject) {
        if(attachObject == m_Head) {
            this.GetComponent<Animator>().SetTrigger("LeftHandPickup");
        }
        else if(attachObject == m_LeftHand) {
            this.GetComponent<Animator>().SetTrigger("LeftHandPickup");
        }

        else if (attachObject == m_RightHand) {
            this.GetComponent<Animator>().SetTrigger("RightHandPickup");
        }


        m_ToBePickedUpInteractable = interactAble;
        m_ToBeUsedAttachObject = attachObject;
    }

    void DropInteractable(GameObject interactAble) {
        interactAble.GetComponent<InteractAble>().DropInteractable();
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.GetComponent<InteractAble>()) {
            m_ActiveTriggeredInteractable = other.gameObject;
            Debug.Log("entered trigger for interactable");
            m_InRangeOfInteractable = true;
            //Debug.Log(other.gameObject.GetComponent<InteractAble>().GetType());
            UpdatePikcupStatus(other.gameObject.GetComponent<InteractAble>());
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.gameObject == m_ActiveTriggeredInteractable) {
            m_ActiveTriggeredInteractable = null;
            m_InRangeOfInteractable = false;
            UpdatePikcupStatus(null);
        }
    }

    void UpdateUIElementsPosition() {
        m_LeftHandInteractionRect.position = Camera.main.WorldToScreenPoint(m_LeftHand.transform.position);
        m_RightHandInteractionRect.position = Camera.main.WorldToScreenPoint(m_RightHand.transform.position);
        m_HeadInteractionRect.position = Camera.main.WorldToScreenPoint(m_Head.transform.position);
    }

    void CheckInteraction() {
       InteractAble[] allActiveInteractables = new InteractAble[] {null, null, null };

        //Checking and setting temp activatable interactables
        if(m_LeftHandInteractable != null) {
            allActiveInteractables[0] = m_LeftHandInteractable.GetComponent<InteractAble>();
            //m_LeftHandInteractable.GetComponent<InteractAble>().Interact();
        }

        if(m_RightHandInteractable != null) {
            allActiveInteractables[1] = m_RightHandInteractable.GetComponent<InteractAble>();
           // m_RightHandInteractable.GetComponent<InteractAble>().Interact();
        }

        if(m_HeadInteractable != null) {
            allActiveInteractables[2] = m_HeadInteractable.GetComponent<InteractAble>();
           // m_HeadInteractable.GetComponent<InteractAble>().Interact();
        }

        //Doing all solo interactings, without limitations (flaslight)
        for (int i = 0; i < allActiveInteractables.Length; i++) {
            if(allActiveInteractables[i] != null) {
                allActiveInteractables[i].Interact();
            } 
        }

        //Shared functionality:
        if(allActiveInteractables[0] != null && allActiveInteractables[1] != null) {
            if(CheckInteractablePairs(ref allActiveInteractables[0], ref allActiveInteractables[1]) == (int)InteractableCombos.WeaponAmmoClip) {

            }
        }

        //at least 1 hand is empty.
        if(m_LeftHandInteractable == null || m_RightHandInteractable == null) {
            if(m_LeftHandInteractable != null) {
                //Rock throwing right hand

                //Gun aiming start right hand

                // 
            }
            if(m_RightHandInteractable != null)
            {

            }
        } 
    }

    int CheckInteractablePairs(ref InteractAble lefthandInt, ref InteractAble rightHand) {
        if (lefthandInt.GetType().ToString() == "Gun" || rightHand.GetType().ToString() == "Gun") {
            if (lefthandInt.GetType().ToString() == "AmmoClip" || rightHand.GetType().ToString() == "AmmoClip")
            {
                Debug.Log(" combo found");
                return (int)InteractableCombos.WeaponAmmoClip;
            }
        }
        return 0;
    }

    void CheckPreInteraction() {

    }

    /// <summary>
    /// logic for actually pickuping up the requested interactable after pikcup animation point has been reached 
    /// </summary>
    void AnimPickupItem() {
        m_ToBePickedUpInteractable.GetComponent<InteractAble>().EquipInteractable(this.gameObject, m_ToBeUsedAttachObject);
    }

    /// <summary>
    /// Function to set the active state of the pickupable ckeck booleans and update the UI
    /// </summary>
    /// <param name="activeInRangeInteractable"></param>
    void UpdatePikcupStatus(InteractAble activeInRangeInteractable) {
        if(activeInRangeInteractable == null) {
            m_RightHandInteractionRect.gameObject.SetActive(false);
            m_LeftHandInteractionRect.gameObject.SetActive(false);
            m_HeadInteractionRect.gameObject.SetActive(false);

            m_CanPickupWithHead = false;
            m_CanPickupWithLHand = false;
            m_CanPickupWithRHand = false;
            return;
        }

        switch (activeInRangeInteractable.GetType().ToString()) {
            case "Gun": {
                    Debug.Log("gun found");
                    m_RightHandInteractionRect.gameObject.SetActive(true);
                    m_LeftHandInteractionRect.gameObject.SetActive(true);
                    m_HeadInteractionRect.gameObject.SetActive(false);

                    m_CanPickupWithHead = false;
                    m_CanPickupWithLHand = true;
                    m_CanPickupWithRHand = true;
                }
                break;
            case "AmmoClip": {
                    Debug.Log("ammo found");
                    m_RightHandInteractionRect.gameObject.SetActive(true);
                    m_LeftHandInteractionRect.gameObject.SetActive(true);
                    m_HeadInteractionRect.gameObject.SetActive(false);

                    m_CanPickupWithHead = false;
                    m_CanPickupWithLHand = true;
                    m_CanPickupWithRHand = true;
                }
                break;
            case "Hat": {
                    Debug.Log("Hat found");
                    m_RightHandInteractionRect.gameObject.SetActive(false);
                    m_LeftHandInteractionRect.gameObject.SetActive(false);
                    m_HeadInteractionRect.gameObject.SetActive(true);

                    m_CanPickupWithHead = true;
                    m_CanPickupWithLHand = false;
                    m_CanPickupWithRHand = false;
                }
                break;
            case "Rock":
                {
                    Debug.Log("Rock found");
                    m_RightHandInteractionRect.gameObject.SetActive(true);
                    m_LeftHandInteractionRect.gameObject.SetActive(true);
                    m_HeadInteractionRect.gameObject.SetActive(false);

                    m_CanPickupWithHead = false;
                    m_CanPickupWithLHand = true;
                    m_CanPickupWithRHand = true;
                }
                break;
            case "FlashLight":
                {
                    Debug.Log("FlashLight found");
                    m_RightHandInteractionRect.gameObject.SetActive(true);
                    m_LeftHandInteractionRect.gameObject.SetActive(true);
                    m_HeadInteractionRect.gameObject.SetActive(false);

                    m_CanPickupWithHead = false;
                    m_CanPickupWithLHand = true;
                    m_CanPickupWithRHand = true;
                }
                break;
            default:
                Debug.Log("nothing found");
                break;
        }
    }
}
