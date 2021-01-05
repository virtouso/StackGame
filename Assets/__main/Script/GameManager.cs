using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    #region public
    public static GameManager _instance;
    [SerializeField] public GameObject EndPanel;
    [SerializeField] public Text EndScoreText;
    [SerializeField] public GameObject GamePanel;
    #endregion



    #region Unity References

    [SerializeField] private Font font;
    [SerializeField] private Button MainLeaderboard;
    [SerializeField] private Button EndLeaderBoard;
    [SerializeField] private Button EndReturn;
    [SerializeField] private Button Play;
    [SerializeField] private Button Exit;
    [SerializeField] private GameObject BoxManager;
    [SerializeField] private GameObject MenuPanel;


    #endregion

    public int PlayCounter;


    #region Unity Calbacks

    private void Awake()
    {
        _instance = this;
        InitButtons();
    }




    #endregion

    #region Button References

    public void PlayGame()
    {
        Debug.Log("play button");
        BoxManager.SetActive(true);
        MenuPanel.SetActive(false);
        GamePanel.SetActive(true);
    }
    public void ShowLeaderBoard() { }
    public void ExitGame() { Application.Quit(); }


    public void InitButtons()
    {
        Play.onClick.AddListener(PlayGame);
        MainLeaderboard.onClick.AddListener(ShowLeaderBoard);
        EndLeaderBoard.onClick.AddListener(ShowLeaderBoard);
        EndReturn.onClick.AddListener(() =>
        {
            MenuPanel.SetActive(true);
            GamePanel.SetActive(false);
            EndPanel.SetActive(false);
            BoxManager.SetActive(false);
        });

        Exit.onClick.AddListener(ExitGame);


    }


 
    #endregion

}
