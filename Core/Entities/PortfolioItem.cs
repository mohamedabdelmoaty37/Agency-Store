//using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class PortfolioItem : EntityBase
    {
        public string Namecatogry { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Title  { get; set; }
        public string gendertype { get; set; }

        public DateTime  Date { get; set; }
        public string Company { get; set; }
        public string location  { get; set; }
        public string Price { get; set; }


        [ForeignKey("Typecat")]
        public Guid typeId { get; set; }
        public Typecat Typecat { get; set; }




    }




}
