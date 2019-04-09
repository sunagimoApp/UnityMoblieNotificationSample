using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NotificationSample : MonoBehaviour
{
    /// <summary>
    /// TimeSpanで10秒後に通知を送るボタン。
    /// </summary>
    [Header("TimeSpanで10秒後に通知を送るボタン")]
    [SerializeField]
    Button sendNotificationAfter10SecondsBtnByTimeSpanBtn = null;

    /// <summary>
    /// 10秒後に通知を送る(DateTime)ボタン。
    /// </summary>
    [Header("DateTimeで10秒後に通知を送るボタン")]
    [SerializeField]
    Button sendNotificationAfter10SecondsBtnByDateTime = null;

    /// <summary>
    /// 1分後に繰り返し通知を送るボタン。
    /// </summary>
    [Header("1分後に繰り返し通知を送るボタン")]
    [SerializeField]
    Button SendRepeatNotificationAfter1MinuteBtn = null;

    /// <summary>
    /// 通知をすべてキャンセルするボタン。
    /// </summary>
    [Header("通知をすべてキャンセルするボタン。")]
    [SerializeField]
    Button cancelAllNotificationBtn = null;

    void Start()
    {
        sendNotificationAfter10SecondsBtnByTimeSpanBtn.onClick.AddListener(OnSendNotificationAfter10SecondsByTimeSpan);
        sendNotificationAfter10SecondsBtnByDateTime.onClick.AddListener(OnSendNotificationAfter10SecondsByDateTime);
        SendRepeatNotificationAfter1MinuteBtn.onClick.AddListener(OnSendRepeatNotificationAfter1Minute);
        cancelAllNotificationBtn.onClick.AddListener(OnCancelAllNotifications);
    }
    
    /// <summary>
    /// TimeSpanで10秒後に通知を送信。
    /// </summary>
    void OnSendNotificationAfter10SecondsByTimeSpan()
    {
        var notificationTimeInterval = new TimeSpan(0, 0, 10);
        NotificationController.SendNotification("通知テスト", "10秒後に通知(TimeSpan)", notificationTimeInterval, NotificationController.Notifications.NOTIFICATION_1);
    }

    /// <summary>
    /// DateTimeで10秒後に通知を送信。
    /// </summary>
    void OnSendNotificationAfter10SecondsByDateTime()
    {
        var notificationDateTime = DateTime.Now + new TimeSpan(0, 0, 10);
        NotificationController.SendNotification("通知テスト", "10秒後に通知(DateTime)", notificationDateTime, NotificationController.Notifications.NOTIFICATION_1);
    }

    /// <summary>
    /// 1分後に繰り返し通知送信。
    /// </summary>
    void OnSendRepeatNotificationAfter1Minute()
    {
        var notificationDateTime = DateTime.Now + new TimeSpan(0, 1, 0);
        var repeatInterval = new TimeSpan(0, 1, 0);
        NotificationController.SendNotification("通知テスト", "1分後に繰り返し通知", notificationDateTime, NotificationController.Notifications.NOTIFICATION_1, true, repeatInterval);
    }
    
    /// <summary>
    /// 全ての通知をキャンセルする。
    /// </summary>
    void OnCancelAllNotifications()
    {
        NotificationController.CancelAllNotification();
    }

    void OnApplicationPause(bool isPause)
    {
        // Android用、バックグラウンド通知送信処理。
#if UNITY_ANDROID
        if(isPause)
        {
            AndroidNotificationController.PauseAndroidNotificationAction();
        }
        else
        {
            AndroidNotificationController.ResumeAndroidNotificationAction();
        }
#endif
    }
}
