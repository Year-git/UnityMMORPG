using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ViewManager : MonoSingleton<ViewManager>
{
    private Transform root;
    private Dictionary<string, GameObject> viewDictionary = new Dictionary<string, GameObject>();
    void Awake()
    {
        root = GameObject.Find("Canvas").transform;
        DontDestroyOnLoad(root);
    }

    void Update()
    {

    }

    public void CreateView(string viewName)
    {
        if (viewDictionary.ContainsKey(viewName))
        {
            return;
        }
        string path = "Assets/AssetsPackage/UI/UIPrefab/" + viewName + ".prefab";
        GameObject view = Resloader.LoadAsset<GameObject>(path);
        view.name = viewName;
        RectTransform rectTransform = view.GetComponent<RectTransform>();
        rectTransform.SetParent(root);
        rectTransform.offsetMax = Vector2.zero;
        rectTransform.offsetMin = Vector2.zero;

        viewDictionary.Add(viewName, view);
    }

    public void RemoveView(string viewName)
    {
        GameObject view;
        if (!viewDictionary.TryGetValue(viewName, out view))
        {
            return;
        }
        Destroy(view);
    }
}
