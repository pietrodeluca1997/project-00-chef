using DevelopmentKit.Patterns.Design.Creational;
using Godot;
using System;

namespace Project00ChefGame.Scripts.Game.PlayerStates;

[Serializable]
public sealed partial class PlayerState : AbstractSingleton<PlayerState>
{
    public delegate void InsufficientCurrencyEventSignature();
    public delegate void CurrencyChangedEventSignature(int newCurrency);

    public event InsufficientCurrencyEventSignature InsufficientCurrencyEvent;
    public event CurrencyChangedEventSignature CurrencyChangedEvent;

    [Export]
    public int Currency { get; private set; }

    public override void _Ready()
    {
        CurrencyChangedEvent?.Invoke(Currency);
    }

    public bool Debit(int decreaseValue)
    {
        if (Currency - decreaseValue < 0)
        {
            InsufficientCurrencyEvent?.Invoke();
            return false;
        }
        else
        {
            Currency -= decreaseValue;
            CurrencyChangedEvent?.Invoke(Currency);
            return true;
        }
    }

    public void Credit(int increaseValue)
    {
        Currency += increaseValue;
        CurrencyChangedEvent?.Invoke(Currency);
    }
}
