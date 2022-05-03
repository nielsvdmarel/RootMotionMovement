using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EscapeQuit : MonoBehaviour {
    public InputAction m_QuitInput;
    void Start() {
        m_QuitInput.Enable();
    }

    private void OnDisable()
    {
        m_QuitInput.Disable();
    }

    void Update() {
        if (m_QuitInput.IsPressed()) {
            Application.Quit();
        }
    }
}
