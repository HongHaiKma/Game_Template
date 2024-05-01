using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UICanvas : MonoBehaviour
{
    public UIID m_ID;
    public UIID ID()
    {
        return m_ID;
    }
    public bool IsPopupUnlock;
    protected RectTransform m_RectTransform;
    public RectTransform RectTransform
    {
        get { return m_RectTransform; }
        set { m_RectTransform = value; }
    }

    protected CanvasGroup m_CanvasGroup;
    public bool IsClosing = false;
    public bool IsOpenPrevious = true;
    public bool IsAutoRemove = false;
    public bool IsAvoidBackKey = false;

    public Button btn_Close;

    public virtual void Start()
    {
        if (btn_Close != null)
        {
            GUIManager.Instance.AddClickEvent(btn_Close, OnClose);
        }
    }

    protected void Init(bool isActive = false)
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        if (m_CanvasGroup == null)
        {
            m_CanvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        if (GUIManager.Instance == null)
        {
            StartCoroutine(OnWaitingRegister());
        }
        else
        {
            GUIManager.Instance.RegisterUI(this);
            gameObject.SetActive(isActive);
        }
    }

    public virtual void OnEnable()
    {
        AddListener();
    }

    public virtual void OnDisable()
    {
        RemoveListener();
    }

    public virtual void OnDestroy()
    {
        RemoveListener();
    }

    public virtual void AddListener()
    {

    }

    public virtual void RemoveListener()
    {

    }

    public virtual void Setup()
    {

    }

    public virtual void OnStartOpen()
    {
        IsClosing = false;
        transform.localEulerAngles = new Vector3(0, 0, 0);
        FadeIn();
    }

    public virtual void OnClose()
    {
        if (IsClosing) return;
        GUIManager.Instance.HideUIPopup(this, IsAutoRemove, IsOpenPrevious);
        IsClosing = true;
        IsPopupUnlock = false;
        Resources.UnloadUnusedAssets();
        // GC.Collect();
    }

    public virtual void OnBack()
    {
        OnClose();
    }

    IEnumerator OnWaitingRegister()
    {
        yield return Yielders.EndOfFrame;

        // if (GUIManager.Instance != null)
        // {
        GUIManager.Instance.RegisterUI(this);
        // }

        gameObject.SetActive(false);
    }


    public void ShowPopup(bool isSetup = true)
    {
        // gameObject.SetActive(false);
        gameObject.SetActive(true);
        if (isSetup)
        {
            Setup();
        }
        OnStartOpen();
        //if (m_CanvasGroup != null)
        //    m_CanvasGroup.alpha = 0;
    }

    // public virtual void OnClose()
    // {
    //     if (IsClosing) return;
    //     GUIManager.Instance.HideUIPopup(this, IsAutoRemove, IsOpenPrevious);
    //     IsClosing = true;
    // }

    public void HidePopup()
    {
        FadeOut();
    }

    public void ShowPanel()
    {
        //Debug.Log("Start Show ID " + ID());
        gameObject.SetActive(true);
    }

    public void SetPosition(Vector3 position)
    {
        if (m_RectTransform != null)
            m_RectTransform.position = position;
    }
    public void SetLocalPosition(Vector3 position)
    {
        if (m_RectTransform != null)
            m_RectTransform.localPosition = position;
    }

    public virtual void FadeOut()
    {
        m_CanvasGroup.DOFade(0, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true); ;
        transform.DOScale(1.05f, 0.2f).SetEase(Ease.Flash).OnComplete(() => { gameObject.SetActive(false); }).SetUpdate(UpdateType.Late, true);
    }
    public void FadeIn()
    {
        if (m_CanvasGroup != null)
        {
            m_CanvasGroup.alpha = 0;
            transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            m_CanvasGroup.DOFade(1, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true);
            transform.DOScale(1, 0.2f).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true); ;
        }
    }

    public void FadeIn(float _timeDuration)
    {
        if (m_CanvasGroup != null)
        {
            m_CanvasGroup.alpha = 0;
            transform.localScale = new Vector3(1.05f, 1.05f, 1.05f);
            m_CanvasGroup.DOFade(1, _timeDuration).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true);
            transform.DOScale(1, _timeDuration).SetEase(Ease.Flash).SetUpdate(UpdateType.Late, true);
        }
    }
}

// public class PopupCaller
// {
//     public static void OpenWinPopup(bool _isClose = false, bool _isSetup = false)
//     {
//         PopupWin popup = GUIManager.Instance.GetUICanvasByID(UIID.POPUP_WIN) as PopupWin;

//         GUIManager.Instance.ShowUIPopup(popup, _isClose, _isSetup);
//     }

//     public static void OpenLosePopup(bool _isClose = false, bool _isSetup = false)
//     {
//         PopupLose popup = GUIManager.Instance.GetUICanvasByID(UIID.POPUP_WIN) as PopupLose;

//         GUIManager.Instance.ShowUIPopup(popup, _isClose, _isSetup);
//     }

//     public static void OpenPopup(UIID _uiid, bool _isClose = false, bool _isSetup = false)
//     {
//         UICanvas popup = GUIManager.Instance.GetUICanvasByID(_uiid) as UICanvas;

//         GUIManager.Instance.ShowUIPopup(popup, _isClose, _isSetup);
//     }

//     public static UICanvas GetPopup(UIID _uiid)
//     {
//         UICanvas popup = GUIManager.Instance.GetUICanvasByID(_uiid) as UICanvas;

//         return popup;
//     }

//     public static PopupInventory GetPopupInventory()
//     {
//         PopupInventory popup = GUIManager.Instance.GetUICanvasByID(UIID.POPUP_INVENTORY) as PopupInventory;

//         return popup;
//     }

//     public static PopupWin GetPopupWin()
//     {
//         PopupWin popup = GUIManager.Instance.GetUICanvasByID(UIID.POPUP_WIN) as PopupWin;

//         return popup;
//     }

//     public static PopupLose GetPopupLose()
//     {
//         PopupLose popup = GUIManager.Instance.GetUICanvasByID(UIID.POPUP_LOSE) as PopupLose;

//         return popup;
//     }
// }
