using Microsoft.EntityFrameworkCore;
using PapaJohnsCloneApi.CustomModelsAnalytics;
using PapaJohnsCloneApi.EnumsAnalytics;
using PapaJohnsCloneApi.Helpers;
using PapaJohnsCloneApi.ModelsAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.Repositories
{
    public class AnalyticsRepository
    {
        private readonly AnalyticsDbContext context;
        public AnalyticsRepository(AnalyticsDbContext _context)
        {
            this.context = _context;
        }

        public List<ItemActivityEnum> GetInfoForIsActive(Guid _itemId)
        {
            List<ItemActivityEnum> result = new List<ItemActivityEnum>();

            // populating data
            bool isItemImageExists = context.ItemImage.Any(x => x.ItemId == _itemId);
            bool isItemDetailExists = context.ItemDetail.Any(x => x.ItemId == _itemId);
            int itemTotalQuantity = 0;            
            if (isItemDetailExists)
                itemTotalQuantity = context.ItemDetail.Where(x => x.ItemId == _itemId).Sum(x => x.Quantity);

            // messages
            if (!isItemDetailExists)
                result.Add(ItemActivityEnum.DetailMissing);
            if (isItemDetailExists && itemTotalQuantity < 1)
                result.Add(ItemActivityEnum.OutOfStock);
            if (!isItemImageExists)
                result.Add(ItemActivityEnum.ImageMissing);

            return result;
        }

        public async Task<string> GetFullName(Guid _employeeId)
        {
            return await context.Employee
                    .Where(x => x.EmployeeId == _employeeId)
                    .Select(x => x.FirstName + " " + x.MiddleName + " " + x.LastName)
                    .FirstOrDefaultAsync();
        }
               
        public async Task<ColorSizeMatrixModel> GetColorSizeMatrix(Guid _itemId)
        {
            ColorSizeMatrixModel result = new ColorSizeMatrixModel();

            try
            {
                #region get size, color and quantity of selected item

                List<ItemDetail> itemDetailsOfItem = await context.ItemDetail
                    .Where(x => x.ItemId == _itemId)
                    .ToListAsync();

                List<SizeModel> sizesOfItem = await context.Size
                    .Where(x => itemDetailsOfItem.Any(y => y.SizeId == x.SizeId))
                    .Select(x => new SizeModel
                    {
                        sizeId = x.SizeId,
                        sizing = x.Sizing,
                        usaSizing = x.UsaSizing,
                        ukSizing = x.UkSizing,
                        europeSizing = x.EuropeSizing,
                        japanSizing = x.JapanSizing,
                        australiaSizing = x.AustraliaSizing
                    })
                    .OrderBy(x => x.sizeId)
                    .ToListAsync();

                List<ColorModel> colorsOfItem = await context.Color
                    .Where(x => itemDetailsOfItem.Any(y => y.ColorId == x.ColorId))
                    .Select(x => new ColorModel {
                            colorId = x.ColorId,
                            name = x.Name,
                            hexCode = x.HexCode
                    })
                    .OrderBy(x => x.colorId)
                    .ToListAsync();

                var quantitiesOfItem = itemDetailsOfItem
                    .GroupBy(x => new { x.SizeId, x.ColorId })
                    .Select(x => new
                    {
                        sizeId = x.Key.SizeId,
                        colorId = x.Key.ColorId,
                        quantity = x.Sum(y => y.Quantity)
                    })
                    .ToList();

                #endregion

                #region populate result

                result.sizes = sizesOfItem;
                result.colors = colorsOfItem;
                // populate result.Quantities
                foreach (SizeModel s in sizesOfItem)
                {
                    List<int?> newRow = new List<int?>();
                    foreach (ColorModel c in colorsOfItem)
                    {
                        int? qnt = null;
                        var isExist = quantitiesOfItem.Any(x => x.sizeId == s.sizeId && x.colorId == c.colorId);
                        if (isExist)
                        {
                            qnt = quantitiesOfItem
                                .Where(x => x.sizeId == s.sizeId && x.colorId == c.colorId)
                                .Select(x => x.quantity)
                                .FirstOrDefault();
                        }
                        newRow.Add(qnt);
                    }
                    result.quantities.Add(newRow);
                }

                #endregion

                return (result.quantities.Count>0) ? result: null;
            }
            catch (Exception ex)
            {
                // kali ex
                return null;
            }
        }

        public async Task<ItemModel> GetItemModel(Guid _itemId)
        {            
            Item selectedItem = await context.Item.Where(x => x.ItemId == _itemId).FirstOrDefaultAsync();
            Gender selectedGender = await context.Gender.Where(x => x.GenderId == selectedItem.GenderId).FirstOrDefaultAsync();
            Category selectedCategory = await context.Category.Where(x => x.CategoryId == selectedItem.CategoryId).FirstOrDefaultAsync();
            Brand selectedBrand = await context.Brand.Where(x => x.BrandId == selectedItem.BrandId).FirstOrDefaultAsync();

            return new ItemModel()
            {
                itemId = selectedItem.ItemId,
                name = selectedItem.Name,
                description = selectedItem.Description,
                genderId = selectedItem.GenderId,
                genderName = selectedGender.Name,
                categoryId = selectedItem.CategoryId,
                categoryName = selectedCategory.Name,
                brandId = selectedItem.BrandId,
                brandName = selectedBrand.Name,
                price = selectedItem.Price,
                isActive = selectedItem.IsActive
            };
        }

        public async Task<List<ItemImageModel>> GetItemImages(Guid _itemId)
        {
            Item selectedItem = await context.Item.Where(x => x.ItemId == _itemId).FirstOrDefaultAsync();
            List<ItemImage> itemImages = await context.ItemImage.Where(x => x.ItemId == _itemId).OrderByDescending(x => x.IsMain).ToListAsync();

            List<ItemImageModel> result = itemImages.Select(x => new ItemImageModel()
            {
                itemImageId = x.ItemImageId,
                itemId = x.ItemId,
                itemName = selectedItem.Name,
                src = x.Src,
                isMain = x.IsMain,
                size = x.Size,
                imageType = x.ImageType
            }).ToList();

            return result;
        }

        public void InsertAdminLog(AdminLog _adminLog)
        {
            /*
            #region AdminLog

            AdminLog adminLog = new AdminLog();
            adminLog.TableName = "Max200_NotNull";
            adminLog.FieldName = "Max200_NotNull";
            adminLog.FieldValue = "Max200_NotNull";
            adminLog.AdminLogActionId = (int)AdminLogActionEnum.XXX;
            adminLog.Notes = "Max2000_Null";
            adminLog.EmployeeId = guid;
            adminLog.Timestamp = DateTime.Now;
            analyticsRepo.InsertAdminLog(adminLog);

            #endregion
            */
            context.AdminLog.Add(_adminLog);
            context.SaveChanges();
        }

        public void InsertErrorLog(ErrorLog _errorLog)
        {
            #region ErrorLog example

            /*
            ErrorLog errorLog = new ErrorLog();
            errorLog.Namespace = "Max200_NotNull";
            errorLog.Class = "Max200_NotNull";
            errorLog.Method = "Max200_NotNull";
            errorLog.MethodParams = "Max2000_Null";
            errorLog.ErrorLogTypeId = (int) ErrorLogTypeEnum.xxx;
            errorLog.ShortComment = "Max200_NotNull";
            errorLog.DetailedComment = "Max2000_Null";
            errorLog.Exception = XmlManipulation.ConvertObjectToXml<Exception>(ex);
            errorLog.EmployeeId = guid;
            errorLog.Timestamp = DateTime.Now;
            analyticsRepo.InsertErrorLog(errorLog);
            */
            #endregion            

            context.ErrorLog.Add(_errorLog);
            context.SaveChanges();
        }

        public void DetachAll()
        {
            foreach (var dbEntityEntry in this.context.ChangeTracker.Entries())
            {
                if (dbEntityEntry.Entity != null)
                {
                    switch (dbEntityEntry.State)
                    {
                        case EntityState.Added:
                            dbEntityEntry.State = EntityState.Detached;
                            break;
                        case EntityState.Modified:
                            dbEntityEntry.CurrentValues.SetValues(dbEntityEntry.OriginalValues);
                            dbEntityEntry.State = EntityState.Unchanged;
                            break;
                        case EntityState.Deleted:
                            dbEntityEntry.State = EntityState.Unchanged;
                            break;
                    }
                }
            }
        }

    }
}
