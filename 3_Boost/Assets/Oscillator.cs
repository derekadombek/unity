using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(20, 20, 0);
    [SerializeField] float period = 4f;
    [Range(0, 1)]
    float movementFactor;
    Vector3 destinationVector;

    Vector3 startingPos;
    float movementStepSize = 0.2f;
    bool stepUp = true;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float rawSineWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSineWave / 2f + 0.5f;
        destinationVector = startingPos + movementVector * movementFactor;
        transform.position = destinationVector;
    }
}
