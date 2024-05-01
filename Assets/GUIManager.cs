using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

// using GoogleMobileAds.Api;
// using GoogleMobileAds.Common;

// using Firebase.RemoteConfig;

[DefaultExecutionOrder(-91)]
public class GUIManager : MonoBehaviour
{
    #region Cheat

    [Title("Cheat Game")]
    public InputField txt_Level;
    public GameObject go_CheatPanel;
    // public TextMeshProUGUI txt_Levels;

    // public List<DevLocker.Utils.SceneReference> m_Levels;
    // public List<> m_Levels;

    public void OpenCheatGame()
    {
        go_CheatPanel.SetActive(!go_CheatPanel.activeInHierarchy);
        if (go_CheatPanel.activeInHierarchy)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void JumpLevel()
    {
        // Time.timeScale = 1;
        int.TryParse(txt_Level.text, out int level);
        // if (level >= 1 && level <= m_Levels.Count - 1)
        // {
        //     ProfileManager.SetLevel(level);
        //     LoadPlayScene();
        // }
        // else
        // {
        //     Helper.DebugLog("Level Profile: " + m_Levels.Count);
        //     ProfileManager.SetLevel(m_Levels.Count);
        //     LoadPlayScene();
        // }

        // ProfileManager.SetLevel(level);
        LoadPlayScene();
    }

    #endregion

    #region Variables

    internal class GUIMap : Dictionary<int, UICanvas>
    {
        public static int m_NextID = 0;
        public int m_ID = 0;
        public GUIMap()
        {
            //Debug.Log("Create new GUI MAP " + m_NextID);
            m_ID = m_NextID;
            m_NextID++;
        }
    }

    AsyncOperation async;
    bool isLoadInitScene = false;
    private GUIMap m_GUIMap;

    private Vector3 m_CenterPos;

    private float m_OffsetTop = 0;
    public bool IsLongDevice = false;

    private UICanvas m_PreviousPopup = null;
    private UICanvas m_PreviousPanel = null;
    public GameObject m_MainCanvas;


    private bool IsHoldBackkey = false;

    public bool m_LoadImmediately;

    public List<UICanvas> m_CurrentOpenedPopup = new List<UICanvas>();
    public List<UICanvas> m_CurrentOpenedPanel = new List<UICanvas>();

    [Header("Get Panels")]
    // private PanelLoadingAds m_PanelLoadingAds;
    public GameObject g_SubCanvas;
    // public PanelLoading m_PanelLoading;
    public GameObject g_PanelLoading;
    public GameObject g_Loading;
    public Image img_LoadingBar;

    [Header("Async")]
    private AsyncOperationHandle<SceneInstance> handle;

    private static GUIManager m_Instance;
    public static GUIManager Instance
    {
        get
        {
            return m_Instance;
        }
    }

    #endregion


    private void Awake()
    {
        if (m_Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
            this.m_GUIMap = new GUIMap();
            // if (m_SubCanvas != null)
            // {
            //     DontDestroyOnLoad(m_SubCanvas);
            // }
            FindMainCanvas();
            // float ratio = (float)Screen.height / (float)Screen.width;
            // if (ratio > 2.1f)
            // {
            //     m_OffsetTop = -50f;
            // }
            // if (ratio > 1.8f)
            // {
            //     IsLongDevice = true;
            // }
            // m_CenterPos = Vector3.zero + new Vector3(0, m_OffsetTop);
        }

        if (g_SubCanvas != null)
        {
            DontDestroyOnLoad(g_SubCanvas);
        }

        int maxScreenHeight = 1080;
        float ratio = (float)Screen.currentResolution.width / (float)Screen.currentResolution.height;
        if (Screen.currentResolution.height > maxScreenHeight)
        {
            Screen.SetResolution(Mathf.RoundToInt(ratio * (float)maxScreenHeight), maxScreenHeight, true);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TestLoad();
        }
    }

    // async UniTask Start()
    // {
    //     await UniTask.WaitUntil(() => AppOpenAdManager.Instance != null);
    //     // AppOpenAdManager.Instance.LoadAd();
    //     // AppStateEventNotifier.AppStateChanged += OnAppStateChanged;
    // }

    // private void OnAppStateChanged(AppState state)
    // {
    //     // Display the app open ad when the app is foregrounded.
    //     // UnityEngine.Debug.Log("App State is " + state);
    //     // if (state == AppState.Foreground)
    //     // {
    //     //     AppOpenAdManager.Instance.ShowAdIfAvailable();
    //     // }
    // }

    void OnEnable()
    {
        // StartCoroutine(LoadPlayScene());
        if (m_LoadImmediately == true)
        {
            LoadPlayScene();
        }
    }

    public void ClearAllOpenedPopupList()
    {
        m_CurrentOpenedPopup.Clear();
    }

    #region LoadScene

    // [Header("Scenes")]
    public AssetReference m_MainMenuScene;
    public AsyncOperationHandle<SceneInstance> m_CurrentLevel;
    public AsyncOperationHandle<SceneInstance> m_NextLevel;
    public AsyncOperationHandle<SceneInstance> m_CurrentUnloadLevel;
    public bool isPlayFirstTime = true;

    [Header("Ads")]
    public GameObject go_LoadingAds;
    public int m_LoadBannerGap = 0;

    public void SetLoadingPopup(bool _value)
    {
        go_LoadingAds.SetActive(_value);
    }

    public void CloseLoadingAdsPopup()
    {
        go_LoadingAds.SetActive(false);
    }

    [Sirenix.OdinInspector.Button]
    public void TestLoad()
    {
        // StartCoroutine(IETestLoad());
        TaskTestLoad();
    }

    async UniTask TaskTestLoad()
    {
        go_LoadingAds.SetActive(true);
        // // LoadInterstitial();

        Helper.DebugLog("Open loading ads");

        // Helper.DebugLog("BBBBBBBBBBBBB");
        // yield return Yielders.Get(1f);
        // Helper.DebugLog("AAAAAAAAAAAAA");

        await UniTask.Delay(1500, true);

        // yield return Yielders.EndOfFrame;
        // await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);

        Helper.DebugLog("Close loading ads");

        go_LoadingAds.SetActive(false);
    }

    public void TestFirebaseFetch()
    {
        // FirebaseManager.Instance.remoteConfig.ActivateAsync();
        // FirebaseManager.Instance.remoteConfig.FetchAsync(TimeSpan.Zero).ContinueWith(task =>
        // {
        //     // handle completion
        //     ConfigValue open_aoa = FirebaseManager.Instance.remoteConfig.GetValue("open_aoa");
        //     ConfigValue inter_min_time = FirebaseManager.Instance.remoteConfig.GetValue("inter_min_time");

        //     // ProfileManager.MyProfile.m_InterTime.Reset(); 
        //     // ProfileManager.MyProfile.m_InterTime = new TimeRefillUnit((int)config.DoubleValue, 1, 0, "inter_cd_time", "inter_cd_time");
        //     // ProfileManager.MyProfile.m_InterTime.Reset();

        //     // GameManager.Instance.m_InterTime = (int)config.DoubleValue;

        //     Helper.DebugLog("open_aoa: " + open_aoa.BooleanValue.ToString());
        //     Helper.DebugLog("inter_min_time: " + inter_min_time.BooleanValue.ToString());
        //     // Helper.DebugLog("Config Value: " + config.DoubleValue.ToString());
        //     // ProfileManager.MyProfile.m_InterTime = (TimeRefillUnit)config.DoubleValue;
        // });
        // Helper.DebugLog("Can Show Ads: " + AppOpenAdManager.Instance.canShowAd);
    }

    // public void TestInter()
    // {
    //     AdsManager.Instance.WatchInterstitial(InterType.NONE);
    // }

    // public void TestReward()
    // {
    //     AdsManager.Instance.WatchRewardVideo(RewardType.X2_GOLD_WIN);
    // }

    public async UniTask LoadPlayScene()
    {
        // StartCoroutine(LoadPlayScreen());

        // EventManager.CallEvent(GameEvent.DespawnAllPool);
        // Time.timeScale = 1;
        // await UniTask.WaitUntil(() => AppOpenAdManager.Instance.canShowAd == true);

        // AppOpenAdManager.Instance.canShowAd = true;

        go_CheatPanel.SetActive(false);
        g_Loading.SetActive(true);

        m_LoadBannerGap++;

        if (img_LoadingBar != null)
        {
            float _loadProgress = 0;
            while (_loadProgress <= 1)
            {
                _loadProgress += 0.05f;
                img_LoadingBar.fillAmount = _loadProgress;
                int percent = (int)(_loadProgress * 100f);
                if (percent > 100) percent = 100;
                // yield return new WaitForSeconds(Time.deltaTime);
                int time = (int)(Time.deltaTime * 1000);
                await UniTask.Delay(time);
            }
        }

        // ObjectsManager.Instance.m_Team1.Clear();
        // ObjectsManager.Instance.m_Team2.Clear();

        SimplePool.Release();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        FindMainCanvas();

        // String curScene = m_Levels[1].SceneName;

        String scene = "MainScene";

        Addressables.LoadSceneAsync(scene, LoadSceneMode.Single).Completed += LoadPlaySceneCompleted;

        // if (isPlayFirstTime)
        // {
        //     isPlayFirstTime = false;
        //     var curLevel = Addressables.LoadSceneAsync(curScene, LoadSceneMode.Single);

        //     // curLevel.Completed += LoadPlaySceneCompleted;

        //     m_CurrentLevel = curLevel;

        //     m_CurrentLevel.Completed += LoadPlaySceneCompleted;
        // }
        // else
        // {
        //     m_CurrentLevel.Completed -= LoadPlaySceneCompleted;
        //     LoadSceneNotFirstTime();
        // }
    }

    void LoadPlaySceneCompleted(AsyncOperationHandle<SceneInstance> _scene)
    {
        if (_scene.Status == AsyncOperationStatus.Succeeded)
        {
            // AnalysticsManager.LogPlayLevel(ProfileManager.GetLevel());
            // Helper.DebugLog("LoadPlayScene Completed");
            // String nextScene = m_Levels[ProfileManager.GetLevel()].SceneName;
            // var nextLevel = Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Additive, false);
            // m_NextLevel = nextLevel;
            // if (m_LoadBannerGap > 2)
            // {
            //     AdsManager.Instance.LoadBanner();
            // }
        }
    }

    // public async UniTask LoadSceneNotFirstTime()
    // {
    //     await UniTask.WaitUntil(() => m_NextLevel.Result.ActivateAsync().isDone == true);

    //     UnloadScene3();

    //     // // String curScene = m_Levels[ProfileManager.GetLevel() - 1].SceneName;
    //     // String nextScene = m_Levels[ProfileManager.GetLevel()].SceneName;

    //     // // var curLevel = Addressables.LoadSceneAsync(curScene, LoadSceneMode.Additive, true);
    //     // var nextLevel = Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Additive, false);

    //     // // m_CurrentLevel = curLevel;
    //     // m_NextLevel = nextLevel;
    // }

    // void UnloadScene3()
    // {
    //     Addressables.UnloadSceneAsync(m_CurrentLevel, true).Completed += op =>
    //     {
    //         if (op.Status == AsyncOperationStatus.Succeeded)
    //         {
    //             Helper.DebugLog("YYYYYYYYYYYYYYYYYYY");
    //             m_CurrentLevel = m_NextLevel;
    //             String nextScene = m_Levels[ProfileManager.GetLevel()].SceneName;
    //             var nextLevel = Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Additive, false);
    //             m_NextLevel = nextLevel;
    //             // LoadSceneNotFirstTime();
    //         }
    //     };
    // }

    // void UnloadScene(AsyncOperationHandle<SceneInstance> _unloadScene)
    // {
    //     Addressables.UnloadSceneAsync(_unloadScene, true).Completed += op =>
    //     {
    //         if (op.Status == AsyncOperationStatus.Succeeded)
    //         {
    //             Helper.DebugLog("YYYYYYYYYYYYYYYYYYY");
    //             m_CurrentLevel = m_NextLevel;
    //             String nextScene = m_Levels[ProfileManager.GetLevel()].SceneName;
    //             var nextLevel = Addressables.LoadSceneAsync(nextScene, LoadSceneMode.Additive, false);
    //             m_NextLevel = nextLevel;
    //             // LoadSceneNotFirstTime();
    //         }
    //     };
    // }

    void UnloadScene2(AsyncOperationHandle<SceneInstance> _unloadScene)
    {
        Addressables.UnloadSceneAsync(_unloadScene, true).Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {
                Helper.DebugLog("YYYYYYYYYYYYYYYYYYY");
            }
        };
    }

