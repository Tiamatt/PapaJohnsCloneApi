using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using PapaJohnsCloneApi.Models;
using PapaJohnsCloneApi.CustomModels;
using Microsoft.EntityFrameworkCore;

namespace PapaJohnsCloneApi.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/item")]
    public class ItemController : Controller
    {      
        private readonly PapaJohnsCloneDbContext context;
        public ItemController(PapaJohnsCloneDbContext _context)
        {
            this.context = _context;
        }

        //http://localhost:52269/api/item
        //https://localhost:44385/api/item
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "PapaJohnsClone1", "PapaJohnsClone2", "PapaJohnsClone3" };
        }

        // for main nav
        [HttpGet("categories")]
        [ProducesResponseType(typeof(IEnumerable<IdName_M>), 200)]
        public async Task<IActionResult> GetItemCategoryList()
        {
            return Ok(await context.ItemCategory
                .Select(x => new IdName_M
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync());
        }

        // for main page content when click on "Papa Jonh's Clone" logo
        [HttpGet("specials")]
        [ProducesResponseType(typeof(IEnumerable<Item_M>), 200)]
        public async Task<IActionResult> GetSpecialList()
        {
            var result = await context.Item
                .Where(item => item.IsDisabled == false && item.IsSpecial == true)
                .Select(x => new Item_M()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    ItemCategoryId = x.ItemCategoryId,
                    ItemCategoryName = "Specials"
                    // Price = CalculatePrice(x.Id, x.ItemCategoryId, null, null) - gives async error
                })
                .ToListAsync();

            foreach (var res in result)
            {
                int? sizeId = await context.ItemSelectedQuestion
                    .Where(x => x.QuestionCategoryId == 2 && x.ItemId == res.Id)
                    .Select(x => x.QuestionId)
                    .FirstOrDefaultAsync();

                int? countTopping = await context.ItemSelectedToppings
                    .Where(x => x.ItemId == res.Id)
                    .CountAsync();

                res.Price = CalculatePrice(res.Id, sizeId, countTopping);
            }

            return Ok(result);
        }

        // for main page content based on selected nav link
        [HttpGet("items/{itemCategoryId}")]
        [ProducesResponseType(typeof(IEnumerable<Item_M>), 200)]
        public async Task<IActionResult> GetItemListByCategoryId(int itemCategoryId)
        {
            IList<Item_M> result = await context.Item
            .Where(item => item.IsDisabled == false && item.ItemCategoryId == itemCategoryId)
            .Join(context.ItemCategory, item => item.ItemCategoryId, cat => cat.Id, (item, cat) => new { item, cat })
            .Select(x => new Item_M()
            {
                Id = x.item.Id,
                Name = x.item.Name,
                Description = x.item.Description,
                ItemCategoryId = x.item.ItemCategoryId,
                ItemCategoryName = x.cat.Name
                // Price = CalculatePrice(x.item.Id, null, null)  - gives async error
            })
            .ToListAsync();

            foreach (var res in result)
            {
                int? sizeId = await context.ItemSelectedQuestion
                    .Where(x => x.QuestionCategoryId == 2 && x.ItemId == res.Id)
                    .Select(x => x.QuestionId)
                    .FirstOrDefaultAsync();

                int? countTopping = await context.ItemSelectedToppings
                    .Where(x => x.ItemId == res.Id)
                    .CountAsync();

                res.Price = CalculatePrice(res.Id, sizeId, countTopping);
            }

            if (result.Count() == 0)
                return NotFound();

            return Ok(result);
        }

        // for nav on customized-item page
        [HttpGet("customized/category")]
        [ProducesResponseType(typeof(IEnumerable<IdName_M>), 200)]
        public async Task<IActionResult> GetCustomizedCategoryList()
        {
            List<IdName_M> result = new List<IdName_M>();
            IdName_M question = new IdName_M
            {
                Id = 0,
                Name = "Size & Crust"
            };
            result.Add(question);

            var list = await context.ToppingCategory.ToListAsync();

            foreach (var obj in list)
            {
                IdName_M topping = new IdName_M
                {
                    Id = obj.Id,
                    Name = obj.Name
                };
                result.Add(topping);
            }

            return Ok(result);
        }

        // for title, question-box and topping-box on customized-item page
        [HttpGet("customized/item/{itemId}")]
        [ProducesResponseType(typeof(CustomizedItem_M), 200)]
        public async Task<IActionResult> GetCustomizedItem(int itemId)
        {
            CustomizedItem_M result = new CustomizedItem_M();

            Item item = await context.Item
                .Where(x => x.Id == itemId && x.IsDisabled == false)
                .FirstOrDefaultAsync();

            if (item != null)
            {
                #region get data for result.QuestionList

                List<Question_M> questionList = new List<Question_M>();
                IEnumerable<QuestionCategory> questionCategoryList = await context.QuestionCategory.OrderBy(x => x.Id).ToListAsync();
                foreach (var qc in questionCategoryList)
                {
                    Question_M ioq = new Question_M();

                    // assign QuestionCategoryId
                    ioq.QuestionCategoryId = qc.Id;

                    // assign QuestionCategoryName
                    ioq.QuestionCategoryName = qc.Name;

                    // assign AllowedQuestionList
                    List<int> allowedQuestionIdList = context.ItemAllowedQuestions
                        .Where(x => x.ItemId == itemId)
                        .Select(x => x.QuestionId)
                        .ToList();
                    ioq.AllowedQuestionList = context.Question
                        .Where(x => x.QuestionCategoryId == qc.Id && allowedQuestionIdList.Contains(x.Id))
                        .Select(x => new IdName_M
                        {
                            Id = x.Id,
                            Name = x.Name
                        })
                        .ToList();

                    // assign SelectedQuestionId
                    ioq.SelectedQuestionId = context.ItemSelectedQuestion
                        .Where(x => x.ItemId == itemId && x.QuestionCategoryId == qc.Id)
                        .Select(x => x.QuestionId)
                        .FirstOrDefault();

                    questionList.Add(ioq);
                }

                #endregion

                #region get data for result.ToppingList

                List<Topping_M> toppingList = await context.Topping
                    .Select(x => new Topping_M
                    {
                        Id = x.Id,
                        ToppingCategoryId = x.ToppingCategoryId,
                        Name = x.Name
                    })
                    .ToListAsync();

                #endregion

                result.ItemId = item.Id;
                result.ItemName = item.Name;
                result.ItemCategoryId = item.ItemCategoryId;
                result.QuestionList = questionList;
                result.ToppingList = toppingList;
            }
            else
            {
                result.ItemId = -1;
                return NotFound();
            }

            return Ok(result);
        }

        // for title, question-box and topping-box on customized-item page
        [HttpGet("customized/selected-topping/{itemId}")]
        public IEnumerable<IdName_M> GetSelectedToppingList(int itemId)
        {
            return context.ItemSelectedToppings
                .Where(x => x.ItemId == itemId)
                .Select(x => new IdName_M {
                    Id = x.ToppingId,
                    Name = x.Topping.Name
                })
                .ToList();
        }

        // for re-calculation of price
        [HttpGet("customized/recalculatedPrize/paramsToRecalculatePrice")]
        public decimal GetRecalculatedPrice(int itemId, int sizeId, int countTopping)
        {
            return (decimal) CalculatePrice(itemId, sizeId, countTopping);
        }

        // method to calculate the price of item
        public decimal? CalculatePrice(int itemId, int? sizeId, int? countTopping)
        {
            // find price based on itemId and size
            ItemPrice ip = GetItemPrice(itemId, sizeId);
            if (ip == null)
                return 0.1M;

            return ip.BasicPrice + Convert.ToDecimal(ip.PricePerTopping) * Convert.ToInt32(countTopping);
        }

        // method to get ItemPrice based on some filters
        public ItemPrice GetItemPrice(int itemId, int? sizeId)
        {         
            ItemPrice result = null;

            // get ItemCategoryId based on itemId
            int itemCategoryId = context.Item.Where(x => x.Id == itemId).Select(x => x.ItemCategoryId).FirstOrDefault();

            var ipList = context.ItemPrice.Where(x => x.ItemCategoryId == itemCategoryId).ToList();  

            // if no result by categoryId, then return null 
            if (ipList == null)
                return null;

            // else search by itemId and sizeId
            result = ipList.Where(x => x.ItemId == itemId && x.QuestionIdSize == sizeId).FirstOrDefault();
            if (result != null)
                return result;

            // else search by itemId and sizeId = NULL
            result = ipList.Where(x => x.ItemId == itemId && x.QuestionIdSize == null).FirstOrDefault();
            if (result != null)
                return result;

            // else search by sizeId and itemId = NULL
            result = ipList.Where(x => x.ItemId == null && x.QuestionIdSize == sizeId).FirstOrDefault();
            if (result != null)
                return result;

            // else search by itemId = NULL and sizeId = NULL
            result = ipList.Where(x => x.ItemId == null && x.QuestionIdSize == null).FirstOrDefault();
            return result;   // return even if null        
        }

    }
}