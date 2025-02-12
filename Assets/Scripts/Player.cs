using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public float combo;
    public float multiplier = 1;

    [SerializeField] TextMeshProUGUI combo_text;
    [SerializeField] TextMeshProUGUI multiplier_text;
    [SerializeField] Camera cam;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform pogoEnd;
    [SerializeField] SliderJoint2D pogoJoint;
    Rigidbody2D rb;
    Vector3 startPos;

    float jumpForce;
    float timeInComboRange;

    void Start()
    {
        startPos = transform.position;
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        combo_text.text = "COMBO: " + combo.ToString();
        multiplier_text.text = "x " + string.Format("{0:0.0}", multiplier.ToString());

        multiplier = Mathf.Clamp(1 + combo / 10, 1, 2);

        PogoJump();
        RotatePlayer();
        Respawn();
    }

    private void PogoJump() 
    {
        if (Input.GetKey(KeyCode.Space))
        {
            JointTranslationLimits2D limits = pogoJoint.limits;
            JointMotor2D motor = pogoJoint.motor;

            limits.min = 0.88f;
            limits.max = 1.3f - 0.042f * jumpForce;

            pogoJoint.limits = limits;
            pogoJoint.motor = motor;

            jumpForce = Mathf.Clamp(jumpForce + Time.deltaTime * 15, 0, 10);
        }
        else if (jumpForce > 0)
        {
            JointTranslationLimits2D limits = pogoJoint.limits;
            JointMotor2D motor = pogoJoint.motor;
            limits.max = 1.3f;
            pogoJoint.limits = limits;
            motor.motorSpeed = jumpForce * multiplier;

            StartCoroutine(PogoReset());
            pogoJoint.motor = motor;
            jumpForce = 0;

            if (IsInComboRange())
            {
                combo += 1;
            }
            else
            {
                combo = 0;
            }
        }

        if (IsInComboRange())
        {
            timeInComboRange += Time.deltaTime;
        }
        else
        {
            timeInComboRange = 0;
        }

        if (IsInComboRange() && timeInComboRange > 0.25f)
        {
            combo = 0;
        }
    }
    private void RotatePlayer()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

        Vector2 direction = (mousePosition - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;

        float angleDifference = Mathf.DeltaAngle(rb.rotation, targetAngle);
        float rotationSpeed = Mathf.Lerp(0.1f, 40, Mathf.Abs(angleDifference) / 180f);

        float rotateAmount = targetAngle - rb.rotation;
        rotateAmount = Mathf.Repeat(rotateAmount + 180f, 360f) - 180f;
        float rotateDirection = Mathf.Sign(rotateAmount);

        float torque = rotateDirection * rotationSpeed;

        rb.AddTorque(torque);
    }

    private bool IsInComboRange()
    {
        return Physics2D.CircleCast(pogoEnd.position, 0.3f, Vector2.zero, 0.3f, groundLayer);
    }

    private void Respawn()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.position = startPos;
            transform.rotation = new Quaternion();
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            startPos = transform.position;
        }
    }

    private IEnumerator PogoReset()
    {
        yield return new WaitForSeconds(0.2f);
        JointMotor2D motor = pogoJoint.motor;
        JointTranslationLimits2D limits = pogoJoint.limits;

        motor.motorSpeed = 0;
        limits.min = 1.26f;

        pogoJoint.motor = motor;
        pogoJoint.limits = limits;
    }
}
