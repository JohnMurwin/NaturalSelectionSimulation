using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace NaturalSelectionSimulation
{
    [RequireComponent(typeof(Animator)), RequireComponent(typeof(CharacterController))]
    public class Rabbit_Controller : MonoBehaviour
    {
        #region PublicVariables
        public bool isPregnant = false;
        public float mateTimer = 0;
        
        public AIState CurrentState;    // Holder for our Enum State

        public GameObject DEBUGBABY;

        #endregion

        #region PrivateVariables

        private const float ContingencyDistance = 1f; // distance for which to check against to determine "if arrived" 
        
        private float _idleTimeOut = 15f;
        private float _wanderRange;

        private Vector3 _origin;
        public Vector3 _targetLocation;
        private Vector3 _distanceFromTarget;
        private Vector3 _wanderTarget;

        public GameObject _chosenMate = null;
        
        private Animator _animator;
        private CharacterController _characterController;
        private NavMeshAgent _navMeshAgent;
        
        private Rabbit_Genes _genes;
        private Rabbit_MateController _mateController;


        #endregion

        #region DebugComponents

        [Header("Debug Gizmos")]
        [SerializeField, Tooltip("If true, gizmos will be drawn in the editor.")]
        private bool _showGizmos = true;
        [SerializeField] 
        private bool _drawWanderRange = true;
        private Color _distanceColor = new Color(0f, 0f, 200f);
        [SerializeField] 
        private bool _drawTargetLine = true;
        private Color _targetLineColor = new Color(10f, 10f, 200f);
        

        #endregion
        
        /// <summary>
        /// STATE SYSTEM: the primary components for what state to trigger.
        /// Each State has its own associated systems and functions to call. 
        /// </summary>
        public enum AIState
        {
            Idle,
            Wander,
            SearchingForMate,
            Mating,
            Dead
        }
        
        
        #region UnityMethods
        private void OnDrawGizmosSelected()
        {
            if (!_showGizmos)   // if ShowGizmos option is disabled, return
                return;
            
            // show WanderRange (distance) gizmo
            if (_drawWanderRange)
            {
                Gizmos.color = _distanceColor;
                Gizmos.DrawWireSphere(_origin == Vector3.zero ? transform.position : _origin, _wanderRange);
            }

            if (_drawTargetLine)
            {
                Gizmos.color = _targetLineColor;
                Gizmos.DrawLine(transform.position, _targetLocation);
            }
            
            
            //Dont Dont Draw in Application Mode
            if (!Application.isPlaying)
                return;
        }

        private void Awake()
        {
            //component assignment
            _animator = GetComponent<Animator>();
            _characterController = GetComponent<CharacterController>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _genes = GetComponent<Rabbit_Genes>();
            _mateController = GetComponent<Rabbit_MateController>();

        }

        private void Start()
        {
            _wanderRange = _genes.SensoryDistance;  // TODO: Undo this WanderRange eventually
        }

        private void Update()
        {
            // get origin for gizmos
            _origin = transform.position;
            var position = _origin;
            _targetLocation = position;
            
            // Reproductive Urge Timer
            mateTimer += Time.deltaTime * 1;

            // sort out if need to eat, drink, mate, or flee
            if (mateTimer >= _genes.reproductiveUrge && !isPregnant && (_chosenMate == null)) //TODO: tie in hunger and thirst
            {
                SetState(AIState.SearchingForMate);
            }

            // TODO: handle dying of thirst and hunger here

            // ! Remember these functions are for MOVING the character, all other logic is handled in HandleSomeThing();
            switch (CurrentState)
            {
                case AIState.Mating:
                    // because we arent going to move while mating this will be an empty call, but might need it for animations
                    break;  
                
                case AIState.SearchingForMate:
                    if (_chosenMate != null)
                        MoveToTarget(transform.position,_chosenMate.transform.position);
                    
                    break;
                
                case AIState.Wander:
                    MoveToTarget(transform.position,_wanderTarget);

                    break;

                case AIState.Idle:
                    
                    // short timer for Idle and then back to wander
                    if (Time.time >= _idleTimeOut)
                    {
                        SetState(AIState.Wander);
                        UpdateAnimals();
                    }

                    break;
            }

            // ** set NavMeshAgent component values
            if (_navMeshAgent)
            {
                _navMeshAgent.destination = _targetLocation;
                _navMeshAgent.speed = 1f;
                _navMeshAgent.angularSpeed = 1f;
            }
            else
                Debug.LogError("No NavMeshAgent found, need Agent to move to targetLocation...");
        }

        #endregion

        #region CustomMethods

        /// <summary>
        /// Core Update Function to Manage, Update, and Trigger Animal Functions
        /// </summary>
        void UpdateAnimals()
        {
            if (CurrentState == AIState.Dead)
            {
                Debug.LogError("Trying to update the AI of a dead animal, something is borked...");
                return;
            }
        }


        /// <summary>
        /// Sets the WanderState based off state input
        /// </summary>
        /// <param name="state">State to set to</param>
        /// <exception cref="ArgumentOutOfRangeException">Exception output for bad state set</exception>
        public void SetState(AIState state)
        {
            var previousState = CurrentState;
            if (previousState == AIState.Dead)
            {
                Debug.LogError("Attempting state on a dead animal, cant do that...");
                return;
            }
            
            CurrentState = state;
            switch (CurrentState)
            {
                case AIState.Mating:
                    HandleMating();

                    break;
                
                case AIState.SearchingForMate:
                    HandleSearchingForMate();
                    break;
                
                case AIState.Idle:
                    HandleIdle();
                    break;
                
                case AIState.Wander:
                    HandleWander();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleMating()
        {
            Debug.Log("Hey we are mating!");
            
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isWalking", false);

            if (_genes.Gender == Rabbit_Genes.Genders.Female)
            {
                isPregnant = true;
                
                // spawn new rabbit
                Instantiate(DEBUGBABY, transform.position, transform.rotation);  //! remove this in favor of below
                Debug.Log("A new rabbit was spawned!"); // TODO: replace this with actual Spawn Rabbit call
            }
            
            // reset mate timer
            mateTimer = 0;

            // reset mate target
            _chosenMate = null;

            // go back to wander state
            SetState(AIState.Wander);
        }

        /// <summary>
        /// 
        /// </summary>
        void HandleSearchingForMate()
        {
            Debug.Log("How many times am I looking for a mate?");

            // if male we want to find a mate
            if (_genes.Gender == Rabbit_Genes.Genders.Male)
            {
                if (_chosenMate == null)
                    _chosenMate = Rabbit_MateController.FindMate(transform.position, _genes.SensoryDistance);

                _animator.SetBool("isIdle", false);
                _animator.SetBool("isWalking", true);
            }
            else // we are female and should probably also find one? but for now will just sit still waiting for a male
            {
                _animator.SetBool("isIdle", true);
                _animator.SetBool("isWalking", false);
            }
        }
        
        /// <summary>
        /// Triggers the Idle State
        /// </summary>
        void HandleIdle()
        {
            // Timer set for when to 'idle-out' after reaching idle state
            _idleTimeOut = Time.time + Random.Range(1f, 10f);
            
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isWalking", false);
        }
        
        /// <summary>
        /// Triggers the Wander State
        /// </summary>
        void HandleWander()
        {
            // find random position inside UnitSphere of distance _wanderRange
            var randomPoint = Random.insideUnitSphere * _wanderRange;

            // set tempTarget position to that position
            var targetPos = _origin + randomPoint;

            // see if position is valid
            ValidatePosition(ref targetPos);

            // set nav target to position
            _wanderTarget = targetPos;
            
            // set animator
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", true);
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveToTarget(Vector3 position, Vector3 targetPos)
        {
            // get targetLocation
            _targetLocation = targetPos;

            // LookAt targetLocation
            transform.LookAt(_targetLocation);

            // find distance to target
            _distanceFromTarget = Vector3.ProjectOnPlane(_targetLocation - position, Vector3.up);

            // if we are at target, figure out state
            if (_distanceFromTarget.magnitude <= ContingencyDistance)
            {
                if (CurrentState == AIState.Wander)
                    SetState(AIState.Idle);
                if (CurrentState == AIState.SearchingForMate) // we found a mate
                {
                    _chosenMate.GetComponent<Rabbit_Controller>()._chosenMate = gameObject; // tell them I am their mate
                    
                    _chosenMate.GetComponent<Rabbit_Controller>().SetState(AIState.Mating); // update them to be mating
                    SetState(AIState.Mating);
                }

                UpdateAnimals();
            }
        }

        /// <summary>
        /// Validate that random Nav position is a valid position
        /// </summary>
        /// <param name="targetPosition">Position to check</param>
        void ValidatePosition(ref Vector3 targetPosition)
        {
            if (_navMeshAgent) // breakout for if no NavMeshAgent
            {
                NavMeshHit hit; // point for good hit

                if (!NavMesh.SamplePosition(targetPosition, out hit, Mathf.Infinity,
                        1 << NavMesh.GetAreaFromName("Walkable")))
                {
                    Debug.LogError("Unable to get NavMesh hit sample. Need a Nav Mesh layer with name 'Walkable'");
                    enabled = false;
                    return;
                }

                targetPosition = hit.position;  // if we find a good location, return it
            }
        }

        #endregion
    }
}