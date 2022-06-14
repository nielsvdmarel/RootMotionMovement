using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSockets : MonoBehaviour
{
    public enum SocketId {
        Spine,
        RightHand,
        LeftHand,
        Head
    }

    Dictionary<SocketId, MeshSocket> socketMap = new Dictionary<SocketId, MeshSocket>();
    void Start() {
        MeshSocket[] sockets = GetComponentsInChildren<MeshSocket>();
        foreach (var socket in sockets) {
            socketMap[socket.m_SocketId] = socket;
        }
    }

    public void Attach(Transform objectTransform, SocketId socketId) {
        socketMap[socketId].Attach(objectTransform);
    }
}
