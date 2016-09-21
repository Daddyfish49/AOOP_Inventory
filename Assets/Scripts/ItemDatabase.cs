using UnityEngine;
using System.Collections;
using System.Collections.Generic; // allows us to use lists
using System.IO; // access the file system
using LitJson; // allows us to take JSON data and turn it into a C# object
public class Stat
{
    public int Power { get; set; }
    public int Defence { get; set; }
    public int Vitality { get; set; }
    public Stat(Stat stats)
    {
        this.Power = stats.Power;
        this.Defence = stats.Defence;
        this.Vitality = stats.Vitality;
    }

    public Stat(JsonData data)
    {
        Power = (int)data["power"];
        Defence = (int)data["defence"];
        Vitality = (int)data["vitality"];
    }
}

public class Item
{
    public int ID { get; set; }
    public string Title { get; set; }
    public int Value { get; set; }
    public Stat Stats { get; set; }
    public string Description { get; set; }
    public bool Stackable { get; set; }
    public int Rarity { get; set; }
    public Sprite Sprite { get; set; }
    public GameObject gameObject { get; set; }
    
    public Item()
    {
        //set the ID to -1 indicating that 
        //the item has not been set
        ID = -1;
    }

    public Item(JsonData data)
    {
        ID = (int)data["id"];
        Title = data["title"].ToString();
        Value = (int)data["value"];
        Stats = new Stat(data["stats"]);
        Description = data["description"].ToString();
        Stackable = (bool)data["stackable"];
        Rarity = (int)data["rarity"];
        string filename = data["sprite"].ToString();
        Sprite = Resources.Load<Sprite>("Sprites/Items/" + filename );

    }



}

public class ItemDatabase : MonoBehaviour {
    //stores all the different tpyes of items ina database
    //use this list to spawn multiple items
    private Dictionary<string,Item> database = new Dictionary<string,Item>();

    //holds the JSON data we pull in from the scene
    private JsonData itemData;

    private static ItemDatabase instance = null;

	// Use this for initialization
	void Awake() {

        if (instance == null)
        {
            instance = this;
            //obtain the file path for the Items.json
            string jsonFilePath = Application.dataPath + "/StreamingAssets/Items.json";
            //read the entire file into a string
            string jsonText = File.ReadAllText(jsonFilePath);
            // load in the data through JsonMapper
            itemData = JsonMapper.ToObject(jsonText);
            //construct item database
            ConstructDatabase();
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
	
    void ConstructDatabase()
    {
        //loop through all the items inside of itemData
        for (int i = 0; i < itemData.Count; i++)
        {
            //obtain data
            JsonData data = itemData[i];
            //create a new item
            Item newItem = new Item(data);
            //add item to database
            database.Add(newItem.Title, newItem);

        }
    }

    public static Item GetItem(string itemName)
    {
        //store database into shorter name
        Dictionary<string, Item> database = instance.database;
        //check if item exists in database
        if (database.ContainsKey(itemName))
        {
            return database[itemName];
        }
        //otherwise return null
        return null;
    }

    public static Dictionary<string,Item> GetDatabase()
    {
        //return the database from singleton
        return instance.database;
    }

}
