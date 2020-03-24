using LambdaShoppingListsWebAPI.Models;
using System.Collections.Generic;

namespace LambdaShoppingListsWebAPI.Services
{
    public interface IShoppingListService
    {
        Dictionary<string, int> GetItemsFromShoppingList();
        void AddItemsToShoppingList(ShoppingListModel shoppingList);
        void RemoveItem(string shoppingListName);
    }
}