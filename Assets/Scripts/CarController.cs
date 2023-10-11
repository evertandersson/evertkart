using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum DriveType { RearWheelDrive, FrontWheelDrive, AllWheelDrive }
public class CarController : MonoBehaviour
{
    ParticleSystem explosion;

    public AudioSource accelerateSound;
    public AudioSource boostSound;
    public AudioSource skrt;
    public AudioSource crashSound;

    public bool readyToFinish = false;

    private Vector3 spawnPos;
    private Quaternion spawnRot;

    Rigidbody rb;
    [SerializeField] Transform roofCheck;
    [SerializeField] LayerMask ground;
    bool gameHasEnded = false;

    [SerializeField] float maxAngle = 30f;
    [SerializeField] float maxTorque = 300f;
    [SerializeField] float brakeTorque = 1000f;
    [SerializeField] GameObject wheelShape;

    [SerializeField] float criticalSpeed = 5f;
    [SerializeField] int stepBelow = 5;
    [SerializeField] int stepAbove = 1;
    private float boostPower = 500000f;

    [SerializeField] DriveType driveType;
    WheelCollider[] m_Wheels;
    [SerializeField] float handBrake, torque;
    public float angle;

    public InputActionAsset inputActions;
    InputActionMap gameplayActionMap;
    InputAction handBrakeInputAction;
    InputAction steeringInputAction;
    InputAction accelerationInputAction;

    private void Awake()
    {
        ResetSpawnPos();

        gameplayActionMap = inputActions.FindActionMap("Gameplay");

        handBrakeInputAction = gameplayActionMap.FindAction("Handbrake");
        steeringInputAction = gameplayActionMap.FindAction("SteeringAngle");
        accelerationInputAction = gameplayActionMap.FindAction("Acceleration");

        handBrakeInputAction.performed += GetHandBrakeInput;
        handBrakeInputAction.canceled += GetHandBrakeInput;

        steeringInputAction.performed += GetAngleInput;
        steeringInputAction.canceled += GetAngleInput;

        accelerationInputAction.performed += GetTorqueInput;
        accelerationInputAction.canceled += GetTorqueInput;
    }

    void GetHandBrakeInput(InputAction.CallbackContext context)
    {
        handBrake = context.ReadValue<float>() * brakeTorque;
    }
    void GetAngleInput(InputAction.CallbackContext context)
    {
        angle = context.ReadValue<float>() * maxAngle;
    }
    void GetTorqueInput(InputAction.CallbackContext context)
    {
        torque = context.ReadValue<float>() * maxTorque;
    }

    private void OnEnable()
    {
        handBrakeInputAction.Enable();
        steeringInputAction.Enable();
        accelerationInputAction.Enable();
    }
    private void OnDisable()
    {
        handBrakeInputAction.Disable();
        steeringInputAction.Disable();
        accelerationInputAction.Disable();
    }

    private void Start()
    {
        explosion = GetComponent<ParticleSystem>();
        rb = GetComponent<Rigidbody>();

        m_Wheels = GetComponentsInChildren<WheelCollider>();
        for (int i = 0; i < m_Wheels.Length; i++)
        {
            var wheel = m_Wheels[i];
            if (wheelShape != null)
            {
                var ws = Instantiate(wheelShape);
                ws.transform.parent = wheel.transform;
            }
        }
    }

    private void Update()
    {
        //Sound
        if (torque > 1)
        {
            if (!accelerateSound.isPlaying)
            {
                accelerateSound.volume = 1;
                accelerateSound.Play();
            }
        }
        else
        {
            accelerateSound.volume -= 0.01f;
            if (accelerateSound.volume == 0) accelerateSound.Stop(); 
        }

        if ((angle == maxAngle || angle == -maxAngle) && handBrake == brakeTorque)
        {
            if (!skrt.isPlaying)
            {
                skrt.time = 0.3f;
                skrt.Play();
            }       
        }

        //Check if upside down
        if ((IsUpsideDown() || transform.position.y < -5) && gameHasEnded == false)
        {
            gameHasEnded = true;
            explosion.Play();
            Debug.Log("Upside down");
            Invoke("RespawnCar", 1f);
        }
        
        m_Wheels[0].ConfigureVehicleSubsteps(criticalSpeed, stepBelow, stepAbove);

        foreach (WheelCollider wheel in m_Wheels)
        {
            if (wheel.transform.localPosition.z > 0)
            {
                wheel.steerAngle = angle;
            }
            if (wheel.transform.localPosition.z < 0)
            {
                wheel.brakeTorque = handBrake;
            }
            if (wheel.transform.localPosition.z < 0 && driveType != DriveType.FrontWheelDrive)
            {
                wheel.motorTorque = torque;
            }
            if (wheel.transform.localPosition.z > 0 && driveType != DriveType.RearWheelDrive)
            {
                wheel.motorTorque = torque;
            }
            if (wheelShape)
            {
                Quaternion q;
                Vector3 p;
                wheel.GetWorldPose(out p, out q);

                Transform shapeTransform = wheel.transform.GetChild(0);

                if (wheel.name == "FrontRightWheelCollider" || wheel.name == "BackRightWheelCollider")
                {
                    shapeTransform.rotation = q * Quaternion.Euler(0, 180, 0);
                    shapeTransform.position = p;
                }
                else
                {
                    shapeTransform.position = p;
                    shapeTransform.rotation = q;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.name == "Checkpoint" && !readyToFinish)
        {
            ResetSpawnPos();
            readyToFinish = true;
        }
        if (readyToFinish == true)
        {
            if (collision.gameObject.name == "FinishLine")
            {
                FindObjectOfType<FinishLine>().PlayerScored();
            }
        }
    }

    //Make crash sound when car hits wall
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Walls")
        {
            crashSound.time = 0.5f;
            crashSound.Play();
        }
    }

    //Boost car
    public void BoostCar()
    {
        boostSound.time = 0.4f;
        boostSound.Play();
        GetComponent<Rigidbody>().AddForce(transform.forward * boostPower);
    }

    //returns true if roof collider touches ground
    bool IsUpsideDown()
    {
        //It checks the box collider that surrounds the cars roof and sides
        return Physics.CheckBox(roofCheck.position, new Vector3(1, 0.45f, 0.5f), roofCheck.rotation, ground);
    }

    
    //Every time player crosses checkpoint the spawn pos changes to position of checkpoint
    private void ResetSpawnPos()
    {
        spawnPos = transform.position;
        spawnRot = transform.rotation;
    }

    private void RespawnCar()
    {
        Debug.Log("Respawn");
        rb.velocity = new Vector3(0f, 0f, 0f);
        rb.angularVelocity = new Vector3(0f, 0f, 0f);
        torque = 0;
        transform.position = spawnPos;
        transform.rotation = spawnRot;
        gameHasEnded = false;
    }
    
}