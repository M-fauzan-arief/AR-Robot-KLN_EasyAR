    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    [System.Serializable]
    public class EndEffector
    {
        public string type;
        public string enable;
    }

    [System.Serializable]
    public class Data
    {
        public string j1;
        public string j2;
        public string j3;
        public string j4;
        public string status;
        public EndEffector endEffector;
    }

    [System.Serializable]
    public class RobotMessage
    {
        public string nodeID;
        public string moveType;
        public Data data;
        public long unixtime;
    }

    public class Arm_Controller : MonoBehaviour
    {
        private int lastJ1YRot = 0;
        private int lastJ2YRot = 0;
        private int lastJ3YRot = 0;
        private int lastJ4YRot = 0;

        [Header("Slider")]
        public Slider J1_Sliders;
        public Slider J2_Sliders;
        public Slider J3_Sliders;
        public Slider J4_Sliders;

        private int J1_SlidersValue = 0;
        private int J2_SlidersValue = 0;
        private int J3_SlidersValue = 0;
        private int J4_SlidersValue = 0;

        [Header("Joint")]
        public Transform J1;
        public Transform J2;
        public Transform J3;
        public Transform J4;

        [Header("Turn Rate")]
        public int J1_TurnRate = 1;
        public int J2_TurnRate = 1;
        public int J3_TurnRate = 1;
        public int J4_TurnRate = 1;

        private int J1YRot = 0;
        private int J1YRotMin = -135;
        private int J1YRotMax = 135;

        private int J2YRot = 0;
        private int J2YRotMin = -8;
        private int J2YRotMax = 80;

        private int J3YRot = 0;
        private int J3YRotMin = -7;
        private int J3YRotMax = 61;

        private int J4YRot = 0;
        private int J4YRotMin = -145;
        private int J4YRotMax = 145;

        [Header("Reset Button")]
        public Button resetButton;

        [Header("Reset Duration")]
        public float resetDuration = 2.0f;

        private MQTT_Client mqttClient;
        private bool endEffectorEnabled;

        void Start()
        {
            mqttClient = GetComponent<MQTT_Client>();

            J1_Sliders.minValue = -1;
            J2_Sliders.minValue = -1;
            J3_Sliders.minValue = -1;
            J4_Sliders.minValue = -1;

            J1_Sliders.maxValue = 1;
            J2_Sliders.maxValue = 1;
            J3_Sliders.maxValue = 1;
            J4_Sliders.maxValue = 1;

            if (resetButton != null)
            {
                resetButton.onClick.AddListener(StartReset);
            }
        }

        void CheckInput()
        {
            J1_SlidersValue = (int)J1_Sliders.value;
            J2_SlidersValue = (int)J2_Sliders.value;
            J3_SlidersValue = (int)J3_Sliders.value;
            J4_SlidersValue = (int)J4_Sliders.value;
        }

        void ProcessMovement()
        {
            J1YRot += J1_SlidersValue * J1_TurnRate;
            J1YRot = Mathf.Clamp(J1YRot, J1YRotMin, J1YRotMax);
            J1.localEulerAngles = new Vector3(J1.localEulerAngles.x, -J1YRot, J1.localEulerAngles.z);

            J2YRot += J2_SlidersValue * J2_TurnRate;
            J2YRot = Mathf.Clamp(J2YRot, J2YRotMin, J2YRotMax);
            J2.localEulerAngles = new Vector3(J2.localEulerAngles.x, J2YRot, J2.localEulerAngles.z);

            J3YRot += J3_SlidersValue * J3_TurnRate;
            J3YRot = Mathf.Clamp(J3YRot, J3YRotMin, J3YRotMax);
            J3.localEulerAngles = new Vector3(J3YRot, J3.localEulerAngles.y, J3.localEulerAngles.z);

            J4YRot += J4_SlidersValue * J4_TurnRate;
            J4YRot = Mathf.Clamp(J4YRot, J4YRotMin, J4YRotMax);
            J4.localEulerAngles = new Vector3(J4.localEulerAngles.x, J4.localEulerAngles.x, J4YRot);

            SendJointValues();
        }

    private bool canSendData = true;

    void SendJointValues()
    {
        if (!mqttClient.IsConnected())
        {
            Debug.LogError("MQTT client is not connected. Cannot send joint values.");
            return;
        }

        if (!canSendData)
        {
            Debug.Log("Delaying data transmission.");
            return;
        }

        if (J1YRot == lastJ1YRot && J2YRot == lastJ2YRot && J3YRot == lastJ3YRot && J4YRot == lastJ4YRot)
        {
            Debug.Log("Duplicate joint values detected. Skipping message.");
            return;
        }

        var endEffector = new EndEffector
        {
            type = "suck",
            enable = endEffectorEnabled.ToString()
        };

        var data = new Data
        {
            j1 = J1YRot.ToString(),
            j2 = J2YRot.ToString(),
            j3 = J3YRot.ToString(),
            j4 = J4YRot.ToString(),
            status = "True",
            endEffector = endEffector
        };

        var robotMessage = new RobotMessage
        {
            nodeID = "dobot-l-01",
            moveType = "joint",
            data = data,
            unixtime = GetUnixTimestamp()
        };

        StartCoroutine(DelayedPublish(robotMessage));
    }


    
    private IEnumerator DelayedPublish(RobotMessage message)
    {
        canSendData = false; // Disable data transmission temporarily
        mqttClient.PublishJointValues(message);

        // Update last joint values
        lastJ1YRot = J1YRot;
        lastJ2YRot = J2YRot;
        lastJ3YRot = J3YRot;
        lastJ4YRot = J4YRot;

        yield return new WaitForSeconds(0.3f); // 0.5 seconds delay

        canSendData = true; // Enable data transmission
    }

   

    private long GetUnixTimestamp()
        {
            System.DateTime unixEpoch = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
            return (long)(System.DateTime.UtcNow - unixEpoch).TotalSeconds;
        }

        public void ResetSlider()
        {
            J1_SlidersValue = 0;
            J2_SlidersValue = 0;
            J3_SlidersValue = 0;
            J4_SlidersValue = 0;

            J1_Sliders.value = 0;
            J2_Sliders.value = 0;
            J3_Sliders.value = 0;
            J4_Sliders.value = 0;
        }

        private IEnumerator ResetJoints()
        {
            float elapsedTime = 0.0f;

            float initialJ1Rotation = J1YRot;
            float initialJ2Rotation = J2YRot;
            float initialJ3Rotation = J3YRot;
            float initialJ4Rotation = J4YRot;

            while (elapsedTime < resetDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / resetDuration;

                J1YRot = (int)Mathf.Lerp(initialJ1Rotation, 0.0f, t);
                J2YRot = (int)Mathf.Lerp(initialJ2Rotation, 0.0f, t);
                J3YRot = (int)Mathf.Lerp(initialJ3Rotation, 0.0f, t);
                J4YRot = (int)Mathf.Lerp(initialJ4Rotation, 0.0f, t);

                J1.localEulerAngles = new Vector3(J1.localEulerAngles.x, J1YRot, J1.localEulerAngles.z);
                J2.localEulerAngles = new Vector3(J2.localEulerAngles.x, J2YRot, J2.localEulerAngles.z);
                J3.localEulerAngles = new Vector3(J3YRot, J3.localEulerAngles.y, J3.localEulerAngles.z);
                J4.localEulerAngles = new Vector3(J4.localEulerAngles.x, J4.localEulerAngles.x, J4YRot);

                yield return null;
            }

            J1YRot = 0;
            J2YRot = 0;
            J3YRot = 0;
            J4YRot = 0;

            J1.localEulerAngles = new Vector3(J1.localEulerAngles.x, 0.0f, J1.localEulerAngles.z);
            J2.localEulerAngles = new Vector3(J2.localEulerAngles.x, 0.0f, J2.localEulerAngles.z);
            J3.localEulerAngles = new Vector3(0.0f, J3.localEulerAngles.y, J3.localEulerAngles.z);
            J4.localEulerAngles = new Vector3(J4.localEulerAngles.x, J4.localEulerAngles.x, 0.0f);
        }

        private void StartReset()
        {
            StartCoroutine(ResetJoints());
            ResetSlider();
        }

        public void SetEndEffectorState(bool isEnabled)
        {
            endEffectorEnabled = isEnabled;
            SendJointValues();
        }

        void Update()
        {
            CheckInput();
            ProcessMovement();
        }
    }