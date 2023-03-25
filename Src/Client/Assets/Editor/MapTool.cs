using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Common.Data;

public class MapTool : Editor
{
    [MenuItem("MapTool/Export Telepoeter")]
    public static void ExportTelepoeter()
    {
        DataManager.Instance.Load();

        Scene curScene = EditorSceneManager.GetActiveScene();
        string curScenePath = curScene.path;
        if (curScene.isDirty)
        {
            EditorUtility.DisplayDialog("提示", "请保存当前场景", "确认");
        }

        List<TeleporterObject> teleporterObjects = new List<TeleporterObject>();

        foreach (var map in DataManager.Instance.Maps)
        {
            string path = "Assets/AssetsPackage/Levels/" + map.Value.Resource + ".unity";
            if (!System.IO.File.Exists(path))
                continue;

            EditorSceneManager.OpenScene(path);
            TeleporterObject[] teleporters = GameObject.FindObjectsOfType<TeleporterObject>();
            foreach (var teleporter in teleporters)
            {
                TeleporterDefine teleporterDefine = null;
                if (!DataManager.Instance.Teleporters.TryGetValue(teleporter.Id, out teleporterDefine))
                    continue;

                teleporterDefine.Position = GameObjectTool.WorldToLogicN(teleporter.transform.position);
                teleporterDefine.Direction = GameObjectTool.WorldToLogicN(teleporter.transform.forward);
            }
        }

        DataManager.Instance.SaveTeleporters();
        EditorSceneManager.OpenScene(curScenePath);
        EditorUtility.DisplayDialog("提示", "地图传送点导出成功！", "确认");
    }
}
