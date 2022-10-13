using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAnimation : MonoBehaviour
{

    [SerializeField]private Animator _animator;
    [SerializeField] private string wheels = "wheels";

    // Start is called before the first frame update
    void Start()
    {
        //car.Play(wheels);
    }

    public void SetAnimationProgress(float ratio)
    {
        _animator.SetFloat("progress", ratio);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
