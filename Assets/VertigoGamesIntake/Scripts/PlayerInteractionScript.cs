using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ActiveTriggeredInteractable;

    [SerializeField]
    bool m_InRangeOfInteractable = false;
    void Start() {
        
    }
    void Update() {
        
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
}
