using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Bayada.Common.DataAccess;
using Bayada.Utility.Common;
using LambdaShoppingListsWebAPI.Models;
using LambdaShoppingListsWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LambdaShoppingListsWebAPI.Controllers
{
    [Route("v1/shoppingList")]
    [ApiController]
    public class ShoppingListController : ControllerBase
    {
        private readonly IShoppingListService _shoppingListService;

        public ShoppingListController(IShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpGet]
        public IActionResult GetShoppingList()
        {
            var result = _shoppingListService.GetItemsFromShoppingList();

            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddItemToShoppingList([FromBody] ShoppingListModel shoppingList)
        {
            _shoppingListService.AddItemsToShoppingList(shoppingList);

            AddUpdateShoppingList(shoppingList.Name, shoppingList.Quantity);

            return Ok();            
        }
        [HttpDelete]
        public IActionResult DeleteItemsFromShoppingList([FromBody] ShoppingListModel shoppingList)
        {
            _shoppingListService.RemoveItem(shoppingList.Name);

            return Ok();
        }

        public static void AddUpdateShoppingList(string name, int quantity)
        {
            var connectionString = "Data Source=GP_TEST_SQL;Initial Catalog=SQADEVA;integrated security=false;persist security info=false;User Id=patrickdevers;Password=bay2010;";

            string sql = "exec awsAddUpdateDeleteShoppingList @name, @quantity";
            SqlHelper.ExecuteScalar<int>(connectionString
                                        , sql
                                        , new SqlParameter("@name", name)
                                        , new SqlParameter("@quantity", quantity)                                       
                                        );
        }

    }
}