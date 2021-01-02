using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Projectile : MonoBehaviour
{
    public Text scoreCount, ringCount, levelCount;
    public Rigidbody projectile, obj;
    public GameObject cursor, stageObj;
    public Transform shootPoint;
    public LayerMask layer;
    public LineRenderer lineVisual;
    public int lineSegment = 10;
    public float flightTime = 1f;
    Vector3 rp;
    bool isThrow = false, isActive = false;
    private Camera cam;
    public List<GameObject> objPos, onStage;
    int ring;

    // Start is called before the first frame update
    void Start()
    {

        GameManager.Instance.score = PlayerPrefs.GetInt("Score");
        if (PlayerPrefs.GetInt("level") <= 3)
        {
            ring = PlayerPrefs.GetInt("level") + 2;
            for (int i = 0; i < ring - 2; i++)
            {
                onStage.Add(Instantiate(stageObj));
                onStage[i].transform.position = objPos[i].transform.position;
                GameManager.Instance.activeObjCount++;
            }
        }
        else if (PlayerPrefs.GetInt("level") > 3 && PlayerPrefs.GetInt("level") <= 10)
        {
            ring = Random.Range(3, 5);
            for (int i = 0; i < ring - 2; i++)
            {
                onStage.Add(Instantiate(stageObj));
                onStage[i].transform.position = objPos[i].transform.position;
                GameManager.Instance.activeObjCount++;
            }
        }
        else if(PlayerPrefs.GetInt("level") > 10 && PlayerPrefs.GetInt("level") <= 20)
        {
            ring = Random.Range(3, 6);
            for (int i = 0; i < ring - 2; i++)
            {
                onStage.Add(Instantiate(stageObj));
                onStage[i].transform.position = objPos[i].transform.position;
                GameManager.Instance.activeObjCount++;
            }
        }
        else if(PlayerPrefs.GetInt("level") > 20)
        {
            ring = Random.Range(3, 7);
            for (int i = 0; i < ring - 2; i++)
            {
                onStage.Add(Instantiate(stageObj));
                onStage[i].transform.position = objPos[i].transform.position;
                GameManager.Instance.activeObjCount++;
            }
        }

        cam = Camera.main;
        lineVisual.positionCount = lineSegment + 1;
        cursor.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        scoreCount.text = "Score : " + GameManager.Instance.score;
        ringCount.text = "X " + ring.ToString();
        levelCount.text = "Level " + PlayerPrefs.GetInt("level");

        //Debug.Log("Objects are active " + GameManager.Instance.activeObjCount + " of "+ onStage.Count);
        if (GameManager.Instance.activeObjCount == 0)
        {
            GameManager.Instance.isGameOver = false;
        }
        else
        {
            GameManager.Instance.isGameOver = true;
        }
        if (Input.GetMouseButton(0))
        {
            if (!isActive)
            {
                LaunchProjectile();
            }
        }
        if (Input.GetMouseButtonUp(0) && isThrow)
        {
            mouseRelease();
        }
    }

    void LaunchProjectile()
    {
        Ray camRay = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(camRay, out hit, 100f, layer))
        {
            if (hit.collider.tag != "backstage")
            {
                cursor.SetActive(true);
                cursor.transform.position = hit.point + Vector3.up * 0.5f;
                Vector3 vo = CalculateVelocty(hit.point, shootPoint.position, flightTime * 2f);
                rp = vo;
                isThrow = true;
                Visualize(vo, cursor.transform.position); //we include the cursor position as the final nodes for the line visual position
                //transform.rotation = Quaternion.LookRotation(vo);
                //if (Input.GetMouseButtonUp(0))
                //{
                //    Rigidbody obj = Instantiate(projectile, shootPoint.position, Quaternion.identity);
                //    obj.velocity = rp;
                //}
            }
            else
            {
                lineVisual.positionCount = 0;
                cursor.SetActive(false);
                afterThrow();
            }
        }

    }

    void mouseRelease()
    {
        obj = Instantiate(projectile, shootPoint.position, Quaternion.identity);
        ring--;
        isActive = true;
        obj.velocity = rp;
        lineVisual.positionCount = 0;
        cursor.SetActive(false);
        afterThrow();
        StartCoroutine(destroyThrowRing(obj.gameObject));
    }

    void afterThrow()
    {
        isThrow = false;
        lineVisual.positionCount = lineSegment + 1;
    }

    //added final position argument to draw the last line node to the actual target
    void Visualize(Vector3 vo, Vector3 finalPos)
    {
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = CalculatePosInTime(vo, (i / (float)lineSegment) * flightTime * 2f);
            lineVisual.SetPosition(i, pos);
        }

        lineVisual.SetPosition(lineSegment, finalPos);
    }

    Vector3 CalculateVelocty(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXz = distance;
        distanceXz.y = 0f;

        float sY = distance.y;
        float sXz = distanceXz.magnitude;

        float Vxz = sXz / time;
        float Vy = (sY / time) + (0.5f * Mathf.Abs(Physics.gravity.y) * time);

        Vector3 result = distanceXz.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    Vector3 CalculatePosInTime(Vector3 vo, float time)
    {
        Vector3 Vxz = vo;
        Vxz.y = 0f;

        Vector3 result = shootPoint.position + vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (vo.y * time) + shootPoint.position.y;
        result.y = sY;

        return result;
    }

    IEnumerator destroyThrowRing(GameObject obj)
    {
        yield return new WaitForSecondsRealtime(4f);
        Destroy(obj);
        isActive = false;
        if ((ring == 0 && !GameManager.Instance.isGameOver) || !GameManager.Instance.isGameOver)
        {
            SceneManager.LoadScene("GAMESCENE");
            PlayerPrefs.SetInt("Score", GameManager.Instance.score);
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        }
        if (ring == 0 && GameManager.Instance.isGameOver)
        {
            SceneManager.LoadScene("GAMEOVER");
            PlayerPrefs.SetInt("Score", GameManager.Instance.score);
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level"));
        }
    }
}