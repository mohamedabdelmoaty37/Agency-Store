//using Microsoft.OData.Edm;
using System;

namespace Core.Entities
{
    public class PortfolioItem : EntityBase
    {
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string Title  { get; set; }
        public DateTime  Date { get; set; }
        public string location  { get; set; }
    }
}
