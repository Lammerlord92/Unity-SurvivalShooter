using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    //Cuando un enemigo muere, la velocidad a la que desaparece el cadaver
    public float sinkSpeed = 2.5f;
    public int scoreValue = 10;
    public AudioClip deathClip;


    Animator anim;
    AudioSource enemyAudio;
    ParticleSystem hitParticles;
    CapsuleCollider capsuleCollider;
    bool isDead;
    //Para determinar cuando se empieza a limpiar, tras la animación de la muerte
    bool isSinking;


    void Awake ()
    {
        anim = GetComponent <Animator> ();
        enemyAudio = GetComponent <AudioSource> ();
        hitParticles = GetComponentInChildren <ParticleSystem> ();
        capsuleCollider = GetComponent <CapsuleCollider> ();

        currentHealth = startingHealth;
    }


    void Update ()
    {
        if(isSinking)
        {
            //-Vector3.up mueve hacia abajo
            transform.Translate (-Vector3.up * sinkSpeed * Time.deltaTime);
        }
    }


    public void TakeDamage (int amount, Vector3 hitPoint)
    {
        if(isDead)
            return;

        enemyAudio.Play ();

        currentHealth -= amount;
        
        //Mueve el sistema de partículas al punto de impacto
        hitParticles.transform.position = hitPoint;
        //Muestra el sistema de partículas
        hitParticles.Play();

        if(currentHealth <= 0)
        {
            Death ();
        }
    }


    void Death ()
    {
        isDead = true;
        //Convierte el colisionador en trigger, para que no moleste
        capsuleCollider.isTrigger = true;

        anim.SetTrigger ("Dead");

        enemyAudio.clip = deathClip;
        enemyAudio.Play ();
    }


    public void StartSinking ()
    {
        //"Quitamos" el NavMesh para que no siga moviéndose
        GetComponent <NavMeshAgent> ().enabled = false;
        //If isKinematic is enabled, Forces, collisions or joints will not affect the rigidbody anymore
        GetComponent <Rigidbody> ().isKinematic = true;
        isSinking = true;
        //Como es static, no se tiene por que tener ningún atributo con GetComponent
        ScoreManager.score += scoreValue;
        Destroy (gameObject, 2f);
    }
}
