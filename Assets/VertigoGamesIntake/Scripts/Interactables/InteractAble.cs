using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interactable requires the GameObject to have a Collider component
[RequireComponent(typeof(Collider))]
public class InteractAble : MonoBehaviour {
    
    public Collider ObjectCollider;

    [SerializeField]
    private float m_PickupRange;
    void Start() {
        
    }

    void Update() {
        
    }

    //responsible for picking up items.
    //responsible for checking if in range.
    //responsible for picking up with specific hand (using input either Q or E).
    //responsible for interact function.
}
