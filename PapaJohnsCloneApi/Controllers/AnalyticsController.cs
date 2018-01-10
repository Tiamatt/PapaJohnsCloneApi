using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using PapaJohnsCloneApi.ModelsAnalytics;
using Microsoft.EntityFrameworkCore;
using PapaJohnsCloneApi.CustomModelsAnalytics;
using PapaJohnsCloneApi.Repositories;
using System.Net;
using PapaJohnsCloneApi.EnumsAnalytics;
using PapaJohnsCloneApi.Helpers;

namespace PapaJohnsCloneApi.Controllers
{
    [EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/analytics")]
    public class AnalyticsController : Controller
    {
        private readonly AnalyticsDbContext context;
        private readonly AnalyticsRepository analyticsRepo;
        public AnalyticsController(AnalyticsDbContext _context)
        {
            this.context = _context;
            this.analyticsRepo = new AnalyticsRepository(_context);
        }

        #region Get data (HttpGet & HttpPost)

        //http://localhost:52269/api/analytics
        //https://localhost:44385/api/analytics
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "analytics1", "analytics2" };
        }

        //https://localhost:44385/api/analytics/genders
        [HttpGet("genders")]
        [ProducesResponseType(typeof(IEnumerable<TextValueCheckedModel>), 200)]
        public async Task<IActionResult> GetGenders()
        {
            return Ok(await context.Gender
                .Select(x => new TextValueCheckedModel
                {
                    valueNum = x.GenderId,
                    text = x.Name,
                    isChecked = false
                })
                .OrderBy(x => x.text)
                .ToListAsync());
        }

