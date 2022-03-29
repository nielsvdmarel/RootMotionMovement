using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugPlayerDisplay : MonoBehaviour
{
    [SerializeField]
    CamerAngleCalculator m_CamAngleCalculator;

    public TextMeshProUGUI textDisplay;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if(player != null)
        {
            m_CamAngleCalculator = player.GetComponent<CamerAngleCalculator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float test = m_CamAngleCalculator.m_DesiredDirection;
        test = Mathf.RoundToInt(test);
        textDisplay.text = test.ToString();
    }
}
