using UnityEngine;
using System;
using Unity.Collections;

internal sealed class WheelGO : MonoBehaviour
{
    [SerializeField, ReadOnly] private string id;
    public Guid ID { get; private set; }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
            UnityEditor.EditorUtility.SetDirty(this);
        }
    }
#endif

    public WheelInputState WheelIPS
    {
        get
        {
            wheelIPDNT ??= new();
            return wheelIPDNT;
        }
    }

    public WheelOutputState WheelOPS
    {
        get
        {
            wheelOPDNT ??= new();
            return wheelOPDNT;
        }
    }

    private WheelInputState wheelIPDNT;
    private WheelOutputState wheelOPDNT;

    private void Awake()
    {
        if (string.IsNullOrEmpty(id))
        {
            Debug.LogError("Empty Guid");
            return;
        }

        if (Guid.TryParse(id, out Guid guid)) { ID = guid; }
    }
}