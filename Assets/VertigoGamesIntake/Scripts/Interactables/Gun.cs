using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : InteractAble
{
    [SerializeField]
    DebugDrawLine m_DebugDrawLine;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        m_DebugDrawLine = GetComponentInChildren<DebugDrawLine>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
}
