using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


[RequireComponent(typeof(VisualEffect))]
[ExecuteInEditMode]

public class KatonSpeed : MonoBehaviour
{
    
    [Range(0.0f, 10.0f)] public float SimulationTimeScale = 1.0f;

    private VisualEffect Graph;

    private void Awake()
    {
        // Initialise Graph ici
        Graph = GetComponent<VisualEffect>();
    }

    private void OnValidate()
    {
        Graph = gameObject.GetComponent<VisualEffect>();
    }

    private void Update()
    {
        Graph.playRate = SimulationTimeScale;
    }

}
