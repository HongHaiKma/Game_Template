using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public static class Helpers
{
    private static Camera _camera;
    public static Camera Camera
    {
        get
        {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    //This returns the angle in radians
    public static float AngleInRad(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    //This returns the angle in degrees
    public static float AngleInDeg(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRad(vec1, vec2) * 180 / Mathf.PI;
    }

    // public static Quaternion AngleTo(Vector3 pos, Vector3 target)
    // {
    //     Vector3 relativePos = target - pos;
    //     Quaternion rotation = Quaternion.LookRotation(relativePos);
    //     rotation.x = pos.x;
    //     rotation.y = pos.y;
    //     return rotation;
    // }

    public static Quaternion QuaternionTo(Vector3 origin, Vector3 target)
    {
        Vector3 myLocation = origin;
        Vector3 targetLocation = target;
        targetLocation.z = myLocation.z; // ensure there is no 3D rotation by aligning Z position

        // vector from this object towards the target location
        Vector3 vectorToTarget = targetLocation - myLocation;
        // rotate that vector by 90 degrees around the Z axis
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, 90) * new Vector3(vectorToTarget.x, vectorToTarget.y, vectorToTarget.z + 180f);

        // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
        // (resulting in the X axis facing the target)
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        return targetRotation;
    }

    public static Quaternion QuaternionTo(Vector3 origin, Vector3 target, float _fixZAxis)
    {
        Vector3 myLocation = origin;
        Vector3 targetLocation = target;
        targetLocation.z = myLocation.z; // ensure there is no 3D rotation by aligning Z position

        // vector from this object towards the target location
        Vector3 vectorToTarget = targetLocation - myLocation;
        // rotate that vector by 90 degrees around the Z axis
        Vector3 rotatedVectorToTarget = Quaternion.Euler(0, 0, _fixZAxis) * new Vector3(vectorToTarget.x, vectorToTarget.y, vectorToTarget.z + 180f);

        // get the rotation that points the Z axis forward, and the Y axis 90 degrees away from the target
        // (resulting in the X axis facing the target)
        Quaternion targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: rotatedVectorToTarget);

        return targetRotation;
    }

    public static float AngleTo(Vector2 origin, Vector2 target)
    {
        Vector2 diference = target - origin;
        float signY = (target.y < origin.y) ? -1.0f : 1.0f;

        return Vector2.Angle(origin, diference) * signY;
    }

    // public static string TimeFormat(int seconds)
    // {
    //     var minutes = seconds / 60;
    //     var remainSec = seconds % 60;
    //     return (minutes < 10 ? "0" + minutes : minutes) + ":" + (remainSec < 10 ? "0" + remainSec : remainSec);
    // }

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary =
        new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictionary.TryGetValue(time, out var wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
    }

    private static PointerEventData _eventDataCurrentPosition;
    private static List<RaycastResult> _results;

    public static bool IsOverUi()
    {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        _results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }

    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }

    public static void DeleteChildren(this Transform t)
    {
        foreach (Transform child in t) UnityEngine.Object.Destroy(child.gameObject);
    }

    public static string GetLevelStringFormat(int level)
    {
        if (level < 0) return "9999";
        if (level < 10) return "000" + level;
        if (level < 100) return "00" + level;
        return "0" + level;
    }

    public static Vector3 WorldToCanvasPosition(this Canvas canvas, Vector3 worldPosition, Camera camera)
    {
        var viewportPosition = camera.WorldToViewportPoint(worldPosition);
        return canvas.ViewportToCanvasPosition(viewportPosition);
    }

    public static Vector3 WorldToCanvasPosition(this Canvas canvas, RectTransform canvasRectTransform, Vector3 worldPosition, Camera camera)
    {
        var viewportPosition = camera.WorldToViewportPoint(worldPosition);
        return canvas.ViewportToCanvasPosition(canvasRectTransform, viewportPosition);
    }

    public static Vector3 ScreenToCanvasPosition(this Canvas canvas, Vector3 screenPosition)
    {
        var viewportPosition = new Vector3(screenPosition.x / Screen.width,
            screenPosition.y / Screen.height,
            0);
        return canvas.ViewportToCanvasPosition(viewportPosition);
    }

    public static Vector3 ViewportToCanvasPosition(this Canvas canvas, Vector3 viewportPosition)
    {
        var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
        var canvasRect = canvas.GetComponent<RectTransform>();
        var scale = canvasRect.sizeDelta;
        return Vector3.Scale(centerBasedViewPortPosition, scale);
    }
    public static Vector3 ViewportToCanvasPosition(this Canvas canvas, RectTransform canvasRectTransform, Vector3 viewportPosition)
    {
        var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
        var scale = canvasRectTransform.sizeDelta;
        return Vector3.Scale(centerBasedViewPortPosition, scale);
    }


    public static class ConvertExtension
    {
        public static T? ConvertNullable<T>(object data) where T : struct
        {
            try
            {
                return (T)Convert.ChangeType(data, typeof(T));
            }
            catch
            {
                return null;
            }
        }
    }

    public static T ChangeType<T>(object data) where T : IDictionaryConvertible<T>, new()
    {
        var obj = (Dictionary<string, object>)Convert.ChangeType(data, typeof(Dictionary<string, object>));
        return new T().fromDictionary(obj);
    }

    public static bool TryParse<TEnum>(string s, out TEnum enumValue) where TEnum : struct
    {
        return Enum.TryParse(s, out enumValue) && Enum.IsDefined(typeof(TEnum), enumValue);
    }

}

public interface IDictionaryConvertible<T>
{
    T fromDictionary(Dictionary<string, object> dictionary);
}
