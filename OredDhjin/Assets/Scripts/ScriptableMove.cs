using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ScriptableMove : ScriptableObject {

    public string MoveName;
    public int Damage;
    public BoxCollider2D[] HurtBox;
    public BoxCollider2D[] HitBox;
    FrameData frameData;

    

}
