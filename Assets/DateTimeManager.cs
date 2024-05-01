using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using Cysharp.Threading.Tasks;
using System.Threading;

[DefaultExecutionOrder(-100)]
public class DateTimeManager : Singleton<DateTimeManager>
{
    // private static DateTimeManager m_Instance;
    // public static DateTimeManager Instance
    // {
    //     get
    //     {
    //         if (m_Instance == null)
    //         {
    //             m_Instance = new GameObject().AddComponent<DateTimeManager>();
    //         }
    //         return m_Instance;
    //     }
    // }
    public DateTime m_GoogleDateTime = DateTime.Now;
    public DateTime Now { get { return Instance.GetDateTimeNow(); } }
    public bool IsConnectedTimeToNetwork = false;
    public override void Awake()
    {
        // m_Instance = this;
        // // base.Awake();
        // DontDestroyOnLoad(gameObject);
        LoadTimeCurrent();
    }
    private void Update()
    {
        m_GoogleDateTime = m_GoogleDateTime.AddSeconds(Time.deltaTime);
    }
    public void LoadTimeCurrent()
    {
        StartCoroutine(GetWorldAsynDateTimeNow());
    }

    public IEnumerator GetWorldAsynDateTimeNow()
    {
        UnityWebRequest myHttpWebRequest = UnityWebRequest.Get("http://www.google.com");
        yield return myHttpWebRequest.SendWebRequest();
        if (myHttpWebRequest.result == UnityWebRequest.Result.Success)
        {
            IsConnectedTimeToNetwork = true;
            string netTime = myHttpWebRequest.GetResponseHeader("date");
            m_GoogleDateTime = System.DateTime.ParseExact(netTime,
                        "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
                        CultureInfo.InvariantCulture.DateTimeFormat,
                        DateTimeStyles.AdjustToUniversal);
            //TimeZone localZone = TimeZone.CurrentTimeZone;
            m_GoogleDateTime = m_GoogleDateTime.ToLocalTime();
            //Debug.Log("Date " + m_GoogleDateTime.ToLongDateString() + " Time " + m_GoogleDateTime.ToLongTimeString());
        }
        else
        {
            Debug.Log("Get date time error. ");
            IsConnectedTimeToNetwork = false;
            m_GoogleDateTime = System.DateTime.Now; //In case something goes wrong. 
        }
    }

    [Sirenix.OdinInspector.Button]
    public async UniTask CheckNetWorkConnected()
    {
        var cts = new CancellationTokenSource();
        UnityWebRequest myHttpWebRequest = UnityWebRequest.Get("http://www.google.com");
        myHttpWebRequest.timeout = 1;
        myHttpWebRequest.SendWebRequest().WithCancellation(cts.Token);
        // await myHttpWebRequest.SendWebRequest().WithCancellation(cts.Token);
        await UniTask.Delay(1000);
        if (myHttpWebRequest.result == UnityWebRequest.Result.Success)
        {
            IsConnectedTimeToNetwork = true;
            Debug.Log("Network connected!!!");
        }
        else
        {
            IsConnectedTimeToNetwork = false;
            Debug.Log("Network error!!!");
        }
    }

    public bool IsWifiTurnOn()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            return false;
        }
        return true;
    }

    // [Sirenix.OdinInspector.Button]
    // public IEnumerator IsNetWorkConnected()
    // {
    //     UnityWebRequest myHttpWebRequest = UnityWebRequest.Get("http://www.google.com");
    //     myHttpWebRequest.timeout = 3;
    //     yield return myHttpWebRequest.SendWebRequest();
    //     if (myHttpWebRequest.result == UnityWebRequest.Result.Success)
    //     {
    //         IsConnectedTimeToNetwork = true;
    //         Debug.Log("Network connected!!!");
    //     }
    //     else
    //     {
    //         IsConnectedTimeToNetwork = false;
    //         Debug.Log("Network error!!!");
    //     }
    // }

    public DateTime GetDateTimeNow()
    {
        return m_GoogleDateTime;
    }
}
