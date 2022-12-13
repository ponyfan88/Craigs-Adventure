/* 
 * Programmers: Anmol Acharya
 * Purpose: create pre fabricated attack patterns and to allow for more complex attack patterns
 * Inputs: projectile spawn datas
 * Outputs: "ProjectileSpawner"
 */
using JetBrains.Annotations;
using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "ScriptableObjects/ProjectileAttackPatternData", order = 1)]
public class ProjectileAttackPatternData : ScriptableObject
{
    public ProjectileSpawnData[] pattern; // patterns of bullets

}
