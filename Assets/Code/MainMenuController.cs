using UnityEngine;
using System.Collections;

public class MainMenuController : MonoBehaviour {

    public TextMesh newGame;
    public TextMesh levelSelect;
    public TextMesh quit;

    private string currentlySelected;

	// Update is called once per frame
	void Update () {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        newGame.color = levelSelect.color = quit.color = Color.magenta;
        currentlySelected = "none";

        if(Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.name == "NewGameTile")
            {
                newGame.color = Color.white;
                currentlySelected = "newGame";
            }
            else if (hit.collider.gameObject.name == "LevelSelectTile")
            {
                levelSelect.color = Color.white;
                currentlySelected = "levelSelect";
            } else if (hit.collider.gameObject.name == "QuitTile")
            {
                quit.color = Color.white;
                currentlySelected = "quitGame";
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
