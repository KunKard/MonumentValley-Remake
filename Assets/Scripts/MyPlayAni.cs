using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayAni : MonoBehaviour
{
    public Animator animator;
    public MyPlayController player;

    void Update()
    {
        if (animator != null && player != null)
            animator.SetBool("walking", player.walking);
    }
}
