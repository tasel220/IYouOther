using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public int IIndex;
    public float impulse;
    public int sprintN = 3;

    static float freezeTime = 3f;

    KeyCode up, down, left, right, sprint;
    enum States { ready, active, frozen, dying, revival}
    States currentState;

    Item.ItemType currentItem;
    public Item.ItemType CurrentItem
    {
        get
        {
            return currentItem;
        }
        set
        {
            currentItem = value;
            switch(currentItem)
            {
                case Item.ItemType.none:
                    iceModel.SetActive(false);
                    spikeModel.SetActive(false);
                    break;
                case Item.ItemType.ice:
                    spikeModel.SetActive(false);
                    iceModel.SetActive(true);
                    break;
                case Item.ItemType.spike:
                    iceModel.SetActive(false);
                    spikeModel.SetActive(true);
                    break;
            }
         }
    }

    public GameObject iceModel;
    public GameObject spikeModel;

    public float sprintTime;
    public float sprintImpulse;
    bool sprintable
    {
        get
        {
            return ((!sprinting && sprintN > 0) /*&& currentState == States.active*/);
        }
    }

    private void Awake()
    {
        up = (IIndex == 0) ? KeyCode.W : KeyCode.I;
        down = (IIndex == 0) ? KeyCode.S : KeyCode.K;
        left = (IIndex == 0) ? KeyCode.A : KeyCode.J;
        right = (IIndex == 0) ? KeyCode.D : KeyCode.L;
        sprint = (IIndex == 0) ? KeyCode.LeftShift : KeyCode.RightShift;
        rb = GetComponent<Rigidbody>();
    }

    public float timeTillSprintCharge = 1;
    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(up))
            dir.z = 1;
        else if (Input.GetKey(down))
            dir.z = -1;
        if (Input.GetKey(right))
            dir.x = 1;
        else if (Input.GetKey(left))
            dir.x = -1;

        rb.AddForce(dir * impulse);
        if(sprintable && Input.GetKey(sprint))
        {
            StartCoroutine(Sprint(dir));
        }
        if (sprintN < 3)
        {
            if (timeTillSprintCharge <= 0)
            {
                sprintN += sprintN == 0 ? 3: 1;
                timeTillSprintCharge = 1;
            }
            else
                timeTillSprintCharge -= Time.deltaTime;
        }
    }

    public bool sprinting = false;
    IEnumerator Sprint(Vector3 dir)
    {
        sprintN--;
        sprinting = true;
        timeTillSprintCharge = sprintN == 0 ? 3f : 1f;
        rb.velocity = dir *  sprintImpulse;
        yield return new WaitForSeconds(sprintTime);
        rb.velocity = dir * 0.2f;
        sprinting = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!sprinting) return;
        var other = collision.gameObject.GetComponent<Player>();
        if(CurrentItem == Item.ItemType.ice)
            StartCoroutine(other.Freeze());
        else
            other.Burst();
        CurrentItem = Item.ItemType.none;
    }

    public IEnumerator Freeze()
    {
        rb.isKinematic = true;
        currentState = States.frozen;
        yield return new WaitForSeconds(freezeTime);
        currentState = States.active;
        rb.isKinematic = false;
    }
    public void Burst()
    {
        Destroy(gameObject);
    }
}
