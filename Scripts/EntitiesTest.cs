using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stats;

public class EntitiesTest : MonoBehaviour {
    Entity e;
	// Use this for initialization
	void Start () {
        e = new Entity();
        e.CSetOnDeath(x => { Debug.Log(x + " died."); });
        e.CSetOnDamage((x, y, z) => { Debug.Log("Ouch " + (z - y)); });
        e.CSetOnHeal((x, y, z) => { Debug.Log("Oooooh " + (z - y)); });


        var w = PerkInheritance.AddChild("Weapon");
        PerkInheritance.AddChild("Magical");
        PerkInheritance.AddChild("OneHanded", "Weapon");
        var n = PerkInheritance.AddChild("TwoHanded", "Weapon");
        PerkInheritance.AddChild("Bigass hammer", "TwoHanded");
        var ex = PerkInheritance.AddChild("Excalibruh", "TwoHanded", "Magical");


        var stats = new CharacterStats();
        stats.AddAdditive(w, 7);
        var id = stats.AddAdditive(n, 25);
        stats.SetAdditive(n, id, 27);
        var exId = stats.AddMultiplicative(ex, 0.2f);
        //stats.RemoveBoth(ex, exId);
        Debug.Log(stats.ApplyStats(ex, 100f));
    }

    // Update is called once per frame
    void Update () {
        //e.ChangeHealth(Random.Range(-5f, 5f));
	}
}
