using DevelopmentKit.Reflection.Attributes;
using DevelopmentKit.Signals;
using Game.Managers;
using Game.Subsystems.CookingSubsystem.Entities;
using Game.UI.ViewComponents;
using Godot;
using GodotKit.UI.ViewControllers;
using Project00ChefGame.Scripts.Game.Subsystems.CookingSubsystem.Resources;
using System.Collections.Generic;
using System.Linq;

namespace Game.UI.Views.ViewControllers;

[ScenePath("res://UI/Views/SupplyTradeView.tscn")]
public partial class SupplyTradeViewController : ViewControllerBase
{
    private PackedScene BuyItemPackedScene { get; set; }
    private PackedScene CartItemPackedScene { get; set; }
    private Control BuyItemsParentControl { get; set; }
    private Control CartItemsParentControl { get; set; }
    private Dictionary<BuyItemComponent, CookingIngredientResource> TradeableItems { get; set; }
    private Dictionary<CartItemComponent, BuyItemComponent> CartItems { get; set; }
    private int CartTotalValue { get; set; }

    public override void _Ready()
    {
        CartTotalValue = 0;
        BuyItemPackedScene = ResourceLoader.Load<PackedScene>(ScenePathAttribute.GetPath<BuyItemComponent>());
        CartItemPackedScene = ResourceLoader.Load<PackedScene>(ScenePathAttribute.GetPath<CartItemComponent>());

        TradeableItems = new();
        CartItems = new();

        BuyItemsParentControl = GetNode<Control>("./Background/MarginContainer/HBoxContainer/BuyPanel/MarginContainer/ScrollContainer/VBoxContainer");
        CartItemsParentControl = GetNode<Control>("./Background/MarginContainer/HBoxContainer/CartPanel/MarginContainer/ScrollContainer/VBoxContainer");

        SignalHelper.PrepareButton(this, "./Background/MarginContainer/CheckoutButton", "CheckoutButtonPressed");
    }

    public void CreateView()
    {
        foreach (CookingIngredientResource ingredient in GameManager.Instance.CookingSubsystem.Ingredients)
        {
            BuyItemComponent buyItemComponent = BuyItemPackedScene.Instantiate<BuyItemComponent>();

            BuyItemsParentControl.AddChild(buyItemComponent);

            buyItemComponent.OnItemBuyed += OnItemBuyed;

            buyItemComponent.ItemNameLabel.Text = ingredient.Name;
            buyItemComponent.PriceLabel.Text = ingredient.PurchasePrice.ToString();

            TradeableItems.Add(buyItemComponent, ingredient);
        }
    }

    public void OnItemBuyed(BuyItemComponent itemBuyed)
    {
        KeyValuePair<CartItemComponent, BuyItemComponent> existingCartItem = CartItems.Where(item => item.Key.ItemNameLabel.Text == itemBuyed.ItemNameLabel.Text).FirstOrDefault();

        if (!existingCartItem.Equals(default(KeyValuePair<CartItemComponent, BuyItemComponent>)))
        {
            existingCartItem.Key.ItemCountLabel.Text = (int.Parse(existingCartItem.Key.ItemCountLabel.Text) + 1).ToString();
        }
        else
        {
            CartItemComponent cartItemComponent = CartItemPackedScene.Instantiate<CartItemComponent>();
            CartItemsParentControl.AddChild(cartItemComponent);
            CartItems.Add(cartItemComponent, itemBuyed);

            cartItemComponent.OnItemDecreased += OnItemDecreased;

            cartItemComponent.ItemNameLabel.Text = itemBuyed.ItemNameLabel.Text;
            cartItemComponent.ItemCountLabel.Text = "1";
        }

        CartTotalValue += (int) TradeableItems[itemBuyed].PurchasePrice;
    }

    public void OnItemDecreased(CartItemComponent itemDecreased)
    {
        if (CartItems.ContainsKey(itemDecreased))
        {
            int currentCount = int.Parse(itemDecreased.ItemCountLabel.Text);

            currentCount--;

            itemDecreased.ItemCountLabel.Text = currentCount.ToString();

            if (currentCount == 0)
            {
                CartItems.Remove(itemDecreased);
                itemDecreased.QueueFree();
            }
        }

        CartTotalValue -= (int) TradeableItems[CartItems[itemDecreased]].PurchasePrice;
    }

    public void CheckoutButtonPressed()
    {
        PackedScene cookingIngredientScene = ResourceLoader.Load<PackedScene>(ScenePathAttribute.GetPath<CookingIngredient>());

        if (!GameManager.Instance.PlayerCharacter.PlayerState.Debit(CartTotalValue)) return;

        foreach ((CartItemComponent cartItem, BuyItemComponent buyedItem) in CartItems)
        {
            for (int i = 0; i < int.Parse(cartItem.ItemCountLabel.Text); i++)
            {
                CookingIngredient cookingIngredient = cookingIngredientScene.Instantiate<CookingIngredient>();

                cookingIngredient.IngredientName = cartItem.ItemNameLabel.Text;

                CookingIngredientResource resource = TradeableItems[buyedItem];

                cookingIngredient.IngredientMeshPath = resource.IngredientMeshPath;
                cookingIngredient.SlicedIngredientMeshPath = resource.SlicedIngredientMeshPath;

                GetTree().Root.AddChild(cookingIngredient);
            }
        }

        CartTotalValue = 0;
    }
}
