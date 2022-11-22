/* 
 * Programmers: Xander Mooney & Jack Kennedy
 * Purpose: Manage grabbing and throwing of objects 
 * Inputs: Distance of objects within range of the player
 * Outputs: whether an item should be grabbed, thrown, or not.
 */

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.U2D.Animation;

public class itemManager : MonoBehaviour
{
    #region Variables

    public GameObject selectedItem;
    controller controller;
    Attack attack;
    Vector2 ItemDistance;
    public Vector2 itemPosOffset, currentPosOffset; 
    public bool holdingItem = false;
    bool doingItemAction = false;
    int itemsInRange = 0;
    public float throwforce = 0.1f; //units to be thrown
    public float throwtime = 2;
    public List<thrownItemProperties> thrownItems = new List<thrownItemProperties>();

    public SpriteLibraryAsset notHolding, holding;
    public SpriteLibrary spritelibrary;

    public Material matUnSelected, matSelected;


    #endregion

    #region Structs

    public struct thrownItemProperties
    {
        public GameObject item; // every thrown item has a gameobject (.item)
        public Vector2 start; // every item has a start destination (.start)
        public Vector2 destination; // every item has a destination (.destination)
        public float timeStarted; // every item has a time it started at (.timeStarted)
    }

    #endregion

    #region Default Methods

    private void Awake()
    {
        controller = GetComponent<controller>();
        attack = GetComponent<Attack>();
        spritelibrary = GetComponent<SpriteLibrary>();
        ItemDistance = new Vector2(0,0);

        // Sets the intital pos offset
        itemPosOffset = new Vector2(.45f, 0.395f);
    }

    private void Update()
    {
        if ((!holdingItem && itemsInRange == 0) || selectedItem.gameObject == null) // no objects can be selected, set selected item to null
        {
            if (selectedItem != null)
                selectedItem.GetComponent<SpriteRenderer>().material = matUnSelected;

            selectedItem = null;
            holdingItem = false;
        }
        if (Input.GetButtonDown("ItemAction") && !attack.isAttacking) // if we press f
        {
            doingItemAction = true; // "queue" our input
        }
    }

    private void FixedUpdate()
    {
        if (doingItemAction) // once we get input on a fixed frame
        {
            if (!holdingItem && selectedItem != null) // we are not holding the object, and need to grab it
            {
                selectedItem.transform.SetParent(transform);
                currentPosOffset = itemPosOffset + selectedItem.GetComponent<Pickupable>().holdingOffset;
                selectedItem.transform.position = new Vector2(transform.position.x + currentPosOffset.x, transform.position.y + currentPosOffset.y);

                holdingItem = true;
                spritelibrary.spriteLibraryAsset = holding;
                selectedItem.GetComponent<SpriteRenderer>().material = matUnSelected;
            }
            else if (holdingItem)  // we are holding the object, and need to throw it
            {
                if (selectedItem.GetComponent<Pickupable>().hasCollision)
                    selectedItem.GetComponent<Pickupable>().collisionScript.enabled = true;

                selectedItem.transform.position = transform.position;
                selectedItem.transform.SetParent(null);
                thrownItemProperties thrownItem;
                thrownItem.item = selectedItem;
                thrownItem.start = (Vector2)selectedItem.transform.position;
                thrownItem.destination = (Vector2)selectedItem.transform.position + (controller.moveDirection * selectedItem.GetComponent<Pickupable>().throwVelocity);
                thrownItem.timeStarted = Time.fixedTime;
                thrownItems.Add(thrownItem);

                holdingItem = false;
                spritelibrary.spriteLibraryAsset = notHolding;
                
            }

            doingItemAction = false; // set to false now that our input has concluded
        }

        itemsInRange = 0;

        for (int i = 0; i < thrownItems.Count; i++)
        {
            // if we are either holding the item we are on or our item has fully moved
            if ((holdingItem && selectedItem == thrownItems[i].item) || (Mathf.Approximately(thrownItems[i].item.transform.position.x, thrownItems[i].destination.x) && Mathf.Approximately(thrownItems[i].item.transform.position.y, thrownItems[i].destination.y)))
            {
                // dont move it any more; remove it from our list
                thrownItems.Remove(thrownItems[i]);
            }
            else // if thats not true, we are supposed to be moving the item
            {
                // move the item
                float divisor = 2f;
                // how far along we are
                thrownItems[i].item.transform.position += (Vector3)((1f / divisor) / Mathf.Pow(divisor, throwtime * (Time.fixedTime - thrownItems[i].timeStarted)) * (thrownItems[i].destination - thrownItems[i].start));
            }
        }
    }

    #endregion

    #region Custom Methods

    // This class is called by all items that are in range of the player, and sees if it is the closest object
    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void SelectedPickup(GameObject item, Vector2 Dist)
    {
        if (!holdingItem) // only do this if we arent currently holding an item
        {
            if (Dist.x + Dist.y < ItemDistance.x + ItemDistance.y || selectedItem == null) // If object is closer to player than previous
            {
                // deselect previous object shader
                if (selectedItem != null)
                selectedItem.GetComponent<SpriteRenderer>().material = matUnSelected;
                // this item should be selected, set all variables
                selectedItem = item;
                ItemDistance = Dist;
                selectedItem.GetComponent<SpriteRenderer>().material = matSelected;
            }
            else if (selectedItem == item) ItemDistance = Dist; // makes sure the currently selected objects information is up to date
           
            ++itemsInRange;
        }
    }

    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void CollideItem(GameObject item)
    {
        for (int i = 0; i < thrownItems.Count; i++)
        {
            // if the item we found has apparently collided
            if (thrownItems[i].item == item)
            {
                // dont move it any more; remove it from our list
                thrownItems.Remove(thrownItems[i]);

                if (selectedItem != null && selectedItem.GetComponent<Pickupable>().hasCollision)
                    selectedItem.GetComponent<Pickupable>().collisionScript.enabled = false;
            }
        }
    }
    
    /*
     * purpose: TODO
     * inputs: TODO
     * outputs: TODO
     */
    public void FlipObject(bool facingRight)
    {
        if (facingRight)
        {
            selectedItem.transform.position = new Vector2(transform.position.x + currentPosOffset.x, transform.position.y + currentPosOffset.y);
        } else
        {
            selectedItem.transform.position = new Vector2(transform.position.x - currentPosOffset.x, transform.position.y + currentPosOffset.y);
        }
    }

    #endregion
}
