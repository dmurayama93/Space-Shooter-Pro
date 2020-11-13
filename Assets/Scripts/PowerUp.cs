using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
   
    [SerializeField] //0 = Triple Shot, 1 = Speed, 2 = Shields
    private int _powerupID;

    [SerializeField]
    private AudioClip _clip;

    // Update is called once per frame
    void Update()
    {
        //move down at a speed of 3
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -8f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();

            AudioSource.PlayClipAtPoint(_clip, transform.position);

            //remember to edit spawn manager each time you add a new powerup
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        //player.NewTripleShot(true);
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    case 3:
                        player.RefillAmmo();
                        break;
                    case 4:
                        player.PlusOneHP();
                        break;
                    case 5:
                        player.RingActive();
                        break;
                    case 6:
                        player.ThrusterDebuff();
                        break;
                    case 7:
                        player.HomingMissileActive();
                        break;

                }
                Destroy(this.gameObject);
            }
        }

        if (other.tag == "EnemyLaser")
        {
            //Debug.Log("PowerUp Hit by Enemy Laser");
            Destroy(this.gameObject);
        }
    }

}
