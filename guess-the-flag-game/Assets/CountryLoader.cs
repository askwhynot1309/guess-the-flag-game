using System.Collections.Generic;
using UnityEngine;

public class CountryLoader : MonoBehaviour
{
    public static List<Country> LoadCountries()
    {
        List<Country> countries = new List<Country>();

        // Load all Texture2D files from Resources/Flags
        Texture2D[] flagTextures = Resources.LoadAll<Texture2D>("Flags");

        foreach (Texture2D texture in flagTextures)
        {
            // Convert "United_States" → "United States"
            string countryName = texture.name.Replace("_", " ");

            countries.Add(new Country
            {
                name = countryName,
                texture = texture
            });
        }

        return countries;
    }
}