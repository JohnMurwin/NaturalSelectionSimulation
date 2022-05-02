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
        public float deadFadeTime = 15f;

        public AIState CurrentState;    // Holder for our Enum State

        #endregion

        #region PrivateVariables
        
        private bool isPregnant = false;
        private float mateTimer = 0;
        private float birthTimer = 0;
        private float growthTimer = 0;

        private const float ContingencyDistance = 1f; // distance for which to check against to determine "if arrived" 
        
        private float _idleTimeOut = 5f;    // short initial time, random time later
        private float _wanderRange;
        
        private float _timePassed = 0f;
        private float _lifeExpectancyLimit;

        public float _foodLevel = 0f;
        private float _thirstLevel = 0f;
 
        private Vector3 _origin;
        private Vector3 _targetLocation;
        private Vector3 _distanceFromTarget;
        private Vector3 _wanderTarget;

        private GameObject _chosenMate = null;
        private GameObject _offspringFather = null;

        private GameObject _chosenFood = null;
        
        private Animator _animator;
        private CharacterController _characterController;
        private NavMeshAgent _navMeshAgent;
        
        private Rabbit_Genes _genes;
        private Rabbit_MateController _mateController;

        private Rabbit_ReproductionController _reproductionController;

        private GrassManager _grassManager;


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
            FindFood,
            SearchingForMate,
            Mating,
            Fleeing,
            Dying,
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

            _reproductionController = GameObject.Find("RabbitSpawner").GetComponent<Rabbit_ReproductionController>();
            _grassManager = GameObject.Find("GrassManager").GetComponent<GrassManager>();
        }

        private void Start()
        {
            _wanderRange = _genes.SensoryDistance;  // TODO: Undo this WanderRange eventually
            
            _lifeExpectancyLimit = _genes.LifeExpectancy;

            _foodLevel = _genes.Health * 4; //TODO: replace with dedicated food value
            _thirstLevel = _genes.Stamina * 4; // TODO: replace with dedicated water value
        }

        private void Update()
        {
            // get origin for gizmos
            _origin = transform.position;
            var position = _origin;
            _targetLocation = position;
            
            // Growth Timer
            if (_genes.IsChild)
            {
                StartCoroutine(GrowSize());
            }
            
            // Timers
            _timePassed += Time.deltaTime;
            _foodLevel -= Time.deltaTime;

            if (_timePassed >= _lifeExpectancyLimit || _foodLevel <= 0) // TODO: handle if starved or out of water also
            {
                SetState(AIState.Dying);
            }
            
            // Birth Timer
            if (isPregnant)
            {
                birthTimer += Time.deltaTime * 1;

                if (birthTimer >= _genes.GestationDuration)
                    GiveBirth();
            }

            // Reproductive Urge Timer
            if (!isPregnant && !_genes.IsChild)    // cant mate if pregnant or child
                mateTimer += Time.deltaTime * 1;

            // sort out if need to eat, drink, mate, or flee
            if (mateTimer >= _genes.ReproductiveUrge && !isPregnant && (_chosenMate == null) && !_genes.IsChild) 
            {
                SetState(AIState.SearchingForMate);
            }

            if (_foodLevel <= (_genes.Health / 4) && (CurrentState != AIState.Fleeing)) 
            {
                SetState(AIState.FindFood);
            }

            // TODO: handle dying of thirst and hunger here

            // ! Remember these functions are for MOVING the character, all other logic is handled in HandleSomeThing();
            switch (CurrentState)
            {
                case AIState.FindFood:
                    //
                    if (_chosenFood != null)
                        MoveToTarget(transform.position, _chosenFood.transform.position);
                    break;
                
                case AIState.Dying:
                    // 
                    break;  
                
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
                _navMeshAgent.speed = _genes.Speed;
                _navMeshAgent.angularSpeed = _genes.Speed;
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
                case AIState.FindFood:
                    HandleFood();
                    break;
                
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
                
                case AIState.Dying:
                    HandleDeath();
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private IEnumerator GrowSize()
        {
            if (transform.localScale.x >= _genes.Size)  // breakout for more than one instance running
                yield break;

            float currentTime = 0f;
            float growTime = _genes.GrowthTime;
            
            Vector3 originalSize = transform.localScale;
            Vector3 matureSize = new Vector3(_genes.Size, _genes.Size, _genes.Size);

            while (currentTime < growTime)
            {
                currentTime += Time.deltaTime;
                transform.localScale = Vector3.Lerp(originalSize, matureSize, currentTime / growTime);
                yield return null;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        private void GiveBirth()
        {
            // spawn new rabbit
            _reproductionController.SpawnRabbit(transform.position, _offspringFather, gameObject);
            
            // reset for next offspring
            _offspringFather = null;
            isPregnant = false;
            birthTimer = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleFood()
        {
            Debug.Log("Im Hungry....");
            
            if (_chosenFood == null)
                _chosenFood = _grassManager.FindNearestGrass(transform.position);

            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", true);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void HandleDeath()
        {
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", false);
            
            float anim = Random.Range(0f, 1f);
            
            if (anim < 0.5f)
                _animator.SetBool("isDead_0", true);
            else
                _animator.SetBool("isDead_1", true);
            
            Destroy(gameObject, deadFadeTime);   
        }

        /// <summary>
        /// 
        /// </summary>
        private void HandleMating()
        {
            _animator.SetBool("isIdle", true);
            _animator.SetBool("isWalking", false);

            if (_genes.Gender == Rabbit_Genes.Genders.Female)
                isPregnant = true;

            // reset mate timer
            mateTimer = 0;

            // reset mate target
            _offspringFather = _chosenMate;
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
                if (CurrentState == AIState.FindFood)
                {
                    SetState(AIState.Idle); // idle down to 'eat'
                    
                    _chosenFood.GetComponent<Grass>().KillPlant();  // kill plant

                    _foodLevel = _genes.Health; // reset food level
                }
                    
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