using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasFaceCamera : MonoBehaviour {
    [SerializeField]
    private Camera m_MainCamera;
    void Start() {
        m_MainCamera = Camera.main;
    }

    void FixedUpdate() {
        RotateTowardsCamera();
    }
     
    void RotateTowardsCamera() {
        transform.LookAt(transform.position + m_MainCamera.transform.rotation * Vector3.back, m_MainCamera.transform.rotation * Vector3.up);
    }
}
