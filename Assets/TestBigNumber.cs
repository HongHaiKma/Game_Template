using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

public class TestBigNumber : MonoBehaviour
{
    public TextMeshProUGUI txt_TestBN;
    public BigNumberVariable aaa;
    public float add;

    private void Update()
    {
        txt_TestBN.text = aaa.ToString();
    }

    [Button]
    public void AddNumber()
    {
        aaa.Value += add;
    }
}
