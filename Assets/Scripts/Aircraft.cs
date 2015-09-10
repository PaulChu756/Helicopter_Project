using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Aircraft : MonoBehaviour
{
    //public List<GameObject> aircraft = new List<GameObject>();
    //public GameObject Helicopter;
    public GameObject mainRotor;
    public GameObject tailRotor;
    Vector3 torqueValue;

    float maxRotorForce = 22241.1081f;
    float maxRotorVel = 7200.0f;
    float rotorVel = 0.0f;
    float rotorRot = 0.0f;

    float maxTailRotorForce = 15000.0f;
    float maxTailRotorVel = 2200.0f;
    float tailRotorVel = 0.0f;
    float tailRotorRot = 0.0f;

    float forwardRotorTorqueMult = 0.5f;
    float sidewaysRotorTorqueMult = 0.5f;

    bool mainRotorActive = true;
    bool tailRotorActive = true;
    
    void Update()
    {
        Vector3 controlTorque = new Vector3(Input.GetAxis("Mouse X") * forwardRotorTorqueMult, 1.0f, // input torque for control sensitivity Y value is 1 to lift off easy.
            -Input.GetAxis("Mouse Y") * sidewaysRotorTorqueMult);

        if (mainRotorActive == true) // Main Rotor is active
        {
            torqueValue += (controlTorque * maxRotorForce * rotorVel); // code to life helicopter
            GetComponent<Rigidbody>().AddRelativeForce(Vector3.up * maxRotorForce * rotorVel);
        }

        if (Vector3.Angle(Vector3.up, transform.up) < 80) // When the helicopter is lifting off, the y axis Increase for it to level out so it won't freak out/crash.
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, // Using Quaternion.Slerp  for X and Z rotations without changing.
            Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0),
            Time.deltaTime * rotorVel * 2);
        }

        if (tailRotorActive == true) // tail rotor active.
        {
            torqueValue -= (Vector3.up * maxTailRotorVel * tailRotorVel);
        }
        GetComponent<Rigidbody>().AddRelativeForce(torqueValue);

        if (mainRotorActive == true)
        {
            mainRotor.transform.rotation = transform.rotation *
            Quaternion.Euler(0, rotorRot, 0);
        }
        if (tailRotorActive == true)
        {
            tailRotor.transform.rotation = transform.rotation *
            Quaternion.Euler(tailRotorRot, 0,0);
        }

        rotorRot += maxRotorVel * rotorVel * Time.deltaTime * 0.01f;
        tailRotorRot += maxTailRotorVel * rotorVel * Time.deltaTime;
        float hoverRotorVel = (GetComponent<Rigidbody>().mass * Mathf.Abs(Physics.gravity.y) / maxRotorForce); // Min throttle to keep helicopter stationary
        float hoverTailRotorVel = (maxRotorForce * rotorVel) / maxTailRotorForce;

        if (Input.GetAxis("Mouse X") != 0.0f)
        {
            rotorVel += Input.GetAxis("Mouse X") * 0.001f;
        }
        else
        {
            rotorVel = Mathf.Lerp(rotorVel, hoverRotorVel, Time.deltaTime * Time.deltaTime * 5);
        }

        tailRotorVel = hoverTailRotorVel - Input.GetAxis("Mouse Y");

        if (rotorVel > 1.0f)
        {
            rotorVel = 1.0f;
        }
        else if (rotorVel < 0.0f)
        {
            rotorVel = 0.0f;
        }
    }


}
