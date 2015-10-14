using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[RequireComponent(typeof(AudioSource))]

public class Aircraft : MonoBehaviour
{
    //public List<GameObject> aircraft = new List<GameObject>();
    //public GameObject Helicopter;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Control Torque applied to the body of the copter based on player input.
        Vector3 torqueValue = Vector3.zero;
        Vector3 controlTorque = new Vector3(Input.GetAxis("Mouse X") * forwardRotorTorqueMult, 1.0f, -Input.GetAxis("Mouse Y") * sidewaysRotorTorqueMult);

        if (mainRotorActive == true) // Main Rotor is active, apply net torque to the copter body as well the lift force to create spinning rotors.
        {
            torqueValue += (controlTorque * maxRotorForce * rotorVel); // notes control torque to net torque. Also applies force to rb = y.
            rb.AddRelativeForce(Vector3.up * maxRotorForce * rotorVel);
        }

        if (Vector3.Angle(Vector3.up, transform.up) < 80) // When the helicopter is lifting off, the y axis Increase for it to level out so it won't freak out/crash.
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, // Using Quaternion.Slerp  for X and Z rotations without changing.
            Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0),
            Time.deltaTime * rotorVel * 2);
        }

        if (tailRotorActive == true) // tail rotor active. apply tail rotor force to net torque, torque to copter body.
        {
            torqueValue -= (Vector3.up * maxTailRotorForce * tailRotorVel); // subtract rotor max force mult by throttle from net forque and apply copter's body.
        }
        rb.AddRelativeTorque(torqueValue);
    }
    
    void Update()
    {
        if (mainRotorActive == true) // animate main rotor
        {
            mainRotor.transform.rotation = mainRotor.transform.rotation * Quaternion.Euler(0, rotorRot, 0); // Rotors JUST RIGHT! 
        }

        if (tailRotorActive == true) // Animite tail rotor
        {
            tailRotor.transform.rotation = tailRotor.transform.rotation * Quaternion.Euler(0, 0, tailRotorRot); // Tail rotate goes in the correct direction
        }

        rotorRot += maxRotorVel * rotorVel * Time.deltaTime; // having the max rotor vel * rotor vel * delta time to equal and increment rotorRot
        tailRotorRot += maxTailRotorVel * rotorVel * Time.deltaTime; // having maxTailRotorVel * rotor Vel * delta time to equal and increment tailRotorRot

        float hoverRotorVel = (rb.mass * Mathf.Abs(Physics.gravity.y) / maxRotorForce); // counteract force of gravity / max force rotors (0.0f to 1.0f) = minimum throttle to keep copter stationary.
        float hoverTailRotorVel = (maxRotorForce * rotorVel) / maxTailRotorForce; // counteract torque of main rotor, find torque by main rotor to use for tail rotor throttle.

        if (Input.GetAxis("Vertical") != 0.0f) // if the player increase rotor throttle, increase throttle to main rotor
        {
            rotorVel += Input.GetAxis("Vertical") * 0.001f;
        }

        else // to slow it back to hover vel.
        {
            rotorVel = Mathf.Lerp(rotorVel, hoverRotorVel, Time.deltaTime * Time.deltaTime * 5);
        }

        tailRotorVel = hoverTailRotorVel - Input.GetAxis("Horizontal"); // setting tail rot vel to min vel. Increase or decrease by player in

        if (rotorVel > 1.0f) // limit rotor vel to 0 and 1 to make sure for a greater force.
        {
            rotorVel = 1.0f;
        }

        else if(rotorVel < 0.0f)
        {
            rotorVel = 0.0f;
        }
    }

    public GameObject mainRotor;
    public GameObject tailRotor;

    static Rigidbody rb;

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

}
