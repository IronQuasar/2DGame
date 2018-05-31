using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    int CubesLvlMax = 3;
    int CubesLvlMin = 1;
    bool initialcords = true;
    bool missString = true;
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] public RectTransform rect;
    [SerializeField] private GameObject objBall;
    public GameObject BottomCollison;
    public List<SceneObject> SceneObjects { get; set; }
    public List<BallContoller> balList = new List<BallContoller>();
    public static GameController I;
    private float bonusChance = 0.1f;
    private float cubeChance = 0.8f;
    private Color initColor = new Color(0.71f, 0.6f, 0.3f);
    public int count;
    public int countball = 1;
    public int cubecount;


    public RectTransform CanvasTransform
    {
       
        get
        {
            return rect;
        }
        set
        {
            rect = value;
        }
    }


    public void InitNewRound()
    {
        int colorChoise;
        int minLimit = 1;
        int maxLimit = 2;
        int testChanse = 0;
        int count = default(int);


        for (int i = 0; i < 1; i++)
        {
            for (int j = 0; j < cubecount; j++)
            {

                if (Random.value > 1 - cubeChance)
                {
                    testChanse++;
                    continue;
                }
                else
                {
                    var cube = Instantiate(cubePrefab);
                    var colorCube = cube.GetComponent<ColorCube>();
                    SceneObjects.Add(colorCube);
                    colorCube.transform.SetParent(rect);
                    colorCube.transform.localPosition = new Vector2(-(Screen.width / 2-53) + (j * 100),Screen.height/2-53);
                }

                colorChoise = Random.Range(minLimit, maxLimit);
                SceneObjects[count].GetComponent<ColorCube>().Init(colorChoise, Color.HSVToRGB(initColor.r, initColor.g, initColor.b * colorChoise / 15));
                count++;
            }
            TestMissChance(testChanse);

        }


    }

    public void InitNewRow()
    {
        StartCoroutine(DoInitNewRow());
    }

    private IEnumerator DoInitNewRow()
    {
        if (missString)
        {
            for (int i = 0; i < SceneObjects.Count; i++)
            {
                var objPosition = SceneObjects[i].transform.position;
                var target = new Vector2(objPosition.x, objPosition.y - 100);
                SceneObjects[i].MoveObject(target);
            }
        }
        missString = true;
        count = SceneObjects.Count;
        int colorChoise;
        int testChance = 0;

        ChangeLvlCube();

        for (int i = 0; i < cubecount; i++)
        {
            var objectPosition = new Vector2(-(Screen.width / 2 - 53) + (i * 100), Screen.height / 2 - 53);
            if (Random.value > 1 - cubeChance)
            {
                testChance++;
                var canGenerateBonus = Random.value > 1 - bonusChance;
                if (canGenerateBonus)
                {
                    BonusManager.I.GenerateBonus(BonusType.NewBall, objectPosition);
                    count++;
                }
                continue;

            }
            else
            {
                var cube = Instantiate(cubePrefab);
                var colorCube = cube.GetComponent<ColorCube>();
                SceneObjects.Add(colorCube);
                colorCube.transform.SetParent(rect);
                colorCube.transform.localPosition = objectPosition;

            }

            colorChoise = Random.Range(CubesLvlMin, CubesLvlMax);


            if (SceneObjects[count].GetComponent<ColorCube>() != null)
            {
                SceneObjects[count].GetComponent<ColorCube>().Init(colorChoise, Color.HSVToRGB(0.71f, 0.6f, 0.3f * colorChoise / 15));
            }
            count++;
        }
        addTestMissChanse(testChance);

        if (CubesLvlMax - CubesLvlMin > 5)
        {
            CubesLvlMin++;
        }

        CubesLvlMax++;
        yield return new WaitForSeconds(0.2f);
    }

    private void addTestMissChanse(int testChanse)
    {
        if (testChanse == cubecount)
        {
            missString = false;
            InitNewRow();
        }

    }

    void Awake()
    {
        if (I == null)
        {
            I = this;
        }
        SceneObjects = new List<SceneObject>();

    }

    public void DestroyAllSceneObject()
    {


        initialcords = false;

        for (int i = 0; i < SceneObjects.Count; i++)
        {
            Destroy(SceneObjects[i].gameObject);
        }
        DestroyAllBalls();
        Application.LoadLevel("MaineMenu");
        SceneObjects.Clear();
        //InitNewRound();
        
    }

    private void ChangeLvlCube()
    {
        if (!initialcords)
        {
            CubesLvlMax = 3;
            CubesLvlMin = 1;
            initialcords = true;
        }

    }



    private void TestMissChance(int testChanse)
    {
        if (testChanse == cubecount)
        {
            InitNewRound();
        }
    }

    public void InitNewBall()
    {
        var firstBall = Instantiate(objBall);
        var componentBalls = firstBall.GetComponent<BallContoller>();
        balList.Add(componentBalls);
        componentBalls.transform.SetParent(rect);
        componentBalls.transform.localPosition = new Vector2(0, -Screen.height / 2 + 25);
    }

    public void addedNewBall(Vector2 stopedPosition)
    {
        for (int i = 0; i < balList.Count; i++)
        {

            if (balList.Count <= BonusManager.I.countBalls)
            {
                var ball = Instantiate(objBall);
                var balls = ball.GetComponent<BallContoller>();
                balList.Add(balls);
                balls.transform.SetParent(rect);
                balls.transform.position = stopedPosition;

            }
        }


    }
    public void DestroyAllBalls()
    {
        for (int i = 0; i < balList.Count; i++)
        {
            Destroy(balList[i].gameObject);
        }
        BonusManager.I.countBalls = 0;
        balList.Clear();
        InitNewBall();
    }
    private void positionBall(Vector2 positionVector)
    {
        for (int i = 0; i < balList.Count; i++)
        {
            balList[i].gameObject.GetComponent<Collider2D>().enabled = false;
            balList[i].gameObject.transform.position = positionVector;

        }

    }
    public void ControllGround(Vector2 position)
    {
        countball++;
        if (countball == balList.Count)
        {

            InitNewRow();
            addedNewBall(position);
            positionBall(position);
        }

    }

    public void ballColliderOn(int countBallList)
    {
        balList[countBallList].gameObject.GetComponent<Collider2D>().enabled = true;
    }

    private void CreateBottomCollider()
    {
        GameObject BottomCollider = new GameObject();
        BottomCollider.gameObject.AddComponent<BoxCollider2D>();
        BottomCollider.name = "BottomCollider";
        BottomCollider.GetComponent<BoxCollider2D>().size = new Vector2(Screen.width, 1);
        BottomCollider.transform.position = new Vector2(GetComponent<Canvas>().transform.position.x, 0);
        BottomCollison = BottomCollider;
    }
    private void CreateLeftCollider()
    {
        GameObject LeftCollider = new GameObject();
        LeftCollider.gameObject.AddComponent<BoxCollider2D>();
        LeftCollider.name = "LeftCollider";
        LeftCollider.GetComponent<BoxCollider2D>().size = new Vector2(1, Screen.height);
        LeftCollider.transform.position = new Vector2(0, GetComponent<Canvas>().transform.position.y);

    }
    private void CrateRightCollider()
    {
        GameObject RightCollider = new GameObject();
        RightCollider.gameObject.AddComponent<BoxCollider2D>();
        RightCollider.name = "RightCollider";
        RightCollider.GetComponent<BoxCollider2D>().size = new Vector2(1, Screen.height);
        RightCollider.transform.position = new Vector2(Screen.width, GetComponent<Canvas>().transform.position.y);

    }
    private void CreateTopCollider()
    {
        GameObject TopCollider = new GameObject();
        TopCollider.gameObject.AddComponent<BoxCollider2D>();
        TopCollider.name = "TopCollider";
        TopCollider.GetComponent<BoxCollider2D>().size = new Vector2(Screen.width, 1);
        TopCollider.transform.position = new Vector2(GetComponent<Canvas>().transform.position.x, Screen.height);

    }
    private void CreateGround()
    {
        GameObject Ground = new GameObject();
        Ground.gameObject.AddComponent<BoxCollider2D>();
        Ground.name = "Ground";
        Ground.tag = "Ground";
        Ground.GetComponent<BoxCollider2D>().isTrigger = true;
        float GroundSize = Screen.height / 5;
        Ground.GetComponent<BoxCollider2D>().size = new Vector2(Screen.width, GroundSize);
        Ground.transform.position = new Vector2(GetComponent<Canvas>().transform.position.x, Screen.height / 5 - GroundSize / 2);
    }
    private void GanerateColliders()
    {
        CreateBottomCollider();
        CreateLeftCollider();
        CrateRightCollider();
        CreateTopCollider();
        CreateGround();
    }
    private void CubesCount() {
        cubecount = Screen.width / 100;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            InitNewRow();
        }
    }


    void Start()
    {
        CubesCount();
        GanerateColliders();
        InitNewRound();
        InitNewBall();
    }
}
