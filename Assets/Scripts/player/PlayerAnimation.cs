using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetRunningAnim(bool isRunning)
    {
        if (anim != null)
        {
            anim.SetBool("Running", isRunning);
        }
    }

    public void SetAttackingAnim()
    {
        anim.SetTrigger("Attacking");
    }

    public void SetHitAnim(bool hitByBomb)
    {
        if (hitByBomb)
        {
            anim.SetTrigger("BombHit");
        }
        else
        {
            anim.SetTrigger("EnemyHit");
        }
    }
}
