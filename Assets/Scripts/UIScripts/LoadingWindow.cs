using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//child of BaseWindow
//code for start Menu
public class LoadingWindow : BaseWindow
{
    #region Attributes
    [SerializeField] private Image loadIcon;
    [SerializeField] private float waitTime;
    private bool loaded;
    #endregion

    #region inBuildMethods
    void Start()
    {
        loaded = false;
    }

    void Update()
    {
        //check waittime and rotate
        if (waitTime > 0)
        {
            //rotate mofo
            loadIcon.rectTransform.RotateAround(loadIcon.gameObject.transform.position, loadIcon.gameObject.transform.forward, 2);

            //drop waitTime 
            waitTime -= Time.deltaTime;
        }
        else if (waitTime > -10.0f) //we waited enough range
        {
            //set loaded to true
            loaded = true;

            //set waittime to out of range of use
            waitTime = -100.00f;

            //goto game
            GoToLevel();
        }




    }
    #endregion

    #region helperMethods
    public override void Open()
    {
        //run baseWindoes open method
        base.Open();
    }

    private void GoToLevel()
    {
        //goto the level
        MenuManager.Instance.MenuEnabled = !MenuManager.Instance.MenuEnabled;
        SceneManager.LoadScene("Poseidon");
    }


    #endregion
}
