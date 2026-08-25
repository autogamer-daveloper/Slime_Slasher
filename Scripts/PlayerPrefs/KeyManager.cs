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

    internal static int GetInt_VisualAccessoryID()
    {
        int i = PlayerPrefs.GetInt("visual_accessoryID", 0);
        return i;
    }

    internal static void SetInt_VisualAccessoryID(int i)
    {
        PlayerPrefs.SetInt("visual_accessoryID", i);
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
        int currentCount = PlayerPrefs.GetInt(id.ToString(), 0);
        
        if (currentCount <= 0)
        { 
            PlayerPrefs.SetInt(id.ToString(), 1);
            Debug.Log($"Item {id} added once (was {currentCount})");
        }
        else
        {
            Debug.Log($"Item {id} already have {currentCount}, skipping");
        }
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

    internal static void Delete_All()
    {
        // Я эту пирамиду хеопса потом снесу наху, щя мне лень
        int playerId = Get_Bool_Key("PlayerID");
        int isInvited = Get_Bool_Key("IsInvitedBefore");
        int letter = Get_Bool_Key("LetterTriggered");
        int note = Get_Bool_Key("NoteTriggered");
        int lang = Get_Bool_Key("Language");
        int astraslimes = Get_Bool_Key("Astraslimes");
        int s_autoUse = Get_Bool_Key("IsNeedAutoUse");
        int s_enableWarn = Get_Bool_Key("IsShowWarning");
        int s_volume = Get_Bool_Key("AudioVolume");
        int visual0 = Get_Bool_Key("visual_bought_0");
        int visual1 = Get_Bool_Key("visual_bought_1");
        int visual2 = Get_Bool_Key("visual_bought_2");
        int visual3 = Get_Bool_Key("visual_bought_3");
        int visual4 = Get_Bool_Key("visual_bought_4");
        int visual5 = Get_Bool_Key("visual_bought_5");
        int visual6 = Get_Bool_Key("visual_bought_6");
        int rate = Get_Bool_Key("Rated");
        int usingVisual = GetInt_VisualAccessoryID();
        PlayerPrefs.DeleteAll();
        Set_Bool_Key("PlayerID", playerId);
        Set_Bool_Key("IsInvitedBefore", isInvited);
        Set_Bool_Key("LetterTriggered", letter);
        Set_Bool_Key("NoteTriggered", note);
        Set_Bool_Key("Language", lang);
        Set_Bool_Key("Astraslimes", astraslimes);
        Set_Bool_Key("visual_bought_0", visual0);
        Set_Bool_Key("visual_bought_1", visual1);
        Set_Bool_Key("visual_bought_2", visual2);
        Set_Bool_Key("visual_bought_3", visual3);
        Set_Bool_Key("visual_bought_4", visual4);
        Set_Bool_Key("visual_bought_5", visual5);
        Set_Bool_Key("visual_bought_6", visual6);
        Set_Bool_Key("IsNeedAutoUse", s_autoUse);
        Set_Bool_Key("IsShowWarning", s_enableWarn);
        Set_Bool_Key("AudioVolume", s_volume);
        Set_Bool_Key("Rated", rate);
        SetInt_VisualAccessoryID(usingVisual);
    }

    internal static void EndedGame()
    {
        int i = Get_Bool_Key("Astraslimes");
        i += 1000;
        Set_Bool_Key("Astraslimes", i);
    }
}