    [Button]
    public void TestUnloadScene()
    {
        UnloadScene2(m_CurrentLevel);
    }

    [Button]
    public void TestLoadSceneNotActivated()
    {
        m_NextLevel.Result.Activate();
    }

    // [Button]
    // public void TestLoadPlayScene()
    // {
    //     ProfileManager.SetLevel(ProfileManager.GetLevel() + 1);
    //     LoadPlayScene();
    // }

    #endregion

    IEnumerator SetCamCharFirstTime()
    {
        yield return Yielders.EndOfFrame;
        // EventManager1<bool>.CallEvent(GameEvent.SET_CMLOOK_TARGET, true);
    }

    void UnloadScene()
    {
        Addressables.UnloadSceneAsync(handle, true).Completed += op =>
        {
            if (op.Status == AsyncOperationStatus.Succeeded)
            {

            }
        };
    }

    IEnumerator LoadPlayScreen()
    {
        Debug.Log("Start Load");
        g_Loading.SetActive(true);

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        async = SceneManager.LoadSceneAsync("PlayScene", LoadSceneMode.Single);
        async.allowSceneActivation = false;

        // ObjectsManager.Instance.m_Team1.Clear();
        // ObjectsManager.Instance.m_Team2.Clear();

        while (async.progress < 0.9f)
        {
            yield return null;
        }
        SimplePool.Release();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        async.allowSceneActivation = true;

        yield return Yielders.Get(0.1f);

        FindMainCanvas();

        yield return Yielders.Get(0.1f);

        // ObjectsManager.Instance.StartGame();

    }

