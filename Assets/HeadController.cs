using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    [Header("Main references")]
    protected Animator m_Animator;
    [SerializeField]
    CamerAngleCalculator m_CamAngleCalc;

    [Header("Animation variables")]
    [SerializeField]
    private bool m_IkActive;
    [SerializeField]
    private float m_lookWeight = 2f;
    [SerializeField]
    private float BlendLerpSpeed = 1.0f;

    [Header("Target variables")]
    [SerializeField]
    private Transform m_LookObject = null;
    [SerializeField]
    private float m_LookWeightTarget;
    [SerializeField]
    private float m_MaxWeightValue = 1;
    [SerializeField]
    private float m_MinWeightValue = 0;

    [Header("camera extra")]
    [SerializeField]
    private bool LookWhileIdle = false;
    private bool m_CanLook = false;

    
    private void Start()
    {
        m_Animator = this.GetComponent<Animator>();
        m_CamAngleCalc = this.GetComponent<CamerAngleCalculator>();
    }

    public void Update() {
        if (LookWhileIdle)
        {
            if (m_CamAngleCalc.m_Input2D.x == 0 && m_CamAngleCalc.m_Input2D.y == 0)
            {
                m_CanLook = true;
            }
            else {
                m_CanLook = false;
            }
        }

        m_lookWeight = Mathf.Lerp(m_lookWeight, m_LookWeightTarget, BlendLerpSpeed * Time.deltaTime);
        if(m_lookWeight <=  m_MinWeightValue + 0.01f)
        {
            m_lookWeight = m_MinWeightValue;
        }
        if (m_lookWeight >= m_MaxWeightValue - 0.01f)
        {
            m_lookWeight = m_MaxWeightValue;
        }

        
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (m_Animator)
        {
            if (m_IkActive)
            {
                m_Animator.SetLookAtWeight(m_lookWeight);
                if (m_LookObject != null)
                {
                    if (m_CanLook && LookWhileIdle)
                    {
                        m_LookWeightTarget = m_MaxWeightValue;
                        m_Animator.SetLookAtPosition(m_LookObject.position);
                        return;
                    }
                }
            }
          m_LookWeightTarget = m_MinWeightValue;
        }
    }
}
