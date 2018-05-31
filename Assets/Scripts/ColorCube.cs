using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorCube : SceneObject
{
    public Text TextNumber;
    public int Number;
    private Image ballImage;


    private void Awake()
    {
        ballImage = GetComponent<Image>();
    }

    public void Init(int number, Color color)
    {
        TextNumber.text = number.ToString();
        Number = number;
        GetComponent<Image>().color = color;
    }

    private void UpdateCubeValues()
    {
        ChangeColor(Number);
        TextNumber.text = Number.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.GetComponent<BallContoller>() != null)
        {
            if (Number <= 1)
            {
                Destroy(gameObject);
                GameController.I.SceneObjects.Remove(this);
            }
            else
            {
                Number--;
                UpdateCubeValues();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GameController.I.DestroyAllSceneObject();
        }
    }


    void ChangeColor(int number)
    {
        var targetColor = Color.HSVToRGB(0.71f, 0.6f, 0.3f * Number / 15);
        StartCoroutine(DoChangeColor(targetColor));
    }

    private IEnumerator DoChangeColor(Color c)
    {
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * 1.3f;
            ballImage.color = Color.Lerp(ballImage.color, c, t);
            yield return null;
        }
    }
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
    public override void MoveObject(Vector2 target)
    {
        StartCoroutine(DoMoveColorCubes(target));
    }
}
