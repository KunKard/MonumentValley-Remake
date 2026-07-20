using UnityEngine;

public class MyPlayAni : MonoBehaviour
{
    public Animator animator;
    public MyPlayController player;

    void Start()
    {
        if (player != null)
            player.OnWalkingChanged += OnWalkingChanged;
    }

    void OnDestroy()
    {
        if (player != null)
            player.OnWalkingChanged -= OnWalkingChanged;
    }

    void OnWalkingChanged(bool walking)
    {
        if (animator != null)
            animator.SetBool("walking", walking);
    }
}
