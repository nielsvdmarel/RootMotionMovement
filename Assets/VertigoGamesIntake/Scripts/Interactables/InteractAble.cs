using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interactable requires the GameObject to have a Collider component
[RequireComponent(typeof(Collider))]
public class InteractAble : MonoBehaviour {
    
    public Collider m_ObjectCollider;
    public Rigidbody m_Rigidbody;

    [SerializeField]
    private float m_PickupRange;

    protected void Start() {
        m_ObjectCollider = this.GetComponent<SphereCollider>();
        m_Rigidbody = this.GetComponent<Rigidbody>();
       
    }

    protected void Update() {
        
    }
    public void EquipInteractable(GameObject parent) {
        m_Rigidbody.useGravity = false;
        m_Rigidbody.isKinematic = true;
        transform.SetParent(parent.transform, true);
        transform.position = parent.transform.position;
        transform.rotation = parent.transform.rotation;
        foreach (var colider in GetComponentsInChildren<BoxCollider>()) {
            colider.enabled = false;
        }    
    }

    //responsible for picking up items.
    //responsible for checking if in range.
    //responsible for picking up with specific hand (using input either Q or E).
    //responsible for interact function.
}
