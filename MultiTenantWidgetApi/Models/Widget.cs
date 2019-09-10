using System;

namespace MultiTenantWidgetApi.Models
{
    public partial class Widget
    {
        public Guid Id { get; set; }
        public string Color { get; set; }
        public string Shape { get; set; }
    }
}
