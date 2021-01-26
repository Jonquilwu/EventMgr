using System;
using System.Collections.Generic;

public class EventMgr
{
    //委托有多个参数，Action是无参数类型的，修改为委托类型
    private static Dictionary<EventType, Delegate> m_EventTable = new Dictionary<EventType, Delegate>();

    #region 内部方法
    private static void OnListenerAdding(EventType eventType, Delegate callback)
    {
        if (!m_EventTable.ContainsKey(eventType))
        {
            m_EventTable.Add(eventType, null);
        }
        Delegate cb = m_EventTable[eventType];
        if (cb != null && cb.GetType() != callback.GetType())
        {
            throw new Exception(string.Format("添加监听错误，尝试为{0}事件添加不同类型的委托，当前委托类型为{1}，添加委托类型为{2}", eventType, cb.GetType(), callback.GetType()));
        }
    }

    private static void OnListenerRemoving(EventType eventType, Delegate callBack)
    {
        if (m_EventTable.ContainsKey(eventType))
        {
            Delegate onCallBack = m_EventTable[eventType];
            if (onCallBack == null)
            {
                throw new Exception(string.Format("移除监听错误，事件{0}没有对应的委托"));
            }
            else if (onCallBack.GetType() != callBack.GetType())
            {
                throw new Exception(string.Format("移除监听错误，尝试为{0}事件移除不同类型的委托，当前委托类型为{1}，移除委托类型为{2}", eventType, onCallBack.GetType(), callBack.GetType()));
            }
        }
        else
        {
            throw new Exception(string.Format("移除监听错误，尝试移除不存在的事件{0}", eventType));
        }

    }

    private static void OnListenerRemoved(EventType eventType)
    {
        if (m_EventTable[eventType] == null)
        {
            m_EventTable.Remove(eventType);
        }
    }
    #endregion

    #region no params
    /// <summary>
    /// no params
    /// </summary>
    public static void AddListener(EventType eventType, Action callback)
    {
        OnListenerAdding(eventType, (Delegate)callback);
        //一个委托持有多个方法引用
        m_EventTable[eventType] = (Action)m_EventTable[eventType] + callback;
    }

    /// <summary>
    /// no params
    /// </summary>
    public static void RemoveListener(EventType eventType, Action callBack)
    {
        OnListenerRemoving(eventType, (Delegate)callBack);
        m_EventTable[eventType] = (Action)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    #endregion

    #region single params
    /// <summary>
    /// single params
    /// </summary>
    public static void AddListener<T>(EventType eventType, Action<T> callback)
    {
        OnListenerAdding(eventType, (Delegate)callback);
        //一个委托持有多个方法引用
        m_EventTable[eventType] = (Action<T>)m_EventTable[eventType] + callback;
    }

    /// <summary>
    /// single params
    /// </summary>
    public static void RemoveListener<T>(EventType eventType, Action<T> callBack)
    {
        OnListenerRemoving(eventType, (Delegate)callBack);
        m_EventTable[eventType] = (Action<T>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    #endregion

    #region two params
    /// <summary>
    /// two params
    /// </summary>
    public static void AddListener<T,U>(EventType eventType, Action<T,U> callback)
    {
        OnListenerAdding(eventType, (Delegate)callback);
        //一个委托持有多个方法引用
        m_EventTable[eventType] = (Action<T, U>)m_EventTable[eventType] + callback;
    }

    /// <summary>
    /// two params
    /// </summary>
    public static void RemoveListener<T,U>(EventType eventType, Action<T,U> callBack)
    {
        OnListenerRemoving(eventType, (Delegate)callBack);
        m_EventTable[eventType] = (Action<T,U>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    }
    #endregion

    #region three params
    public static void AddListener<T, U, V>(EventType eventType, Action<T, U, V> callback)
    {
        OnListenerAdding(eventType, (Delegate)callback);
        //一个委托持有多个方法引用
        m_EventTable[eventType] = (Action<T, U, V>)m_EventTable[eventType] + callback;
    }
    public static void RemoveListener<T, U, V>(EventType eventType, Action<T, U, V> callBack)
    {
        OnListenerRemoving(eventType, (Delegate)callBack);
        m_EventTable[eventType] = (Action<T, U, V>)m_EventTable[eventType] - callBack;
        OnListenerRemoved(eventType);
    } 
    #endregion

    public static void Broadcast(EventType eventType)
    {
        Delegate d = null;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            Action callback = d as Action;
            if(callback != null)
            {
                callback();
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }

    public static void Broadcast<T>(EventType eventType, T arg)
    {
        Delegate d = null;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            Action<T> callback = d as Action<T>;
            if (callback != null)
            {
                callback(arg);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }

    public static void Broadcast<T,U>(EventType eventType, T arg1, U arg2)
    {
        Delegate d = null;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            Action<T,U> callback = d as Action<T,U>;
            if (callback != null)
            {
                callback(arg1, arg2);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }

    public static void Broadcast<T,U,V>(EventType eventType, T arg1, U arg2, V arg3)
    {
        Delegate d = null;
        if (m_EventTable.TryGetValue(eventType, out d))
        {
            Action<T,U,V> callback = d as Action<T,U,V>;
            if (callback != null)
            {
                callback(arg1, arg2, arg3);
            }
            else
            {
                throw new Exception(string.Format("广播事件错误：事件{0}对应委托具有不同的类型", eventType));
            }
        }
    }
}
