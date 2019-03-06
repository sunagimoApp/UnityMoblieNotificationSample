using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class NotificationSample : MonoBehaviour
{
    /// <summary>
    /// 10秒後に通知を送る(TimeSpan)ボタン。
    /// </summary>
    [Header("10秒後に通知を送る(TimeSpan)ボタン")]
    [SerializeField]
    Button After10SencondsTimeSpanBtn = null;

    /// <summary>
    /// 10秒後に通知を送る(DateTime)ボタン。
    /// </summary>
    [Header("10秒後に通知を送る(DateTime)ボタン")]
    [SerializeField]
    Button After10SencondsDateTimeBtn = null;

    /// <summary>
    /// 1分後に繰り返し通知を送るボタン。
    /// </summary>
    [Header("1分後に繰り返し通知を送るボタン")]
    [SerializeField]
    Button After1MinuteRepeatBtn = null;

    /// <summary>
    /// 通知をすべてキャンセルするボタン。
    /// </summary>
    [Header("通知をすべてキャンセルするボタン。")]
    [SerializeField]
    Button AllNotificationCancelBtn = null;

    void Start()
    {
        After10SencondsTimeSpanBtn.onClick.AddListener(SendNotificationAfter10SecondsTimeSpan);
        After10SencondsDateTimeBtn.onClick.AddListener(SendNotificationAfter10SecondsDateTime);
        After1MinuteRepeatBtn.onClick.AddListener(SendRepeatNotificationAfter1Minute);
        AllNotificationCancelBtn.onClick.AddListener(AllNotificationCancel);
    }
    
    /// <summary>
    /// 10秒後に通知を送信。
    /// </summary>
    void SendNotificationAfter10SecondsTimeSpan()
    {
        var notificationTimeInterval = new TimeSpan(0, 0, 10);
        NotificationController.SendNotification("通知テスト", "10秒後に通知(TimeSpan)", notificationTimeInterval, NotificationController.NotificationType.TYPE_1);
    }

    /// <summary>
    /// 10秒後に通知を送信。
    /// </summary>
    void SendNotificationAfter10SecondsDateTime()
    {
        var notificationDateTime = DateTime.Now + new TimeSpan(0, 0, 10);
        NotificationController.SendNotification("通知テスト", "10秒後に通知(DateTime)", notificationDateTime, NotificationController.NotificationType.TYPE_1);
    }

    /// <summary>
    /// 1分後に繰り返し通知送信。
    /// </summary>
    void SendRepeatNotificationAfter1Minute()
    {
        var notificationDateTime = DateTime.Now + new TimeSpan(0, 1, 0);
        var repeatInterval = new TimeSpan(0, 1, 0);
        NotificationController.SendNotification("通知テスト", "1分後に繰り返し通知", notificationDateTime, NotificationController.NotificationType.TYPE_1, true, repeatInterval);
    }
    
    /// <summary>
    /// 全ての通知をキャンセルする。
    /// </summary>
    void AllNotificationCancel()
    {
        NotificationController.AllCancelNotification();
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
