using SimpleFlow;

namespace WebApiSample.Processes.Contexts
{
    public class SaleContext : FlowContext
    {
        public decimal Value => Products.Sum(p => p.Value);
        public decimal Discount => Discounts.Sum(d => d.Value);
        public decimal TotalValue => GetTotalValue();

        public bool MunicipalTaxIsent { get; set; } = false;

        public ICollection<SaleProduct> Products { get; set; } = [];
        public ICollection<SaleTax> Taxes { get; set; } = [];
        public ICollection<SaleDiscount> Discounts { get; set; } = [];

        private decimal GetTotalValue()
        {
            var productsValue = Products.Sum(p => p.Value);
            var taxesValue = Taxes.Sum(t => t.Value);
            var discountsValue = Discounts.Sum(d => d.Value);

            return productsValue + taxesValue - discountsValue;
        }
    }
}
