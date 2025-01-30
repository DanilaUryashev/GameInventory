using System;
using System.Collections.Generic;
using UnityEngine;

public class DefaultItemsList : MonoBehaviour
{
    public static DefaultItemsList Instance { get; private set; } // Singleton

    public List<DefaultItems> defaultItems = new List<DefaultItems>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject); // Чтобы объект не удалялся при загрузке сцены
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
