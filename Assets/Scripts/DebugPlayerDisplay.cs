using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

public class DebugPlayerDisplay : MonoBehaviour
{
    private RootMotionMovement m_RootMotionMovement;
    private bool b_PlayerDebugEnabled = false;
   
    public TextMeshProUGUI HeaderText;
    public TextMeshProUGUI DesiredDirectionText;

    [Header("Main player variables")]
    public TextMeshProUGUI DesiredDirectionValue;

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

    void Start() {
        m_RootMotionMovement = this.GetComponent<RootMotionMovement>();
        if (m_RootMotionMovement != null) {
            b_PlayerDebugEnabled = m_RootMotionMovement.b_DebugModeEnabled;
            EnableDebugElements(b_PlayerDebugEnabled);
        }
    }

    void Update() {
        CheckForDebugUpdate();
        if (b_PlayerDebugEnabled) {
            float DirectionValue = m_RootMotionMovement.m_DesiredDirection;
            DirectionValue = Mathf.RoundToInt(DirectionValue);
            DesiredDirectionValue.text = DirectionValue.ToString();
            //Arrows
            UpdateDirectionalArrow();

            //Arc
        }
    }

    void CheckForDebugUpdate() {
        if (b_PlayerDebugEnabled != m_RootMotionMovement.b_DebugModeEnabled) {
            b_PlayerDebugEnabled = m_RootMotionMovement.b_DebugModeEnabled;
            EnableDebugElements(b_PlayerDebugEnabled);
        }
    }

    void EnableDebugElements(bool enabled) {
        //HUD
        HeaderText.enabled = enabled;
        DesiredDirectionText.enabled = enabled;
        DesiredDirectionValue.enabled = enabled;

        //Objects
        ForwardArrow.SetActive(enabled);
        m_DirectionalArrow.SetActive(enabled);
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
