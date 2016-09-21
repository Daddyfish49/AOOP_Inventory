using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class Slot
{
    public GameObject gameObject;
    public Item item;
    public Slot(GameObject gameObject, Item item)
    {
        this.gameObject = gameObject;
        this.item = item;
    }
}

public class inventoryScript : MonoBehaviour
{
    [Header("UI")]
    //amount of slots to spawn
    public int slotAmount;
    [Header("Prefabs")]
    //prefab of an individual slot
    public GameObject slotPrefab;
    public GameObject itemPrefab;
    //parent of all slots
    public GameObject slotPanel;
    [Header("Items/ Slots")]
    public List<Item> items = new List<Item>();
    public List<Slot> slots = new List<Slot>();


    private ItemDatabase itemDatabase;

    // Use this for initialization
    void Start()
    {
        itemDatabase = GetComponent<ItemDatabase>();
        //Loop through and add our slots to our item inventory
        for (int i = 0; i < slotAmount; i++)
        {
            //clone the slot
            GameObject clone = Instantiate(slotPrefab);
            //set slots parent to be slot panel
            clone.transform.SetParent(slotPanel.transform);
            //create a new slot
            Slot slot = new Slot(clone, null);

            // get slot data
            SlotData slotData = clone.GetComponent<SlotData>();
            slotData.inventory = this;
            slotData.slot = slot;
            //Add that new slot to the list
            slots.Add(slot);
        }



        /*
                //loop through and create all the slots for the inventory
                for (int i = 0; i < slotAmount; i++)
                {
                    //instantiate to a new slot
                    GameObject slot = Instantiate(slotPrefab);
                    //set it's position to be relative to slot panel
                    slot.transform.position = slotPanel.transform.position;
                    //set slots parent to be slot panel
                    slot.transform.SetParent(slotPanel.transform);
              }
              */
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddItem("Steel Gloves", 1);
        }else if (Input.GetKeyDown(KeyCode.A))
        {
            AddItem("Top Hat", 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            AddItem("Polo Shirt", 1);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            AddItem("Health Potion", 1);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            AddItem("Steel Sword", 1);
        }
    }

    public void AddItem(string itemname, int itemAmount = 1)
    {
        Item newItem = ItemDatabase.GetItem(itemname); // find the item name
        Slot newSlot = GetEmptySlot(); // find an empty slot
        if (newItem != null && newSlot != null)
        {
            if(HasStacked(newItem, itemAmount))
            {
                return;
            }
            //Set the empty slot
            newSlot.item = newItem;
            //Creat a new item instance
            GameObject item = Instantiate(itemPrefab);
            item.transform.position = newSlot.gameObject.transform.position;
            item.transform.SetParent(newSlot.gameObject.transform);
            item.name = newItem.Title;
            //Set the items gameObject
            newItem.gameObject = item;
            //Get the image componnt from the item
            Image image = item.GetComponent<Image>();
            image.sprite = newItem.Sprite;
            //set the itemdata
            ItemData itemData = item.GetComponent<ItemData>();
            itemData.item = newItem;
            itemData.slot = newSlot;
        }
    }

    public Slot GetEmptySlot()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        print("No empty slot has been found!");
        return null;
    }

    bool HasStacked(Item itemToAdd, int itemAmount = 1)
    {
        //check if item is stackable
        if (itemToAdd.Stackable)
        {
            //obtain the occupied slot with the same item
            Slot occupiedSlot = GetSlotWithItem(itemToAdd);
            if(occupiedSlot != null)
            {
                //Get reference to item in occupied slot
                Item item = occupiedSlot.item;
                //Obtain the script attached to that item
                ItemData itemData = item.gameObject.GetComponent<ItemData>();
                //increase the item amount
                itemData.amount += itemAmount;
                //set its text element
                Text textElement = item.gameObject.GetComponentInChildren<Text>();
                textElement.text = itemData.amount.ToString();
                return true;
            }
        }
        return false;
    }

    Slot GetSlotWithItem(Item item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            Item currentItem = slots[i].item;
            //check if slot is not empty and the same
            if(currentItem !=null && currentItem.Title == item.Title)
            {
                return slots[i];
            }
        }
        return null;
    }
}
