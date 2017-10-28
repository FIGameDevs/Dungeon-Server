using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Entity
{
    bool _isAlive;
    Action<Entity> _onDeath;
    public bool IsAlive { get { return _isAlive; } protected set { _isAlive = value; if (!value) _onDeath?.Invoke(this); } }

    float _oldHealth;
    float _health;
    protected float maxHealth;
    Action<Entity, float, float> _onHeal;// sends this entity, old health and new health
    Action<Entity, float, float> _onDamage;
    public float Health
    {
        get { return _health; } //If players can't see health, why not make it a real number
        protected set
        {
            _oldHealth = _health;
            _health = value;
            if (_health < _oldHealth)
                _onDamage?.Invoke(this, _oldHealth, _health);
            else if (_health > _oldHealth)
                _onHeal?.Invoke(this, _oldHealth, _health);
        }
    }
    float _oldEnergy;
    float _energy;
    protected float maxEnergy;
    Action<Entity, float, float> _onAddEnergy;// sends this entity, old energy and new energy
    Action<Entity, float, float> _onSubstractEnergy;
    public float Energy
    {
        get { return _energy; } //If players can't see energy, why not make it a real energyenergyenergynumber
        protected set
        {
            _oldEnergy = _energy;
            _energy = value;
            if (_energy < _oldEnergy)
                _onSubstractEnergy?.Invoke(this, _oldEnergy, _energy);
            else if (_energy > _oldEnergy)
                _onAddEnergy?.Invoke(this, _oldEnergy, _energy);
        }
    }
    /// <summary>
    /// Makes an instance of full health entity.
    /// </summary>
    /// <param name="maxHealth"></param>
    public Entity(float maxHealth = 100f)
    {
        this.maxHealth = maxHealth;
        Health = maxHealth;
        IsAlive = true;
    }
    /// <summary>
    /// makes an instance of entity with set health, if health is 0, the entity is dead.
    /// </summary>
    /// <param name="maxHealth"></param>
    /// <param name="health"></param>
    public Entity(float maxHealth, float health)
    {
        this.maxHealth = maxHealth;
        Health = health;
        if (Health > 0f)
            IsAlive = true;
        else
            IsAlive = false;
    }

    public virtual void ChangeHealth(float value)
    {
        //insert custom logic in children
        if (Health + value <= 0f)
        {
            Health = 0f;
            IsAlive = false;
        }
        else if (Health + value > maxHealth)
            Health = maxHealth;
        else
            Health += value;
    }

    public virtual void ChangeEnergy(float value)
    {
        if (Energy + value <= 0f)
        {
            Energy = 0f;
        }
        else if (Energy + value > maxEnergy)
            Energy = maxEnergy;
        else
            Energy += value;
    }
    public virtual bool TryChangeEnergy(float value)
    {
        if (Energy + value <= 0f)
        {
            return false;
        }
        else if (Energy + value > maxEnergy)
            return false;

        Energy += value;
        return true;
    }


    public void CSetOnDeath(Action<Entity> a)
    {
        _onDeath += a;
    }
    public void CSetOnHeal(Action<Entity, float, float> a)
    {
        _onHeal += a;
    }
    public void CSetOnDamage(Action<Entity, float, float> a)
    {
        _onDamage += a;
    }
    public void CSetOnAddEnergy(Action<Entity, float, float> a)
    {
        _onAddEnergy += a;
    }
    public void CSetOnSubstractEnergy(Action<Entity, float, float> a)
    {
        _onSubstractEnergy += a;
    }
}