using System;
using UnityEngine;

[Serializable]
public class AndroidNotificationStackData
{
    /// <summary>
    /// タイトル。
    /// </summary>
    [SerializeField]
    private string title;

    public string Title {
        set { title = value; }
        get { return title; }
    }

    /// <summary>
    /// テキスト。
    /// </summary>
    [SerializeField]
    private string text;

    public string Text { 
        set { text = value; }
        get { return text; }
    }

    /// <summary>
    /// 通知時間。
    /// </summary>
    [SerializeField]
    private string fireTime;

    public DateTime FireTime {
        set { fireTime = value.ToString(); }
        get { 
            DateTime dateTime;
            DateTime.TryParse(fireTime, out dateTime);
            return dateTime;
        }
    }

    /// <summary>
    /// 繰り返し時間。
    /// </summary>
    [SerializeField]
    private string repeatInterval;

    public TimeSpan? RepeatInterval {
        set { repeatInterval = value.ToString(); }
        get {
            TimeSpan timeSpan;
            TimeSpan.TryParse(repeatInterval, out timeSpan);
            return timeSpan;
        }
    }

    /// <summary>
    /// 通知チャンネルId。
    /// </summary>
    [SerializeField]
    private string notificationChannelId;

    public string NotificationChannelId {
        set { notificationChannelId = value; }
        get { return notificationChannelId; }
    }

    /// <summary>
    /// 通知Id。    
    /// </summary>
    [SerializeField]
    private int notificationid;

    public int NotificationId {
        set { notificationid = value; }
        get { return notificationid; }
    }
    
    /// <summary>
    /// 通知時間が現在の時間を超えているかどうか。
    /// </summary>
    /// <returns>通知時間が現在の時間を超えているかどうか。</returns>
    public bool HasNowTimeExceededFireTime()
    {
        return FireTime <= DateTime.Now;
    }

    /// <summary>
    /// 繰り返し時間が0かどうか。
    /// </summary>
    /// <returns>繰り返し時間が0かどうか。</returns>
    public bool IsRepeatIntervalTimeSpanZero()
    {
        return RepeatInterval >= TimeSpan.Zero;
    }
}
