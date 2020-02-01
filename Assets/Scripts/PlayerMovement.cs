using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerPart
{
    PlayerPart GetPartType();
}

public enum PlayerPart
{
    Feet,
    Front,
};

interface ISpecialTarget
{
    void ForceGrab();
    void ForceRelease();
    void ForceOperate(Vector2 aim);
    void ForceShoot(float forceOfShot);
    float GetForceDistance();
    Vector2 GetPosition();
}

public class PlayerMovement : MonoBehaviour {

    public bool disableMovement = false;
    public float speed = 30.5f;
    public float speedRun = 50.5f;
    public float brakeForce = 30f;
    public float airBrakeForce = 30f;
    public float airDragForce = 30f;
    public float walkDragForce = 10f;
    public float jumpVelocity = 10.0f;
    public float JumpFuelTime = 0.5f;
    public float JumpFuelForceBase = 10f;
    public Animator animator;
    public Rigidbody2D rb;
    public int Health = 1;
    public Transform InitialPosition;
    public float ForceGrabRadius = 5f;
    public float ForceOfShot = 10f;

    public bool Dead
    {
        get
        {
            return state == State.dead;
        }
    }

    internal State state;

    internal enum State
    {
        grounded,
        jumping,
        dead,
    }

    private Vector3 _initialPos;
    private Quaternion _initialRot;
    private Vector3 _initialScale;
    private bool _tookDamage = false;
    private float _jumpFuelTime;
    private bool _jumpFuelActive = false;
    private SpeedControl _xSpeedControl;
    private Vector2 _specialAim = new Vector2(0f,0f);
    private ISpecialTarget _specialTarget;
    private bool _usingForce = false;
    private bool _usingForceCooldown = false;
    private bool _jumpButton = false;

    public void OnGround(IPlayerPart sender)
    {
        if (Dead) return;
        switch (sender.GetPartType())
        {
            case PlayerPart.Feet:
                _jumpFuelTime = JumpFuelTime;
                state = State.grounded;
                break;
        }
    }

    public void InAir(IPlayerPart sender)
    {
        if (Dead) return;
        switch (sender.GetPartType())
        {
            case PlayerPart.Feet:
                state = State.jumping;
                break;
        }
    }

   

    public void PlayerPartTriggerEnter(IPlayerPart part, Collider2D other)
    {
        if (Dead) return;
        switch (other.gameObject.tag)
        {
            case "Ground":
                _xSpeedControl.Reset();
                break;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (Dead) return;
        switch (other.gameObject.tag)
        {
            case "Spit":
                TakeDamage(other.GetComponent<Rigidbody2D>().velocity);
                Destroy(other.gameObject);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == State.dead)
            return;
        if (collision.gameObject.tag == "Mushroom")
        {
            Health += 1;
            animator.SetTrigger("mushroomEaten");
            Destroy(collision.gameObject);
        }
    }

    void TookDamage()
    {
        _tookDamage = false;
        animator.SetBool("GetSmall", false);
    }

    void TakeDamage(Vector2 velocity)
    {
        _tookDamage = true;
        Health -= 1;
        rb.velocity = velocity;
        if (Health == 1)
        {
            animator.SetBool("GetSmall", true);
        }
        if (Health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        state = State.dead;
        rb.constraints = RigidbodyConstraints2D.None;
        Invoke("Revive", 2f);
    }

    void Revive()
    {
        _xSpeedControl.Reset();
        _tookDamage = false;
        rb.transform.position = _initialPos;
        rb.transform.rotation = _initialRot;
        rb.transform.localScale = _initialScale;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        state = State.jumping;
        Health = 1;

    }

    void Moving()
    {
        rb.velocity = new Vector2(_xSpeedControl.Speed, rb.velocity.y);
        animator.SetBool("run", state == State.grounded && Mathf.Abs(rb.velocity.x) >= Mathf.Epsilon);
    }

    void Jumping()
    {
        if (_usingForceCooldown) return;
        if (_jumpButton)
        {
            if (state == State.grounded && _jumpFuelActive == false)
            {   //rb.AddForce(new Vector2(0, jumpVelocity));
                rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);
                state = State.jumping;
                _jumpFuelActive = true;
            }
            if (state == State.jumping && _jumpFuelActive && _jumpFuelTime > 0.0f)
            {
                rb.AddForce(new Vector2(rb.velocity.x, JumpFuelForceBase), ForceMode2D.Force);
                _jumpFuelTime -= Time.fixedDeltaTime;
            }
        }
        else _jumpFuelActive = false;
        animator.SetBool("fly", state == State.jumping);
    }

    // Use this for initialization
    void Start () {
        state = State.grounded;
        _initialPos =  InitialPosition.position;
        _initialRot = InitialPosition.rotation;
        _initialScale = InitialPosition.localScale;
        _jumpFuelTime = JumpFuelTime;
        _xSpeedControl = new SpeedControl(walkDragForce, airDragForce, brakeForce, airBrakeForce, speed, speedRun);
    }
	
	// Update is called once per frame
	void FixedUpdate() {
        if (state == State.dead)
            return;
        if (_specialTarget != null && _usingForce)
        {
            //operate target
            _specialTarget.ForceOperate(_specialAim);
            animator.SetBool("left", _specialTarget.GetPosition().x < transform.position.x);

            return;
        }
        Moving();
        Jumping();
    }

    private void SpecialUse()
    {
        if (_usingForceCooldown) return;
        _usingForce = true;
        ISpecialTarget target = null;
        GameObject targetHit = null;

        if (_specialTarget == null)
        {
            //search for target
            var hits = Physics2D.OverlapCircleAll(transform.position, ForceGrabRadius, 1 << 11);
            float nearestDistance = float.MaxValue;
            foreach (var hit in hits)
            {
                var distance = (transform.position - hit.transform.position).sqrMagnitude;
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    targetHit = hit.gameObject;
                }
            }
            if (targetHit != null)
            {
                target = targetHit.gameObject.GetComponent<ISpecialTarget>();
            }
        }

        if (target != null)
        {
            //grab target
            _specialTarget = target;
            _specialTarget.ForceGrab();
        }
    }

