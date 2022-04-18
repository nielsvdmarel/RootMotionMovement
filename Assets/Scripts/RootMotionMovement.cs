using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RootMotionMovement : MonoBehaviour
{
    [Header("Debug Controls")]
    public bool b_DebugModeEnabled = false;

    [Header("Main component references")]
    [SerializeField]
    Camera m_Camera;

    [SerializeField]
    Animator m_Animator;

    [Header("Input")]
    [SerializeField]
    public InputAction playerControls;
    [SerializeField]
    public InputAction isRunning;
    public Vector2 m_Input2D;
    [SerializeField]
    private Vector3 m_Input;

    [Header("Main Root motion variables")]

    [SerializeField]
    public float m_DesiredDirection;
    [SerializeField]
    public int m_NormalizedRotationDirection;
    [SerializeField]
    private float m_StartDesiredDirection;

    [SerializeField]
    bool b_CanUpdateStartDirection = true;
    
    /// <summary>
    /// direct player speed regarding input
    /// </summary>
    [SerializeField]
    private float m_PlayerSpeed;

    [SerializeField]
    private float m_SmoothPlayerSpeed;

    /// <summary>
    /// last set input speed while moving, used for stop animation
    /// </summary>
    [SerializeField]
    private float m_LastRecordedSpeed;

    [SerializeField]
    private float m_LerpSpeed;

    [SerializeField]
    bool m_PlayerRunning = false;

    [Header("Camera variables")]
    [SerializeField]
    public bool AnimEnabledCustomRotation = false;
    [SerializeField]
    public bool m_EnableCustomRotation = false;

    [Header("Target values")]
    public Vector3 m_DesiredManualRotation;
    public float m_RotationSpeed;

    [Header("Turn In place")]
    [SerializeField]
    private bool m_InPlaceTurnsEnabled;
    [SerializeField]
    private float m_IdleRotation;
    [SerializeField]
    private bool m_RotationSet;

    [Header("Head retargeting")]
    [SerializeField]
    private Transform head;

    void Start() {
        m_Camera = Camera.main;
        m_Animator = this.GetComponent<Animator>();
        m_PlayerSpeed = 0;
        m_SmoothPlayerSpeed = 0;
    }
    private void OnEnable() {
        playerControls.Enable();
        isRunning.Enable();
    }

    private void OnDisable() {
        playerControls.Disable();
        isRunning.Disable();
    }

    void Update() {
        CalculateDesiredAngle(); //Calculating desired angle with Camera, input and player rotation
        CalculateInputMovementSpeed(); //Calculating the desired movement value using input.
        SetStartDirection();
        ManualPlayerRotation(); //Responsible for rotational movement after the start/stops.
        HandleIdleTurns(); //Responsible for handling idle turning when rotating on idle.
        HandleRunningInput();
        
        m_SmoothPlayerSpeed = Mathf.Lerp(m_SmoothPlayerSpeed, m_PlayerSpeed, m_LerpSpeed * Time.deltaTime);//Lerping needed variables

        UpdateAnimator(); //Updating the animator with all the calculated variables.
    }

    public void UpdateAnimator() {
        m_Animator.SetFloat("Speed", m_PlayerSpeed);
        m_Animator.SetFloat("SmoothSpeed", m_SmoothPlayerSpeed);
        m_Animator.SetFloat("LastRecordedSpeed", m_LastRecordedSpeed);
        m_Animator.SetInteger("TurningDirection", m_NormalizedRotationDirection);
        m_Animator.SetFloat("Direction", m_DesiredDirection);
    }

    private void CalculateDesiredAngle() {
        Vector3 delta;
        delta = m_Camera.transform.rotation.eulerAngles.normalized - this.transform.rotation.eulerAngles.normalized;
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
        if (m_DesiredDirection > 0) {
            m_NormalizedRotationDirection = 1;
        }
        else if (m_DesiredDirection < 0) {
            m_NormalizedRotationDirection = -1;
        }
        else if (m_DesiredDirection == 0) {
            m_NormalizedRotationDirection = 0;
        }
    }

    public void CalculateInputMovementSpeed() {
        m_PlayerSpeed = Mathf.Clamp(Mathf.Abs(m_Input2D.x) + Mathf.Abs(m_Input2D.y), 0, 1);
        //m_PlayerSpeed -= 0.5f; to walk
        if (m_PlayerRunning) {
            m_PlayerSpeed *= 2;
        }
        if (m_PlayerSpeed != 0) {
            m_LastRecordedSpeed = m_PlayerSpeed;
        }
    }

    /// <summary>
    /// Function for defining the start direction into the animator, when start has been detected. 
    /// This value needs to be constant during start anim, otherwise blending issues occur between different angled starts.
    /// </summary>
    private void SetStartDirection() {
            if (b_CanUpdateStartDirection) {
                if (!IsInputZero()) {
                    b_CanUpdateStartDirection = false;
                m_StartDesiredDirection = m_DesiredDirection;
                    m_Animator.SetFloat("StartDirection", m_StartDesiredDirection);
                }
            }
            else {
                if (IsInputZero()) {
                    b_CanUpdateStartDirection = true;
                }
            }
    }

    /// <summary>
    /// Function to enable rotation based on the camera and input.
    /// </summary>
    public void ManualPlayerRotation() {
        if (AnimEnabledCustomRotation) {
            if (m_EnableCustomRotation) {
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

                //transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_DesiredManualRotation), Time.deltaTime * m_RotationSpeed);
            }
        }
    }

    /// <summary>
    /// Functionality behind handling the in place idle turns
    /// </summary>
    void HandleIdleTurns()
    {
        if (m_InPlaceTurnsEnabled)
        {
            if (m_Input2D.x == 0 || m_Input2D.y == 0)
            {
                if (Mathf.Abs(m_DesiredDirection) >= 90.0f)
                {
                    m_Animator.SetBool("TurnInPlace", true);
                }
                else
                {
                    m_Animator.SetBool("TurnInPlace", false);
                }
            }
        }
    }

    /// <summary>
    /// Function to read values of the running input, using the new input system
    /// </summary>
    void HandleRunningInput() {
        if (isRunning.ReadValue<float>() == 1) {
            m_PlayerRunning = true;
        }
        else {
            m_PlayerRunning = false;
        }
    }

    /// <summary>
    /// function to wrap input angle to usable directional angle
    /// </summary>
    /// <param name="angle"></param>
    /// <returns> returns input angle, translated to fit between negative and positive 180 </returns>
    private static float WrapAngle(float angle) {
        angle %= 360;
        if (angle > 180)
            return angle - 360;
        else if (angle < -180)
            return angle + 360;
        return angle;
    }

    /// <summary>
    /// Function to check if input is equal to zero
    /// </summary>
    /// <returns> returns boolean, equal to the state of input </returns>
    bool IsInputZero() {
        if (m_Input2D.x != 0 || m_Input2D.y != 0) {
            return false;
        }
        return true;
    }
}
