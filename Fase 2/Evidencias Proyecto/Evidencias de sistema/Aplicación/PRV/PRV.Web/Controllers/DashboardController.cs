
using Microsoft.AspNetCore.Mvc;
using PRV.Web.Services;

namespace PRV.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly DashboardService _dashboardService;

        public DashboardController(DashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // =========================================
        // MOSTRAR PÁGINA
        // =========================================
        public IActionResult Index()
        {
            var idUsuario = HttpContext.Session.GetString("IdUsuario");

            if (string.IsNullOrEmpty(idUsuario))
                return RedirectToAction("Index", "Inicio");

            return View();
        }

        // =========================================
        // OBTENER INFORMACIÓN DEL DASHBOARD
        // =========================================
        [HttpGet]
        public IActionResult ObtenerResumen(int anio, int? mes)
        {
            var idSesion = HttpContext.Session.GetString("IdEmpresa");

            if (!long.TryParse(idSesion, out long idEmpresa))
                return Unauthorized();

            if (anio < 2000 || anio > 2100 ||
                (mes.HasValue && (mes < 1 || mes > 12)))
            {
                return BadRequest("Año o mes inválido.");
            }

            var resumen = _dashboardService.ObtenerResumen(
                idEmpresa, anio, mes);

            return Json(resumen);
        }
    }
}
