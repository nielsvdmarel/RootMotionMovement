using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HumanBone {
    public HumanBodyBones bone;
    public float weight = 1.0f;
}

public class WeaponIK : MonoBehaviour
{
    [SerializeField]
    private Transform m_TargetTransform;
    [SerializeField]
    private Transform m_AimTransform;

    [SerializeField]
    private int m_Iterations = 10;

    [Range(0, 1)]
    public float m_Weight = 1.0f;
    public bool m_WeaponIKEnabled = false;

    public float m_AngleLimit = 90.0f;
    public float m_DistanceLimit = 1.5f;

    public HumanBone[] humanBones;
    Transform[] m_BoneTransforms;

    // Start is called before the first frame update
    void Start() {
        Animator animator = GetComponent<Animator>();
        m_BoneTransforms = new Transform[humanBones.Length];
        for (int i = 0; i < m_BoneTransforms.Length; i++) {
            m_BoneTransforms[i] = animator.GetBoneTransform(humanBones[i].bone);
        }
    }

    Vector3 GetTargetPosition() {
        Vector3 targetDirection = m_TargetTransform.position - m_AimTransform.position;
        Vector3 aimDirection = m_AimTransform.forward;
        float blendOut = 0.0f;

        float targetAngle = Vector3.Angle(targetDirection, aimDirection);
        if(targetAngle > m_AngleLimit) {
            blendOut += (targetAngle - m_AngleLimit) / 50.0f;
        }

        float targetDistance = targetDirection.magnitude;
        if(targetDistance < m_DistanceLimit) {
            blendOut += m_DistanceLimit - targetDistance;
        }

        Vector3 direction = Vector3.Slerp(targetDirection, aimDirection, blendOut);
        return m_AimTransform.position + direction;
    }

    // Update is called once per frame
    void LateUpdate() {
        if (m_WeaponIKEnabled) {
            Vector3 targetPosition = GetTargetPosition();
            //Calling AimTarget multiple times to be more accurate
            for (int i = 0; i < m_Iterations; i++) {
                for (int y = 0; y < m_BoneTransforms.Length; y++) {
                    Transform bone = m_BoneTransforms[y];
                    if (bone != null) {
                        float boneWeight = humanBones[y].weight * m_Weight;
                        AimAtTarget(bone, targetPosition, m_Weight);
                    }
                }
            }
        }
    }

    private void AimAtTarget(Transform m_Bone, Vector3 targetPosition, float weight) {
        Vector3 aimDirection = m_AimTransform.forward;
        Vector3 targetDirection = targetPosition - m_AimTransform.position;
        Quaternion aimTowards = Quaternion.FromToRotation(aimDirection, targetDirection);
        Quaternion blendRotation = Quaternion.Slerp(Quaternion.identity, aimTowards, weight);
        m_Bone.rotation = blendRotation * m_Bone.rotation;
    }
}
