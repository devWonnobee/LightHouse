using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))] //Protecci�n por si no tenemos Rigidbody.
public class PlayerMove : MonoBehaviour
{
    #region Private Variables
    private Rigidbody _rb; //Variable para el rigidbody.
    private Animator _anim; //Variable para el animator.
    private Vector3 moveForward; //Vector para guardar el movimiento.
    private Vector2 mouseInput; //Vector para guardar la posici�n del mouse.
    private float xRot; //Variable de rotaci�n del mouse.
    private bool isRunning; //Booleano para saber si el personaje est� corriendo.
    private bool isGround; //Booleano para saber si el personaje est� saltando.
    #endregion

    #region SerializeField Variables
    [Header("Variables de cantidad de movmiento")]
    [SerializeField] private float moveSpeed; //Velocidad de movmiento.
    [SerializeField] private float jumpForce; //Velocidad de salto.
    [SerializeField] private float sensitivity; //Sensibilidad del rat�n.
    [Space][Header("Variables de transform")]
    [SerializeField] private Transform cameraPlayer; //Transform de la c�mara.
    [SerializeField] private Transform positionToRay; //Transform desde donde lanzamos el rayo hacia delante.
    [SerializeField] private Transform groundCheck; //Transform desde donde lanzamos el rayo hacia abajo.
    [Space][Header("LayerMask evitable por el Raycast")]
    [SerializeField] private LayerMask layerToRaycast; //Layer evitable por el Raycast
    #endregion

    #region Monobehaviour Method
    private void Start()
    {
        _rb = GetComponent<Rigidbody>(); //Capturamos el rigidbody.
        _anim = GetComponent<Animator>(); //Capturamos el animator.
    }

    private void Update()
    {
        moveForward = new Vector3(Input.GetAxisRaw("Vertical"),0f, Input.GetAxisRaw("Horizontal")); //Input del teclado.
        mouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y")); //Input del rat�n.

        RaycastHit hit; //Variable de la informaci�n del RayCast.
        if(Physics.Raycast(positionToRay.position, cameraPlayer.TransformDirection(Vector3.forward), out hit, layerToRaycast))
        {
            CheckRay(hit); //Si el rayo golpea con alg�n objeto lo chequeamos.
        }
   
        if (Physics.Raycast(groundCheck.position,Vector3.down)) { isGround = false; } //Chequeamos si estamos tocando el suelo
        else { isGround = true; } //Seteamos el isGround.

        MoveHead(); //A�adimos una llamada al m�todo que comprueba la rotaci�n.
    }

    private void FixedUpdate()
    {
        if(moveForward != Vector3.zero) //Si el movimiento es diferente de 0.
        {
            Vector3 finalVector; //Creamos una variable local vector3.
            finalVector = (transform.forward * moveForward.x) + (transform.right * moveForward.z); //Sumanos el movimiento.
            _rb.velocity = finalVector * moveSpeed; //Movemos el personaje.
        }
        else
        {
            _rb.velocity = Vector3.up * _rb.velocity.y; //Si no lo dejamos parado.
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround)//Si pulsamos espacio y estamos en el suelo.
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);//Impulsamos al personaje hacia arriba.
        }
    } 

    private void LateUpdate()
    {
        if (moveForward.x != 0) { isRunning = true; } //Si la x es diferente de 0, seteamos en true el booleano.
        else { isRunning = false; } //Si no la dejamos el falso.
        _anim.SetBool("isRunning", isRunning); //seteamos la animaci�n.
    }
    #endregion
    #region Private Method
    /// <summary>
    /// M�todo que se encarga de rotar el movimiento del mouse.
    /// </summary>
    private void MoveHead()
    {
        xRot -= mouseInput.y * sensitivity; //Invertimos la y adem�s de multiplicarla por la sensibilidad.
        xRot = Mathf.Clamp(xRot, -15, 30); //L�mitamos el movimiento.

        transform.Rotate(0f, mouseInput.x * sensitivity, 0f); //Rotamos el personaje en y
        cameraPlayer.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f); //Rotamos la c�mara en x.
    }
    /// <summary>
    /// M�todo que chequea los raycast.
    /// </summary>
    /// <param name="hit">La variable que guarda lo que hemos golpeado</param>
    private void CheckRay(RaycastHit hit)
    {
        if (hit.collider.name == "Cube") //Ejemplo con un "Cubo"
        {
            hit.collider.gameObject.GetComponent<Item>().ActiveItem();//Accedemos al componente y activamos su funci�n.
        } 
    }
    #endregion
}
