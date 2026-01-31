using UnityEngine;
using System;
using Unity.Collections;

public sealed class WheelGO : MonoBehaviour
{
    [SerializeField, ReadOnly] private string id;
    public string ID => id;

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

    private void Awake()
    {
        if (string.IsNullOrEmpty(id))
        {
            id = Guid.NewGuid().ToString();
        }
    }
}