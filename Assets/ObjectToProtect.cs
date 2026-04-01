using UnityEngine;

public class ObjectToProtect : Entity
{
     private Transform player;

    protected override void Awake()
    {
        base.Awake();
        player = FindAnyObjectByType<Player>().transform;
    }

    protected override void Update()
    {
        HandleFLip();
    }

    protected override void HandleFLip()
    {
        if ( player == null)
        {
            return;
        }
        if (player.transform.position.x > transform.position.x && !facingright)
        {
            Flip();
        }
        else if (player.transform.position.x < transform.position.x && facingright)
        {
            Flip();
        }
    }
}
