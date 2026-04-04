using UnityEngine;

public class KeyManager : MonoBehaviour
{
    internal static int GetInt_WeaponID()
    {
        int i = PlayerPrefs.GetInt("weaponID", 0);
        return i;
    }

    internal static void SetInt_WeaponID(int i)
    {
        PlayerPrefs.SetInt("weaponID", i);
    }

    internal static int GetInt_AccessoryID()
    {
        int i = PlayerPrefs.GetInt("accessoryID", 0);
        return i;
    }

    internal static void SetInt_AccessoryID(int i)
    {
        PlayerPrefs.SetInt("accessoryID", i);
    }

    internal static int GetInt_InstrumentPower_Axe()
    {
        int i = PlayerPrefs.GetInt("axePower", 0);
        return i;
    }

    internal static int GetInt_InstrumentPower_Pickaxe()
    {
        int i = PlayerPrefs.GetInt("pickaxePower", 0);
        return i;
    }

    internal static void SetInt_InstrumentPower_Axe(int i)
    {
        PlayerPrefs.SetInt("axePower", i);
    }

    internal static void SetInt_InstrumentPower_Pickaxe(int i)
    {
        PlayerPrefs.SetInt("pickaxePower", i);
    }

    internal static int Get_Item_Count(int id)
    {
        int count = PlayerPrefs.GetInt(id.ToString(), 0);
        if (count <= 0) { count = 0; }
        return count;
    }

    internal static void Spend_Item(int id, int count)
    {
        int i = PlayerPrefs.GetInt(id.ToString(), 0);
        i -= count;
        if (i <= 0) { i = 0; }
        PlayerPrefs.SetInt(id.ToString(), i);
    }

    internal static void Receive_Item(int id, int count)
    {
        int i = PlayerPrefs.GetInt(id.ToString(), 0);
        i += count;
        if (i <= 0) { i = 0; }
        PlayerPrefs.SetInt(id.ToString(), i);
    }

    internal static void Receive_Item_Once(int id)
    {
        if (PlayerPrefs.HasKey(id.ToString())) { return; }
        else { PlayerPrefs.SetInt(id.ToString(), 1); }
    }

    internal static void Set_Bool_Key(string name, int number)
    {
        PlayerPrefs.SetInt(name.ToString(), number);
    }

    internal static int Get_Bool_Key(string name)
    {
        int i = PlayerPrefs.GetInt(name.ToString(), 0);
        return i;
    }
}