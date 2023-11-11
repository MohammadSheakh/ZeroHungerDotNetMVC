//using E_Commerce_Web_Application.DTOs;
//using E_Commerce_Web_Application.EF;
using E_Commerce_Web_Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeroHunger.DTOs.CollectRequstForm;
using ZeroHunger.EF;

namespace E_Commerce_Web_Application.Helper.Converter
{
    public class ManualConverter
    {

        /**
            conversion er method gula amra controller er moddhe likhbo 

            // Student -> Student DTO
         */
        public NGODTO Convert(NGO product)
        {
            // Product type er data pathacchi .. 
            // ProductDTO er intance er moddhe 
            // shob value copy kore .. DTO format e  return kortese 
            return new NGODTO()
            {
                ngoId = product.ngoId,
                name = product.name,
                email = product.email,
                password = product.password,
                address = product.address,
                phoneNo = product.phoneNo,
                image = product.phoneNo,
                
            };
        }

        //public NGODTO SingleManualConvertForCollectRequestForm(CollectRequstForm crf)
        //{
        //    // Product type er data pathacchi .. 
        //    // ProductDTO er intance er moddhe 
        //    // shob value copy kore .. DTO format e  return kortese 
        //    return new CollectRequstFormDTO()
        //    {
        //        reqFormId = crf.reqFormId,
        //        requestStatus = crf.requestStatus,
        //        createdAt = crf.createdAt,
        //        employeeId = crf.employeeId,
        //        ngoId = crf.ngoId,
        //        foodSourceId = crf.foodSourceId,
        //        FoodSource = crf.FoodSource,
        //        NGO = crf.NGO,

        //    };
        //}


        // method er nam same 
        // parameter different jehetu 
        // so, method overload
        public NGO Convert(NGODTO product)
        {
            // Product type er data pathacchi .. 
            // ProductDTO er intance er moddhe 
            // shob value copy kore .. DTO format e  return kortese 
            return new NGO()
            {
                ngoId = product.ngoId,
                name = product.name,
                email = product.email,
                password = product.password,
                address = product.address,
                phoneNo = product.phoneNo,
                image = product.phoneNo,
            };
        }

        public List<NGODTO> Convert(List<NGO> products)
        {
            // Product type er data pathacchi .. 
            // ProductDTO er intance er moddhe 
            // shob value copy kore .. DTO format e  return kortese 
            var productDto = new List<NGODTO>();
            // list initialize na korle null assign hobe .. 
            foreach (var product in products)
            {
                productDto.Add(Convert(product));
            }

            return productDto;
        }
    }
}