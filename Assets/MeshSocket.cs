using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public MeshSockets.SocketId m_SocketId;
    Transform m_AttachPoint;
   
    void Start() {
        m_AttachPoint = transform.GetChild(0);
    }

    public void Attach(Transform objectTransform) {
        objectTransform.SetParent(m_AttachPoint, false);
    }

}
