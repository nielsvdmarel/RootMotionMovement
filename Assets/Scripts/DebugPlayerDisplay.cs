using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEditor;
using TMPro;

public class DebugPlayerDisplay : MonoBehaviour
{
    [Header("Input")]
    [SerializeField]
    public InputAction DebugControl;

    [Header("Debug system Enabled")] 
    public bool m_Enabled;

    private RootMotionMovement m_RootMotionMovement;
    private bool b_PlayerDebugEnabled = false;
    private List<GameObject> m_AllDebugUIElements;

    [Header("Main player texts")]
    public TextMeshProUGUI HeaderText;
    public TextMeshProUGUI DesiredDirectionText;
    public TextMeshProUGUI m_PlayerIsGroundedText;
    public TextMeshProUGUI m_PlayerRaycastIsGroundedText;
    public TextMeshProUGUI m_PlayerFinalGroundedText;

    [Header("Main player variables")]
    public TextMeshProUGUI DesiredDirectionValue;
    public TextMeshProUGUI m_PlayerIsGroundedValue;
    public TextMeshProUGUI m_PlayerRaycastIsGroundedValue;
    public TextMeshProUGUI m_PlayerFinalGroundedValue;

    [Header("Player indicator arrows")]
    [SerializeField]
    private GameObject ForwardArrow;
    [SerializeField]
    private GameObject m_DirectionalArrow;

    [SerializeField]
    private float testRotation;

    public class WireArcExample : MonoBehaviour
    {
        public float shieldArea;
    }

    private void OnEnable() {
        DebugControl.Enable();
    }

    private void OnDisable() {
        DebugControl.Disable();
    }

    void Start() {
        m_AllDebugUIElements = new List<GameObject>();

        //Add all elements for easy enabling/disabling.
        m_AllDebugUIElements.Add(HeaderText.gameObject);
        m_AllDebugUIElements.Add(DesiredDirectionText.gameObject);
        m_AllDebugUIElements.Add(m_PlayerIsGroundedText.gameObject);
        m_AllDebugUIElements.Add(DesiredDirectionValue.gameObject);
        m_AllDebugUIElements.Add(m_PlayerIsGroundedValue.gameObject);
        m_AllDebugUIElements.Add(ForwardArrow.gameObject);
        m_AllDebugUIElements.Add(m_DirectionalArrow.gameObject);
        m_AllDebugUIElements.Add(m_PlayerRaycastIsGroundedText.gameObject);
        m_AllDebugUIElements.Add(m_PlayerRaycastIsGroundedValue.gameObject);
        m_AllDebugUIElements.Add(m_PlayerFinalGroundedText.gameObject);
        m_AllDebugUIElements.Add(m_PlayerFinalGroundedValue.gameObject);

        m_RootMotionMovement = this.GetComponent<RootMotionMovement>();
        if (m_RootMotionMovement != null) {
            b_PlayerDebugEnabled = m_RootMotionMovement.b_DebugModeEnabled;
            EnableDebugElements(b_PlayerDebugEnabled);
        }
    }

    void Update() {
        CheckForDebugUpdate();
        if (b_PlayerDebugEnabled) {
            //Directional value update:
            float DirectionValue = m_RootMotionMovement.m_DesiredDirection;
            DirectionValue = Mathf.RoundToInt(DirectionValue);
            DesiredDirectionValue.text = DirectionValue.ToString();

            // IsGrounded Update
            m_PlayerIsGroundedValue.text = m_RootMotionMovement.m_IsGrounded.ToString();
            SetTextColorOnBoolean(m_PlayerIsGroundedValue, m_RootMotionMovement.m_IsGrounded);

            m_PlayerRaycastIsGroundedValue.text = m_RootMotionMovement.m_RayCastGrounded.ToString();
            SetTextColorOnBoolean(m_PlayerRaycastIsGroundedValue, m_RootMotionMovement.m_RayCastGrounded);

            m_PlayerFinalGroundedValue.text = m_RootMotionMovement.m_FinalGrounded.ToString();
            SetTextColorOnBoolean(m_PlayerFinalGroundedValue, m_RootMotionMovement.m_FinalGrounded);

            //Arrows
            UpdateDirectionalArrow();
        }
    }

    void CheckForDebugUpdate() {
        if (b_PlayerDebugEnabled != m_RootMotionMovement.b_DebugModeEnabled) {
            b_PlayerDebugEnabled = m_RootMotionMovement.b_DebugModeEnabled;
            EnableDebugElements(b_PlayerDebugEnabled);
        }
    }

    void EnableDebugElements(bool enabled) {
        foreach (var item in m_AllDebugUIElements) {
            item.SetActive(enabled);
        }
    }

    /// <summary>
    /// Function for setting specific texts color to either red or green depending on boolean value
    /// </summary>
    /// <param name="text"> Text element to change color </param>
    /// <param name="value"> Boolean value to use</param>
    void SetTextColorOnBoolean(TextMeshProUGUI text, bool value) {
        text.color = value ? Color.green : Color.red;
    }

    void UpdateDirectionalArrow() {
        m_DirectionalArrow.transform.rotation = transform.rotation * Quaternion.Euler(90,90 + m_RootMotionMovement.m_DesiredDirection, 0);

        Vector3 directionalArrowPos = transform.position;
        directionalArrowPos.y += 0.355f;
        directionalArrowPos += -m_DirectionalArrow.transform.right * 0.6f;
        m_DirectionalArrow.transform.position = directionalArrowPos;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (Application.isPlaying) {
            if (b_PlayerDebugEnabled) {
                Handles.color = Color.white;
                Vector3 directionalArrowPos = transform.position;
                directionalArrowPos.y += 0.355f;
                Handles.DrawWireArc(directionalArrowPos, transform.up, transform.forward, m_RootMotionMovement.m_DesiredDirection, 1.5f, 10);
            }
        }
    }
#endif
}
