using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StackMover : MonoBehaviour
{


    public static StackMover _instance;

    #region unity references
    [SerializeField] private int Score;
    [SerializeField] private Text ScoreText;
    [SerializeField] private GameObject BaseCube;
    [SerializeField] private GameObject MovingCube;
    [SerializeField] private float xAmount;
    [SerializeField] private float zAmount;
    [SerializeField] private float speed;

    [SerializeField] private bool xDirection = true;
    [SerializeField] private float Epsilon;

    [SerializeField] private Transform Xmax;
    [SerializeField] private Transform Xmin;
    [SerializeField] private Transform Zmax;
    [SerializeField] private Transform Zmin;


    [SerializeField] private bool finished = false;
    [SerializeField] private GameObject VeryBaseCube;

    [Range(0, 1)] [SerializeField] private float MinSpeed;
    [Range(0, 1)] [SerializeField] private float MaxSpeed;
    #endregion


    [SerializeField] private List<GameObject> plates;

    private void Awake()
    {
        _instance = this;
    }
    private void OnEnable()
    {

        RemoveAllPlates();
        CubeStart();

        finished = false;
        Score = 0;
        ScoreText.text = Score.ToString();
    }





    private void Update()
    {
        MoveCube();
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CubeCut();
        }
    }






    public void CubeCut()
    {
        if (finished)
        {
            return;
        }


        Vector3 baseSize = BaseCube.GetComponent<BoxCollider>().size;
        Vector3 movingSize = MovingCube.GetComponent<BoxCollider>().size;

        #region raycasts
        RaycastHit baseMaxXhit;
        Vector3 baseMaxXPosition = BaseCube.transform.position + new Vector3((baseSize.x * BaseCube.transform.localScale.x) / 2, 0, 0);
        bool baseMaxX = Physics.Raycast(baseMaxXPosition, Vector3.up, out baseMaxXhit);
        Debug.DrawRay(BaseCube.transform.position + new Vector3((baseSize.x * BaseCube.transform.localScale.x) / 2, 0, 0), Vector3.up);
        Debug.Log("maxbaseX:" + baseMaxX);

        RaycastHit baseMinXHit;
        Vector3 baseMinXPosition = BaseCube.transform.position - new Vector3((baseSize.x * BaseCube.transform.localScale.x) / 2, 0, 0);
        bool baseMinx = Physics.Raycast(baseMinXPosition, Vector3.up, out baseMinXHit);
        Debug.DrawRay(BaseCube.transform.position - new Vector3((baseSize.x * BaseCube.transform.localScale.x) / 2, 0, 0), Vector3.up);
        Debug.Log("MinBaseX:" + baseMinx);


        RaycastHit baseMaxZHit;
        Vector3 baseMaxZPosition = BaseCube.transform.position + new Vector3(0, 0, (baseSize.z * BaseCube.transform.localScale.z) / 2);
        bool baseMaxZ = Physics.Raycast(baseMaxZPosition, Vector3.up, out baseMaxZHit);
        Debug.DrawRay(BaseCube.transform.position + new Vector3(0, 0, (baseSize.z * BaseCube.transform.localScale.z) / 2), Vector3.up);
        Debug.Log("maxbaseZ:" + baseMaxZ);

        RaycastHit baseMinZHit;
        Vector3 baseMinZPosition = BaseCube.transform.position - new Vector3(0, 0, (baseSize.z * BaseCube.transform.localScale.z) / 2);
        bool baseMinZ = Physics.Raycast(baseMinZPosition, Vector3.up, out baseMinZHit);
        Debug.DrawRay(BaseCube.transform.position - new Vector3(0, 0, (baseSize.z * BaseCube.transform.localScale.z) / 2), Vector3.up);
        Debug.Log("minbaseZ:" + baseMinZ);

        RaycastHit movingMaxXHit;
        Vector3 movingMaxXPosition = MovingCube.transform.position + new Vector3((movingSize.x * MovingCube.transform.localScale.x) / 2, 0, 0);
        bool movingMaxX = Physics.Raycast(movingMaxXPosition, Vector3.down, out movingMaxXHit);
        Debug.DrawRay(MovingCube.transform.position + new Vector3((movingSize.x * MovingCube.transform.localScale.x) / 2, 0, 0), Vector3.down);
        Debug.Log("maxmovingX:" + movingMaxX);

        RaycastHit movingMinXHit;
        Vector3 movingMinXPosition = MovingCube.transform.position - new Vector3((movingSize.x * MovingCube.transform.localScale.x) / 2, 0, 0);
        bool movingMinx = Physics.Raycast(movingMinXPosition, Vector3.down, out movingMinXHit);
        Debug.DrawRay(MovingCube.transform.position - new Vector3((movingSize.x * MovingCube.transform.localScale.x) / 2, 0, 0), Vector3.down);
        Debug.Log("MinmovingX:" + movingMinx);

        RaycastHit movingMaxZHit;
        Vector3 movingMaxZPosition = MovingCube.transform.position + new Vector3(0, 0, (movingSize.z * MovingCube.transform.localScale.z) / 2);
        bool movingMaxZ = Physics.Raycast(movingMaxZPosition, Vector3.down, out movingMaxZHit);
        Debug.DrawRay(MovingCube.transform.position + new Vector3(0, 0, (movingSize.z * MovingCube.transform.localScale.z) / 2), Vector3.down);
        Debug.Log("maxmovingZ:" + movingMaxZ);

        RaycastHit movingMinZHit;
        Vector3 movingMinZPosition = MovingCube.transform.position - new Vector3(0, 0, (movingSize.z * MovingCube.transform.localScale.z) / 2);
        bool movingMinZ = Physics.Raycast(movingMinZPosition, Vector3.down, out movingMinZHit);
        Debug.DrawRay(MovingCube.transform.position - new Vector3(0, 0, (movingSize.z * MovingCube.transform.localScale.z) / 2), Vector3.down);
        Debug.Log("minMovingZ:" + movingMinZ);
        #endregion



        if (baseMaxX == false && baseMinx == false && baseMaxZ == false && baseMinZ == false)
        {
            finished = true;
            GameManager._instance.PlayCounter++;

            Debug.Log("its the end. you lost");
            MovingCube.AddComponent<Rigidbody>();
            GameManager._instance.EndScoreText.text = Score.ToString();
            StartCoroutine(ShowEnd());
            return;
        }
        #region real cutting section
        switch (xDirection)
        {
            case true:
                if (Mathf.Abs(BaseCube.transform.position.x - MovingCube.transform.position.x) < Epsilon)
                {

                }

                if (baseMaxX)
                {
                    MovingCube.transform.localScale = new Vector3(movingMinXHit.point.x - baseMaxXhit.point.x, MovingCube.transform.localScale.y, MovingCube.transform.localScale.z);
                    MovingCube.transform.position = new Vector3((movingMinXHit.point.x + baseMaxXhit.point.x) / 2, MovingCube.transform.position.y, MovingCube.transform.position.z);


                    GameObject additive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    additive.transform.localScale = new Vector3(movingMaxXPosition.x - baseMaxXhit.point.x, MovingCube.transform.localScale.y, MovingCube.transform.localScale.z);
                    additive.transform.position = new Vector3((baseMaxXPosition.x + movingMaxXPosition.x) / 2, MovingCube.transform.position.y, MovingCube.transform.position.z);
                    additive.AddComponent<Rigidbody>();
                    additive.name = "additive";
                    Destroy(additive, 5);
                }
                if (baseMinx)
                {
                    MovingCube.transform.localScale = new Vector3(movingMaxXHit.point.x - baseMinXHit.point.x, MovingCube.transform.localScale.y, MovingCube.transform.localScale.z);
                    MovingCube.transform.position = new Vector3((movingMaxXHit.point.x + baseMinXHit.point.x) / 2, MovingCube.transform.position.y, MovingCube.transform.position.z);


                    GameObject additive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    additive.transform.localScale = new Vector3(movingMinXPosition.x - baseMinXHit.point.x, MovingCube.transform.localScale.y, MovingCube.transform.localScale.z);
                    additive.transform.position = new Vector3((movingMinXPosition.x + baseMinXHit.point.x) / 2, MovingCube.transform.position.y, MovingCube.transform.position.z);
                    additive.AddComponent<Rigidbody>();
                    additive.name = "additive";
                    Destroy(additive, 5);
                }
                break;
            case false:
                if (Mathf.Abs(BaseCube.transform.position.z - MovingCube.transform.position.z) < Epsilon)
                {

                }


                if (baseMaxZ)
                {
                    MovingCube.transform.localScale = new Vector3(MovingCube.transform.localScale.x, MovingCube.transform.localScale.y, movingMinZHit.point.z - baseMaxZHit.point.z);
                    MovingCube.transform.position = new Vector3(MovingCube.transform.localPosition.x, MovingCube.transform.position.y, (movingMinZHit.point.z + baseMaxZHit.point.z) / 2);


                    GameObject additive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    additive.transform.localScale = new Vector3(MovingCube.transform.localScale.x, MovingCube.transform.localScale.y, movingMaxZPosition.z - baseMaxZHit.point.z);
                    additive.transform.position = new Vector3(MovingCube.transform.localPosition.x, MovingCube.transform.localPosition.y, (movingMaxZPosition.z + baseMaxZHit.point.z) / 2);

                    additive.AddComponent<Rigidbody>();
                    additive.name = "additive";
                    Destroy(additive, 5);
                }
                if (baseMinZ)
                {
                    MovingCube.transform.localScale = new Vector3(MovingCube.transform.localScale.x, MovingCube.transform.localScale.y, movingMaxZHit.point.z - baseMinZHit.point.z);
                    MovingCube.transform.position = new Vector3(MovingCube.transform.position.x, MovingCube.transform.position.y, (movingMaxZHit.point.z + baseMinZHit.point.z) / 2);


                    GameObject additive = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    additive.transform.localScale = new Vector3(MovingCube.transform.localScale.x, MovingCube.transform.localScale.y, movingMinZPosition.z - baseMinZHit.point.z);
                    additive.transform.position = new Vector3(MovingCube.transform.position.x, MovingCube.transform.position.y, (movingMinZPosition.z + baseMinZHit.point.z) / 2);

                    additive.AddComponent<Rigidbody>();
                    additive.name = "additive";
                    Destroy(additive, 5);
                }
                break;
        }
        CubeGenerationTest();
        Score++;
        ScoreText.text = Score.ToString();

        #endregion
    }









    #region Utility

    private void CubeStart()
    {
        BaseCube = VeryBaseCube;
        CameraFollow._instance.goal = BaseCube.transform;
        speed = UnityEngine.Random.Range(MinSpeed, MaxSpeed);
        MovingCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        MovingCube.transform.localScale = new Vector3(1, 0.2f, 1);

        float BaseHeight = BaseCube.GetComponent<BoxCollider>().size.y * BaseCube.transform.localScale.y / 2;
        float gHeight = MovingCube.GetComponent<BoxCollider>().size.y * MovingCube.transform.localScale.y / 2;
        float BasePos = BaseCube.transform.position.y;
        MovingCube.transform.position = new Vector3(xAmount * UnityEngine.Random.Range(-100, 100), BaseHeight + gHeight + BasePos, zAmount * UnityEngine.Random.Range(-100, 100));
        plates.Add(MovingCube);
    }

    private void RemoveAllPlates()
    {
        foreach (GameObject p in plates)
        {
            Destroy(p);
        }
        plates.Clear();
    }

    private void Swap()
    {
        xDirection = !xDirection;
        speed = UnityEngine.Random.Range(MinSpeed, MaxSpeed);
        if (xDirection)
        {
            zAmount = 0;
            xAmount = speed;
        }

        else
        {
            xAmount = 0;
            zAmount = speed;
        }
    }

    private void MoveCube()
    {
        if (MovingCube == null)
        {
            return;
        }

        if (MovingCube.transform.position.x > Xmax.transform.position.x) { xAmount = -speed; }
        else if (MovingCube.transform.position.x < Xmin.transform.position.x) { xAmount = speed; }

        if (MovingCube.transform.position.z > Zmax.transform.position.z) { zAmount = -speed; }
        else if (MovingCube.transform.position.z < Zmin.transform.position.z) { zAmount = speed; }
        MovingCube.transform.Translate(xAmount, 0, zAmount);


    }
    private IEnumerator ShowEnd()
    {
        yield return new WaitForSeconds(3);

        GameManager._instance.GamePanel.SetActive(false);
        GameManager._instance.EndPanel.SetActive(true);
        gameObject.SetActive(false);
    }
    private void CubeGenerationTest()
    {
        BaseCube.GetComponent<BoxCollider>().enabled = false;
        BaseCube = MovingCube;
        Swap();
        MovingCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        MovingCube.transform.localScale = new Vector3(1, 0.2f, 1);

        float BaseHeight = BaseCube.GetComponent<BoxCollider>().size.y * BaseCube.transform.localScale.y / 2;
        float gHeight = MovingCube.GetComponent<BoxCollider>().size.y * MovingCube.transform.localScale.y / 2;
        float BasePos = BaseCube.transform.position.y;
        plates.Add(MovingCube);



        switch (xDirection)
        {
            case true:
                MovingCube.transform.position = new Vector3(xAmount * UnityEngine.Random.Range(-100, 100), BaseHeight + gHeight + BasePos, BaseCube.transform.position.z);

                break;
            case false:
                MovingCube.transform.position = new Vector3(BaseCube.transform.position.x, BaseHeight + gHeight + BasePos, zAmount * UnityEngine.Random.Range(-100, 100));

                break;
        }

        MovingCube.transform.localScale = BaseCube.transform.localScale;
        CameraFollow._instance.goal = BaseCube.transform;

    }

    #endregion





}
