using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]
public class UltimateData : MonoBehaviour
{
    [SerializeField] PlayableDirector timeline;

    public HitboxData hitboxPreviewData;
    public UltimateHitboxTrigger hitboxTrigger;

    public Sprite icon;

    public void PlayUltimate()
    {
        timeline.Play();
    }

    private void SetHitboxTriggerSize() //just set the trigger size inside the timeline
    {
        var collider = hitboxTrigger.GetComponent<BoxCollider>();

        collider.center = hitboxPreviewData.hitboxPosition;
        collider.size = hitboxPreviewData.hitboxScale;
    }
}
