#if UNITY_IOS
using System;
using System.Collections;
using Unity.Notifications.iOS;

public class IOSNotificationController
{
    /// <summary>
    /// iOSの通知を送信する。
    /// </summary>
    /// <param name="title">タイトル。</param>
    /// <param name="detail">詳細。</param>
    /// <param name="notificationTimeInterval">通知までの時間。</param>
    /// <param name="identifier">識別子。</param>
    /// <param name="isRepeat">通知を繰り返すか。</param>
    static public void SendIOSNotification(string title, string detail, TimeSpan notificationTimeInterval, string identifier, bool isRepeat = false)
    {
        // すでにスケジュール登録されている通知であれば削除する。
        CancelIOSNotificationIfScheduled(identifier);

        // 通知を作成。
        var notification = CreateCommonNotification(title, detail, identifier);

        // 通知時間を設定。
        var timeTrigger = new iOSNotificationTimeIntervalTrigger();
        timeTrigger.TimeInterval = notificationTimeInterval;
        timeTrigger.Repeats = isRepeat;
        notification.Trigger = timeTrigger;

        // 通知を送信。
        iOSNotificationCenter.ScheduleNotification(notification);
    }

    /// <summary>
    /// iOSの通知を送信する。
    /// </summary>
    /// <param name="title">タイトル。</param>
    /// <param name="detail">詳細。</param>
    /// <param name="identifier">識別子。</param>
    /// <param name="notificationTime">通知時間。</param>
    /// <param name="identifier">識別子。</param>
    /// <param name="isRepeat">通知を繰り返すか。</param>
    static public void SendIOSNotification(string title, string detail, DateTime notificationTime, string identifier, bool isRepeat = false)
    {
        // すでにスケジュール登録されている通知であれば削除する。
        CancelIOSNotificationIfScheduled(identifier);

        // 通知を作成。
        var notification = CreateCommonNotification(title, detail, identifier);

        // 通知時間を設定。
        var timeTrigger = new iOSNotificationCalendarTrigger();
        timeTrigger.Year = notificationTime.Year;
        timeTrigger.Month = notificationTime.Month;
        timeTrigger.Day = notificationTime.Day;
        timeTrigger.Hour = notificationTime.Hour;
        timeTrigger.Minute = notificationTime.Minute;
        timeTrigger.Second = notificationTime.Second;
        timeTrigger.Repeats = isRepeat;
        notification.Trigger = timeTrigger;

        // 通知を送信。
        iOSNotificationCenter.ScheduleNotification(notification);
    }

    /// <summary>
    /// 通知共通生成処理。
    /// </summary>
    /// <param name="title">タイトル。</param>
    /// <param name="detail">詳細。</param>
    /// <param name="identifier">識別子。</param>
    /// <returns>通知データ。</returns>
    static iOSNotification CreateCommonNotification(string title, string detail, string identifier)
    {
        // 通知を作成。
        var notification = new iOSNotification();
        notification.Title = title;
        notification.Body = detail;
        notification.Identifier = identifier;
        return notification;
    }

    /// <summary>
    /// iOSの通知を全てキャンセルする。
    /// </summary>
    static public void CancelAllIOSNotification()
    {
        iOSNotificationCenter.RemoveAllScheduledNotifications();
    }

    /// <summary>
    /// スケジュールされていれば、iOSの通知をキャンセルする。 
    /// </summary>
    /// <param name="identifier">識別子。</param>
    /// <returns>成功したかどうか。</returns>
    static public bool CancelIOSNotificationIfScheduled(string identifier)
    {
        if(!IsScheduledIOSNotification(identifier))
        {
            return false;
        }

        iOSNotificationCenter.RemoveScheduledNotification(identifier);
        return true;
    }

    /// <summary>
    /// スケジュールされているiOSの通知かどうか。
    /// </summary>
    /// <param name="identifier">識別子。</param>
    /// <returns>スケジュールされている通知かどうか</returns>
    static public bool IsScheduledIOSNotification(string identifier)
    {
        var scheduledNotifications = iOSNotificationCenter.GetScheduledNotifications();
        foreach(var scheduledNotification in scheduledNotifications)
        {
            if(scheduledNotification.Identifier == identifier)
            {
                return true;
            }
        }
        return false;
    }
}
#endif