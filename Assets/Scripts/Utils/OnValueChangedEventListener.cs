using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnValueChangedEventListener<T>
{
    public delegate void OnValueChanged(T newValue);
    public event OnValueChanged OnValueChangedEvent;
    private T m_Value;
    public T Value
    {
        set
        {
            if (m_Value.Equals(value)) return;
            else
            {
                m_Value = value;
                OnValueChangedEvent?.Invoke(value);
            }
        }
        get
        {
            return m_Value;
        }
    }

}
