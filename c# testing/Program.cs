using System;
using System.Collections;
using System.Collections.Generic;
namespace c__testing
{
    class Program
    {

        static void Main(string[] args)
        {
            var AlignmentAction = new BoidAction
            {
                actionType = BoidActionType.Alignment,
                range = 4,
                weight = 1,
                divideByNearby = false,
                viewangle = 20,

            };
            var cohesionAction = new BoidAction
            {
                actionType = BoidActionType.Cohesion,
                range = 10,
                weight = 2,
                divideByNearby = false,
                viewangle = 20,

            };
            var seperationAction = new BoidAction
            {
                actionType = BoidActionType.Seperation,
                range = 0.2f,
                weight = 3,
                divideByNearby = false,
                viewangle = 20,

            };
            var fleeAction = new NonBoidAction
            {
                actionType = NonBoidActionType.Fleeing,
                range = 10f,
                weight = 4,
                divideByNearby = false,
                viewangle = 50,

            };

            var aBoid = new MainBoid
            {
                boidActions = new BoidAction[]
                {
                AlignmentAction,
                cohesionAction,
                seperationAction,
                },
                nonBoidActions = new NonBoidAction[]
                {
                fleeAction
                }

            };
            var bBoid = new MainBoid
            {
                boidActions = new BoidAction[]
                {
                AlignmentAction,
                cohesionAction,
                seperationAction,
                },
                nonBoidActions = new NonBoidAction[]
                {
                fleeAction
                }

            };
            var a = new int[] { 10, 1 };
            var b = new int[] { 10, 1 };
            Console.WriteLine(((IStructuralEquatable) a).GetHashCode(EqualityComparer<int>.Default) + " : " + ((IStructuralEquatable) b).GetHashCode(EqualityComparer<int>.Default));
        }
        int hashArray(object hash)
        {
            if (hash is int[])
            {
                ((IStructuralEquatable) hash).GetHashCode(EqualityComparer<int>.Default);
            }
            else if (hash is float[])
            {

            }
            else
            {

            }
        }

        public struct MainBoid : IEquatable<MainBoid>
        {
            public BoidAction[] boidActions;
            public NonBoidAction[] nonBoidActions;
            public int group;

            public bool Equals(MainBoid other)
            {
                if (GetType() != other.GetType())
                {
                    return false;
                }

                // TODO: write your implementation of Equals() here
                var a = this.GetHashCode();
                var b = other.GetHashCode();

                return (a == b);
            }

            private int GetintArrayHash(int[] toBeHashed)
            {
                return ((IStructuralEquatable) toBeHashed).GetHashCode(EqualityComparer<float>.Default);
            }

            public override int GetHashCode()
            {
                // TODO: write your implementation of GetHashCode() here

                var boidActionsHash = boidActions.GetHashCode();
                var nonBoidActionsHash = nonBoidActions.GetHashCode();

                float[] combination = { boidActionsHash, nonBoidActionsHash, group };

                var hashCode = ((IStructuralEquatable) combination).GetHashCode(EqualityComparer<float>.Default);

                return hashCode.GetHashCode();
            }
        }

        [Serializable]
        public struct BoidAction
        {
            public BoidActionType actionType;
            public float range;
            public float weight;
            public bool divideByNearby;
            public float viewangle;
        }

        [Serializable]
        public struct NonBoidAction
        {
            public NonBoidActionType actionType;
            public float range;
            public float weight;
            public bool divideByNearby;
            public float viewangle;
        }
        //Actions which involve checking other boids
        [Serializable]
        public enum BoidActionType
        {
            Alignment,
            Cohesion,
            Seperation,
        }

        //Actions which do not involve checking other boids
        [Serializable]
        public enum NonBoidActionType
        {
            Fleeing,
            Targeting
        }
    }
}