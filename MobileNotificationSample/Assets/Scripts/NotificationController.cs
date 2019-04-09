using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class NotificationController
{
    /// <summary>
    /// 通知一覧。
    /// </summary>
    public enum Notifications
    {
        /// <summary>
        /// 通知1。
        /// </summary>
        NOTIFICATION_1 = 0,
        
        /// <summary>
        /// 通知2。
        /// </summary>
        NOTIFICATION_2 = 1,

        /// <summary>
        /// 通知3。
        /// </summary>
        NOTIFICATION_3 = 2,
    }

    /// <summary>
    /// 通知一覧をIDに変換。
    /// </summary>
    /// <param name="type">通知タイプ。</param>
    /// <returns>通知タイプID。</returns>
    static public int ToIdNotifications(Notifications notification)
    {
        switch(notification)
        {
            case Notifications.NOTIFICATION_1:
                return 1;
            case Notifications.NOTIFICATION_2:
                return 2;
            case Notifications.NOTIFICATION_3:
                return 3;
            default:
                return 0;
        }
    }

    /// <summary>
    /// 通知を送信する。
    /// </summary>
    /// <param name="title">タイトル。</param>
    /// <param name="detail">詳細。</param>
    /// <param name="notificationTime">通知時間。</param>
    /// <param name="notificationId">通知ID。</param>
    /// <param name="isRepeat">繰り返し通知するか。</param>
    /// <param name="repeatInterval">繰り返し通知間隔。</param>
    static public void SendNotification(string title, string detail, DateTime notificationTime, int notificationId, bool isRepeat = false, TimeSpan? repeatInterval = null)
    {
#if UNITY_ANDROID
        if(isRepeat && repeatInterval != null)
        {
                AndroidNotificationController.SendRepeatAndroidNotification(title, detail, notificationTime, notificationId, repeatInterval);
        }
        else
        {
                AndroidNotificationController.SendAndroidNotification(title, detail, notificationTime, notificationId);
        }
#elif UNITY_IOS
        IOSNotificationController.SendIOSNotification(title, detail, notificationTime, notificationId.ToString(), isRepeat);
#endif
    }

    /// <summary>
    /// 通知を送信する。
    /// </summary>
    /// <param name="title">タイトル。</param>
    /// <param name="detail">詳細。</param>
    /// <param name="notificationTimeInterval">通知までの時間。</param>
    /// <param name="notificationId">通知ID。</param>
    /// <param name="isRepeat">繰り返し通知するか。</param>
    /// <param name="repeatInterval">繰り返し通知間隔。</param>
    static public void SendNotification(string title, string detail, TimeSpan notificationTimeInterval, int notificationId, bool isRepeat = false, TimeSpan? repeatInterval = null)
    {
        var notificationTime = DateTime.Now + notificationTimeInterval;
#if UNITY_ANDROID
        if(isRepeat && repeatInterval != null)
        {
                AndroidNotificationController.SendRepeatAndroidNotification(title, detail, notificationTime, notificationId, repeatInterval);
        }
        else
        {
                AndroidNotificationController.SendAndroidNotification(title, detail, notificationTime, notificationId);
        }
#elif UNITY_IOS
        IOSNotificationController.SendIOSNotification(title, detail, notificationTimeInterval, notificationId.ToString(), isRepeat);
#endif
    }

    /// <summary>
    /// 通知を送信する。
    /// </summary>
    /// <param name="title">タイトル。</param>
    /// <param name="detail">詳細。</param>
    /// <param name="notificationTime">通知時間。</param>
    /// <param name="notificationType">通知タイプ。</param>
    /// <param name="isRepeat">繰り返し通知するか。</param>
    /// <param name="repeatInterval">繰り返し通知間隔。</param>
    static public void SendNotification(string title, string detail, DateTime notificationTime, Notifications notificationType, bool isRepeat = false, TimeSpan? repeatInterval = null)
    {
        var notificationId = ToIdNotifications(notificationType);
#if UNITY_ANDROID
        if(isRepeat && repeatInterval != null)
        {
                AndroidNotificationController.SendRepeatAndroidNotification(title, detail, notificationTime, notificationId, repeatInterval);
        }
        else
        {
                AndroidNotificationController.SendAndroidNotification(title, detail, notificationTime, notificationId);
        }
#elif UNITY_IOS
        IOSNotificationController.SendIOSNotification(title, detail, notificationTime, notificationId.ToString(), isRepeat);
#endif
    }

    /// <summary>
    /// 通知を送信する。
    /// </summary>
    /// <param name="title">タイトル。</param>
    /// <param name="detail">詳細。</param>
    /// <param name="notificationTimeInterval">通知までの時間。</param>
    /// <param name="notificationType">通知タイプ。</param>
    /// <param name="isRepeat">繰り返し通知するか。</param>
    /// <param name="repeatInterval">繰り返し通知間隔。</param>
    static public void SendNotification(string title, string detail, TimeSpan notificationTimeInterval, Notifications notificationType, bool isRepeat = false, TimeSpan? repeatInterval = null)
    {
        var notificationId = ToIdNotifications(notificationType);
        var notificationTime = DateTime.Now + notificationTimeInterval;
#if UNITY_ANDROID
        if(isRepeat && repeatInterval != null)
        {
                AndroidNotificationController.SendRepeatAndroidNotification(title, detail, notificationTime, notificationId, repeatInterval);
        }
        else
        {
                AndroidNotificationController.SendAndroidNotification(title, detail, notificationTime, notificationId);
        }
#elif UNITY_IOS
        IOSNotificationController.SendIOSNotification(title, detail, notificationTimeInterval, notificationId.ToString(), isRepeat);
#endif
    }

    /// <summary>
    /// 全ての通知をキャンセルする。
    /// </summary>
    static public void CancelAllNotification()
    {
#if UNITY_ANDROID
        AndroidNotificationController.CancelAllAndroidNotification();
#elif UNITY_IOS
        IOSNotificationController.CancelAllIOSNotification();
#endif
    }

    /// <summary>
    /// 通知をキャンセルする。
    /// </summary>
    /// <param name="notificationId">通知Id。</param>
    static public void CancelNotification(int notificationId)
    {
#if UNITY_ANDROID
        AndroidNotificationController.CancelAndroidNotificationIfScheduled(notificationId);
#elif UNITY_IOS
        IOSNotificationController.CancelIOSNotificationIfScheduled(notificationId.ToString());
#endif
    }

    /// <summary>
    /// 通知をキャンセルする。
    /// </summary>
    /// <param name="notificationType">通知タイプ。</param>
    static public void CancelNotification(Notifications notificationType)
    {
        var notificationId = ToIdNotifications(notificationType);
#if UNITY_ANDROID
        AndroidNotificationController.CancelAndroidNotificationIfScheduled(notificationId);
#elif UNITY_IOS
        IOSNotificationController.CancelIOSNotificationIfScheduled(notificationId.ToString());
#endif
    }
    
    /// <summary>
    /// スケジュールされている通知かどうか。
    /// </summary>
    /// <param name="notificationId">通知ID。</param>
    /// <returns>スケジュールされている通知かどうか。</returns>
    static public bool IsScheduledNotification(int notificationId)
    {
#if UNITY_ANDROID
        return AndroidNotificationController.IsScheduledAndroidNotification(notificationId);
#elif UNITY_IOS
        return IOSNotificationController.IsScheduledIOSNotification(notificationId.ToString());
#else
        return false;
#endif
    }

    /// <summary>
    /// スケジュールされている通知かどうか。
    /// </summary>
    /// <param name="notificationType">通知タイプ。</param>
    /// <returns>スケジュールされている通知かどうか。</returns>
    static public bool IsScheduledNotification(Notifications notificationType)
    {
        var notificationId = ToIdNotifications(notificationType);
#if UNITY_ANDROID
        return AndroidNotificationController.IsScheduledAndroidNotification(notificationId);
#elif UNITY_IOS
        return IOSNotificationController.IsScheduledIOSNotification(notificationId.ToString());
#else 
        return false;
#endif
    }
}
