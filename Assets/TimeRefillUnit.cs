using UnityEngine;
using System.Collections;
using System;

public delegate void RefillCallback();
public class TimeRefillUnit
{
    /// <summary>
    /// Time to Add
    /// </summary>
    public int ta = 20 * 60; // 20 minutes
    /// <summary>
    /// Max Value
    /// </summary>
    public int mv = 1;
    /// <summary>
    /// Current Value
    /// </summary>
    public int cv = 10;
    /// <summary>
    /// Default Value
    /// </summary>
    public int dv = 10;
    /// <summary>
    /// LastTime Change
    /// </summary>
    public DateTime ltc;
    /// <summary>
    /// Time To Next Add
    /// </summary>
    public int tta = 0;
    /// <summary>
    /// Key Value
    /// </summary>
    public string kv = "";
    /// <summary>
    /// Key Time
    /// </summary>
    public string kt = "";
    /// <summary>
    /// Event Trigger Name
    /// </summary>
    public GameEvent etn;
    /// <summary>
    /// Fullfill Event Trigger Name
    /// </summary>
    public string fetn = "UpdateLife";
    private RefillCallback m_RefillCallback = null;
    public TimeRefillUnit()
    {

    }
    public TimeRefillUnit(int refillTime, int maxValue, int defaultValue, string key, GameEvent eventTriggerName, string fullfillEventTriggerName = "")
    {
        ta = refillTime;
        mv = maxValue;
        cv = defaultValue;
        dv = defaultValue;
        kv = key;
        kt = "LastTimeAdd" + key;
        etn = eventTriggerName;
        fetn = fullfillEventTriggerName;
    }
    public void InitNew()
    {
        cv = dv;
    }

