#if UNITY_ANDROID
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

public class AndroidNotificationController
{
    /// <summary>
    /// 初期化が行われたかどうか。
    /// </summary>
    static bool isInitialized = false;

    /// <summary>
    /// Androidの通知チャンネルId。
    /// </summary>
    const string ANDROID_NOTIFICATION_CHANNEL_ID = "channel_id";

    /// <summary>
    /// Androidの通知チャンネル名。
    /// </summary>
    const string ANDROID_NOTIFICATION_CHANNEL_NAME = "channel_name";

    /// <summary>
    /// Androidの通知チャンネル説明。
    /// </summary>
    const string ANDROID_NOTIFICATION_CHANNEL_DESC = "channel_desc";

    /// <summary>
    /// Androidの通知PrefsKey。
    /// </summary>
    const string ANDROID_NOTIFICATION_PREFS_KEY = "android_notification_prefs_key";

    /// <summary>
    /// Android通知スタックリスト。
    /// </summary>
    static List<AndroidNotificationStackData> androidNotificationStackList = new List<AndroidNotificationStackData>();
    
    /// <summary>
    /// 初期化がされていなければ初期化を行う。
    /// </summary>
    static public void InitializeIfNotInitialized()
    {
        if(isInitialized)
        {
            return;
        }

        var channel = new AndroidNotificationChannel();
        channel.Id = ANDROID_NOTIFICATION_CHANNEL_ID;
        channel.Name = ANDROID_NOTIFICATION_CHANNEL_NAME;
        channel.Importance = Importance.High;
        channel.Description = ANDROID_NOTIFICATION_CHANNEL_DESC;

        // 通知チャンネルの登録。
        AndroidNotificationCenter.RegisterNotificationChannel(channel);

        // Prefsから通知データを読み込む。
        LoadPrefsNotificationData();
    }

    /// <summary>
    /// Androidの通知を送信する。
    /// </summary>
    /// <param name="title">タイトル。</param>
    /// <param name="detail">詳細。</param>
    /// <param name="notificationTime">通知時間。</param>
    /// <param name="notificationId">通知Id。</param>
    static public void SendAndroidNotification(string title, string detail, DateTime notificationTime, int notificationId)
    {
        var notification = CommonCreateNotification(title, detail, notificationTime);
        StackAndroidNotificationData(notification, ANDROID_NOTIFICATION_CHANNEL_ID, notificationId);
    }

    /// <summary>
    /// Androidの繰り返し通知を送信する。
    /// </summary>
    /// <param name="title">タイトル。</param>
    /// <param name="detail">詳細。</param>
    /// <param name="notificationTime">通知時間。</param>
    /// <param name="notificationId">通知Id。</param>
    /// <param name="repeatInterval">繰り返し通知間隔。</param>
    static public void SendRepeatAndroidNotification(string title, string detail, DateTime notificationTime, int notificationId, TimeSpan? repeatInterval)
    {
        var notification = CommonCreateNotification(title, detail, notificationTime);
        notification.RepeatInterval =  repeatInterval;
        StackAndroidNotificationData(notification, ANDROID_NOTIFICATION_CHANNEL_ID, notificationId);
    }

    /// <summary>
    /// Androidの通知の共通生成処理。
    /// </summary>
    /// <param name="title">タイトル。</param>
    /// <param name="detail">詳細。</param>
    /// <param name="notificationTime">通知時間。</param>
    /// <returns>Androidの通知データ。</returns>
    static public AndroidNotification CommonCreateNotification(string title, string detail, DateTime notificationTime)
    {
        InitializeIfNotInitialized();

        // 通知を作成。
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = detail;
        notification.FireTime = notificationTime;
        notification.SmallIcon = "icon_0";
        notification.LargeIcon = "icon_1";
        return notification;
    }   

    /// <summary>
    /// Androidの通知データのスタック。
    /// </summary>
    /// <param name="notificationData">通知データ。</param>
    /// <param name="channelId">チャンネルId。</param>
    /// <param name="notificationId">通知Id。</param>
    static void StackAndroidNotificationData(AndroidNotification notificationData, string channelId, int notificationId)
    {
        // すでにスタックに登録されているId、通知時間がすぎているもの、リピート時間が設定されていなければ削除する。
        androidNotificationStackList.RemoveAll(
            e => (e.NotificationId == notificationId || 
            e.HasNowTimeExceededFireTime()) &&
            e.IsRepeatIntervalTimeSpanZero()
        );
    
        var stackData = new AndroidNotificationStackData();
        stackData.Title = notificationData.Title;
        stackData.Text = notificationData.Text;
        stackData.FireTime = notificationData.FireTime;
        if(notificationData.RepeatInterval != null)
        {
            stackData.RepeatInterval = notificationData.RepeatInterval;
        }
        stackData.NotificationChannelId = channelId;
        stackData.NotificationId = notificationId;
        androidNotificationStackList.Add(stackData);

        // Prefsデータに保存。
        SavePrefsNotificationData();
    }

