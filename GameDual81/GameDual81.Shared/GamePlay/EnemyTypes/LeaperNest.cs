using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThielynGame.GamePlay
{
    class LeaperNest : Enemy
    {
        float spawnInterval = 4000, spawnTimer;
        Vector2 spawnLocation = new Vector2(50,0);
        int maxSpawnCount = 5;

        List<Leaper> currentSpawns = new List<Leaper>();
        // List<Leaper> temp = new List<Leaper>();
        

        public LeaperNest(Vector2 StartPosition, int Level) : base(StartPosition, Level)
        {
            characterType = "dummy_large";
            TextureFileName = "TODO";
            actualSize = new Rectangle(0,0,80,80);

            detectionDistance = 1000;
            releaseAggroDistance = 1050;

            maxSpeedX = 0;      // stationary enemy
            acceleration = 0;

            MaxHealth = 50;         // TODO

            setParameters();

        }

        public override void Update(TimeSpan time)
        {
            spawnTimer += time.Milliseconds;

            base.Update(time);

            // copy all spawns that are alive into temp 
            // and overwrite od list with filtered list
            List<Leaper> temp = new List<Leaper>();

            foreach (Leaper L in currentSpawns)
            {
                if (!L.IsDead)
                temp.Add(L);
            }
            currentSpawns.Clear();
            currentSpawns.AddRange(temp); 
        }

        protected override bool CheckPlayerDetection()
        {
            if (DistanceToPlayer() <= detectionDistance)
                return true;

            return false;
        }

        protected override void DoCombatAI(TimeSpan time)
        {
            if (spawnTimer > spawnInterval && maxSpawnCount > currentSpawns.Count)
            {
                Vector2 _spawnLocation = spawnLocation + position;
                Leaper E = new Leaper(_spawnLocation, level);

                LevelManager LM = new LevelManager();
                LM.AddGameObject(E);
                currentSpawns.Add(E);

                spawnTimer = 0;

            }

        }
    }
}
