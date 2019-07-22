using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody rb;
    public Collider myCollider;
    public int deathCount = 0;

    public static Player[] players = new Player[4];
    public int IIndex;
    public float impulse;
    public int sprintN = 3;

    static float freezeTime = 3f;

    KeyCode up, down, left, right, sprint, joySprint;
    enum States { ready, active, frozen, dying, revival, finish}
    //직접 할당하지 말고 SwitchState 함수를 통해서만 바꿀 것
    [SerializeField]
    States currentState = States.active;

    public static void Initiate()
    {
        foreach(Player p in players)
            if(p != null)
                p.SwitchState(States.active);
    }

    public static void Finish()
    {
        foreach (Player p in players)
            if (p != null)
                p.SwitchState(States.finish);
    }

    //currentItem 변수 직접 건들지 말고 프로퍼티 통해서만 할당할 것
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
        joySprint = (KeyCode) System.Enum.Parse(typeof(KeyCode), ("Joystick" + (IIndex+1) +"Button0"));
        
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
    }

    private void Start()
    {
        players[IIndex] = this;

    }

    public float timeTillSprintCharge = 1;
    // Update is called once per frame
    void Update()
    {
        if (currentState != States.dying)
        {
            if (transform.position.y < 0.5f)
            {
                BattleManager.inst.KillPlayer(this);
            }
        }
        if (currentState != States.active) return;
        Vector3 dir = Vector3.zero;
        if (Input.GetKey(up))
            dir.z = 1;
        else if (Input.GetKey(down))
            dir.z = -1;
        if (Input.GetKey(right))
            dir.x = 1;
        else if (Input.GetKey(left))
            dir.x = -1;
        dir.x += Input.GetAxis("Horizontal_" + IIndex);
        dir.z += -Input.GetAxis("Vertical_" + IIndex);

        rb.AddForce(dir * impulse);
        if(sprintable && (Input.GetKeyDown(sprint) || Input.GetKeyDown(joySprint)))
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
        rb.velocity = dir * 1f;
        sprinting = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!sprinting) return;
        var other = collision.gameObject.GetComponent<Player>();
        if (other == null) return;
        other.rb.AddExplosionForce(1000, collision.contacts[0].point, 0.4f);
        if(CurrentItem == Item.ItemType.ice)
            StartCoroutine(other.Freeze());
        else if(CurrentItem == Item.ItemType.spike)
            StartCoroutine(other.Death());
        CurrentItem = Item.ItemType.none;
    }

    public IEnumerator Freeze()
    {
        SwitchState(States.frozen);
        yield return new WaitForSeconds(freezeTime);
        SwitchState(States.active);
    }
    public IEnumerator Death()
    {
        Debug.Log("death");
        SwitchState(States.dying);
        yield return new WaitForSeconds(1f);
        transform.position = BattleManager.inst.SpawnPoint[Random.Range(0, BattleManager.playerN)].transform.position;
        Debug.Log("revive");
        gameObject.SetActive(true);
        SwitchState(States.active);
        yield return new WaitForSeconds(1f);
        gameObject.layer = chrLayer;
    }

    int ghostLayer = 9;
    int chrLayer = 8;
    private void SwitchState(States newState)
    {
        switch(newState)
        {
            case States.dying:
                rb.velocity = Vector3.zero;
                CurrentItem = Item.ItemType.none;
                sprintN = 3;
                timeTillSprintCharge = 1;
                deathCount++;
                gameObject.SetActive(false);
                gameObject.layer = ghostLayer;
                break;
            case States.frozen:
                rb.isKinematic = true;
                break;
            case States.active:
                rb.isKinematic = false;
                break;
        }
        currentState = newState;
    }
}
