using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CamerAngleCalculator : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    public InputAction playerControls;

    [SerializeField]
    private float DesiredDirection;

    [SerializeField]
    Vector3 m_Input;
    [SerializeField]
    public Vector2 moveVal;

    [SerializeField]
    bool CanRotateWithCamera = false;

    private void OnEnable()
    {
        playerControls.Enable();
    }
    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        Player = this.gameObject;
        mainCamera = Camera.main;
    }

    void Update()
    {
        CalculateDesiredAngle();
        //transform.Rotate(0,0.4f,0);
        TestRotatePlayerTowardsCamera();
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
        delta = mainCamera.transform.rotation.eulerAngles.normalized - Player.transform.rotation.eulerAngles.normalized;
        Vector2 input2D = playerControls.ReadValue<Vector2>();
        m_Input = new Vector3(input2D.y, input2D.x, 0);


        float target_rotation_ = Mathf.Atan2(input2D.x, input2D.y) * Mathf.Rad2Deg; // + mainCamera.transform.eulerAngles.y;
        Vector3 InputTargetRotation = new Vector3(0, target_rotation_, 0);
        //  float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, target_rotation_, ref rotation_velocity_, rotation_smooth_time);
        float rotationX;
        float rotationY;
        float rotationZ;

        rotationX = mainCamera.transform.eulerAngles.x;
        rotationY = mainCamera.transform.eulerAngles.y;
        rotationZ = mainCamera.transform.eulerAngles.z;

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

        DesiredDirection = FinalDelta.y;
    }

    public void TestRotatePlayerTowardsCamera() {
        if (CanRotateWithCamera) {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
        }
    }
}
