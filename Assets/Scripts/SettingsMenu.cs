using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;

public class SettingsMenu : MonoBehaviour
{
    [Header("space between menu items")]
    [SerializeField] Vector2 spacing;
    public float closeDelay;

    [Space]
    [Header("Main button rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] Ease rotationEase;

    [Space]
    [Header("Animation")]
    [SerializeField] float expandDuration;
    [SerializeField] float collapseDuration;
    [SerializeField] Ease expandEase;
    [SerializeField] Ease collapseEase;

    [Space]
    [Header("Fading")]
    [SerializeField] float expandFadeDuration;
    [SerializeField] float collapseFadeDuration;

    [Header("Sprites")]
    public Sprite musicOn;
    public Sprite musicOff;
    public Sprite play;
    public Sprite stop;
    [Header("Buttons")]
    public Button musicButton;
    public Button playButton;

    Button mainButton;
    SettingsMenuItem[] menuItems;

    //is menu opened or not
    bool isExpanded = false;

    Vector2 mainButtonPosition;
    int itemsCount;

    void Start()
    {
        //add all the items to the menuItems array
        itemsCount = transform.childCount - 1;
        menuItems = new SettingsMenuItem[itemsCount];
        for (int i = 0; i < itemsCount; i++)
        {
            // +1 to ignore the main button
            menuItems[i] = transform.GetChild(i + 1).GetComponent<SettingsMenuItem>();
        }

        mainButton = transform.GetChild(0).GetComponent<Button>();
        mainButton.onClick.AddListener(ToggleMenu);
        //SetAsLastSibling () to make sure that the main button will be always at the top layer
        mainButton.transform.SetAsLastSibling();

        mainButtonPosition = mainButton.GetComponent<RectTransform>().anchoredPosition;

        //set all menu items position to mainButtonPosition
        ResetPositions();
    }

    void ResetPositions()
    {
        for (int i = 0; i < itemsCount; i++)
        {
            menuItems[i].rectTrans.anchoredPosition = mainButtonPosition;
        }
    }

    void ToggleMenu()
    {
        isExpanded = !isExpanded;

        if (isExpanded)
        {
            //menu opened
            for (int i = 0; i < itemsCount; i++)
            {
                menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition + spacing * (i + 1), expandDuration).SetEase(expandEase);
                //Fade to alpha=1 starting from alpha=0 immediately
                menuItems[i].img.DOFade(1f, expandFadeDuration).From(0f);
            }
            CloseAfterSec();
            mainButton.transform
              .DORotate(Vector3.forward * 180f, rotationDuration)
              .From(Vector3.zero)
              .SetEase(rotationEase);
        }
        else
        {
            //menu closed
            for (int i = 0; i < itemsCount; i++)
            {
                menuItems[i].rectTrans.DOAnchorPos(mainButtonPosition, collapseDuration).SetEase(collapseEase);
                //Fade to alpha=0
                menuItems[i].img.DOFade(0f, collapseFadeDuration);
            }
            mainButton.transform
              .DORotate(Vector3.zero, rotationDuration)
              .From(Vector3.forward * 180f)
              .SetEase(rotationEase);
        }

        //rotate main button arround Z axis by 180 degree starting from 0
        
    }

    public void OnItemClick(int index)
    {
        //here you can add you logic 
        switch (index)
        {
            case 0:
                //first button
                Debug.Log("Music");
                bool sesStatus = GameManager.instance.ses;
                if (sesStatus)
                    musicButton.GetComponent<Image>().sprite = musicOff;
                else
                    musicButton.GetComponent<Image>().sprite = musicOn;
                break;
            case 1:
                //second button
                Debug.Log("Sounds");
                break;
            case 2:
                bool playStatus = GameManager.instance.play;
                if (playStatus)
                    playButton.GetComponent<Image>().sprite = stop;
                else
                    playButton.GetComponent<Image>().sprite = play;
                //third button
                Debug.Log("Vibration");
                break;
        }
    }

    void OnDestroy()
    {
        //remove click listener to avoid memory leaks
        mainButton.onClick.RemoveListener(ToggleMenu);
    }

    private async void CloseAfterSec()
    {
        await Task.Delay((int)(closeDelay * 1000));
        if(isExpanded)
            ToggleMenu();
    }
}