﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class Helper
{
    //DISTANCE
    public static float CalDistance(Vector2 _origin, Vector2 _des)
    {
        return Vector2.Distance(_origin, _des);
    }

    public static float CalDistance(Vector3 _origin, Vector3 _des)
    {
        // return Vector3.Distance(_origin, _des);
        return (_origin - _des).sqrMagnitude;
    }

    // public static float CalDistanceXZ(Vector3 _origin, Vector3 _des)
    // {
    //     Vector3 origin = new Vector3();
    //     Vector3 des = _des;

    //     return Vector3.Distance(origin, des);
    // }

    public static bool IsBehind(Transform _target, Vector3 _checker)
    {
        Vector3 forward = _target.TransformDirection(Vector3.forward);
        Vector3 toOther = _checker - _target.position;

        return (Vector3.Dot(forward, toOther) < 1);
    }

    public static bool InRange(Vector3 _origin, Vector3 _des, float _maxDistance)
    {
        return (Vector3.Distance(_origin, _des) <= _maxDistance);
    }

    public float CalDistance2(Vector2 origin, Vector2 des)
    {
        return (origin - des).magnitude;
    }

    public float CalDistance2(Vector3 origin, Vector3 des)
    {
        return Vector3.Distance(origin, des);
    }

    //ROTATION
    public static float ClampAngle(float angle, float min, float max)
    {
        if (min < 0 && max > 0 && (angle > max || angle < min))
        {
            angle -= 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }
        else if (min > 0 && (angle > max || angle < min))
        {
            angle += 360;
            if (angle > max || angle < min)
            {
                if (Mathf.Abs(Mathf.DeltaAngle(angle, min)) < Mathf.Abs(Mathf.DeltaAngle(angle, max))) return min;
                else return max;
            }
        }

        if (angle < min) return min;
        else if (angle > max) return max;
        else return angle;
    }

    public static Quaternion Random8Direction(Vector3 _ownerPos)
    {
        // Vector3 dir = new Vector3();
        // int a = Random.Range(0, 8);
        // switch (a)
        // {
        //     case 0:
        //         dir = new Vector3(1, 0, 0);
        //         break;
        //     case 1:
        //         dir = new Vector3(-1, 0, 0);
        //         break;
        //     case 2:
        //         dir = new Vector3(0, 0, 1);
        //         break;
        //     case 3:
        //         dir = new Vector3(0, 0, -1);
        //         break;
        //     case 4:
        //         dir = new Vector3(1, 0, 1);
        //         break;
        //     case 5:
        //         dir = new Vector3(-1, 0, -1);
        //         break;
        //     case 6:
        //         dir = new Vector3(1, 0, -1);
        //         break;
        //     case 7:
        //         dir = new Vector3(-1, 0, 1);
        //         break;

        // }

        Vector3 dir = new Vector3();
        int a = Random.Range(0, 4);
        switch (a)
        {
            case 0:
                dir = Vector3.left;
                break;
            case 1:
                dir = Vector3.right;
                break;
            case 2:
                dir = Vector3.forward;
                break;
            case 3:
                dir = Vector3.back;
                break;
        }

        // dir = tf_Owner.position + new Vector3(5, 0, 0);

        Vector3 dir2 = _ownerPos + dir;

        Vector3 lookPos = dir2 - _ownerPos;

        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);

        return rotation;
    }

    #region DEBUG
    public static void DebugLog(string mess)
    {
        Debug.Log(mess);
    }

    public static void DebugLog(float mess)
    {
        Debug.Log(mess);
    }

    public static void DebugLog(int mess)
    {
        Debug.Log(mess);
    }
    #endregion

    #region RANDOM
    public static bool Random2Probability(float percent)
    {
        float pickPercent = Random.Range(1, 101);

        if (pickPercent <= percent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    public static bool JsonDataContainsKey(JsonData data, string key)
    {
        bool result = false;
        if (data == null)
            return result;
        if (!data.IsObject)
        {
            return result;
        }
        IDictionary tdictionary = data as IDictionary;
        if (tdictionary == null)
            return result;
        if (tdictionary.Contains(key))
        {
            result = true;
        }
        return result;
    }

    #region Color

    public static Color ConvertColor(float r, float g, float b, float a = 255)
    {
        return new Color(r/255f, g/255f, b/255f, a/255f);
    }
    
    public static Color ConvertColor(Color _color)
    {
        return new Color(_color.r/255f, _color.g/255f, _color.b/255f, _color.a/255f);
    }

    #endregion

    public static bool GetKeyDown(KeyCode _key)
    {
        return Input.GetKeyDown(_key);
    }
    
    public static bool GetKey(KeyCode _key)
    {
        return Input.GetKey(_key);
    }

    public IList<T> ForSort<T>(IList<T> a)
    {
        return a;
    }
}
