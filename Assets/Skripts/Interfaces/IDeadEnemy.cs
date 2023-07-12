using UnityEngine;

interface IDeadEnemy
{
    void DeactivateRenderAndCollision(PolygonCollider2D polygonCollider2D);
    void PlayAnimAndEffectsOfDeath(Animator _anim, AudioSource _audioSource, AudioClip _DeathClip);
}
    
