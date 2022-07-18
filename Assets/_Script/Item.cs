using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private GameObject text; //Componente texto que tiene el item.

    /// <summary>
    /// Método que activará el objeto adicional del item.
    /// </summary>
    public void ActiveItem()
    {
        text.SetActive(true); //Activamos el objeto.
    }
}
