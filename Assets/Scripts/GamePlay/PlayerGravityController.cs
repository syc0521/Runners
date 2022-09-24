using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravityController : MonoBehaviour
{
    private Rigidbody rb;
    public float defaultGravity = -9.81f;

    private float g;
    public float G
    {
        get
        {
            return g;
        }
        set
        {
            g = value;
        }
    }

    public OnValueChangedEventListener<bool> useGravity;


    private void Awake()
    {
        useGravity = new OnValueChangedEventListener<bool>();
        rb = GetComponent<Rigidbody>();
        useGravity.OnValueChangedEvent += newValue =>
        {
            if (newValue)
            {
                g = defaultGravity;
            }
            else
            {
                g = 0f;
            }
        };
        useGravity.Value = true;
    }
}
