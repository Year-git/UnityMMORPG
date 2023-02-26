using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

class Resloader
{
    public static T LoadAsset<T>(string path) where T : UnityEngine.Object
    {
        T prebab = AssetDatabase.LoadAssetAtPath<T>(path);
        return GameObject.Instantiate(prebab);
    }

    public static T LoadResources<T>(string path) where T : UnityEngine.Object
    {
        return Resources.Load<T>(path);
    }
}