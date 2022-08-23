using UnityEngine;
[RequireComponent(typeof(CharacterController2D))]

public class FrogAI : MonoBehaviour
{
    private CharacterController2D character;
    private IState state;
    private interface IState
    {
        void Update();
    }

    private abstract class State<TOwner> : IState
    {
        protected State(TOwner owner)
        {
            Owner = owner;
        }
        protected TOwner Owner
        {
            get; private set;
        }
        public abstract void Update();
    }

    private class Idle : State<FrogAI>
    {
        private float elapsed;
        public Idle(FrogAI owner, float time = 2.5f):base(owner)
        {
            elapsed = time;
        }
        public override void Update()
        {
            elapsed -= Time.deltaTime;
            if(elapsed <0)
            {
                Owner.state = new Think(Owner);
            }
        }
    }
    private class Flip : State<FrogAI>
    {
        public Flip(FrogAI owner): base(owner)
        {

        }
        public override void Update()
        {
            Owner.character.Flip();
            Owner.state = new Think(Owner);
        }
    }
    private class Jump : State<FrogAI>
    {
        public float leap = 3f;
        public float height = 1f;
        private float airElapsed = 0.3f;
        public Jump(FrogAI owner) : base(owner)
        {

        }
        public override void Update()
        {
            var character = Owner.character;
            airElapsed -= Time.deltaTime;
            if (airElapsed < 0 && character.isGrounded)
            {
                Owner.state = new Think(Owner);
            }
            else
            {
                float offsetX = character.isFacingRight ? leap : -leap;
                character.Move(offsetX);
                character.Jump(height);
            }
        }
    }
    private class Think : State<FrogAI>
    {
        public Think(FrogAI owner):base(owner)
        {

        }
        public override void Update()
        {
            var state = Random.Range(0, 7);
            if (state <= 1)
            {
                Owner.state = new Idle(Owner);
            }
            else if (state <=3)
            {
                Owner.state = new Flip(Owner);
            }
            else
            {
                Owner.state = new Jump(Owner);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController2D>();
        state = new Idle(this);
    }

    // Update is called once per frame
    void Update()
    {
        state.Update();
    }
}
