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
    private float m_PickupRange;

    public bool m_Equiped;

    protected void Start() {
        m_ObjectCollider = this.GetComponent<SphereCollider>();
        m_Rigidbody = this.GetComponent<Rigidbody>();
        m_Equiped = false;
       
    }

    protected void Update() {
        
    }
    public virtual void EquipInteractable(GameObject player, GameObject attachObject) {
        Debug.Log("normal pickedup");
        m_Equiped = true;
        m_PlayerReference = player.transform.root.gameObject;
        transform.SetParent(attachObject.transform, true);
        m_Rigidbody.useGravity = false;
        m_Rigidbody.isKinematic = true;

        //transform.SetParent(anchorPoint.transform, true);
        //transform.position = anchorPoint.transform.position;
        //transform.rotation = anchorPoint.transform.rotation;
        foreach (var colider in GetComponentsInChildren<BoxCollider>()) {
            colider.enabled = false;
        }
        foreach (var colider in GetComponentsInChildren<SphereCollider>())
        {
            colider.enabled = false;
        }
    }

    public virtual void DropInteractable() {
        m_Equiped = false;
        m_PlayerReference = null;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.isKinematic = false;
        this.transform.SetParent(null);

        foreach (var colider in GetComponentsInChildren<BoxCollider>())
        {
            colider.enabled = true;
        }
        foreach (var colider in GetComponentsInChildren<SphereCollider>())
        {
            colider.enabled = true;
        }


    }

    //responsible for picking up items.
    //responsible for checking if in range.
    //responsible for picking up with specific hand (using input either Q or E).
    //responsible for interact function.
}
