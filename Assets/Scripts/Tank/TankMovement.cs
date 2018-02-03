using UnityEngine;

public class TankMovement : MonoBehaviour
{
    public int m_PlayerNumber = 1;         
    public float m_Speed = 12f;            
    public float m_TurnSpeed = 180f;       
    public AudioSource m_MovementAudio;    
    public AudioClip m_EngineIdling;       
    public AudioClip m_EngineDriving;      
    public float m_PitchRange = 0.2f;

    
    private string m_MovementAxisName;     
    private string m_TurnAxisName;         
    private Rigidbody m_Rigidbody;         
    private float m_MovementInputValue;    
    private float m_TurnInputValue;        
    private float m_OriginalPitch;    

	private Quaternion turnRotation= Quaternion.Euler (0f, 0f, 0f);
	private Quaternion lastRotation= Quaternion.Euler (0f, 0f, 0f);

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable ()
    {
        m_Rigidbody.isKinematic = false;
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }


    private void OnDisable ()
    {
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        m_MovementAxisName = "Vertical" + m_PlayerNumber;
        m_TurnAxisName = "Horizontal" + m_PlayerNumber;

        m_OriginalPitch = m_MovementAudio.pitch;
    }
   

    private void Update()
    {
        // Store the player's input and make sure the audio for the engine is playing.
		m_MovementInputValue = Input.GetAxis(m_MovementAxisName);
		m_TurnInputValue = Input.GetAxis (m_TurnAxisName);

		EngineAudio ();
	}


    private void EngineAudio()
    {
        // Play the correct audio clip based on whether or not the tank is moving and what audio is currently playing.
    
		if (Mathf.Abs (m_MovementInputValue) < 0.1f
		    && Mathf.Abs (m_TurnInputValue) < 0.1f) {
			if (m_MovementAudio.clip == m_EngineDriving) {
				m_MovementAudio.clip = m_EngineIdling;
				m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange , m_OriginalPitch + m_PitchRange);
				m_MovementAudio.Play ();
			}
		} else {
			if (m_MovementAudio.clip == m_EngineIdling) {
				m_MovementAudio.clip = m_EngineDriving;
				m_MovementAudio.pitch = Random.Range (m_OriginalPitch - m_PitchRange , m_OriginalPitch + m_PitchRange);
				m_MovementAudio.Play ();
			}
		}
	}


    private void FixedUpdate()
    {
        // Move and turn the tank.
//		Move();
//		Turn();
		getTankDirection ();
    }



    private void Move()
    {
        // Adjust the position of the tank based on the player's input.
		Vector3 movement = transform.forward * m_MovementInputValue * m_Speed * Time.deltaTime;
		m_Rigidbody.MovePosition (m_Rigidbody.position + movement);
    }


    private void Turn()
    {
        // Adjust the rotation of the tank based on the player's input.
		float turn = m_TurnInputValue * m_TurnSpeed * Time.deltaTime;
		Quaternion turnRotation = Quaternion.Euler (0f, turn, 0f);
		m_Rigidbody.MoveRotation (m_Rigidbody.rotation * turnRotation); 
//		m_Rigidbody.rotation = turnRotation;
    }

	private void getTankDirection()
	{
//		if (lastRotation) {
//			lastRotation = Quaternion.Euler (0f, 0f, 0f);
//		}
		m_TurnInputValue = 0;
		m_MovementInputValue = 0;

		if (Input.GetKey (KeyCode.W)) {
			m_MovementInputValue = 1;
		}else if (Input.GetKey (KeyCode.S)) {
			m_MovementInputValue = -1;
		} 
		if (Input.GetKey (KeyCode.A)) {
			m_TurnInputValue = -1;
		}else if (Input.GetKey (KeyCode.D)) {
			m_TurnInputValue = 1;
		}

		if (m_TurnInputValue == 0) {
			
			if (m_MovementInputValue > 0) {
				turnRotation = Quaternion.Euler (0f, 0f, 0f);
			} else if (m_MovementInputValue < 0) {
				turnRotation = Quaternion.Euler (0f, 180f, 0f);
			}

		} else if (m_TurnInputValue > 0) {
			
			if (m_MovementInputValue > 0) {
				turnRotation = Quaternion.Euler (0f, 45f, 0f);
			} else if (m_MovementInputValue < 0) {
				turnRotation = Quaternion.Euler (0f, 135f, 0f);
			} else if (m_MovementInputValue == 0) {
				turnRotation = Quaternion.Euler (0f, 90f, 0f);
			}
		} else if (m_TurnInputValue < 0) {

			if (m_MovementInputValue > 0) {
				turnRotation = Quaternion.Euler (0f, -45f, 0f);
			} else if (m_MovementInputValue < 0) {
				turnRotation = Quaternion.Euler (0f, -135f, 0f);
			} else if (m_MovementInputValue == 0) {
				turnRotation = Quaternion.Euler (0f, -90f, 0f);
			}
		}
		turnRotation = Quaternion.RotateTowards (lastRotation, turnRotation,20);

//		Debug.Log ("m_TurnInputValue"+ m_TurnInputValue);
//		Debug.Log ("m_MovementInputValue"+ m_MovementInputValue);
		m_Rigidbody.MoveRotation(turnRotation);
		lastRotation = turnRotation;
//		m_Rigidbody.rotation = turnRotation;


		Vector3 movement = transform.forward * 1 * m_Speed * Time.deltaTime;
		m_Rigidbody.MovePosition (m_Rigidbody.position + movement);
	}
}