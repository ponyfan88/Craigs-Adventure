/* 
 * Programmers: Anmol Acharya
 * Purpose: Decide how the bullets will move
 * Inputs: none 
 * Outputs: projectileSpawner.cs projectile.cs ProjectileSpawnData.cs
 */

namespace BulletEffects
{
   public enum Bulletpaths {linear,sineWave,SpeedChange, }
   public enum endlifeEffect{none,shatter,}
   // potental effects when the bullet reaches the end of it's life
   public enum collisonEffect { Destroy, bounce, returnToThrower, }
   // i just added some that sound usful feel free to add more
}