    public void Load()
    {
        // cv = PlayerPrefs.GetInt(kv, dv);
        cv = ES3.Load<int>(kv, defaultValue: dv);
        // ltc = DateTime.Parse(PlayerPrefs.GetString(kt, DateTimeManager.Now.ToString()));
        ltc = DateTime.Parse(ES3.Load<string>(kt, defaultValue: DateTimeManager.Instance.Now.ToString()));
        Debug.Log(ltc);
        PreloadData();
    }
    void PreloadData()
    {
        DateTime dateTimeNow = DateTimeManager.Instance.Now;
        TimeSpan span = dateTimeNow - ltc;
        double totalSecs = span.TotalSeconds;
        while (cv < mv && totalSecs >= ta)
        {
            cv++;
            totalSecs -= ta;
            ltc += TimeSpan.FromSeconds(ta);
            Save();
            EventManager.CallEvent(etn);
            if (m_RefillCallback != null) m_RefillCallback();
        }
        if (cv < mv)
        {
            int val = ta - (int)totalSecs;
            if (tta != val)
            {
                tta = val;
            }
        }
    }
    public void Save()
    {
        // PlayerPrefs.SetInt(kv, cv);
        ES3.Save<int>(kv, cv);
        // PlayerPrefs.SetString(kt, ltc.ToString());
        ES3.Save<string>(kt, ltc.ToString());
    }
    public void Reset()
    {
        // PlayerPrefs.SetInt(kv, dv);
        ES3.Save<int>(kv, dv);
        // cv = PlayerPrefs.GetInt(kv, dv);
        cv = ES3.Load<int>(kv, defaultValue: dv);
        string now = DateTimeManager.Instance.Now.ToString();
        // PlayerPrefs.SetString(kt, now);
        ES3.Save<string>(kt, now);
        // ltc = DateTime.Parse(PlayerPrefs.GetString(kt, now));
        ltc = DateTime.Parse(ES3.Load<string>(kt, defaultValue: now));
    }
    void UpdateLastTimeChanged()
    {
        ltc = DateTimeManager.Instance.Now;
    }
    public bool Add(int amount, bool isCapMax = false)
    {
        cv += amount;
        if (cv >= mv)
        {
            if (isCapMax)
            {
                cv = mv;
            }
            EventManagers2.TriggerEvent(fetn);
        }
        EventManager.CallEvent(etn);
        return true;
    }
    public void AddToMax()
    {
        if (cv < mv)
        {
            cv = mv;
        }
    }
    public void Consume(int amount = 1)
    {
        if (cv == mv)
        {
            UpdateLastTimeChanged();
        }
        cv -= amount;
        if (cv < 0)
        {
            cv = 0;
        }
        EventManager.CallEvent(etn);
        Save();
    }
    public void SetupRefillTime(int refillTime)
    {
        ta = refillTime;
        Save();
    }
    public void SetRefillCallback(RefillCallback callback)
    {
        m_RefillCallback = callback;
    }
    public int GetCurrentValue()
    {
        return cv;
    }
    public int GetMaxValue()
    {
        return mv;
    }
    public bool IsMaxValue()
    {
        return cv >= mv;
    }
    public string GetCurrentValueProgress()
    {
        return cv + "/" + mv;
    }
    public string GetTimeToNextAdd(int type, string _defValue)
    {
        switch (type)
        {
            case 2:
                {
                    string ret = _defValue;
                    if (cv < mv)
                    {
                        int min = tta / 60;
                        int sec = tta % 60;
                        ret = (min >= 10 ? min.ToString() : "0" + min.ToString()) + "M:" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString() + "S");
                    }
                    return ret;
                }
            case 3:
                {
                    string ret = _defValue;
                    if (cv < mv)
                    {
                        int min = tta / 60;
                        int sec = tta % 60;
                        int hour = min / 60;
                        min = min % 60;
                        ret = (hour >= 10 ? hour.ToString() : "0" + hour.ToString()) + ":" + (min >= 10 ? min.ToString() : "0" + min.ToString()) + ":" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString()) + ""; ;
                    }
                    return ret;
                }
            case 4:
                {
                    string ret = _defValue;
                    if (cv <= mv)
                    {
                        int min = tta / 60;
                        int sec = tta % 60;
                        int hour = min / 60;
                        int day = hour / 24;
                        min = min % 60;
                        hour = hour % 24;
                        ret = (day >= 10 ? day.ToString() : "0" + day.ToString()) + "D:" + (hour >= 10 ? hour.ToString() : "0" + hour.ToString()) + "H:" + (min >= 10 ? min.ToString() : "0" + min.ToString()) + "M:" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString()) + "S";
                    }
                    return ret;
                }
            case 5:
                {
                    string ret = _defValue;
                    if (cv < mv)
                    {
                        int min = tta / 60;
                        int sec = tta % 60;
                        int hour = min / 60;
                        min = min % 60;
                        ret = (hour >= 10 ? hour.ToString() : "0" + hour.ToString()) + ":" + (min >= 10 ? min.ToString() : "0" + min.ToString()) + ":" + (sec >= 10 ? sec.ToString() : "0" + sec.ToString());
                    }
                    return ret;
                }
            default:
                {
                    return "";
                }
        }

    }
    public double GetTimeToFullfill()
    {
        int current = GetCurrentValue();
        int max = GetMaxValue();
        if (current >= max)
        {
            return 0;
        }
        double num = tta;
        int num1 = GetCurrentValue() + 1;
        if (num1 < max)
        {
            double num2 = (max - num1) * ta;
            num += num2;
        }
        return num;
    }
    public void Update()
    {
        if (cv < mv)
        {
            DateTime now = DateTimeManager.Instance.Now;
            TimeSpan span = now - ltc;
            double totalSecs = span.TotalSeconds;
            while (cv < mv && totalSecs >= ta)
            {
                cv++;
                totalSecs -= ta;
                ltc += TimeSpan.FromSeconds(ta);
                Save();
                EventManager.CallEvent(etn);
                // Debug.Log("AAAAAAAAAA");
                if (m_RefillCallback != null) m_RefillCallback();
            }
            if (cv < mv)
            {
                int val = ta - (int)totalSecs;
                if (tta != val)
                {
                    tta = val;
                }
                // Save();
                // Debug.Log("BBBBBBBBBB");
            }
            else
            {
                tta = 0;
            }
        }
    }

    // public void SaveEvent()
    // {

    // }
}