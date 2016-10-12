using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class PlayerHealth : MonoBehaviour
{
    //Vida inicial
    public int startingHealth = 100;
    public int currentHealth;
    //Barra de vida de la UI
    public Slider healthSlider;
    //Imagen al sufrir daño
    public Image damageImage;
    public AudioClip deathClip;
    //Duración de la imagen al sufrir daño
    public float flashSpeed = 5f;
    //El flash de daño, es un color rojo transparente (10% opacidad)
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);


    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        playerAudio = GetComponent <AudioSource> ();
        playerMovement = GetComponent <PlayerMovement> ();
        playerShooting = GetComponentInChildren <PlayerShooting> ();
        currentHealth = startingHealth;
    }

    
    void Update ()
    {
        //Si se ha sido dañado
        if(damaged)
        {
            //Cambia el color de la imagen a rojo
            damageImage.color = flashColour;
        }
        //Si no sufrimos daño
        else
        {
            //Limpiamos el color de la imagen de daño, "moviéndonos" de un color a otro mediante Lerd, desde
            //el color que tenemos en ese momento a un estado sin color en un tiempo establecido
            damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        //Terminamos poniendo a false la variable daño en cualquier caso
        damaged = false;
    }

    //No es llamada en este Script, sirve para que los NPC hagan daño al jugador
    public void TakeDamage (int amount)
    {
        //Se comprueba que el jugador esté recibiendo daño
        damaged = true;

        //En ese caso se resta a la salud actual el daño que hace el NPC
        currentHealth -= amount;

        //Se resta en la barra de vida
        healthSlider.value = currentHealth;

        //Se reproduce el sonido de haber sido herido
        playerAudio.Play ();

        if(currentHealth <= 0 && !isDead)
        {
            //Se llama a muerto en caso de que la vida baje a 0 o menos.
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;

        playerShooting.DisableEffects ();

        anim.SetTrigger ("Die");

        playerAudio.clip = deathClip;
        playerAudio.Play ();

        playerMovement.enabled = false;
        playerShooting.enabled = false;
    }


    public void RestartLevel ()
    {
        SceneManager.LoadScene(Application.loadedLevelName);
    }
}
