/* 
 * Programmers: Jack Kennedy
 * Purpose: Describes a prefab to be loaded
 * Inputs: none 
 * Outputs: loadinformation.cs
 */

namespace thingEnums
{
    // can be an item or an enemy as of now
    public enum thingType {
        item,
        enemy,
    }

    public enum thingPrefab
    {
        // enemy types
        goblin,
        skeleton,
        slime,
        redSlime,
        wizard,
        // item types
        bomb,
        bone,
        book,
        book2,
        box,
        heartJar,
        lantern,
    }
}