    public void LoadMainMenu()
    {
        // StartCoroutine(LoadMainMenuScreen());

        g_Loading.SetActive(true);

        SimplePool.Release();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        FindMainCanvas();

        Addressables.LoadSceneAsync(m_MainMenuScene, LoadSceneMode.Single).Completed += LoadMainMenuSceneCompleted;
    }

    void LoadMainMenuSceneCompleted(AsyncOperationHandle<SceneInstance> _scene)
    {
        if (_scene.Status == AsyncOperationStatus.Succeeded)
        {
            g_Loading.SetActive(false);
            handle = _scene;
        }
    }

    IEnumerator LoadMainMenuScreen()
    {
        Debug.Log("Start Load");
        g_Loading.SetActive(true);

        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;

        async = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        async.allowSceneActivation = false;

        while (async.progress < 0.9f)
        {
            yield return null;
        }
        SimplePool.Release();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        async.allowSceneActivation = true;

        yield return Yielders.Get(0.1f);

        FindMainCanvas();

        yield return Yielders.Get(0.1f);

        g_Loading.SetActive(false);

    }

    public GameObject GetGOPanelLoading()
    {
        return g_PanelLoading;
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if (!isLoadInitScene)
        {
            isLoadInitScene = true;
            // GameManager.Instance.ChangeToPlayScene(false);
        }
    }

