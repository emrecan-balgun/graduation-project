using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

public class TreasureHuntAgent : Agent
{
    public TreasureHuntSettings m_TreasureHuntSettings;
    public GameObject arena;
  
    
    bool m_Frozen;
    bool m_Poisoned;
    bool m_Satiated;
    bool m_Shoot;

    float m_FrozenTime;
    float m_EffectTime;

    Rigidbody m_AgentRb;
    float m_LaserLength;

    // Speed of agent rotation.
    public float turnSpeed = 300;

    // Speed of agent movement.
    public float moveSpeed = 2;
    public GameObject myLaser;
    public bool contribute;
    public bool useVectorObs;

    EnvironmentParameters m_ResetParams;

    public override void Initialize()
    {
        m_AgentRb = GetComponent<Rigidbody>();
        m_TreasureHuntSettings = FindObjectOfType<TreasureHuntSettings>();
        m_ResetParams = Academy.Instance.EnvironmentParameters;
        SetResetParameters();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        if (useVectorObs)
        {
            var localVelocity = transform.InverseTransformDirection(m_AgentRb.velocity);
            sensor.AddObservation(localVelocity.x);
            sensor.AddObservation(localVelocity.z);
            sensor.AddObservation(System.Convert.ToInt32(m_Frozen));
            sensor.AddObservation(System.Convert.ToInt32(m_Shoot));
        }
    }

    public Color32 ToColor(int hexVal)
    {
        var r = (byte)((hexVal >> 16) & 0xFF);
        var g = (byte)((hexVal >> 8) & 0xFF);
        var b = (byte)(hexVal & 0xFF);
        return new Color32(r, g, b, 255);
    }

    public void MoveAgent(float[] act)
    {
        m_Shoot = false;

        if (Time.time > m_FrozenTime + 4f && m_Frozen)
        {
            Unfreeze();
        }
        if (Time.time > m_EffectTime + 0.5f)
        {
            if (m_Poisoned)
            {
                Unpoison();
            }
            if (m_Satiated)
            {
                Unsatiate();
            }
        }

        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        if (!m_Frozen)
        {
            var shootCommand = false;
            var forwardAxis = (int)act[0];
            var rightAxis = (int)act[1];
            var rotateAxis = (int)act[2];
            var shootAxis = (int)act[3];

            switch (forwardAxis)
            {
                case 1:
                    dirToGo = transform.forward;
                    break;
                case 2:
                    dirToGo = -transform.forward;
                    break;
            }

            switch (rightAxis)
            {
                case 1:
                    dirToGo = transform.right;
                    break;
                case 2:
                    dirToGo = -transform.right;
                    break;
            }

            switch (rotateAxis)
            {
                case 1:
                    rotateDir = -transform.up;
                    break;
                case 2:
                    rotateDir = transform.up;
                    break;
            }
            switch (shootAxis)
            {
                case 1:
                    shootCommand = true;
                    break;
            }
            if (shootCommand)
            {
                m_Shoot = true;
                dirToGo *= 0.5f;
                m_AgentRb.velocity *= 0.75f;
            }
            m_AgentRb.AddForce(dirToGo * moveSpeed, ForceMode.VelocityChange);
            transform.Rotate(rotateDir, Time.fixedDeltaTime * turnSpeed);
        }

        if (m_AgentRb.velocity.sqrMagnitude > 25f) // slow it down
        {
            m_AgentRb.velocity *= 0.95f;
        }

        if (m_Shoot)
        {
            var myTransform = transform;
            myLaser.transform.localScale = new Vector3(1f, 1f, m_LaserLength);
            var rayDir = 25.0f * myTransform.forward;
            Debug.DrawRay(myTransform.position, rayDir, Color.red, 0f, true);
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, 2f, rayDir, out hit, 25f))
            {
                if (hit.collider.gameObject.CompareTag("agent"))
                {
                    hit.collider.gameObject.GetComponent<TreasureHuntAgent>().Freeze();
                }
            }
        }
        else
        {
            myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
        }
    }

    void Freeze()
    {
        gameObject.tag = "frozenAgent";
        m_Frozen = true;
        m_FrozenTime = Time.time;
    }

    void Unfreeze()
    {
        m_Frozen = false;
        gameObject.tag = "agent";
    }

    void Poison()
    {
        m_Poisoned = true;
        m_EffectTime = Time.time;
        moveSpeed = 1;
    }

    void Unpoison()
    {
        m_Poisoned = false;
        moveSpeed = 2;
    }

    void Satiate()
    {
        m_Satiated = true;
        m_EffectTime = Time.time;
        moveSpeed = 3;
    }

    void Unsatiate()
    {
        m_Satiated = false;
        moveSpeed = 2;
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        MoveAgent(vectorAction);
    }

    public override void Heuristic(float[] actionsOut)
    {
        actionsOut[0] = 0f;
        actionsOut[1] = 0f;
        actionsOut[2] = 0f;
        if (Input.GetKey(KeyCode.D))
        {
            actionsOut[2] = 2f;
        }
        if (Input.GetKey(KeyCode.W))
        {
            actionsOut[0] = 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            actionsOut[2] = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            actionsOut[0] = 2f;
        }
        actionsOut[3] = Input.GetKey(KeyCode.Space) ? 1.0f : 0.0f;
    }

    public override void OnEpisodeBegin()
    {
        Unfreeze();
        Unpoison();
        Unsatiate();
        m_Shoot = false;
        m_AgentRb.velocity = Vector3.zero;
        myLaser.transform.localScale = new Vector3(0f, 0f, 0f);
        transform.position = new Vector3(Random.Range(-Arena.range, Arena.range),
            -23.2f, Random.Range(-Arena.range, Arena.range))
            + arena.transform.position;
        transform.rotation = Quaternion.Euler(new Vector3(0f, Random.Range(0, 360)));

        SetResetParameters();
    }

    void OnCollisionEnter(Collision collision)
    {
      
        if (collision.gameObject.CompareTag("gem"))
        {
            Satiate();
            collision.gameObject.GetComponent<ObjectLogic>().OnEaten();
            AddReward(1f);
            if (contribute)
            {
                m_TreasureHuntSettings.totalScore += 1;
              
            }
        }
        if (collision.gameObject.CompareTag("mutant"))
        {
            Poison();
            collision.gameObject.GetComponent<ObjectLogic>().OnEaten();

            AddReward(-1f);
            if (contribute)
            {
                m_TreasureHuntSettings.totalScore -= 1;
              
            }
        }
        if (collision.gameObject.CompareTag("chest"))
        {
            int feature = collision.gameObject.GetComponent<Chest>().feature;
            
            if (feature>0)
                Satiate();
            
            else
            {
                Poison();
                print("POISOON");
            }
            
            collision.gameObject.GetComponent<ObjectLogic>().OnEaten();
            AddReward(feature);
            m_TreasureHuntSettings.totalScore += feature;
            collision.gameObject.transform.position = ChestManager.SetRandomPosition(); 
            
        }
    }

    public void SetLaserLengths()
    {
        m_LaserLength = m_ResetParams.GetWithDefault("laser_length", 1.0f);
    }

    public void SetAgentScale()
    {
        float agentScale = m_ResetParams.GetWithDefault("agent_scale", 1.0f);
        gameObject.transform.localScale = new Vector3(agentScale, agentScale, agentScale);
    }

    public void SetResetParameters()
    {
        SetLaserLengths();
        SetAgentScale();
    }
}
