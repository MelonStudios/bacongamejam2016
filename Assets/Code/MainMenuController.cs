using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour
{
    public TextMesh newGame;
    public TextMesh levelSelect;
    public TextMesh quit;

    public TextMesh backButton;
    public TextMesh level1, level2, level3, level4;

    public Transform mainMenuPoint;
    public Transform levelSelectPoint;

    public float CameraAnimationTime;

    private string currentlySelected;

    public bool animating;

    void Start()
    {
        animating = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (animating) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        newGame.color = levelSelect.color = quit.color = backButton.color = level1.color = level2.color = level3.color = level4.color = Color.magenta;
        currentlySelected = "none";

        if (Physics.Raycast(ray, out hit))
        {
            switch (hit.collider.gameObject.name)
            {
                case "NewGameTile":
                    newGame.color = Color.white;
                    currentlySelected = "newGame";
                    break;
                case "LevelSelectTile":
                    levelSelect.color = Color.white;
                    currentlySelected = "levelSelect";
                    break;
                case "QuitTile":
                    quit.color = Color.white;
                    currentlySelected = "quitGame";
                    break;
                case "BackTile":
                    backButton.color = Color.white;
                    currentlySelected = "back";
                    break;
                case "Level1Tile":
                    level1.color = Color.white;
                    currentlySelected = "level1";
                    break;
                case "Level2Tile":
                    level2.color = Color.white;
                    currentlySelected = "level2";
                    break;
                case "Level3Tile":
                    level3.color = Color.white;
                    currentlySelected = "level3";
                    break;
                case "Level4Tile":
                    level4.color = Color.white;
                    currentlySelected = "level4";
                    break;
                default:
                    break;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            switch (currentlySelected)
            {
                case "newGame":
                    Application.LoadLevel(1);
                    break;
                case "levelSelect":
                    StartCoroutine(AnimateCameraToPoint(levelSelectPoint));
                    break;
                case "back":
                    StartCoroutine(AnimateCameraToPoint(mainMenuPoint));
                    break;
                case "level1":
                    Application.LoadLevel(1);
                    break;
                case "level2":
                    Application.LoadLevel(2);
                    break;
                case "level3":
                    Application.LoadLevel(3);
                    break;
                case "level4":
                    Application.LoadLevel(4);
                    break;
                case "quitGame":
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
    }

    IEnumerator AnimateCameraToPoint(Transform goalPoint)
    {
        animating = true;

        Camera cam = Camera.main;
        Vector3 originalPosition = cam.transform.position;
        Quaternion originalRotation = cam.transform.rotation;
        Vector3 targetPosition = goalPoint.position;
        Quaternion targetRotation = goalPoint.rotation;

        float deltaTime = 0;

        do
        {
            deltaTime += Time.deltaTime;
            
            cam.transform.position = Vector3.Slerp(originalPosition, targetPosition, MathUtility.PercentageBetween(deltaTime, 0, CameraAnimationTime));
            //cam.transform.rotation = Quaternion.Slerp(originalRotation, targetRotation, MathUtility.PercentageBetween(deltaTime, 0, CameraAnimationTime));

            yield return new WaitForEndOfFrame();
        } while (deltaTime <= CameraAnimationTime);

        animating = false;
    }
}