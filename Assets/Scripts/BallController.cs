using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    public Rigidbody rb;
    public float impulseForce = 5f;

    private Vector3 startPos;
    public int perfectPass = 0;
    private bool ignoreNextCollision;
    public bool isSuperSpeedActive;
    public DeathPart deathPart;

    readonly HelixController controller;

    [SerializeField] private GameObject RestartPanel;

    [SerializeField] private GameObject _PaintSplashPrefab;
    private float _destroyTime = 1.5f;

    private void Awake()
    {
        startPos = transform.position;
        RestartPanel.SetActive(false);
    }



    private void OnCollisionEnter(Collision other)
    {
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out RaycastHit hitInfo))
        {
            GameObject obj = Instantiate(_PaintSplashPrefab, hitInfo.point, _PaintSplashPrefab.transform.rotation);
            obj.transform.SetParent(other.gameObject.transform);
            obj.transform.position += obj.transform.TransformDirection(Vector3.back) / 1000;
            Destroy(obj, _destroyTime);
        }
        if (ignoreNextCollision)
            return;
        if (isSuperSpeedActive)
        {
            if (!other.transform.GetComponent<Goal>())
            {
                /*foreach (Transform t in other.transform.parent)
                {
                    gameObject.AddComponent<TriangleExplosion>();

                    StartCoroutine(gameObject.GetComponent<TriangleExplosion>().SplitMesh(true));
                    //Destroy(other.gameObject);
                    Debug.Log("exploding - exploding - exploding - exploding");
                }*/
                Destroy(other.transform.parent.gameObject);

            }

        }
        // If super speed is not active and a death part git hit -> restart game
        else
        {
            deathPart = other.transform.GetComponent<DeathPart>();
            if (deathPart)
            {
                RestartPanel.SetActive(true);
                Time.timeScale = 0f;
                //controller.enabled = false;
            }
        }

        rb.velocity = Vector3.zero; // Remove velocity to not make the ball jump higher after falling done a greater distance
        rb.AddForce(Vector3.up * impulseForce, ForceMode.Impulse);



        // Safety check
        ignoreNextCollision = true;
        Invoke("AllowCollision", .2f);

        // Handlig super speed
        perfectPass = 0;
        isSuperSpeedActive = false;
    }

    public void Restarbtn()
    {
        deathPart.HittedDeathPart();
        RestartPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void Update()
    {
        // activate super speed
        if (perfectPass >= 3 && !isSuperSpeedActive)
        {
            isSuperSpeedActive = true;
            rb.AddForce(Vector3.down * 10, ForceMode.Impulse);
        }
    }

    public void ResetBall()
    {
        transform.position = startPos;
    }

    private void AllowCollision()
    {
        ignoreNextCollision = false;
    }


}
