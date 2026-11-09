
namespace PRV.Web.Models
{
    public class DashboardVenta
    {
        public long IdVenta { get; set; }
        public DateTime FechaVenta { get; set; }
        public string Cliente { get; set; } = "";
        public string Estado { get; set; } = "";
        public decimal Total { get; set; }
        public DateTime? FechaAnulacion { get; set; }
    }
}
