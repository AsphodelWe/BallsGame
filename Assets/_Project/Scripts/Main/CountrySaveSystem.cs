using System.Linq;
using UnityEngine;

public static class CountrySaveSystem
{
    [System.Serializable]
    private class CountrySaveData
    {
        public string selectedWeaponName;
        public int selectedSlotIndex;
        public string selectedSideName;
        public int maxHealth;
    }

    public static void SaveCountry(CountryConfig country)
    {
        var data = new CountrySaveData
        {
            selectedWeaponName = country.SelectedAttacker?.name,
            selectedSlotIndex = country.SelectedSlotIndex,
            selectedSideName = country.Side?.SideName,
            maxHealth = country.CountryGameplayConfig.MaxHealth
        };
        
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString($"country_{country.CountryName}", json);
        PlayerPrefs.Save();
        Debug.Log($"✅ {country.CountryName} сохранена в JSON");
    }

    public static void LoadCountry(CountryConfig country, AttackerData weapons, SideData sides)
    {
        string json = PlayerPrefs.GetString($"country_{country.CountryName}", "");
        if (string.IsNullOrEmpty(json)) return;
        
        var data = JsonUtility.FromJson<CountrySaveData>(json);
        
        country.SelectedAttacker = weapons.AttackerList.FirstOrDefault(w => w.name == data.selectedWeaponName);
        country.SelectedSlotIndex = data.selectedSlotIndex;
        country.Side = sides.SideInfo.FirstOrDefault(s => s.SideName == data.selectedSideName);
        country.CountryGameplayConfig.MaxHealth = data.maxHealth;
        
        Debug.Log($"📂 {country.CountryName} загружена из JSON");
    }
}
