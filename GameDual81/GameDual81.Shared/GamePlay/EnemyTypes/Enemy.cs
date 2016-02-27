using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThielynGame.LevelGenerator;

namespace ThielynGame.GamePlay
{

    class Enemy : Character
    {
        protected bool hasDetectedPlayer;
        protected int detectionDistance = 200, DetectionHeight;   // how far away this enemy can detect player and how big elevevation difference can be for detection
        protected int releaseAggroDistance = 300;

        protected int primaryAttackRange, secondartAttackRange;

        protected int elevationStep = 3;        // the elevation difference this character can walk up or down
        protected bool nextStepFall;        // flag is true if standing on an edge
        protected bool nextStepWall;        // flag is true if there is a wall in front

        public static List<Platform> AIrelevantTerrain;
        public static Vector2 playerPosition;       // the player position AI uses
        protected Vector2 distanceToPlayer = new Vector2();

        public Enemy(Vector2 startPosition, int level) : base(startPosition, level)
        {
            // default values
            characterType = "dummy_medium";
            TextureFileName = "TODO";

            position = startPosition;
            this.level = level;
            alignment = ObjectAlignment.Enemy;

            // DEFAULT VALUES
            acceleration = 1f;           
        }

        public override void Update(TimeSpan time)
        {
            if (CheckPlayerDetection())
                hasDetectedPlayer = true;

            if (DistanceToPlayer() >= releaseAggroDistance)
                hasDetectedPlayer = false;

            if (hasDetectedPlayer) DoCombatAI(time);
            else DoDefaultAI(time);

            base.Update(time);
            

            // reset obsticle flags before next check
            nextStepFall = true;
            nextStepWall = false;
        }


        // function to update when enemy has detected player
        protected virtual void DoCombatAI(TimeSpan time) { }

        // function to update when enemy has not detected player
        protected virtual void DoDefaultAI(TimeSpan time) { }

        protected virtual bool isFacingPlayer()
        {

            if (facing == FacingDirection.Left && playerPosition.X < position.X)
                return true;

            if (facing == FacingDirection.Right && playerPosition.X >= position.X)
                return true;

            return false;
        }

        // function to analyze if movement to the distance is valid
        protected void CheckNextStep(int distance, FacingDirection facing)
        {
            Vector2 CheckPointUp = new Vector2(0, 0);
            Vector2 CheckPointDown = new Vector2(0, 0);

            // calculate the reference points depending on facing
            if (facing == FacingDirection.Left)
                CheckPointDown = new Vector2(BoundingBox.Left - distance, BoundingBox.Bottom + elevationStep);
                CheckPointUp = new Vector2(BoundingBox.Left - distance, BoundingBox.Bottom - elevationStep - 1);

            if (facing == FacingDirection.Right)
                CheckPointDown = new Vector2(BoundingBox.Right + distance, BoundingBox.Bottom + elevationStep);
                CheckPointUp = new Vector2(BoundingBox.Right + distance, BoundingBox.Bottom - elevationStep -1);

            
            // compare check points to a list of terrain
            foreach (Platform P in AIrelevantTerrain)
            {
                if (P.BoundingBox.Contains(CheckPointUp))
                    nextStepWall = true;

                if (P.BoundingBox.Contains(CheckPointDown))
                    nextStepFall = false;
            }
            
        }

        protected float DistanceToPlayer()
        {
            Vector2 V = new Vector2();
            V.X = playerPosition.X - position.X;
            V.Y = playerPosition.Y - position.Y;

            return V.Length();

        }

        protected virtual bool CheckPlayerDetection()
        {
            if (DistanceToPlayer() <= detectionDistance && isFacingPlayer())
                return true;

            return false;
        }

        protected void ChangeFacing()
        {
            if (facing == FacingDirection.Left)
                facing = FacingDirection.Right;

            else if (facing == FacingDirection.Right)
                facing = FacingDirection.Left;
        }
        // MOVEMENT PATTERNS

        protected virtual void ConstantPatrol()
        {
            CheckNextStep(3, facing);

            if (facing == FacingDirection.Left)
            {
                if (nextStepWall || nextStepFall)
                    DoMovementRight();
                else DoMovementLeft();
            }
            if (facing == FacingDirection.Right)
            {
                if (nextStepWall || nextStepFall)
                    DoMovementLeft();
                else DoMovementRight();
            }
        }


        // ENEMY FACTORY
        static public Enemy CreateEnemy(int TypeID, Vector2 Location, int Level)
        {
            Enemy E = null;

            switch (TypeID)
            {
                case 1: E = new PlasmaWalker(Location, Level);  break;
                case 2: E = new PlasmaFlyer(Location, Level); break;
                case 3: E = new Swarm(Location, Level); break;
                case 4: E = new SporeCannon(Location, Level); break;
                case 5: E = new FloatingMine(Location, Level); break;
                case 6: E = new LeaperNest(Location, Level); break;
                case 7: E = new Leaper(Location, Level); break;
                case 8: E = new Worm(Location, Level); break;
            }

            return E;
        }   
    }

}
