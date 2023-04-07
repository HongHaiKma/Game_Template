using System.Numerics;
using UnityEngine;
using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
    [System.Serializable]
    public class BigNumberEvent : UnityEvent<BigNumber> { }

    [CreateAssetMenu(
        fileName = "BigNumberVariable.asset",
        menuName = SOArchitecture_Utility.VARIABLE_SUBMENU + "BigNumber",
        order = SOArchitecture_Utility.ASSET_MENU_ORDER_COLLECTIONS + 13)]
    public class BigNumberVariable : BaseVariable<BigNumber, BigNumberEvent>
    {
        // public override bool Clampable { get { return true; } }
        // protected override int ClampValue(int value)
        // {
        //     if (value.CompareTo(MinClampValue) < 0)
        //     {
        //         return MinClampValue;
        //     }
        //     else if (value.CompareTo(MaxClampValue) > 0)
        //     {
        //         return MaxClampValue;
        //     }
        //     else
        //     {
        //         return value;
        //     }
        // }
    }
}