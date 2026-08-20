using UnityEngine;

public class PlayerIdGenerator : MonoBehaviour
{
    private int[] digits = new int[_digitsCount];
    private int playerId = 1000;

    private const int _digitsCount = 4;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey("PlayerID"))
        {
            for (int i = 0; i < digits.Length; i++)
            {
                int id = i;
                if (id == 0)
                {
                    digits[id] = Random.Range(1, 5);
                }
                else
                {
                    digits[id] = GenerateRandomDigit();
                }
            }

            playerId = digits[0] * 1000
                 + digits[1] * 100
                 + digits[2] * 10
                 + digits[3];


            KeyManager.Set_Bool_Key("PlayerID", playerId);
            Debug.Log($"Generated new playerId: {playerId}");
        }
        else
        {
            playerId = KeyManager.Get_Bool_Key("PlayerID");
            SeparateDigits();
            Debug.Log($"Loaded playerId: {playerId}");
        }
    }

    private void SeparateDigits()
    {
        digits[0] = playerId / 1000;
        digits[1] = playerId / 100 % 10;
        digits[2] = playerId / 10 % 10;
        digits[3] = playerId % 10;
    }

    private int GenerateRandomDigit() { return Random.Range(0, 10); }

    internal int GetPlayerId() { return playerId; }
}
