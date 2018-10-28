using System.Collections;
using UnityEngine;

public class CardBechaviour : MonoBehaviour
{
    public bool isReady;
    public int x;

    GameController gameController;
    bool selected = false;
    bool rotating = false, movingUp = false, movingDown = false;

    private Vector3 rotator;

    void Start()
    {
        gameController = GetComponentInParent<GameController>();
        rotator = new Vector3(0f, 3f, 0f);
        isReady = false;
    }

    void Update()
    {


    }

    public void Select()
    {
        if (!selected)
        {
            if (gameController.AddCard(gameObject))
            {
                selected = true;
                rotating = true;
                StartCoroutine(CardMovement());
            }
        }
    }

    public void Unselect()
    {
        if (selected)
        {
            Debug.Log("Unselect");
            selected = false;
            rotating = true;
            StartCoroutine(CardMovement());
        }
    }

    IEnumerator CardMovement()
    {

        while (rotating)
        {
            Quaternion oldRotation = transform.rotation;
            transform.Rotate(rotator);
            if ((oldRotation.y > transform.rotation.y || transform.rotation.y == 1) && selected)
            {
                transform.rotation = new Quaternion(0, 180, 0, 0);
                rotating = false;
                isReady = true;
            }
            else if ((oldRotation.y < transform.rotation.y || transform.rotation.y <= 0) && !selected)
            {
                transform.rotation = new Quaternion();
                rotating = false;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }
}
