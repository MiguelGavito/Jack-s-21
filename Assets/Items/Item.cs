using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public abstract class Item : ScriptableObject
{
    public string itemName; // nombre del objeto
    public string itemDescription; // descripcion del objeto
    public int price; // precio del objeto
    public bool isUsed = false; // indica si el objeto ya fue usado

    // Metodo para usar el objeto, este varia segun el tipo de objeto
    public abstract void UseItem(GameManager gameManager);

    // este metodo puede ser util para los objetos consumibles o activables
    public virtual void ResetItem()
    {
        isUsed = false;
    }
}



