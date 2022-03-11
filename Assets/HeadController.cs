using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadController : MonoBehaviour
{
    protected Animator m_Animator;
    [SerializeField]
    private bool m_IkActive;
    [SerializeField]
    private Transform m_LookObject = null;
    [SerializeField]
    private float m_lookWeight = 2f;

    [SerializeField]
    CamerAngleCalculator m_CamAngleCalc;
    private void Start()
    {
        m_Animator = this.GetComponent<Animator>();
        m_CamAngleCalc = this.GetComponent<CamerAngleCalculator>();
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (m_Animator)
        {
            if (m_IkActive)
            {
                if (m_LookObject != null)
                {
                    m_Animator.SetLookAtWeight(m_lookWeight);
                    m_Animator.SetLookAtPosition(m_LookObject.position);

                }
            }
            else
            {
                m_Animator.SetLookAtWeight(0);
            }
        }
    }
}
