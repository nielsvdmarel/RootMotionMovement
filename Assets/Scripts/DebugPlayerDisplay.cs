using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugPlayerDisplay : MonoBehaviour
{
    private RootMotionMovement m_RootMotionMovement;
    private bool b_PlayerDebugEnabled = false;
   
    public TextMeshProUGUI HeaderText;
    public TextMeshProUGUI DesiredDirectionText;

    [Header("Main player variables")]
    public TextMeshProUGUI DesiredDirectionValue;

    void Start() {
        GameObject player = GameObject.FindWithTag("Player");
        if(player != null) {
            m_RootMotionMovement = player.GetComponent<RootMotionMovement>();
            if(m_RootMotionMovement != null) {
                b_PlayerDebugEnabled = m_RootMotionMovement.b_DebugModeEnabled;
                EnableDebugHUD(b_PlayerDebugEnabled);
            }
        }
    }

    void Update() {
        CheckForDebugUpdate();
        if (b_PlayerDebugEnabled) {
            float DirectionValue = m_RootMotionMovement.m_DesiredDirection;
            DirectionValue = Mathf.RoundToInt(DirectionValue);
            DesiredDirectionValue.text = DirectionValue.ToString();
        }
    }

    void CheckForDebugUpdate() {
        if (b_PlayerDebugEnabled != m_RootMotionMovement.b_DebugModeEnabled) {
            b_PlayerDebugEnabled = m_RootMotionMovement.b_DebugModeEnabled;
            EnableDebugHUD(b_PlayerDebugEnabled);
        }
    }

    void EnableDebugHUD(bool enabled) {
    HeaderText.enabled = enabled;;
    DesiredDirectionText.enabled = enabled; ;
    DesiredDirectionValue.enabled = enabled;
    }
}
