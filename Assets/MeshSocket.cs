using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSocket : MonoBehaviour
{
    public MeshSockets.SocketId m_SocketId;
    Transform attachPoint;
   
    void Start() {
        attachPoint = transform.GetChild(0);
    }

    public void Attach(Transform objectTransform) {
        objectTransform.SetParent(attachPoint, false);
    }

}
