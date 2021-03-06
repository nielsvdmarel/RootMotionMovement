using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamerAngleCalculator : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    [SerializeField]
    Camera m_Camera;

    [SerializeField]
    Animator m_Animator;

    [SerializeField]
    public InputAction playerControls;

    [SerializeField]
    public InputAction isRunning;

    [Header("Main Root motion variables")]

    [SerializeField]
    public float m_DesiredDirection;

    [SerializeField]
    public int m_NormalizedRotationDirection;

    [SerializeField]
    private float FixedDesiredDirection;
    [SerializeField]
    bool canUpdateDirection = true;

    [SerializeField]
    private float m_PlayerSpeed;

    [SerializeField]
    private float m_SmoothPlayerSpeed;

    [SerializeField]
    private float m_LerpSpeed;

    [SerializeField]
    private float m_LastRecordedSpeed;

    [SerializeField]
    bool m_PlayerRunning = false;

    [Header("extra input")]

    public Vector2 m_Input2D;

    [SerializeField]
    Vector3 m_Input;

    [Header("camera extra")]

    [SerializeField]
    public bool AnimEnabledCustomRotation = false;
    [SerializeField]
    public bool m_EnableCustomRotation = false;

    [Header("updated")]
    public Vector3 m_DesiredManualRotation;

    public float m_RotationSpeed;

    [SerializeField]
    private Transform head;

    [Header("Turn In place")]
    [SerializeField]
    private float m_IdleRotation;
    [SerializeField]
    private bool m_RotationSet;
    [SerializeField]
    private float m_IdleRotationDistance;

    [Header("Idle Turning")]
    [SerializeField]
    private bool m_InPlaceTurnsEnabled;

    private void OnEnable()
    {
        playerControls.Enable();
        isRunning.Enable();
        
    }
    private void OnDisable()
    {
        playerControls.Disable();
        isRunning.Disable();
    }

    void Start()
    {
        Player = this.gameObject;
        m_Camera = Camera.main;
        m_Animator = this.GetComponent<Animator>();
        m_PlayerSpeed = 0;
        m_SmoothPlayerSpeed = 0;

        //Time.timeScale = 0.1f;
    }

    void Update()
    {

        CalculateDesiredAngle();
        //transform.Rotate(0,0.4f,0);
        CalculateMovementSpeed();
        ManualPlayerRotation();
        CalCulateDifferenceFromIdle();
        HandleIdleTurns();
        m_SmoothPlayerSpeed = Mathf.Lerp(m_SmoothPlayerSpeed, m_PlayerSpeed, m_LerpSpeed * Time.deltaTime);

        UpdateAnimator();

        if(isRunning.ReadValue<float>() == 1) {
            m_PlayerRunning = true;
        }
        else {
            m_PlayerRunning = false;
        }

        if (canUpdateDirection)
        {
            if(m_Input2D.x != 0 || m_Input2D.y != 0)
            {
                canUpdateDirection = false;
                FixedDesiredDirection = m_DesiredDirection;
                m_Animator.SetFloat("Direction", m_DesiredDirection);
            }
        }
        else
        {
            if(m_Input2D.x == 0 && m_Input2D.y == 0)
            {
                canUpdateDirection = true;
            }
        }
    }

    private void LateUpdate()
    {
       
    }
    private static float WrapAngle(float angle)
    {
        angle %= 360;
        if (angle > 180)
            return angle - 360;
        else if (angle < -180)
            return angle + 360;
        return angle;
    }

    private void CalculateDesiredAngle() {
        Vector3 delta;
        delta = m_Camera.transform.rotation.eulerAngles.normalized - Player.transform.rotation.eulerAngles.normalized;
        m_Input2D = playerControls.ReadValue<Vector2>();
        m_Input = new Vector3(m_Input2D.y, m_Input2D.x, 0);

        float target_rotation_ = Mathf.Atan2(m_Input2D.x, m_Input2D.y) * Mathf.Rad2Deg; // + mainCamera.transform.eulerAngles.y;
        Vector3 InputTargetRotation = new Vector3(0, target_rotation_, 0);

        float rotationX;
        float rotationY;
        float rotationZ;

        rotationX = m_Camera.transform.eulerAngles.x;
        rotationY = m_Camera.transform.eulerAngles.y;
        rotationZ = m_Camera.transform.eulerAngles.z;

        Vector3 trueCamRotation = new Vector3(WrapAngle(rotationX), WrapAngle(rotationY), WrapAngle(rotationZ));
        trueCamRotation.x *= -1;

        //Vector3 truePlayerRotation = transform.eulerAngles;
        Vector3 truePlayerRotation = new Vector3(WrapAngle(transform.eulerAngles.x), WrapAngle(transform.eulerAngles.y), WrapAngle(transform.eulerAngles.z));

        Vector3 test = truePlayerRotation - trueCamRotation;
        Vector3 CamAndPlayer;
        CamAndPlayer.x = WrapAngle(test.x);
        CamAndPlayer.y = WrapAngle(test.y);
        CamAndPlayer.z = WrapAngle(test.z);

        Vector3 InputCamPlayer = InputTargetRotation - CamAndPlayer;
        Vector3 FinalDelta;
        FinalDelta.x = WrapAngle(InputCamPlayer.x);
        FinalDelta.y = WrapAngle(InputCamPlayer.y);
        FinalDelta.z = WrapAngle(InputCamPlayer.z);

        m_DesiredDirection = FinalDelta.y;

        //Calculate normalized direction rotation, used for idle rotation direction
        if(m_DesiredDirection > 0) {
            m_NormalizedRotationDirection = 1;
        }else if(m_DesiredDirection < 0) {
            m_NormalizedRotationDirection = -1;
        }
        else if(m_DesiredDirection == 0) {
            m_NormalizedRotationDirection = 0;
        }
    }

    public void ManualPlayerRotation() {
        if (AnimEnabledCustomRotation)
        {
            if (m_EnableCustomRotation)
            {
                //Can be done in the same calculaton as setting the m_DesiredMoveDirection
                Vector3 forward = m_Camera.transform.forward;
                Vector3 right = m_Camera.transform.right;

                //Can be done in the same calculaton as setting the m_DesiredMoveDirection
                forward.y = 0f;
                right.y = 0f;

                //Can be done in the same calculaton as setting the m_DesiredMoveDirection
                forward.Normalize();
                right.Normalize();

                m_DesiredManualRotation = forward * m_Input2D.y + right * m_Input2D.x;
                //m_DesiredMoveDirection.Normalize();

                //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_DesiredManualRotation), Time.deltaTime * m_RotationSpeed);
            }
        }
    }

    public void CalculateMovementSpeed() {
        m_PlayerSpeed = Mathf.Clamp(Mathf.Abs(m_Input2D.x) + Mathf.Abs(m_Input2D.y), 0, 1);
        //m_PlayerSpeed -= 0.5f; to walk
        if (m_PlayerRunning) {
            m_PlayerSpeed *= 2;
        }

        if(m_PlayerSpeed != 0) {
            m_LastRecordedSpeed = m_PlayerSpeed;
        }
    }

    public void UpdateAnimator() {
       
        m_Animator.SetFloat("Speed", m_PlayerSpeed);
        m_Animator.SetFloat("SmoothSpeed", m_SmoothPlayerSpeed);
        m_Animator.SetFloat("LastRecordedSpeed", m_LastRecordedSpeed);
        m_Animator.SetInteger("TurningDirection", m_NormalizedRotationDirection);
    }

    public void OnAnimatorIK(int layerIndex) {
        m_Animator.SetLookAtPosition(Vector3.right);
    }

    public void CalCulateDifferenceFromIdle()
    {
        if (m_Input2D.x == 0 && m_Input2D.y == 0)
        {
            if (!m_RotationSet)
            {
                //Reset
                m_IdleRotation = m_DesiredDirection;
                m_RotationSet = true;
            }
            m_IdleRotationDistance = m_DesiredDirection + m_IdleRotation;
           // m_IdleRotationDistance =  WrapAngle(m_IdleRotationDistance);
        }

        if (m_Input2D.x != 0 || m_Input2D.y != 0)
        {
            m_RotationSet = false;
        }
    }

    void HandleIdleTurns() {
        if (m_InPlaceTurnsEnabled) {
            if (m_Input2D.x == 0 || m_Input2D.y == 0)
            {
                if(Mathf.Abs(m_DesiredDirection) >= 90.0f){
                    m_Animator.SetBool("TurnInPlace", true);
                }
                else
                {
                    m_Animator.SetBool("TurnInPlace", false);
                }
            }
        }
    }
}
