using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

[Serializable]
public class PokedexModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public string PokemonName { get; set; }

    public Pokemon Pokemon { get; set; }

    public async Task OnGetAsync()
    {
        if (!string.IsNullOrEmpty(PokemonName))
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync($"https://pokeapi.co/api/v2/pokemon/{PokemonName.ToLower()}");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    PokemonData pokemonData = JsonConvert.DeserializeObject<PokemonData>(json);

                    Pokemon = new Pokemon
                    {
                        Name = pokemonData.Name,
                        Height = pokemonData.Height,
                        Weight = pokemonData.Weight,
                        Abilities = new string[pokemonData.Abilities.Length],
                        Types = new string[pokemonData.Types.Length]
                    };

                    for (int i = 0; i < pokemonData.Abilities.Length; i++)
                    {
                        Pokemon.Abilities[i] = pokemonData.Abilities[i].Ability.Name;
                    }

                    for (int i = 0; i < pokemonData.Types.Length; i++)
                    {
                        Pokemon.Types[i] = pokemonData.Types[i].Type.Name;
                    }
                }
            }
        }

    }
}

[Serializable]
public class Pokemon
{
    public string Name { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public string[] Abilities { get; set; }
    public string[] Types { get; set; }
}

[Serializable]
public class PokemonData
{
    public string Name { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public AbilityData[] Abilities { get; set; }
    public TypeData[] Types { get; set; }
}

[Serializable]
public class AbilityData
{
    public AbilityInfo Ability { get; set; }
}

[Serializable]
public class AbilityInfo
{
    public string Name { get; set; }
}

[Serializable]
public class TypeData
{
    public TypeInfo Type { get; set; }
}

[Serializable]
public class TypeInfo
{
    public string Name { get; set; }
}
