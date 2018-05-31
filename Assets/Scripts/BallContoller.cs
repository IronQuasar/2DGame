
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BallContoller : MonoBehaviour, IEndDragHandler, IBeginDragHandler, IDragHandler
{
    public static BallContoller I;
    public LineRenderer lineRenderer;

    Vector2 started;
    Vector2 guidingVector;
    private void Awake()
    {
        if (I == null)
        {
            I = this;
        }
    }

    
   
    public void OnBeginDrag(PointerEventData eventData)
    {

        lineRenderer.SetPosition(0, transform.position);

    }

    public void OnDrag(PointerEventData eventData)
    {

        float differenceX = transform.position.x - eventData.position.x;
        float differenceY = eventData.position.y - transform.position.y;
        lineRenderer.SetPosition(1, new Vector2(transform.position.x+ differenceX, (differenceY-transform.position.y)*-1));

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lineRenderer.SetPosition(1, gameObject.transform.position);
        GameController.I.countball = 0;
        StartCoroutine(ArrayBalls());

    }
    private IEnumerator ArrayBalls()
    {
        guidingVector =  transform.position- Input.mousePosition ;
        for (int i = 0; i < GameController.I.balList.Count; i++)
        {
            GameController.I.balList[i].GetComponent<Rigidbody2D>().velocity = guidingVector;

            yield return new WaitForSeconds(0.2f);
            GameController.I.ballColliderOn(i);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject == GameController.I.BottomCollison)
        {
            started = transform.position;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            GameController.I.ControllGround(started);


        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>());

        }
    }

}