    public void AddClickEvent(Button _bt, UnityAction _callback)
    {
        _bt.onClick.AddListener(() =>
        {
            // SoundManager.Instance.PlayButtonClick();
            if (_callback != null)
            {
                _callback();
            }
        });
    }

    public void RegisterUI(UICanvas uicanvas)
    {
        int id = (int)uicanvas.ID();

        UICanvas rUI = null;
        if (m_GUIMap.TryGetValue(id, out rUI))
        {
            m_GUIMap[id] = uicanvas;
            //Debug.Log(m_GUIMap.m_ID + " Register overlap " + uicanvas.ID());
        }
        else
        {
            m_GUIMap.Add((int)uicanvas.ID(), uicanvas);
            //Debug.Log(m_GUIMap.m_ID + " Register add " + uicanvas.ID());
        }
    }

    public UICanvas GetUICanvasByID(UIID uid)
    {
        return GetUICanvasByID((int)uid);
    }

    public void FindMainCanvas()
    {
        m_MainCanvas = GameObject.Find("MainCanvas");
    }

    // public void FindPanelLoading()
    // {
    //     m_PanelLoading = GameObject.FindObjectOfType<PanelLoading>().GetComponent<PanelLoading>();
    // }

    // public PanelInGame FindPanelInGame()
    // {
    //     if (m_PanelInGame == null)
    //     {
    //         m_PanelInGame = FindObjectOfType<PanelInGame>().GetComponent<PanelInGame>();
    //     }
    //     return m_PanelInGame;
    // }

