using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Public Variables
    public static GameManager instance; //Variable estatica del GameManager
    #endregion
    #region Monobehaviour Method
    private void Awake()
    {
        if(GameManager.instance == null) //Si la variable está vacía.
        {
            GameManager.instance = this; //La rellenamos con lo que hay aquí.
            DontDestroyOnLoad(this.gameObject); //Impedimos que se destruya.
        }
        else
        {
            Destroy(this.gameObject); //En caso de que contenga algo, destruimos el GameObject.
        }
    }
    private void Start()
    {
        Application.targetFrameRate = 60; //Seteamos los Fps a 60.
        Cursor.lockState = CursorLockMode.Locked; //Hacemos desaparecer el cursor.
    }

    #endregion
}
