using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public class NotificationController
{
    /// <summary>
    /// 通知タイプ。
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// 通知タイプ1。
        /// </summary>
        TYPE_1 = 0,
        
        /// <summary>
        /// 通知タイプ2。
        /// </summary>
        TYPE_2 = 1,

        /// <summary>
        /// 通知タイプ3。
        /// </summary>
        TYPE_3 = 2,
    }

    /// <summary>
    /// 通知タイプをIDに変換。
    /// </summary>
    /// <param name="type">通知タイプ。</param>
    /// <returns>通知タイプID。</returns>
    static public int NotificationTypeToId(NotificationType type)
    {
        switch(type)
        {
            case NotificationType.TYPE_1:
                return 1;
            case NotificationType.TYPE_2:
                return 2;
            case NotificationType.TYPE_3:
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
    static public void SendNotification(string title, string detail, DateTime notificationTime, NotificationType notificationType, bool isRepeat = false, TimeSpan? repeatInterval = null)
    {
        var notificationId = NotificationTypeToId(notificationType);
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
    static public void SendNotification(string title, string detail, TimeSpan notificationTimeInterval, NotificationType notificationType, bool isRepeat = false, TimeSpan? repeatInterval = null)
    {
        var notificationId = NotificationTypeToId(notificationType);
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
    static public void AllCancelNotification()
    {
#if UNITY_ANDROID
        AndroidNotificationController.AllCancelAndroidNotification();
#elif UNITY_IOS
        IOSNotificationController.AllCancelIOSNotification();
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
    static public void CancelNotification(NotificationType notificationType)
    {
        var notificationId = NotificationTypeToId(notificationType);
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
#endif
    }

    /// <summary>
    /// スケジュールされている通知かどうか。
    /// </summary>
    /// <param name="notificationType">通知タイプ。</param>
    /// <returns>スケジュールされている通知かどうか。</returns>
    static public bool IsScheduledNotification(NotificationType notificationType)
    {
        var notificationId = NotificationTypeToId(notificationType);
#if UNITY_ANDROID
        return AndroidNotificationController.IsScheduledAndroidNotification(notificationId);
#elif UNITY_IOS
        return IOSNotificationController.IsScheduledIOSNotification(notificationId.ToString());
#endif
    }
}