    public UICanvas GetUICanvasByID(int id)
    {
        if (m_MainCanvas == null)
        {
            FindMainCanvas();
        }
        UICanvas rUI = null;
        m_GUIMap.TryGetValue(id, out rUI);
        if (rUI == null)
        {
            string name = GetUIName((UIID)id);
            GameObject go = FindObject(m_MainCanvas, name);
            if (go != null)
            {
                go.gameObject.SetActive(true);
                return go.GetComponent<UICanvas>();
            }
            else
            {
                GameObject prefab = GetPrefab((UIID)id);
                if (prefab == null) return null;
                GameObject goUICanvas = GameObject.Instantiate(prefab, m_MainCanvas.transform.position, Quaternion.identity, m_MainCanvas.transform);
                RectTransform rect = goUICanvas.GetComponent<RectTransform>();
                rect.SetParent(m_MainCanvas.GetComponent<RectTransform>());
                rect.localPosition = new Vector3(0, 0, 0);
                rect.localScale = new Vector3(1, 1, 1);
                return goUICanvas.GetComponent<UICanvas>();
            }
        }
        else
        {
            return rUI;
        }
    }

    public void HideUIPopup(UICanvas popup, bool isClosed, bool isOpenPrevious = true)
    {
        if (popup == null) return;
        popup.HidePopup();

        if (isClosed || isOpenPrevious)
        {
            if (m_CurrentOpenedPopup.Contains(popup))
            {
                m_CurrentOpenedPopup.Remove(popup);
                CleanOpenPopup();
            }
        }
        if (isOpenPrevious)
        {
            if (m_CurrentOpenedPopup.Count > 0)
            {
                UICanvas previusPopup = m_CurrentOpenedPopup[m_CurrentOpenedPopup.Count - 1];
                ShowUIPopup(previusPopup, false);
            }
        }
    }

    public void ShowUIPopup(UICanvas popup, bool isClosePrevious = true, bool isSetup = true)
    {
        ShowUIPopup(popup, GetCenterPosition(), isClosePrevious, isSetup);
    }

