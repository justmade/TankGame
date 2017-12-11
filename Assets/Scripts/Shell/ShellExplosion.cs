using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;
    public ParticleSystem m_ExplosionParticles;       
    public AudioSource m_ExplosionAudio;    
	public AudioEvent m_ExplosionAudioEvent;
    public float m_MaxDamage = 100f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 5f;              


    private void Start()
    {
        Destroy(gameObject, m_MaxLifeTime);
    }


    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
		Collider[] colliders = Physics.OverlapSphere (transform.position, m_ExplosionRadius);
		// Go through all the colliders...
		for (int i = 0; i < colliders.Length; i++)
		{
			// ... and find their rigidbody.
			Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody> ();

			// If they don't have a rigidbody, go on to the next collider.
			if (targetRigidbody) {
				targetRigidbody.AddExplosionForce (m_ExplosionForce, transform.position, m_ExplosionRadius);
			}
				
			// Add an explosion force.


			Destructible buildingDes = colliders[i].GetComponent<Destructible> ();

			if (buildingDes) {				
				buildingDes.TakeDamage (50);
			}


			// Find the TankHealth script associated with the rigidbody.
			TankHealth targetHealth =  colliders[i].GetComponent<TankHealth> ();
			// If there is no TankHealth script attached to the gameobject, go on to the next collider.
			if (!targetHealth) {
				continue;
			} else {
			}




			// Calculate the amount of damage the target should take based on it's distance from the shell.
			float damage = CalculateDamage (colliders[i].transform.position);
			Debug.Log (damage);

			// Deal this damage to the tank.
			targetHealth.TakeDamage (damage);
		}

		// Unparent the particles from the shell.
		m_ExplosionParticles.transform.parent = null;

		// Play the particle system.
		m_ExplosionParticles.Play();

		// Play the explosion sound effect.
//		m_ExplosionAudio.Play();
		m_ExplosionAudioEvent.Play (m_ExplosionAudio);

		// Once the particles have finished, destroy the gameobject they are on.
		Destroy (m_ExplosionParticles.gameObject, m_ExplosionParticles.main.duration);

		// Destroy the shell.
		Destroy (gameObject);
	}


    private float CalculateDamage(Vector3 targetPosition)
    {
		// Create a vector from the shell to the target.
		Vector3 explosionToTarget = targetPosition - transform.position;

		// Calculate the distance from the shell to the target.
		float explosionDistance = explosionToTarget.magnitude;

		// Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
		float relativeDistance = (m_ExplosionRadius - explosionDistance) / m_ExplosionRadius;

		// Calculate damage as this proportion of the maximum possible damage.
		float damage = relativeDistance * m_MaxDamage;

		// Make sure that the minimum damage is always 0.
		damage = Mathf.Max (0f, damage);

		return damage;
    }
}