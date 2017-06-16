using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    class ParticleData
    {
        public ParticleData() { }
        public GameObject particleObject;
        public Vector3 direction;
        public float speed;
        public Vector3 rotation;
        public float scale;
        public float dampen_rate;

        public void dampen(bool dampen_speed, bool dampen_scale, bool dampen_alpha)
        {
            if(dampen_speed)
                this.speed *= this.dampen_rate;

            if(dampen_scale)
                this.scale *= this.dampen_rate;

            if (dampen_alpha)
                this.particleObject.GetComponent<MeshRenderer>().material.color *= new Color(1.0f, 1.0f, 1.0f, this.dampen_rate);
        }

        public bool isDead()
        {
            float minDims = 0.01f;
            return this.speed <= minDims || this.scale <= minDims;
        }
    }

    public GameObject particle;
    public int min_number_of_particles;
    public int max_number_of_particles;
    public float min_particle_speed;
    public float max_particle_speed;
    public float min_particle_rotate;
    public float max_particle_rotate;
    public float min_dampen;
    public float max_dampen;
    public float min_particle_scale;
    public float max_particle_scale;
    public bool dampen_speed;
    public bool dampen_scale;
    public float max_spawn_range_x;
    public float max_spawn_range_y;
    public float max_spawn_range_z;
    public float colour_r;
    public float colour_g;
    public float colour_b;
    public float colour_a;
    public bool dampen_alpha;

    private Vector3 base_position;
    private List<ParticleData> particles;
    private int number_of_particles;

	// Use this for initialization
	void Start () {

        //initialise a list of random particles
        this.base_position = Vector3.zero;
        int number_of_particles = Random.Range(min_number_of_particles, max_number_of_particles);
        //Debug.Log("explosion of size: " + number_of_particles);

        this.particles = new List<ParticleData>();

        for(int i = 0; i < number_of_particles; ++i)
        {
            Vector3 direction = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
            direction.Normalize();
            Vector3 spawnOffset = new Vector3(
                                    Random.Range(-max_spawn_range_x / 2, max_spawn_range_x / 2),
                                    Random.Range(-max_spawn_range_y / 2, max_spawn_range_y / 2),
                                    Random.Range(-max_spawn_range_z / 2, max_spawn_range_z / 2));
            GameObject particleTemp = (Instantiate(particle, this.transform.position + spawnOffset, this.transform.rotation) as GameObject);
            particleTemp.GetComponent<MeshRenderer>().material.color = 
                new Color(colour_r, colour_g, colour_b, colour_a);
            ParticleData pData = new ParticleData();
            pData.particleObject = particleTemp;
            pData.speed = Random.Range(min_particle_speed, max_particle_speed);
            pData.rotation.x = Random.Range(min_particle_rotate, max_particle_rotate);
            pData.rotation.y = Random.Range(min_particle_rotate, max_particle_rotate);
            pData.rotation.z = Random.Range(min_particle_rotate, max_particle_rotate);
            pData.scale = Random.Range(min_particle_scale, max_particle_scale);
            pData.dampen_rate = Random.Range(min_dampen, max_dampen);
            pData.direction = direction;
            this.particles.Add(pData);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
        for(int i = 0; i < this.particles.Count; ++i)
        {
            this.particles[i].particleObject.transform.SetPositionAndRotation(
                this.base_position +
                this.particles[i].particleObject.transform.position + this.particles[i].direction * this.particles[i].speed,
                this.particles[i].particleObject.transform.rotation);
            this.particles[i].particleObject.transform.Rotate(this.particles[i].rotation);
            this.particles[i].particleObject.transform.localScale = new Vector3(this.particles[i].scale, this.particles[i].scale, this.particles[i].scale);

            //dampen
            this.particles[i].dampen(dampen_speed, dampen_scale, dampen_alpha);

            if(this.particles[i].isDead())
            {
                Destroy(this.particles[i].particleObject.gameObject);
                this.particles.RemoveAt(i);
                --i;
            }
        }

        if (this.particles.Count <= 0)
            Destroy(this.gameObject);
	}

    public void setBasePosition(Vector3 pos) { this.base_position = pos; }
    public void iterateBasePosition(Vector3 iter) { this.base_position += iter; }
}
