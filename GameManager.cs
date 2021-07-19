using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject titleScreen;
    public GameObject gameMenu;

    public GameObject firstSixElem;
    public GameObject nextNineElem;
    public GameObject nextTwelveElem;

    public Button ready;

    public Button newGameButton;

    public Image gameImage;

    public GameObject win;
    public GameObject losing;
    public GameObject game;

    public int currentImageNum;

    [SerializeField] private int _difLvl;

    [SerializeField]
    private int _gameElements;

    public Button[] gameButtons = new Button[12];

    [SerializeField] private int _attempts;

    [SerializeField] private int _score;

    public int[] buttonsOnArray = new int[12];
    [SerializeField] private int _num;

    [SerializeField] private Sprite[] _imageArrey;

    [SerializeField] private bool isGameActive;
    [SerializeField] private bool isButtonActive;

    private readonly float _showDelay = 1.0f;
    private float _timeLeft;
    private readonly float _winDelay = 0.1f;

    public TextMeshProUGUI attemptsText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI tapIconText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI excellentText;
    public TextMeshProUGUI rememberText;
    public int[] NumArray { get; private set; }

    [SerializeField] private string _starsKeyPlayerPrefs;
    public Sprite star, noStar;
    public Button[] difButtons;
    private float _luck;

    private void Awake()
    {
        titleScreen.gameObject.SetActive(true);
        gameMenu.gameObject.SetActive(false);
        tapIconText.gameObject.SetActive(false);
        excellentText.gameObject.SetActive(false);
        losing.gameObject.SetActive(false);
        win.gameObject.SetActive(false);
        //remT.gameObject.SetActive(true);
        //ready.gameObject.SetActive(true);
        game.gameObject.SetActive(false);
    }

    private void Start()
    {
        for (int i = 1; i <= 3; i++)
        {
            if (PlayerPrefs.HasKey(_starsKeyPlayerPrefs + i))
            {
                if (PlayerPrefs.GetInt(_starsKeyPlayerPrefs + i) == 1)
                {
                    difButtons[i - 1].transform.GetChild(0).GetComponent<Image>().sprite = star;
                    difButtons[i - 1].transform.GetChild(1).GetComponent<Image>().sprite = noStar;
                    difButtons[i - 1].transform.GetChild(2).GetComponent<Image>().sprite = noStar;
                }
                else if (PlayerPrefs.GetInt(_starsKeyPlayerPrefs + i) == 2)
                {
                    difButtons[i - 1].transform.GetChild(0).GetComponent<Image>().sprite = star;
                    difButtons[i - 1].transform.GetChild(1).GetComponent<Image>().sprite = star;
                    difButtons[i - 1].transform.GetChild(2).GetComponent<Image>().sprite = noStar;
                }
                else if (PlayerPrefs.GetInt(_starsKeyPlayerPrefs + i) == 3)
                {
                    difButtons[i - 1].transform.GetChild(0).GetComponent<Image>().sprite = star;
                    difButtons[i - 1].transform.GetChild(1).GetComponent<Image>().sprite = star;
                    difButtons[i - 1].transform.GetChild(2).GetComponent<Image>().sprite = star;
                }
            }
            else
            {
                difButtons[i - 1].transform.GetChild(0).gameObject.SetActive(false);
                difButtons[i - 1].transform.GetChild(1).gameObject.SetActive(false);
                difButtons[i - 1].transform.GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (isGameActive)
        {
            _timeLeft -= Time.deltaTime;
            timerText.SetText("TIME: " + Mathf.Round(_timeLeft));
            if (_timeLeft < 0 || (_gameElements / (_difLvl / 4.0)) <= _attempts)
            {
                Defeat();
            }
            else if (_score == _gameElements)
            {
                StartCoroutine(Winner());
            }
        }
    }
    public void StartGame(int difficulty)
    {
        _difLvl = difficulty;
        _attempts = 0;
        _score = 0;
        titleScreen.gameObject.SetActive(false);

        if (_difLvl == 1)
        {
            _gameElements = 6;
            _timeLeft = 10;
            int[] _numArray = { 0, 1, 2, 3, 4, 5 };
            NumArray = ShuffleArray(_numArray);
            firstSixElem.gameObject.SetActive(true);
            nextNineElem.gameObject.SetActive(false);
            nextTwelveElem.gameObject.SetActive(false);
            tapIconText.gameObject.SetActive(true);
            rememberText.gameObject.SetActive(true);
        }
        else if (_difLvl == 2)
        {
            _gameElements = 9;
            _timeLeft = 20;
            int[] _numArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
            NumArray = ShuffleArray(_numArray);
            firstSixElem.gameObject.SetActive(true);
            nextNineElem.gameObject.SetActive(true);
            nextTwelveElem.gameObject.SetActive(false);
            tapIconText.gameObject.SetActive(true);
            rememberText.gameObject.SetActive(true);
        }
        else
        {
            _gameElements = 12;
            _timeLeft = 30;
            int[] _numArray = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
            NumArray = ShuffleArray(_numArray);
            firstSixElem.gameObject.SetActive(true);
            nextNineElem.gameObject.SetActive(true);
            nextTwelveElem.gameObject.SetActive(true);
            rememberText.gameObject.SetActive(true);
        }

        gameMenu.gameObject.SetActive(true);
        //ready.gameObject.SetActive(false);
        //remT.gameObject.SetActive(false);
        game.gameObject.SetActive(true);
        Debug.Log(NumArray[_attempts]);
        StartCoroutine(Show());
    }

    private int[] ShuffleArray(int[] numbers)
    {
        int[] newArray = numbers.Clone() as int[];
        for (int i = 0; i < newArray.Length; i++)
        {
            int temp = newArray[i];
            int random = Random.Range(i, newArray.Length);
            newArray[i] = newArray[random];
            newArray[random] = temp;
        }
        return newArray;
    }

    private IEnumerator Show()
    {
        if (!isGameActive)
        {
            for (int i = 0; i < NumArray.Length; i++)
            {
                yield return new WaitForSeconds(_showDelay);
                currentImageNum = NumArray[i];
                RoundUpN();
            }
            yield return new WaitForSeconds(_showDelay);
            gameImage.color = new Color(1f, 1f, 1f, 0.0f);

            rememberText.gameObject.SetActive(false);
            //tapIconT.gameObject.SetActive(true);
            isGameActive = true;
        }
    }

    public void One()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[0] != 1)
        {
            isButtonActive = true;
            _num = 0;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void Two()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[1] != 2)
        {
            isButtonActive = true;
            _num = 1;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void Three()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[2] != 3)
        {
            isButtonActive = true;
            _num = 2;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void Four()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[3] != 4)
        {
            isButtonActive = true;
            _num = 3;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void Five()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[4] != 5)
        {
            isButtonActive = true;
            _num = 4;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void Six()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[5] != 6)
        {
            isButtonActive = true;
            _num = 5;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void Seven()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[6] != 7)
        {
            isButtonActive = true;
            _num = 6;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void Eight()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[7] != 8)
        {
            isButtonActive = true;
            _num = 7;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void Nine()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[8] != 9)
        {
            isButtonActive = true;
            _num = 8;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void Ten()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[9] != 10)
        {
            isButtonActive = true;
            _num = 9;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void Eleven()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[10] != 11)
        {
            isButtonActive = true;
            _num = 10;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void Twelve()
    {
        if (isGameActive & !isButtonActive & buttonsOnArray[11] != 12)
        {
            isButtonActive = true;
            _num = 11;
            StartCoroutine(Comparison());
            isButtonActive = false;
        }
    }
    public void RoundUpN()
    {
        gameImage.sprite = _imageArrey[currentImageNum];

        gameImage.gameObject.SetActive(true);
    }

    public IEnumerator Comparison()
    {
        if (NumArray[_score] == _num)
        {
            gameButtons[_num].GetComponent<Image>().color = Color.yellow;

            _score++;

            scoreText.text = "Score: " + _score;
            yield return new WaitForSeconds(0.2f);

            gameButtons[_num].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
            int buttonOn = _num + 1;
            buttonsOnArray[_num] = buttonOn;
        }
        else
        {
            gameButtons[_num].GetComponent<Image>().color = Color.red;
            yield return new WaitForSeconds(0.2f);
            gameButtons[_num].GetComponent<Image>().color = Color.white;
        }


        _attempts++;
        attemptsText.text = "Attempts: " + _attempts;

        _luck = _score / (_attempts * 1f);
        Debug.Log("Luck " + (_luck * 100) + " %");

        if (_attempts == _score && _gameElements == _score)
        {
            excellentText.gameObject.SetActive(true);
        }
    }


    public void Stop()
    {
        StartCoroutine(Winner());
    }

    public IEnumerator Winner()
    {
        if (isGameActive)
        {
            isGameActive = false;

            foreach (int e in NumArray)
            {
                yield return new WaitForSeconds(_winDelay);
                gameButtons[e].GetComponent<Image>().color = Color.white;
            }

            //yield return new WaitForSeconds(_winDelay);

            foreach (int e in NumArray)
            {
                yield return new WaitForSeconds(_winDelay);
                gameButtons[e].GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
            }

            StopAllCoroutines();
        }
        if ((_luck < 0.75f) && !PlayerPrefs.HasKey(_starsKeyPlayerPrefs + _difLvl))
        {
            PlayerPrefs.SetInt(_starsKeyPlayerPrefs + _difLvl, 1);
        }
        else if ((_luck >= 0.75f) && _luck <= 0.9f && (!PlayerPrefs.HasKey(_starsKeyPlayerPrefs + _difLvl) || PlayerPrefs.GetInt(_starsKeyPlayerPrefs + _difLvl) < 2))
        {
            PlayerPrefs.SetInt(_starsKeyPlayerPrefs + _difLvl, 2);
        }
        else if ((_luck > 0.9f) && (!PlayerPrefs.HasKey(_starsKeyPlayerPrefs + _difLvl) || PlayerPrefs.GetInt(_starsKeyPlayerPrefs + _difLvl) < 3))
        {
            PlayerPrefs.SetInt(_starsKeyPlayerPrefs + _difLvl, 3);
        }
        Debug.Log(PlayerPrefs.GetInt(_starsKeyPlayerPrefs + _difLvl));

        win.gameObject.SetActive(true);
    }

    public void Defeat()
    {
        isGameActive = false;

        losing.gameObject.SetActive(true);
    }
    
    public void RestartGame()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        Application.Quit();
#endif
    }
}
