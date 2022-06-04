using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimations
{
    private Dictionary<Animation, string> animations;
    public enum Animation
    {
        Idle,
        TakeDamage,
        MoveRight,
        MoveForward,
        MoveBackward
    }

    public EnemyAnimations(string tag)
    {
        if (tag.Contains("Slime"))
        {
            string[] animation = GameManager.GetEnemyTypeByTag(tag).GetTypeAnimationNames();
            animations = new Dictionary<Animation, string>
            {
                {Animation.Idle,  animation[0]},
                {Animation.TakeDamage, animation[1]}
            };
        }
        else if (tag.CompareTo("Ghost") == 0)
        {
            string[] animation = GameManager.GetEnemyTypeByTag(tag).GetTypeAnimationNames();
            animations = new Dictionary<Animation, string>
            {
                {Animation.Idle,  animation[0]},
                {Animation.MoveRight, animation[1]},
                {Animation.MoveBackward, animation[2]},
                {Animation.TakeDamage, animation[3]}
            };
        }
    }

    public string GetAnimation(Animation name)
    {
        return animations[name];
    }
}
