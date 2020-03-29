using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : MonoBehaviour
{
    private enum LadderPart { complete, bottom, top }
    [SerializeField] LadderPart part = LadderPart.complete;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case LadderPart.complete:
                    player.canClimb = true;
                    player.ladder = this;
                    break;
                case LadderPart.top:
                    player.topLadder = true;
                    break;
                case LadderPart.bottom:
                    player.bottomLadder = true;
                    break;
                default:
                    break;
            }
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case LadderPart.complete:
                    player.canClimb = false;
                    break;
                case LadderPart.top:
                    player.topLadder = false;
                    break;
                case LadderPart.bottom:
                    player.bottomLadder = false;
                    break;
                default:
                    break;
            }
        }
    }
}
