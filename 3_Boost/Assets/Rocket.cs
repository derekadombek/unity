using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 300f;
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip success;
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem deathParticles;
    [SerializeField] ParticleSystem successParticles;
    enum State {  Alive, Dying, Transcending };
    State state = State.Alive;
    Rigidbody rigidBody;
    AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            RespondToRotateInput();
            RespondToThrustInput();
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;
        float rotationThisFrame = rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        rigidBody.freezeRotation = false;
    } 

    private void RespondToThrustInput()
    {
        float thrustThisFrame = mainThrust * Time.deltaTime;
        ApplyThrust(thrustThisFrame);
    }

    private void ApplyThrust(float thrustThisFrame)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * thrustThisFrame);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(mainEngine);
            }
            if (!mainEngineParticles.isPlaying)
            {
                mainEngineParticles.Play(mainEngine);
            }
        }
        else
        {
            audioSource.Stop();
            mainEngineParticles.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive) { return; }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    private void StartDeathSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(death);
        deathParticles.Play();
        state = State.Dying;
        Invoke("LoadFirstLevel", 3f);
    }

    private void StartSuccessSequence()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(success);
        successParticles.Play();
        state = State.Transcending;
        Invoke("LoadNextLevel", 3f);
    }

    private void LoadFirstLevel()
    {
        state = State.Alive;
        SceneManager.LoadScene(1);
    }

    private void LoadNextLevel()
    {
        state = State.Alive;
        SceneManager.LoadScene(0);
    }
}
