using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interactable requires the GameObject to have a Collider component
[RequireComponent(typeof(Collider))]
public class InteractAble : MonoBehaviour {
    
    public Collider m_ObjectCollider;
    public Rigidbody m_Rigidbody;

    protected GameObject m_PlayerReference;

    [SerializeField]
    protected string m_AttachmentPointName = null;
    private GameObject m_AttachmentObject;

    [SerializeField]
    private float m_PickupRange;

    private float m_TriggerRadius;

    public bool m_Equiped;

    protected void Start() {
        m_ObjectCollider = this.GetComponent<SphereCollider>();
        m_Rigidbody = this.GetComponent<Rigidbody>();
        m_Equiped = false;

        m_TriggerRadius = this.GetComponent<SphereCollider>().radius;
       
    }

    protected void Update() {
        
    }
    public virtual void EquipInteractable(GameObject player, GameObject attachObject) {
       
        //Setting references and variables
        m_Equiped = true;
        m_PlayerReference = player.transform.root.gameObject;
        m_AttachmentObject = attachObject;

        //Disabling ridgidbody elements
        m_Rigidbody.useGravity = false;
        m_Rigidbody.isKinematic = true;

        //Parenting
        transform.SetParent(attachObject.transform, true);
        this.GetComponent<SphereCollider>().radius = 0;

        if (!string.IsNullOrEmpty(m_AttachmentPointName)) {
            //Positioning with offset name
            Debug.Log("attachment offset name found, positioning correctly");
            transform.localPosition = attachObject.transform.Find(m_AttachmentPointName).localPosition;
            transform.localEulerAngles = attachObject.transform.Find(m_AttachmentPointName).localEulerAngles;

            //transform.localPosition = playerInteractionScript.m_GunRightIdleOffset.localPosition;
            //transform.localEulerAngles = playerInteractionScript.m_GunRightIdleOffset.localEulerAngles;
        }
        else {
            //Positioning without offset specified
            Debug.Log("positioning without specified offset");
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
        }

        StartCoroutine(DisableTriggerAfterFrame());

        //Disabling colliders
        foreach (var colider in GetComponentsInChildren<BoxCollider>()) {
            colider.enabled = false;
        }

        /*
        if (m_PlayerReference != null) {
            PlayerInteractionScript playerInteractionScript = m_PlayerReference.GetComponent<PlayerInteractionScript>();
        }
        */
        Debug.Log("base pickedup");
    }

    public virtual void DropInteractable() {
        //Reseting references and variables
        m_Equiped = false;
        m_PlayerReference = null;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.isKinematic = false;
        this.transform.SetParent(null);
        m_AttachmentObject = null;
        this.GetComponent<SphereCollider>().radius = m_TriggerRadius;
        //Enabling colliders
        foreach (var colider in GetComponentsInChildren<BoxCollider>()) {
            colider.enabled = true;
        }
        foreach (var colider in GetComponentsInChildren<SphereCollider>()) {
            colider.enabled = true;
        }
    }

    public virtual void Interact() {

    }

    public virtual void PreInteract() {

    }

    IEnumerator DisableTriggerAfterFrame() {
        yield return 0;

        //code goes here
        foreach (var colider in GetComponentsInChildren<SphereCollider>()) {
            colider.enabled = false;
        }

    }
}
