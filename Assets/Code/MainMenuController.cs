using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    public TextMesh newGame;
    public TextMesh levelSelect;
    public TextMesh quit;

    public TextMesh backButton;
    public TextMesh level1, level2, level3;

    public Transform mainMenuPoint;
    public Transform levelSelectPoint;

    private Transform goalPoint;

    private string currentlySelected;

    void Start()
    {
        goalPoint = mainMenuPoint;
    }

	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        newGame.color = levelSelect.color = quit.color = backButton.color = level1.color = level2.color = level3.color = Color.magenta;
        currentlySelected = "none";

        Camera.main.transform.position = goalPoint.position;

        if(Physics.Raycast(ray, out hit))
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
                default:
                    break;
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            switch(currentlySelected)
            {
                case "newGame":
                    Application.LoadLevel(1);
                    break;
                case "levelSelect":
                    goalPoint = levelSelectPoint;
                    break;
                case "back":
                    goalPoint = mainMenuPoint;
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
                case "quitGame":
                    Application.Quit();
                    break;
                default:
                    break;
            }
        }
	}
}
