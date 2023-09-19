using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;

namespace Productivity;

public class ItemManager
{
    // singleton nonsense
    private static readonly ItemManager instance = new ItemManager();
    public static ItemManager Instance { get { return instance; } }
    private ItemManager()
    {
        items = new List<Item>(0);

        if (File.Exists("items.json"))
        {
            string readJson = File.ReadAllText("items.json");
            List<Item>? readItems = JsonConvert.DeserializeObject<List<Item>>(readJson);

            if (readItems != null)
            {
                items = readItems;
            }

        }
    }

    private List<Item> items;

    public void AddItem(Item item)
    {
        items.Add(item);
        saveToFile();
    }

    public System.Collections.ObjectModel.ReadOnlyCollection<Item> GetItems()
    {
        return items.AsReadOnly();
    }

    private void saveToFile()
    {
        string json = JsonConvert.SerializeObject(items, Formatting.None);
        File.WriteAllText("items.json", json);
    }
}