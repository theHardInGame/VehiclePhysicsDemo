using UnityEngine;
using System;
using Unity.Collections;

public sealed class WheelGO : MonoBehaviour
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