using UnityEngine;

public class ladder : interactable
{
    //remove gravity to float in place, move through tp so you cant gain speed, make sure gravity always gets set to normal after leaving ladder to stop glitches
    [SerializeField] private float speed;
    private GameObject player;
    private bool pause = false;
    private float pausePos = 0;
    void FixedUpdate()
    {
        if (player == null && target != null)
        {
            player = target.GetComponent<HitboxPass>().passHost();
        }
        else if (player != null && target == null)
        {
            player = null;
            pause = false;
            pausePos = 0;
        }
        
        if (player != null)
        {
            player.GetComponent<Player>().setYVel(0);
            if (yMove > 0)
            {
                pause = false;
                player.GetComponent<Player>().offsetPos(new Vector2(0,speed));
            } 
            else if (yMove < 0)
            {
                pause = false;
                player.GetComponent<Player>().offsetPos(new Vector2(0,-speed));
            } 
            else if (yMove == 0)
            {
                if (!pause)
                {
                    pausePos = player.GetComponent<Player>().getPosY();
                    pause = true; 
                }
            }
            if (pause)
            {
                player.GetComponent<Player>().setPosY(pausePos);
            }
        }
    }
}
