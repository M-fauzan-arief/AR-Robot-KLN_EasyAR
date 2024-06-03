 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Arm_Controller : MonoBehaviour
{

    //Slider di UI
    public Slider J1_Sliders;
    public Slider J2_Sliders;
    public Slider J3_Sliders;
    public Slider J4_Sliders;


    //Default nilai Slider
    public float J1_SlidersValue = 0.0f;
    public float J2_SlidersValue = 0.0f;
    public float J3_SlidersValue = 0.0f;
    public float J4_SlidersValue = 0.0f;

    //Mengambil nilai Transform dari Joint
    public Transform J1;
    public Transform J2;
    public Transform J3;
    public Transform J4;


    //Pengaturan kecepatan rotasi tiap joint
    public float J1_TurnRate = 1.0f;
    public float J2_TurnRate = 1.0f;
    public float J3_TurnRate = 1.0f;
    public float J4_TurnRate = 1.0f;


    //Nilai max & min rotasi joint 1
    private float J1YRot = 0.0f;
    private float J1YRotMin = -135.0f;
    private float J1YRotMax = 135.0f;

    //Nilai max & min rotasi joint 2
    private float J2YRot = 180.0f;
 
    private float J2YRotMin = 120.0f;
    private float J2YRotMax = 225.0f;

    //Nilai max & min rotasi joint 3
    private float J3YRot = 0.0f;
    private float J3YRotMin = -10.0f;
    private float J3YRotMax = 85.0f;


    //Nilai max & min rotasi joint 4
    private float J4YRot = 0.0f;
    private float J4YRotMin = -360.0f;
    private float J4YRotMax = 360.0f;




    // Start is called before the first frame update
    void Start()
    {
        //mengatur nilai minimum dari slider joint
        J1_Sliders.minValue = -1;
        J2_Sliders.minValue = -1;
        J3_Sliders.minValue = -1;
        J4_Sliders.minValue = -1;

        //mengatur nilai maksimum dari slider joint
        J1_Sliders.maxValue = 1;
        J2_Sliders.maxValue = 1;
        J3_Sliders.maxValue = 1;
        J4_Sliders.maxValue = 1;

    }

    void CheckInput()
    {
        J1_SlidersValue = J1_Sliders.value;
        J2_SlidersValue = J2_Sliders.value;
        J3_SlidersValue = J3_Sliders.value;
        J4_SlidersValue = J4_Sliders.value;
    }

    void ProcessMovement()
    {

        //J1
        J1YRot += J1_SlidersValue * J1_TurnRate;
        J1YRot = Mathf.Clamp(J1YRot, J1YRotMin, J1YRotMax);
        J1.localEulerAngles = new Vector3(J1.localEulerAngles.x, J1YRot, J1.localEulerAngles.z);

        //J2
        J2YRot += J2_SlidersValue * J2_TurnRate;
        J2YRot = Mathf.Clamp(J2YRot, J2YRotMin, J2YRotMax);
        J2.localEulerAngles = new Vector3(J2.localEulerAngles.x, - J2YRot, J2.localEulerAngles.z);


        //J3
        J3YRot += J3_SlidersValue * J3_TurnRate;
        J3YRot = Mathf.Clamp(J3YRot, J3YRotMin, J3YRotMax);
        J3.localEulerAngles = new Vector3(J3.localEulerAngles.x, J3YRot, J3.localEulerAngles.z);

        //J4
        J4YRot += J4_SlidersValue * J4_TurnRate;
        J4YRot = Mathf.Clamp(J4YRot, J4YRotMin, J4YRotMax);
        J4.localEulerAngles = new Vector3(J4.localEulerAngles.x, J4.localEulerAngles.x, J4YRot);

    }
     

    public void ResetSlider()
    {
        J1_SlidersValue = 0.0f;
        J2_SlidersValue = 0.0f;
        J3_SlidersValue = 0.0f;
        J4_SlidersValue = 0.0f;

        J1_Sliders.value = 0.0f;
        J2_Sliders.value = 0.0f;
        J3_Sliders.value = 0.0f;
        J4_Sliders.value = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        CheckInput();
        ProcessMovement();
    }
}