        //https://localhost:44385/api/analytics/categories
        [HttpGet("categories")]
        [ProducesResponseType(typeof(IEnumerable<TextValueCheckedModel>), 200)]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await context.Category
                .Select(x => new TextValueCheckedModel
                {
                    valueNum = x.CategoryId,
                    text = x.Name,
                    isChecked = false
                })
                .OrderBy(x => x.text)
                .ToListAsync());
        }

        //https://localhost:44385/api/analytics/brands
        [HttpGet("brands")]
        [ProducesResponseType(typeof(IEnumerable<TextValueCheckedModel>), 200)]
        public async Task<IActionResult> GetBrands()
        {
            return Ok(await context.Brand
                .Select(x => new TextValueCheckedModel
                {
                    valueNum = x.BrandId,
                    text = x.Name,
                    isChecked = false
                })
                .OrderBy(x => x.text)
                .ToListAsync());
        }

        //https://localhost:44385/api/analytics/sizes
        [HttpGet("sizes")]
        [ProducesResponseType(typeof(IEnumerable<TextValueCheckedModel>), 200)]
        public async Task<IActionResult> GetSizes()
        {
            return Ok(await context.Size
                .Select(x => new TextValueCheckedModel
                {
                    valueNum = x.SizeId,
                    text = x.Sizing,
                    isChecked = false
                })
                .OrderBy(x => x.text)
                .ToListAsync());
        }

        //https://localhost:44385/api/analytics/colors
        [HttpGet("colors")]
        [ProducesResponseType(typeof(IEnumerable<TextValueCheckedModel>), 200)]
        public async Task<IActionResult> GetColors()
        {
            return Ok(await context.Color
                .Select(x => new TextValueCheckedModel
                {
                    valueNum = x.ColorId,
                    text = x.Name,
                    extraText = x.HexCode,
                    isChecked = false
                })
                .OrderBy(x => x.text)
                .ToListAsync());
        }

        //https://localhost:44385/api/analytics/item-actions
        [HttpGet("item-actions")]
        [ProducesResponseType(typeof(IEnumerable<TextValueCheckedModel>), 200)]
        public async Task<IActionResult> GetItemActions()
        {
            return Ok(await context.ItemAction
                .Select(x => new TextValueCheckedModel
                {
                    valueNum = x.ItemActionId,
                    text = x.Name,
                    isChecked = false
                })
                .OrderBy(x => x.text)
                .ToListAsync());
        }

        //https://localhost:44385/api/analytics/price-ranges
        [HttpGet("price-ranges")]
        [ProducesResponseType(typeof(IEnumerable<TextValueCheckedModel>), 200)]
        public async Task<IActionResult> GetPriceRanges()
        {
            return Ok(await context.PriceRange
                .Select(x => new TextValueCheckedModel
                {
                    valueNum = x.PriceRangeId,
                    text = "$" + x.Min + " - $" + x.Max,
                    isChecked = false
                })
                .OrderBy(x => x.valueNum) // from smallest to largest range
                .ToListAsync());
        }

        //https://localhost:44385/api/analytics/customers
        [HttpGet("customers")]
        [ProducesResponseType(typeof(IEnumerable<TextValueCheckedModel>), 200)]
        public async Task<IActionResult> GetCustomers()
        {
            return Ok(await context.Customer
                .Select(x => new TextValueCheckedModel
                {
                    valueStr = x.CustomerId.ToString(),
                    text = x.Email,
                    isChecked = false
                })
                .OrderBy(x => x.text)
                .ToListAsync());
        }

        //https://localhost:44385/api/analytics/item-names-lowercase/XXX
        [HttpGet("item-names-lowercase/{itemId}")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public async Task<IActionResult> GetItemNamesLowercase(Guid? itemId)
        {
            List<string> result = await context.Item
                .Where(x => x.ItemId != itemId)
                .Select(x => x.Name.ToLower())
                .ToListAsync();
            return Ok(result);
        }

        // Note: TVC means "Text Value Checked"
        //https://localhost:44385/api/analytics/items-tvc/null
        [HttpGet("items-tvc/{isActive}")]
        [ProducesResponseType(typeof(IEnumerable<TextValueCheckedModel>), 200)]
        public async Task<IActionResult> GetItemsTvc(bool? isActive)
        {
            return Ok(await context.Item
                .Where(x => (isActive == null) || x.IsActive == isActive)
                .Select(x => new TextValueCheckedModel() {
                    valueStr = x.ItemId.ToString(),
                    text = x.Name
                })
                .ToListAsync());
        }

        //https://localhost:44385/api/analytics/item-model/XXX
        [HttpGet("item-model/{itemId}")]
        [ProducesResponseType(typeof(ItemModel), 200)]
        public async Task<IActionResult> GetItemModel(Guid itemId)
        {
            try
            {
                ItemModel result = await analyticsRepo.GetItemModel(itemId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        //https://localhost:44385/api/analytics/item-image-model/XXX
        [HttpGet("item-image-model/{itemId}")]
        [ProducesResponseType(typeof(ItemImageModel), 200)]
        public async Task<IActionResult> GetItemImageModel(Guid itemId)
        {
            try
            {
                List<ItemImageModel> result = await context.ItemImage
                    .Where(x => x.ItemId == itemId)
                    .Select(x => new ItemImageModel
                    {
                        itemImageId = x.ItemImageId,
                        itemId = x.ItemId,
                        itemName = "",
                        src = x.Src,
                        isMain = x.IsMain,
                        size = x.Size,
                        imageType = x.ImageType
                    })
                .ToListAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException.Message);
            }
        }

        // getting data, but as param is big, it was send in HttpBody
        //https://localhost:44385/api/analytics/item-views + HttpBody param
        [HttpPost("item-views")]
        [ProducesResponseType(typeof(IEnumerable<ItemViewModel>), 200)] // not all fields are populated!
        public async Task<IActionResult> GetItemViews([FromBody] ItemListFilterPanelModel filters)
        {
            // not all data are populated!
            IEnumerable<ItemViewModel> result = new List<ItemViewModel>();

            var items = await context.Item.ToListAsync();

            if (filters != null)
            {
                // filter by %Name% (case insensitive)
                if (filters.isShowPartialName_txt && !String.IsNullOrWhiteSpace(filters.partialName))
                    items = items.Where(x => x.Name.ToLower().Contains(filters.partialName.ToLower())).ToList();

                // filter by IsActive (0 is "All", 1 is "Active", 2 is "Not Active")
                if (filters.isShowActive_ddl && filters.active > 0)
                {
                    switch (filters.active)
                    {
                        case 1:
                            items = items.Where(x => x.IsActive == true).ToList();
                            break;
                        case 2:
                            // false and NULL
                            items = items.Where(x => x.IsActive != true).ToList();
                            break;
                    }
                }

                // filter by GenderId (0 is "All", 1 is "Women", 2 is "Men")
                if (filters.isShowGender_ddl && filters.gender > 0)
                    items = items.Where(x => x.GenderId == filters.gender).ToList();

                // filter by CategoryId (multiple selection)
                if (filters.isShowCategory_chb && filters.category.Length > 0)
                    items = items.Where(x => filters.category.Contains(x.CategoryId)).ToList();

                // filter by BrandId (multiple selection)
                if (filters.isShowBrand_chb && filters.brand.Length > 0)
                    items = items.Where(x => filters.brand.Contains(x.BrandId)).ToList();

            }

            result = items
            .Join(
                context.Gender,
                it => it.GenderId,
                gen => gen.GenderId,
                (it, gen) => new { Gen = gen, It = it })
            .Join(
                context.Category,
                itgen => itgen.It.CategoryId,
                cat => cat.CategoryId,
                (itgen, cat) => new { itgen.It, itgen.Gen, Cat = cat })
            .Join(
                context.Brand,
                itgencat => itgencat.It.BrandId,
                br => br.BrandId,
                (itgencat, br) => new { itgencat.It, itgencat.Gen, itgencat.Cat, Br = br })
            .Select(x => new ItemViewModel
            {
                item = new ItemModel
                {
                    itemId = x.It.ItemId,
                    name = x.It.Name,
                    genderId = x.It.GenderId,
                    genderName = x.Gen.Name,
                    categoryId = x.It.CategoryId,
                    categoryName = x.Cat.Name,
                    brandId = x.It.BrandId,
                    brandName = x.Br.Name,
                    price = x.It.Price,
                    isActive = x.It.IsActive,
                }
            })
            .ToList();

            return Ok(result);
        }

        //https://localhost:44385/api/analytics/item-view + HttpBody param
        [HttpPost("item-view/{itemId}")]
        [ProducesResponseType(typeof(ItemViewModel), 200)]
        public async Task<IActionResult> GetItemView(Guid itemId)
        {
            ItemViewModel result = new ItemViewModel();
            result.item = await analyticsRepo.GetItemModel(itemId);
            result.itemImages = await analyticsRepo.GetItemImages(itemId);
            //  populate itemActivities // kali

            return Ok(result);
        }

        //https://localhost:44385/api/analytics/color-size-matrix/XXX
        [HttpPost("color-size-matrix/{itemId}")]
        [ProducesResponseType(typeof(ColorSizeMatrixModel), 200)]
        public async Task<IActionResult> GetColorSizeMatrix(Guid itemId) {
            ColorSizeMatrixModel result = await analyticsRepo.GetColorSizeMatrix(itemId);
            return Ok(result);
        }        

        //https://localhost:44385/api/analytics/item-details/null
        [HttpGet("item-details/{itemId}")]
        [ProducesResponseType(typeof(List<ItemDetailModel>), 200)]
        public async Task<IActionResult> GetItemDetails(Guid? itemId)
        {
            List<ItemDetailModel> result = new List<ItemDetailModel>();
            var itemDetails = await context.ItemDetail
                .Where(x => itemId == null || x.ItemId == itemId).ToListAsync();
            if (itemDetails != null)
            {
                var data = itemDetails
                    // left outer join Customer
                    .GroupJoin(context.Customer, idet => idet.CustomerId, cus => cus.CustomerId, (idet, cus) => new { idet, cus })
                    .SelectMany(idetCus => idetCus.cus.DefaultIfEmpty(), (idetCus, cus) => new { idetCus.idet, cus })
                    // inner join Item
                    .Join(context.Item, idetCus => idetCus.idet.ItemId, it => it.ItemId, (idetCus, it) => new { idetCus, it })
                    // inner join Size
                    .Join(context.Size, idetCusIt => idetCusIt.idetCus.idet.SizeId, sz => sz.SizeId, (idetCusIt, sz) => new { idetCusIt, sz })
                    // inner join Color
                    .Join(context.Color, idetCusItSz => idetCusItSz.idetCusIt.idetCus.idet.ColorId, clr => clr.ColorId, (idetCusItSz, clr) => new { idetCusItSz, clr })
                    // inner join ItemAction
                    .Join(context.ItemAction, idetidetCusItSzCusItClr => idetidetCusItSzCusItClr.idetCusItSz.idetCusIt.idetCus.idet.ItemActionId, iact => iact.ItemActionId, (idetidetCusItSzCusItClr, iact) => new { idetidetCusItSzCusItClr, iact })
                    .ToList();

                result = data.Select(x => new ItemDetailModel
                {
                    itemDetailId = x.idetidetCusItSzCusItClr.idetCusItSz.idetCusIt.idetCus.idet.ItemDetailId,
                    itemId = x.idetidetCusItSzCusItClr.idetCusItSz.idetCusIt.idetCus.idet.ItemId,
                    itemName = x.idetidetCusItSzCusItClr.idetCusItSz.idetCusIt.it.Name,
                    sizeId = x.idetidetCusItSzCusItClr.idetCusItSz.idetCusIt.idetCus.idet.SizeId,
                    sizeName = x.idetidetCusItSzCusItClr.idetCusItSz.sz.Sizing,
                    colorId = x.idetidetCusItSzCusItClr.idetCusItSz.idetCusIt.idetCus.idet.ColorId,
                    colorName = x.idetidetCusItSzCusItClr.clr.Name,
                    quantity = x.idetidetCusItSzCusItClr.idetCusItSz.idetCusIt.idetCus.idet.Quantity,
                    itemActionId = x.idetidetCusItSzCusItClr.idetCusItSz.idetCusIt.idetCus.idet.ItemActionId,
                    itemActionName = x.iact.Name,
                    customerId = x.idetidetCusItSzCusItClr.idetCusItSz.idetCusIt.idetCus.idet.CustomerId,
                    customerEmail = (x.idetidetCusItSzCusItClr.idetCusItSz.idetCusIt.idetCus.cus == null)? null : x.idetidetCusItSzCusItClr.idetCusItSz.idetCusIt.idetCus.cus.Email
                })
                .OrderBy(x => x.quantity)
                .ToList();
            }
            return Ok(result);


        }

        //https://localhost:44385/api/analytics/item-detail/XXX
        [HttpPost("item-detail/{itemDetailId}")]
        [ProducesResponseType(typeof(ItemDetailModel), 200)] // not all fields
        public async Task<IActionResult> GetItemDetail(Guid itemDetailId)
        {
            ItemDetailModel result = await context.ItemDetail
                .Where(x => x.ItemDetailId == itemDetailId)
                .Select(x => new ItemDetailModel
                {
                    itemDetailId = x.ItemDetailId,
                    itemId = x.ItemId,
                    itemName = null,
                    sizeId = x.SizeId,
                    sizeName = null,
                    colorId = x.ColorId,
                    colorName = null,
                    quantity = x.Quantity,
                    itemActionId = x.ItemActionId,
                    itemActionName = null,
                    customerId = x.CustomerId,
                    customerEmail = null
                })
                .FirstOrDefaultAsync();

            return Ok(result);
        }

        //https://localhost:44385/api/analytics/item-activity/XXX
        [HttpPost("item-activity/{itemId}")]
        [ProducesResponseType(typeof(ItemActivateModel), 200)]
        public async Task<IActionResult> GetItemActivity(Guid itemId) {
            ItemActivateModel result = new ItemActivateModel();

            Item item = await context.Item
                .Where(x => x.ItemId == itemId)
                .FirstOrDefaultAsync();

            result.itemId = itemId.ToString();
            result.itemName = item.Name;
            result.isItemActive = item.IsActive;
            result.itemActivities = analyticsRepo.GetInfoForIsActive(itemId);

            return Ok(result);
        }

        #endregion Get data (HttpGet & HttpPost)

        #region Insert data (HttpPost)

        //https://localhost:44385/api/analytics/insert-item/XXX
        [HttpPost("insert-item/{employeeId}")]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> InsertItem(Guid employeeId, [FromBody] ItemModel item)
        {
            #region serverside validation

            if(String.IsNullOrWhiteSpace(item.name))
                return BadRequest("Name can't be empty");
            if (item.name.Length > 50)
                return BadRequest("Name shouldn't exceed 50 characters");
            if (String.IsNullOrWhiteSpace(item.description))
                return BadRequest("Description can't be empty");
            if (item.description.Length > 2000)
                return BadRequest("Description shouldn't exceed 2000 characters");

            #endregion
            Item newItem = new Item();

            try
            {
                //newItem.ItemId generated by default
                newItem.Name = item.name; // not null
                newItem.Description = item.description; // not null
                newItem.BrandId = item.brandId; // not null
                newItem.CategoryId = item.categoryId; // not null  //kali
                newItem.GenderId = item.genderId; // not null
                newItem.Price = item.price; // not null      
                newItem.IsActive = false;

                context.Item.Add(newItem);
                await context.SaveChangesAsync();

                #region AdminLog

                AdminLog adminLog = new AdminLog();
                adminLog.TableName = "Item";
                adminLog.FieldName = "ItemId";
                adminLog.FieldValue = newItem.ItemId.ToString();
                adminLog.AdminLogActionId = (int)AdminLogActionEnum.Create;
                adminLog.Notes = null;
                adminLog.EmployeeId = employeeId;
                adminLog.Timestamp = DateTime.Now;
                analyticsRepo.InsertAdminLog(adminLog);

                #endregion

                return Ok(newItem.ItemId);
            }
            catch (Exception ex)
            {
                #region AdminLog

                ErrorLog errorLog = new ErrorLog();
                errorLog.Namespace = "PapaJohnsCloneApi.Controllers";
                errorLog.Class = "AnalyticsController";
                errorLog.Method = "InsertItem";
                errorLog.MethodParams = "(Guid employeeId, [FromBody] ItemModel item)";
                errorLog.ErrorLogTypeId = (int)ErrorLogTypeEnum.Error;
                errorLog.ShortComment = "Failed to insert into Item";
                errorLog.DetailedComment = ex.InnerException.Message;
                errorLog.Exception = XmlManipulation.ConvertObjectToXml<Exception>(ex.InnerException);
                errorLog.EmployeeId = employeeId;
                errorLog.Timestamp = DateTime.Now;
                // clean Exception before saving AdminLog in db
                context.Entry(newItem).State = EntityState.Detached;
                analyticsRepo.InsertErrorLog(errorLog);

                #endregion
                return BadRequest();
            }
        }

        //https://localhost:44385/api/analytics/insert-item-detail/XXX
        [HttpPost("insert-item-detail/{employeeId}")]
        [ProducesResponseType(typeof(Guid), 200)]
        public async Task<IActionResult> InsertItemDetail(Guid employeeId, [FromBody] ItemDetailModel itemDetail)
        {
            #region serverside validation

            if (itemDetail.quantity > 1000000000 || itemDetail.quantity < -1000000000)
                return BadRequest("Quantity must fall within range between minus billion and plus billion");

            #endregion

            ItemDetail newItemDetail = new ItemDetail();
            try
            {                
                // newItemDetail.ItemDetailId will be generated by default
                newItemDetail.ItemId = itemDetail.itemId; // not null
                newItemDetail.SizeId = itemDetail.sizeId; // not null
                newItemDetail.ColorId = itemDetail.colorId; // not null
                // if Sold (2) or Lost (6) then quantity is negative
                newItemDetail.Quantity = (itemDetail.itemActionId == 2 || itemDetail.itemActionId == 6)? (-1)*itemDetail.quantity: itemDetail.quantity; // not null
                newItemDetail.ItemActionId = itemDetail.itemActionId; // not null
                // if Sold (2) or Returned (3) then add CustomerId
                newItemDetail.CustomerId = (itemDetail.itemActionId == 2 || itemDetail.itemActionId == 3) ? itemDetail.customerId : null;
                context.ItemDetail.Add(newItemDetail);
                await context.SaveChangesAsync();

                #region AdminLog

                AdminLog adminLog = new AdminLog();
                adminLog.TableName = "ItemDetail";
                adminLog.FieldName = "ItemDetailId";
                adminLog.FieldValue = newItemDetail.ItemDetailId.ToString();
                adminLog.AdminLogActionId = (int)AdminLogActionEnum.Create;
                adminLog.Notes = null;
                adminLog.EmployeeId = employeeId;
                adminLog.Timestamp = DateTime.Now;
                analyticsRepo.InsertAdminLog(adminLog);

                #endregion

                return Ok(newItemDetail.ItemDetailId);
            }
            catch (Exception ex)
            {
                #region ErrorLog

                ErrorLog errorLog = new ErrorLog();
                errorLog.Namespace = "PapaJohnsCloneApi.Controllers";
                errorLog.Class = "AnalyticsController";
                errorLog.Method = "InsertItemDetail";
                errorLog.MethodParams = "(Guid employeeId, [FromBody] ItemDetailModel itemDetail)";
                errorLog.ErrorLogTypeId = (int)ErrorLogTypeEnum.Error;
                errorLog.ShortComment = "Failed to insert into ItemDetail";
                errorLog.DetailedComment = ex.InnerException.Message;
                errorLog.Exception = XmlManipulation.ConvertObjectToXml<Exception>(ex.InnerException);
                errorLog.EmployeeId = employeeId;
                errorLog.Timestamp = DateTime.Now;
                // clean Exception before saving AdminLog in db
                context.Entry(newItemDetail).State = EntityState.Detached;
                analyticsRepo.InsertErrorLog(errorLog);

                #endregion
                return BadRequest();
            }
        }

        //https://localhost:44385/api/analytics/insert-item-image
        [HttpPost("insert-item-image/{employeeId}")]
        [ProducesResponseType(typeof(string), 200)]
        public async Task<IActionResult> InsertItemImage(Guid employeeId, [FromBody] ItemImageModel[] itemImage)
        {
            int count = 0;
            ItemImage newItemImage = new ItemImage(); // need it for catch part
            try
            {
                Guid? itemId = itemImage.Select(x => x.itemId).First();
                // Step 1. Delete all old images
                List<ItemImage> itemImages = await context.ItemImage
                    .Where(x => x.ItemId == itemId)
                    .ToListAsync();
                context.ItemImage.RemoveRange(itemImages);
                context.SaveChanges();
                // Step 2. Add/readd images
                foreach (ItemImageModel img in itemImage)
                {
                    #region serverside validation

                    // skip images if size exceedes  100 MB
                    if (img.size > 104857600) // 100 MB = 104 857 600 KB
                        continue;
                    // skip if type is not image
                    if (!img.imageType.StartsWith("image"))
                        continue;
                    
                    #endregion serverside validation

                    newItemImage = new ItemImage();// emptify for each new image
                    // newItemImage.ItemImageId will be generated by default
                    newItemImage.ItemId = (Guid)itemId;
                    newItemImage.Src = img.src;
                    newItemImage.IsMain = (bool)img.isMain;
                    newItemImage.Size = img.size;
                    newItemImage.ImageType = img.imageType;

                    context.ItemImage.Add(newItemImage);
                    await context.SaveChangesAsync();
                    count++;

                    #region AdminLog

                    AdminLog adminLog = new AdminLog();
                    adminLog.TableName = "ItemImage";
                    adminLog.FieldName = "ItemImageId";
                    adminLog.FieldValue = newItemImage.ItemImageId.ToString();
                    adminLog.AdminLogActionId = (int)AdminLogActionEnum.Create;
                    adminLog.Notes = null;
                    adminLog.EmployeeId = employeeId;
                    adminLog.Timestamp = DateTime.Now;
                    analyticsRepo.InsertAdminLog(adminLog);

                    #endregion
                }

                return Ok("Number of successfully save images is " + count);
            }
            catch (Exception ex)
            {
                #region ErrorLog

                ErrorLog errorLog = new ErrorLog();
                errorLog.Namespace = "PapaJohnsCloneApi.Controllers";
                errorLog.Class = "AnalyticsController";
                errorLog.Method = "InsertItemImage";
                errorLog.MethodParams = "(Guid employeeId, [FromBody] ItemImageModel[] itemImage)";
                errorLog.ErrorLogTypeId = (int)ErrorLogTypeEnum.Error;
                errorLog.ShortComment = "Failed to insert into ItemImage";
                errorLog.DetailedComment = ex.InnerException.Message;
                errorLog.Exception = XmlManipulation.ConvertObjectToXml<Exception>(ex.InnerException);
                errorLog.EmployeeId = employeeId;
                errorLog.Timestamp = DateTime.Now;
                // clean Exception before saving AdminLog in db
                context.Entry(newItemImage).State = EntityState.Detached;
                analyticsRepo.InsertErrorLog(errorLog);

                #endregion
                return BadRequest();
            }
        }

        #endregion Insert data (HttpPost)

        #region Update data (HttpPut)

        //https://localhost:44385/api/analytics/update-item/XXX
        [HttpPost("update-item/{employeeId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateItem(Guid employeeId, [FromBody] ItemModel item)
        {
            #region serverside validation
            if (String.IsNullOrWhiteSpace(item.name))
                return BadRequest("Name can't be empty");
            if (item.name.Length > 50)
                return BadRequest("Name shouldn't exceed 50 characters");
            if (String.IsNullOrWhiteSpace(item.description))
                return BadRequest("Description can't be empty");
            if (item.description.Length > 2000)
                return BadRequest("Description shouldn't exceed 2000 characters");

            #endregion

            Item updateItem = new Item(); // need for catch part
            try
            {
                updateItem = context.Item.Find(item.itemId);
                updateItem.Name = item.name; // not null
                updateItem.Description = item.description; // not null
                updateItem.BrandId = item.brandId; // not null
                updateItem.CategoryId = item.categoryId; // not null
                updateItem.GenderId = item.genderId; // not null
                updateItem.Price = item.price; // not null
                await context.SaveChangesAsync();

                #region AdminLog

                AdminLog adminLog = new AdminLog();
                adminLog.TableName = "Item";
                adminLog.FieldName = "ItemId";
                adminLog.FieldValue = item.itemId.ToString();
                adminLog.AdminLogActionId = (int)AdminLogActionEnum.Update;
                adminLog.Notes = null;
                adminLog.EmployeeId = employeeId;
                adminLog.Timestamp = DateTime.Now;
                analyticsRepo.InsertAdminLog(adminLog);                

                #endregion

                return NoContent();
            }
            catch (Exception ex)
            {
                #region ErrorLog

                ErrorLog errorLog = new ErrorLog();
                errorLog.Namespace = "PapaJohnsCloneApi.Controllers";
                errorLog.Class = "AnalyticsController";
                errorLog.Method = "UpdateItem";
                errorLog.MethodParams = "(Guid employeeId, [FromBody] ItemModel item)";
                errorLog.ErrorLogTypeId = (int)ErrorLogTypeEnum.Error;
                errorLog.ShortComment = "Failed to update Item";
                errorLog.DetailedComment = ex.InnerException.Message;
                errorLog.Exception = XmlManipulation.ConvertObjectToXml<Exception>(ex.InnerException);
                errorLog.EmployeeId = employeeId;
                errorLog.Timestamp = DateTime.Now;
                // clean Exception before saving AdminLog in db
                context.Entry(updateItem).State = EntityState.Detached;
                analyticsRepo.InsertErrorLog(errorLog);

                #endregion
                return BadRequest();
            }
        }

        //https://localhost:44385/api/analytics/update-item-detail + ...
        [HttpPost("update-item-detail/{employeeId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateItemDetail(Guid employeeId, [FromBody] ItemDetailModel itemDetail)
        {
            ItemDetail updateItemDetail = new ItemDetail(); // need for catch part
            try
            {
                updateItemDetail = context.ItemDetail.Find(itemDetail.itemDetailId);
                updateItemDetail.ItemId = itemDetail.itemId;
                updateItemDetail.SizeId = itemDetail.sizeId;
                updateItemDetail.ColorId = itemDetail.colorId;
                // if Sold (2) or Lost (6) then quantity is negative
                updateItemDetail.Quantity = (itemDetail.itemActionId == 2 || itemDetail.itemActionId == 6) ? (-1) * Math.Abs(itemDetail.quantity) : Math.Abs(itemDetail.quantity); // not null
                updateItemDetail.ItemActionId = itemDetail.itemActionId;
                // if Sold (2) or Returned (3) then add CustomerId
                updateItemDetail.CustomerId = (itemDetail.itemActionId == 2 || itemDetail.itemActionId == 3) ? itemDetail.customerId : null;

                await context.SaveChangesAsync();

                #region AdminLog

                AdminLog adminLog = new AdminLog();
                adminLog.TableName = "ItemDetail";
                adminLog.FieldName = "ItemDetailId";
                adminLog.FieldValue = itemDetail.itemDetailId.ToString();
                adminLog.AdminLogActionId = (int)AdminLogActionEnum.Update;
                adminLog.Notes = null;
                adminLog.EmployeeId = employeeId;
                adminLog.Timestamp = DateTime.Now;
                analyticsRepo.InsertAdminLog(adminLog);

                #endregion

                return NoContent();
            }
            catch (Exception ex)
            {
                #region ErrorLog

                ErrorLog errorLog = new ErrorLog();
                errorLog.Namespace = "PapaJohnsCloneApi.Controllers";
                errorLog.Class = "AnalyticsController";
                errorLog.Method = "UpdateItemDetail";
                errorLog.MethodParams = "(Guid employeeId, [FromBody] ItemDetailModel itemDetail)";
                errorLog.ErrorLogTypeId = (int)ErrorLogTypeEnum.Error;
                errorLog.ShortComment = "Failed to update ItemDetail";
                errorLog.DetailedComment = ex.InnerException.Message;
                errorLog.Exception = XmlManipulation.ConvertObjectToXml<Exception>(ex.InnerException);
                errorLog.EmployeeId = employeeId;
                errorLog.Timestamp = DateTime.Now;
                // clean Exception before saving AdminLog in db
                context.Entry(updateItemDetail).State = EntityState.Detached;
                analyticsRepo.InsertErrorLog(errorLog);

                #endregion
                return BadRequest();
            }
        }

        //https://localhost:44385/api/analytics/update-item-activity/XXX
        [HttpPost("update-item-activity/{employeeId}/{itemId}/{isActive}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> UpdateItemActivity(Guid employeeId, Guid itemId, bool isActive)
        {
            Item updateItem = new Item(); // need for catch part
            try
            {
                updateItem = context.Item.Find(itemId);
                updateItem.IsActive = isActive;
                await context.SaveChangesAsync();

                #region AdminLog

                AdminLog adminLog = new AdminLog();
                adminLog.TableName = "Item";
                adminLog.FieldName = "ItemId";
                adminLog.FieldValue = itemId.ToString();
                adminLog.AdminLogActionId = (int)AdminLogActionEnum.Update;
                adminLog.Notes = (isActive) ? "Item was activated" : "Item was deactivated";
                adminLog.EmployeeId = employeeId;
                adminLog.Timestamp = DateTime.Now;
                analyticsRepo.InsertAdminLog(adminLog);

                #endregion

                return NoContent();
            }
            catch (Exception ex)
            {
                #region ErrorLog

                ErrorLog errorLog = new ErrorLog();
                errorLog.Namespace = "PapaJohnsCloneApi.Controllers";
                errorLog.Class = "AnalyticsController";
                errorLog.Method = "UpdateItemActivity";
                errorLog.MethodParams = "(Guid employeeId, Guid itemId, bool isActive)";
                errorLog.ErrorLogTypeId = (int)ErrorLogTypeEnum.Error;
                errorLog.ShortComment = "Failed to update item's isActive field";
                errorLog.DetailedComment = ex.InnerException.Message;
                errorLog.Exception = XmlManipulation.ConvertObjectToXml<Exception>(ex.InnerException);
                errorLog.EmployeeId = employeeId;
                errorLog.Timestamp = DateTime.Now;
                // clean Exception before saving AdminLog in db
                context.Entry(updateItem).State = EntityState.Detached;
                analyticsRepo.InsertErrorLog(errorLog);

                #endregion
                return BadRequest();
            }
        }

        #endregion Update data (HttpPut)

        #region Delete data (HttpDelete)

        //https://localhost:44385/api/analytics/delete-item/XXX
        [HttpDelete("delete-item/{employeeId}/{itemId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteItem(Guid employeeId, Guid itemId)
        {
            Item item = new Item(); // need for catch part
            try
            {
                List<ItemDetail> itemDetails = await context.ItemDetail
                    .Where(x => x.ItemId == itemId)
                    .ToListAsync();

                List<ItemImage> itemImages = await context.ItemImage
                    .Where(x => x.ItemId == itemId)
                    .ToListAsync();

                item = await context.Item
                    .Where(x => x.ItemId == itemId)
                    .FirstOrDefaultAsync();

                context.ItemDetail.RemoveRange(itemDetails);
                context.ItemImage.RemoveRange(itemImages);
                context.Item.Remove(item);
                context.SaveChanges();

                #region AdminLog

                AdminLog adminLog1 = new AdminLog();
                adminLog1.TableName = "ItemDetail";
                adminLog1.FieldName = "ItemId";
                adminLog1.FieldValue = itemId.ToString();
                adminLog1.AdminLogActionId = (int)AdminLogActionEnum.Delete;
                adminLog1.Notes = "Delete from ItemDetail, ItemImage and Item simultaneously";
                adminLog1.EmployeeId = employeeId;
                adminLog1.Timestamp = DateTime.Now;
                analyticsRepo.InsertAdminLog(adminLog1);

                AdminLog adminLog2 = new AdminLog();
                adminLog2.TableName = "ItemImage";
                adminLog2.FieldName = adminLog1.FieldName;
                adminLog2.FieldValue = adminLog1.FieldValue;
                adminLog2.AdminLogActionId = adminLog1.AdminLogActionId;
                adminLog2.Notes = adminLog1.Notes;
                adminLog2.EmployeeId = adminLog1.EmployeeId;
                adminLog2.Timestamp = adminLog1.Timestamp;
                analyticsRepo.InsertAdminLog(adminLog2);

                AdminLog adminLog3 = new AdminLog();
                adminLog3.TableName = "Item";
                adminLog3.FieldName = adminLog1.FieldName;
                adminLog3.FieldValue = adminLog1.FieldValue;
                adminLog3.AdminLogActionId = adminLog1.AdminLogActionId;
                adminLog3.Notes = adminLog1.Notes;
                adminLog3.EmployeeId = adminLog1.EmployeeId;
                adminLog3.Timestamp = adminLog1.Timestamp;
                analyticsRepo.InsertAdminLog(adminLog3);

                #endregion

                return NoContent();
            }
            catch (Exception ex)
            {
                #region ErrorLog

                ErrorLog errorLog = new ErrorLog();
                errorLog.Namespace = "PapaJohnsCloneApi.Controllers";
                errorLog.Class = "AnalyticsController";
                errorLog.Method = "DeleteItem";
                errorLog.MethodParams = "(Guid employeeId, Guid itemId)";
                errorLog.ErrorLogTypeId = (int)ErrorLogTypeEnum.Error;
                errorLog.ShortComment = "Failed to delete item from Item, ItemDetail and ItemImage";
                errorLog.DetailedComment = "ItemId: " + itemId.ToString() + ". Message:"+ ex.InnerException.Message;
                errorLog.Exception = XmlManipulation.ConvertObjectToXml<Exception>(ex.InnerException);
                errorLog.EmployeeId = employeeId;
                errorLog.Timestamp = DateTime.Now;
                // clean Exception before saving AdminLog in db
                context.Entry(item).State = EntityState.Detached;
                analyticsRepo.InsertErrorLog(errorLog);


                #endregion ErrorLog
                return BadRequest();
            }
        }

        //https://localhost:44385/api/analytics/delete-item-detail/XXX
        [HttpDelete("delete-item-detail/{employeeId}/{itemDetailId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> DeleteItemDetail(Guid employeeId, Guid itemDetailId)
        {
            ItemDetail itemDetail = new ItemDetail(); // need for catch part
            try
            {              
                itemDetail = await context.ItemDetail
                    .Where(x => x.ItemDetailId == itemDetailId)
                    .FirstOrDefaultAsync();

                context.ItemDetail.Remove(itemDetail);
                context.SaveChanges();

                #region AdminLog

                AdminLog adminLog = new AdminLog();
                adminLog.TableName = "ItemDetail";
                adminLog.FieldName = "ItemDetailId";
                adminLog.FieldValue = itemDetailId.ToString();
                adminLog.AdminLogActionId = (int)AdminLogActionEnum.Delete;
                adminLog.Notes = "Delete single row from ItemDetail by ItemDetailId";
                adminLog.EmployeeId = employeeId;
                adminLog.Timestamp = DateTime.Now;
                analyticsRepo.InsertAdminLog(adminLog);

                #endregion

                return NoContent();
            }
            catch (Exception ex)
            {
                #region ErrorLog

                ErrorLog errorLog = new ErrorLog();
                errorLog.Namespace = "PapaJohnsCloneApi.Controllers";
                errorLog.Class = "AnalyticsController";
                errorLog.Method = "DeleteItemDetail";
                errorLog.MethodParams = "(Guid employeeId, Guid itemDetailId)";
                errorLog.ErrorLogTypeId = (int)ErrorLogTypeEnum.Error;
                errorLog.ShortComment = "Failed to delete row from ItemDetail";
                errorLog.DetailedComment = "ItemDetailId: " + itemDetailId.ToString() + ". Message:" + ex.InnerException.Message;
                errorLog.Exception = XmlManipulation.ConvertObjectToXml<Exception>(ex.InnerException);
                errorLog.EmployeeId = employeeId;
                errorLog.Timestamp = DateTime.Now;
                // clean Exception before saving AdminLog in db
                context.Entry(itemDetail).State = EntityState.Detached;
                analyticsRepo.InsertErrorLog(errorLog);

                #endregion ErrorLog
                return BadRequest();
            }
        }

        #endregion Delete data (HttpDelete)

    }
}