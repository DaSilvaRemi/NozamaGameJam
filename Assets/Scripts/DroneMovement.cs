using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    private GameObject m_drone;
    private Animator m_animator;
    public Animation anim; 
    float[] movement;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        foreach (AnimationState state in anim)
        {
            state.speed = 0.5F;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