    /// <summary>
    /// Prefs通知データを保存。
    /// </summary>
    static void SavePrefsNotificationData()
    {
        var jsonNotificationData = JsonHelper.ToJson(androidNotificationStackList.ToArray());
        if(PlayerPrefs.HasKey(ANDROID_NOTIFICATION_PREFS_KEY))
        {
            PlayerPrefs.DeleteKey(ANDROID_NOTIFICATION_PREFS_KEY);
        }
        PlayerPrefs.SetString(ANDROID_NOTIFICATION_PREFS_KEY, jsonNotificationData);
    }

    /// <summary>
    /// Prefs通知データを読み込み。
    /// </summary>
    static void LoadPrefsNotificationData()
    {
        if(PlayerPrefs.HasKey(ANDROID_NOTIFICATION_PREFS_KEY))
        {
            var prefsNotificationData = PlayerPrefs.GetString(ANDROID_NOTIFICATION_PREFS_KEY);
            var jsonNotificationData = JsonHelper.FromJson<AndroidNotificationStackData>(prefsNotificationData);
            androidNotificationStackList = jsonNotificationData.ToList();
        }
    }

    /// <summary>
    /// アプリケーション再開時のAndroid通知アクション。
    /// </summary>
    static public void ResumeAndroidNotificationAction()
    {
        // スケジュール登録されている通知をすべてキャンセルする。
        AllCancelAndroidNotification();
    }

    /// <summary>
    /// アプリケーションポーズ時のAndroid通知アクション。
    /// </summary>
    static public void PauseAndroidNotificationAction()
    {
        SendStackAndroidNotification();
    }

    /// <summary>
    /// スタックされたAndroidの通知を送信する。
    /// </summary>
    static void SendStackAndroidNotification()
    {
        InitializeIfNotInitialized();

        // 通知時間がすぎているもの、繰り返し時間が設定されていなければ削除する。
        androidNotificationStackList.RemoveAll(
            e => e.HasNowTimeExceededFireTime() && e.IsRepeatIntervalTimeSpanZero()
        );

        SavePrefsNotificationData();

        foreach(var androidNotificationStackData in androidNotificationStackList)
        {
            var notificationData = CommonCreateNotification(androidNotificationStackData.Title, androidNotificationStackData.Text, androidNotificationStackData.FireTime);
            if(!androidNotificationStackData.IsRepeatIntervalTimeSpanZero())
            {
                notificationData.RepeatInterval = androidNotificationStackData.RepeatInterval;
            }

            AndroidNotificationCenter.SendNotificationWithExplicitID(
                notificationData, 
                androidNotificationStackData.NotificationChannelId, 
                androidNotificationStackData.NotificationId
            );
        }
    }
    
    /// <summary>
    /// Androidの全ての通知をキャンセルする。
    /// </summary>
    static public void AllCancelAndroidNotification()
    {
        LoadPrefsNotificationData();
        AndroidNotificationCenter.CancelAllNotifications();
    }

    /// <summary>
    /// もしスケジュールされていればAndroidの通知をキャンセルする。
    /// </summary>
    /// <param name="notificationId">通知Id。</param>
    /// <returns>成功したかどうか。</returns>
    static public bool CancelAndroidNotificationIfScheduled(int notificationId)
    {
        if(IsScheduledAndroidNotification(notificationId))
        {
            AndroidNotificationCenter.CancelScheduledNotification(notificationId);
            return true;
        }
        return false;
    }

    /// <summary>
    /// スケジュールされているAndroidの通知かどうか。
    /// </summary>
    /// <param name="notificationId">通知Id。</param>
    /// <returns>スケジュールされているAndroidの通知かどうか。</returns>
    static public bool IsScheduledAndroidNotification(int notificationId)
    {
        var notificationStatus = AndroidNotificationCenter.CheckScheduledNotificationStatus(notificationId);
        if(notificationStatus == NotificationStatus.Scheduled)
        {
            return true;
        }
        return false;
    }
}
#endif