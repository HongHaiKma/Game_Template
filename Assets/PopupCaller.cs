using AdOne;

public class PopupCaller
{
    // public static void OpenWinPopup(bool _isClose = false, bool _isSetup = false)
    // {
    //     PopupWin popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupWin) as PopupWin;

    //     GUIManager.Instance.ShowUIPopup(popup, _isClose, _isSetup);
    // }

    // public static void OpenLosePopup(bool _isClose = false, bool _isSetup = false)
    // {
    //     PopupLose popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupWin) as PopupLose;

    //     GUIManager.Instance.ShowUIPopup(popup, _isClose, _isSetup);
    // }

    // public static void OpenPopup(UIID _uiid, bool _isClose = false, bool _isSetup = false)
    // {
    //     UICanvas popup = GUIManager.Instance.GetUICanvasByID(_uiid) as UICanvas;

    //     GUIManager.Instance.ShowUIPopup(popup, _isClose, _isSetup);
    // }

    // public static UICanvas GetPopup(UIID _uiid)
    // {
    //     UICanvas popup = GUIManager.Instance.GetUICanvasByID(_uiid) as UICanvas;
    //     return popup;
    // }

    // public static PopupWin GetPopupWin()
    // {
    //     PopupWin popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupWin) as PopupWin;
    //     return popup;
    // }
    // public static PopupDailyReward GetPopupDaily()
    // {
    //     PopupDailyReward popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupDailyReward) as PopupDailyReward;
    //     return popup;
    // }
    // public static PopupShop GetPopupShop()
    // {
    //     PopupShop popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupShop) as PopupShop;
    //     return popup;
    // }
    // public static PopupRevive GetPopupRevive()
    // {
    //     PopupRevive popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupRevive) as PopupRevive;
    //     return popup;
    // }
    // public static PopupLose GetPopupLose()
    // {
    //     PopupLose popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupLose) as PopupLose;
    //     return popup;
    // }

    // public static PopupSpin GetPopupSpin()
    // {
    //     PopupSpin popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupSpin) as PopupSpin;
    //     return popup;
    // }

    // public static PopupUpgrade GetPopupUpgrade()
    // {
    //     PopupUpgrade popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupUpgrade) as PopupUpgrade;
    //     return popup;
    // }
    // public static PopupUnlockCat GetPopupUnlockCat()
    // {
    //     PopupUnlockCat popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupUnlockCat) as PopupUnlockCat;
    //     return popup;
    // }
    // public static PopupBuyItem GetPopupBuyItem()
    // {
    //     PopupBuyItem popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupBuyItem) as PopupBuyItem;
    //     return popup;
    // }
    // public static PopupTutorial GetPopupTutorial(){
    //     PopupTutorial popup = GUIManager.Instance.GetUICanvasByID(UIID.PopupTutorial) as PopupTutorial;
    //     return popup;
    // }
}

public enum UIID
{
    POPUP_PAUSE = 0,
    POPUP_WIN = 1,
    POPUP_LOSE = 2,
    POPUP_INVENTORY = 3,
}