using UnityEngine;

public class DollySplineActivator : MonoBehaviour
{
    Animator animator;
    [SerializeField]
    string animationName;
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.Play(animationName, 0);
    }

}
