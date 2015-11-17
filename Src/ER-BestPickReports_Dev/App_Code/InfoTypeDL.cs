using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ER_BestPickReports_Dev.App_Code
{
    public class InfoTypeDL
    {
        public static string GetColumnName(InfoType t)
        {
            switch (t)
            {
                case InfoType.Category: return "CategoryID";
                case InfoType.CategoryCity: return "CategoryCityID";
                case InfoType.CategoryArea: return "CategoryAreaID";
                case InfoType.ContractorCategory: return "ContractorCategoryID";
                case InfoType.ContractorCategoryCity: return "ContractorCategoryCityID";
                case InfoType.ContractorCategoryArea: return "ContractorCategoryAreaID";
                default: return "";
            }
        }

        public static void GetNextLevelUp(ref InfoType infoType, ref int objectID)
        {
            switch (infoType)
            {
                case InfoType.Category:
                    infoType = InfoType.None;
                    break;

                case InfoType.CategoryCity:
                    objectID = InfoDL.FindID(infoType, objectID);
                    infoType = InfoType.Category;
                    break;

                case InfoType.CategoryArea:
                    objectID = InfoDL.FindID(infoType, objectID);
                    infoType = InfoType.CategoryCity;
                    break;

                case InfoType.ContractorCategory:
                    infoType = InfoType.None;
                    break;

                case InfoType.ContractorCategoryCity:
                    objectID = InfoDL.FindID(infoType, objectID);
                    infoType = InfoType.ContractorCategory;
                    break;

                case InfoType.ContractorCategoryArea:
                    objectID = InfoDL.FindID(infoType, objectID);
                    infoType = InfoType.ContractorCategoryCity;
                    break;
            }
        }
    }



    public enum InfoType
    {
        None = 0,
        Category = 1,
        City = 2,
        Area = 3,
        Contractor = 4,
        CategoryCity = 5,
        CategoryArea = 6,
        CityArea = 7,
        ContractorCategory = 8,
        ContractorCategoryCity = 9,
        ContractorCategoryArea = 10
    }
}