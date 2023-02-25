using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public Animation animation;

    private void Start()
    {
        animation.Play("CounselAnimation");
    }
}