    private void SpecialEnd()
    {
        _jumpButton = false;
        _usingForce = false;
        _usingForceCooldown = true;
        Invoke("SpecialCooldownEnd", 0.5f);
        _specialAim = Vector2.zero;
    }

    private void SpecialCooldownEnd()
    {
        _usingForceCooldown = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Dead)
        {
            SpecialEnd();
            return;
        }
        _jumpButton = Input.GetButton("Jump");
        var _specialButton = Input.GetButton("Special");
        if (_specialButton && state == State.grounded)
        {
            SpecialUse();
        }
        if (_usingForce && _specialButton == false)
        {
            SpecialEnd();
        }

        var xInput = Input.GetAxis("Horizontal");
        if (_usingForce)
        {
            _xSpeedControl.Update(0f, rb.velocity.x, false, false);
            _specialAim = new Vector2(xInput, Input.GetAxis("Vertical"));
            if (_jumpButton && _specialTarget != null)
            {
                _specialTarget.ForceRelease();
                _specialTarget.ForceShoot(ForceOfShot);
                SpecialEnd();
            }
        }
        else if (false == _xSpeedControl.Update(xInput, rb.velocity.x, state == State.jumping, Input.GetButton("Run")))
        {
            animator.SetBool("left", xInput < 0f);
        }
    }
}

public class SpeedControl
{
    private float _xSpeed;
    private readonly float _walkDragForce;
    private readonly float _airDragForce;
    private readonly float _brakeForce;
    private readonly float _airBrakeForce;
    private readonly float _speed;
    private readonly float _runSpeed;

    public float Speed
    {
        get
        {
            return _xSpeed;
        }
    }

    internal SpeedControl(float walkDragForce, float airDragForce, float brakeForce, float airBrakeForce, float speed, float runSpeed)
    {
        _walkDragForce = walkDragForce;
        _airDragForce  = airDragForce ;
        _brakeForce    = brakeForce   ;
        _airBrakeForce = airBrakeForce;
        _speed         = speed;
        _runSpeed      = runSpeed;
    }

    internal void Reset()
    {
        _xSpeed = 0f;
    }

    internal bool Update(float xInput, float xRbVelocity, bool jump, bool run)
    {
        var xInputAbs = Mathf.Abs(xInput);
        var xInputDead = xInputAbs <= 0.1f;
        var walkingInputDead = xInputDead;
        var xSpeedSign = Mathf.Sign(xRbVelocity);
        var newXSpeed = _xSpeed;

        if (walkingInputDead && Mathf.Abs(newXSpeed) <= 0.1f)
        {
            _xSpeed = 0f;
            return walkingInputDead;
        }
        else if (walkingInputDead)
        { // free walk - no input
            if (jump)
                newXSpeed += -xSpeedSign * _airDragForce * Time.deltaTime;
            else
                newXSpeed += -xSpeedSign * _walkDragForce * Time.deltaTime;
            _xSpeed = newXSpeed;
            return walkingInputDead;
        }

        var xInputSign = xInputAbs <= 0.1f ? 0 : Mathf.Sign(xInput);
        var maxSpeed = run ? _runSpeed : _speed;
        if (xSpeedSign != xInputSign)
        {// braking
            if (jump)
                newXSpeed += xInputSign * _airBrakeForce * Time.deltaTime;
            else
                newXSpeed += xInputSign * _brakeForce * Time.deltaTime;
            _xSpeed = newXSpeed;
            return walkingInputDead;
        }
        else if (newXSpeed <= maxSpeed || newXSpeed >= -maxSpeed)
        {// runing
            if (jump)
                newXSpeed += xInputSign * (Mathf.Pow(xInputAbs, 0.9f) * maxSpeed) * Time.deltaTime;
            else
                newXSpeed += xInputSign * (Mathf.Pow(xInputAbs, 2f) * maxSpeed) * Time.deltaTime;
            _xSpeed = Mathf.Clamp(newXSpeed, -maxSpeed, maxSpeed);
            return walkingInputDead;
        }
        else
        {// free walk - run freed
            if (jump)
                newXSpeed += -xSpeedSign * _airDragForce * Time.deltaTime;
            else
                newXSpeed += -xSpeedSign * _walkDragForce * Time.deltaTime;
            _xSpeed = newXSpeed;
            return walkingInputDead;
        }
    }
}
