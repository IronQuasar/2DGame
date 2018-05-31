using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusElement : SceneObject
{

    [SerializeField] private BonusType bonusType;

    public IEnumerator DoMoveColorCubes(Vector2 target)
    {
        float t = 0;
        float speed = 1.5f;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            transform.position = Vector2.Lerp(transform.position, target, t);
            yield return null;
        }
    }
    public void Init(BonusType type)
    {
        bonusType = type;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BonusManager.I.AddNewBall();
            Destroy(gameObject);
            GameController.I.SceneObjects.Remove(this);


        }
    }

    public override void MoveObject(Vector2 target)
    {
        StartCoroutine(DoMoveColorCubes(target));
    }
}

