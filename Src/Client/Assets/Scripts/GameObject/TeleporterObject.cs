using Common.Data;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterObject : MonoBehaviour
{

    public int Id;
    //private Mesh mesh;
    void Strat()
    {
        //mesh = transform.GetComponent<MeshFilter>().sharedMesh;
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter -> other = " + other.name);
        EntityController entityController = other.gameObject.GetComponent<EntityController>();
        if (entityController == null && !entityController.isPlayer)
            return;

        TeleporterDefine teleporterDefine = DataManager.Instance.Teleporters[Id];

        if (teleporterDefine == null)
            return;

        if (teleporterDefine.LinkTo > 0 && DataManager.Instance.Teleporters.ContainsKey(teleporterDefine.LinkTo))
        {
            MapService.Instance.SendMapTeleporter(this.Id);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        //if (mesh != null)
        //{
        //    Gizmos.DrawWireMesh(mesh, this.transform.position + Vector3.up * this.transform.localScale.y * .5f, this.transform.rotation, this.transform.localScale);
        //}
        UnityEditor.Handles.color = Color.blue;
        UnityEditor.Handles.ArrowHandleCap(0, this.transform.position, this.transform.rotation, 1f, EventType.Repaint);
    }
#endif
}
