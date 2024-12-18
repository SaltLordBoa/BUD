using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;

    public void SetMoving(bool value)
    {
        if (anim != null)
        {
            anim.SetBool("isMoving", value);
        }
    }

    public void SetSprinting(bool value)
    {
        if (anim != null)
        {
            anim.SetBool("isSprinting", value);

        }
    }

    public void SetFalling(bool value)
    {
        if (anim != null)
        {
            anim.SetBool("isFalling", value);
        }
    }

    public void SetJumping(bool value)
    {
        if (anim != null)
        {
            anim.SetBool("isJumping", value);
        }
    }

}
