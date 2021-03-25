using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Szczesniak {
    public class BossScript : MonoBehaviour {


        static class States {
            public class State {

                protected BossScript boss;

                virtual public State Update() {

                    return null;
                }

                virtual public void OnStart(BossScript boss) {
                    this.boss = boss;
                }

                virtual public void OnEnd() {

                }
            }

            //////////////////////////// Child Classes: 

            public class Idle : State {

                float idleTime = 5;

                public Idle(float time) {
                    idleTime = time;
                }

                public override State Update() {

                    // transitions: 
                    if (boss.CanSeeThing(boss.attackTarget))
                        return new States.Pursuing();

                    // Patroling
                    idleTime -= Time.deltaTime;
                    if (!boss.CanSeeThing(boss.attackTarget) && idleTime <= 0)
                        return new States.Pursuing();

                    return null;
                }
            }

            public class Pursuing : State {
                public override State Update() {
                    // behavior:
                    boss.MoveTowardTarget();

                    if (!boss.CanSeeThing(boss.attackTarget))
                        return new States.Idle(boss.timeToStopIdle);

                    return null;
                }
            }

            public class Patrolling : State {
                public override State Update() {


                    return null;
                }
            }

            public class Death : State {

            }

            public class Stunned : State {

            }

            public class SpawnMinions : State {

            }

            public class MiniGunAttack : State {

            }

            public class HomingMissleAttack : State {

            }

        }

        // EnemyBasicController.States.State

        private States.State state;

        private NavMeshAgent nav;

        public Transform attackTarget;

        public float viewingDistance = 10;
        public float viewingAngle = 35;
        public float stoppingDistance = 25;
        private float timeToStopIdle = 5;

        void Start() {
            nav = GetComponent<NavMeshAgent>();
        }

        void Update() {

            if (state == null) SwitchState(new States.Idle(timeToStopIdle));
            //if (state == null) state = 

            if (state != null) SwitchState(state.Update());

            print(state);
        }

        void MoveTowardTarget() {
            if (attackTarget) nav.SetDestination(attackTarget.position);
            
        }

        void SwitchState(States.State newState) {
            if (newState == null) return; // don't switch to nothing...

            if (state != null) state.OnEnd(); // tell previous state it is done
            state = newState; // swap states
            state.OnStart(this);
        }

        private bool CanSeeThing(Transform thing) {

            if (!thing) return false; // uh... error

            Vector3 vToThing = thing.position - transform.position;

            // check distance
            if (vToThing.sqrMagnitude > viewingDistance * viewingDistance) {
                StopNavMovement();
                return false; // Too far away to see...
            }

            // check direction
            if (Vector3.Angle(transform.forward, vToThing) > viewingAngle) return false; // out of vision "cone"

            if (vToThing.sqrMagnitude < viewingDistance * (viewingDistance - stoppingDistance)) {
                StopNavMovement();
            } else
                ContinueNavMovement();
            // TODO: Check occulusion

            return true;
        }

        void StopNavMovement() {
            nav.updatePosition = false;
            nav.nextPosition = gameObject.transform.position;
        }

        void ContinueNavMovement() {
            nav.updatePosition = true;
        }

        void PatrolingPoints() {

        }
    }
}