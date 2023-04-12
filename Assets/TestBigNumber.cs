using System;
using System.Security.AccessControl;
using System.Numerics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using AdOne;

public class TestBigNumber : MonoBehaviour
{
    public TextMeshProUGUI txt_TestBN;
    public BigNumberVariable aaa;
    public float add;
    public DoubleVariable a;

    private void Update()
    {
        txt_TestBN.text = aaa.ToString();
    }

    [Button]
    public void AddNumber()
    {
        aaa.Value += add;
    }

    [Button]
    public void TestDouble()
    {
        // double a = 10000000000;
        txt_TestBN.text = a.Value.ToShortString();
    }
}