    // IEnumerator IEShowUIPopup(UICanvas popup, bool isClosePrevious = true)
    // {
    //     // Yielders.Get();
    // }

    public void ShowUIPopup(UICanvas popup, Vector3 position, bool isClosePreviousPopup = true, bool isSetup = true)
    {
        if (popup == null) return;
        if (m_CurrentOpenedPopup.Count > 0)
        {
            m_PreviousPopup = m_CurrentOpenedPopup[m_CurrentOpenedPopup.Count - 1];
            if (isClosePreviousPopup)
            {
                HideUIPopup(m_PreviousPopup, m_PreviousPopup.IsAutoRemove, false);
            }
        }
        popup.ShowPopup(isSetup);
        popup.SetLocalPosition(position);
        if (!m_CurrentOpenedPopup.Contains(popup))
        {
            m_CurrentOpenedPopup.Add(popup);
            CleanOpenPopup();
        }
    }
    public Vector3 GetCenterPosition()
    {
        return m_CenterPos;
    }

    public void ShowUIPanel(UIID id)
    {
        //Debug.Log(id.ToString());
        UICanvas panel = GetUICanvasByID((int)id);
        ShowUIPanel(panel, m_MainCanvas.transform.position);
    }
    public void ShowUIPanel(UIID id, Vector3 position)
    {
        UICanvas panel = GetUICanvasByID((int)id);
        ShowUIPanel(panel, position);
    }
    public void ShowUIPanel(UICanvas panel)
    {
        ShowUIPanel(panel, m_MainCanvas.transform.position);
    }
    public void ShowUIPanel(UICanvas panel, Vector3 position, bool isLocalPositon = false)
    {
        if (panel == null)
        {
            //Debug.Log(" NULL");
            return;
        }
        if (m_CurrentOpenedPanel.Count > 0)
        {
            m_PreviousPanel = m_CurrentOpenedPanel[m_CurrentOpenedPanel.Count - 1];
        }
        panel.ShowPanel();

        Vector3 v = Vector3.zero + new Vector3(0, m_OffsetTop);
        v.z = 0;
        panel.RectTransform.localPosition = v;
        if (!m_CurrentOpenedPanel.Contains(panel))
        {
            m_CurrentOpenedPanel.Add(panel);
        }
        //Debug.Log(" Complete Show");
    }

    public void CleanOpenPopup()
    {
        int num = 0;
        while (num < m_CurrentOpenedPopup.Count)
        {
            if (m_CurrentOpenedPopup[num] == null)
            {
                m_CurrentOpenedPopup.RemoveAt(num);
                continue;
            }
            num++;
        }
    }

    public GameObject GetPrefab(UIID uid)
    {
        string prefabName = "";
        GameObject prefab = null;
        switch (uid)
        {
            case UIID.POPUP_PAUSE:
                prefabName = "PopupPause";
                break;
            case UIID.POPUP_WIN:
                prefabName = "PopupWin";
                break;
            case UIID.POPUP_LOSE:
                prefabName = "PopupLose";
                break;
            case UIID.POPUP_INVENTORY:
                prefabName = "PopupInventory";
                break;
        }
        prefab = GetPopupPrefabByName(prefabName);
        return prefab;
    }

    public UICanvas GetCurrentPopup()
    {
        int num = m_CurrentOpenedPopup.Count;
        if (num > 0)
            return m_CurrentOpenedPopup[num - 1];
        return null;
    }

    public GameObject GetPopupPrefabByName(string name)
    {
        GameObject go = Resources.Load<GameObject>("UI/Popups/" + name);
        return go;
    }

    public string GetUIName(UIID uiid)
    {
        string name = "";
        switch (uiid)
        {

        }
        return name;
    }

    public GameObject FindObject(GameObject parent, string name)
    {
        if (parent == null) return null;
        foreach (Transform t in parent.transform)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }
        return null;
    }
}