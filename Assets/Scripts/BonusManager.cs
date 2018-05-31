using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    public static BonusManager I;
    public int countBalls;
    void Awake()
    {
        if (I == null)
        {
            I = this;
        }
    }

    public void AddNewBall()
    {
        countBalls++;
    }

    public void GenerateBonus(BonusType bonusType, Vector2 position)
    {
        var bonus = Instantiate(prefab);
        var bonusElement = bonus.GetComponent<BonusElement>();
        bonusElement.Init(bonusType);
        GameController.I.SceneObjects.Add(bonusElement);
        bonus.transform.SetParent(GameController.I.CanvasTransform);
        bonus.transform.localPosition = position;
    }

    public void OnBonusCollected(BonusType bonusType)
    {
        switch (bonusType)
        {
            case BonusType.NewBall:
                break;
            default:
                break;
        }
    }
}
public enum BonusType
{
    NewBall
}
