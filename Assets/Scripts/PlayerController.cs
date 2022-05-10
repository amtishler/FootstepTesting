using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class PlayerController : MonoBehaviour
{
    //Footstep variables
    private RaycastHit foot;
    [SerializeField] private float rayDistance = 3.0f;
    [SerializeField] private float stepDistance = 3.0f;
    // [SerializeField] bool isMoving;
    [SerializeField] private EventReference footstepsEventPath;
    [SerializeField] private string materialParameterName;
    public string[] materialTypes;
    private int F_materialValue;
    [HideInInspector] public int defaultMaterialValue;
    private float stepRandom;
    private Vector3 prevPos;
    private float distanceTravelled;

    [SerializeField] Transform playerCamera = null;
    [SerializeField] public float mouseSensitivity = 3.5f;
    [SerializeField] float walkSpeed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;

    float cameraPitch = 0.0f;
    CharacterController controller = null;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;
    [SerializeField] private GameManager GM;

    // Start is called before the first frame update
    void Start()
    {   
        controller = GetComponent<CharacterController>();
        stepRandom = Random.Range(0f, 0.5f);
        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMouseLook();
        UpdateMovement();
        //timeTakenSinceStep = Time.deltaTime;
        distanceTravelled += (transform.position - prevPos).magnitude;
        if(distanceTravelled >= stepDistance + stepRandom)
        {
            MaterialCheck();
            PlayFootsteps();
            stepRandom = Random.Range(0f, 0.5f);
            distanceTravelled = 0f;
        }
        prevPos = transform.position;
     
    }

     void UpdateMouseLook()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;

        cameraPitch = Mathf.Clamp(cameraPitch, -48f, 48f);

        playerCamera.localEulerAngles = Vector3.right * cameraPitch;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMovement()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        Vector3 velocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * walkSpeed;
        
        controller.Move(velocity * Time.deltaTime);
    }

    void MaterialCheck()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out foot, rayDistance))
        {
            Debug.Log(foot.collider.gameObject.GetComponent<MaterialSetter>().materialValue);
            if(foot.collider.gameObject.GetComponent<MaterialSetter>().materialValue == 1)
            {
                F_materialValue = foot.collider.gameObject.GetComponent<MaterialSetter>().materialValue;
            }
            else if(foot.collider.gameObject.GetComponent<MaterialSetter>().materialValue == 2)
            {
                F_materialValue = foot.collider.gameObject.GetComponent<MaterialSetter>().materialValue;
                GM.incrementDoom();
            }
            else
            {
                F_materialValue = defaultMaterialValue;
            }
        }
        else
        {
            F_materialValue = defaultMaterialValue;
        }
    }

    void PlayFootsteps()
    {
        EventInstance footsteps = RuntimeManager.CreateInstance(footstepsEventPath);
        RuntimeManager.AttachInstanceToGameObject(footsteps, transform, GetComponent<Rigidbody>());
        footsteps.setParameterByName(materialParameterName, F_materialValue);
        footsteps.start();
        footsteps.release();
    }


    public void changeSensitivity(float dpi) {
        mouseSensitivity = dpi;
    }
}
