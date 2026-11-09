
using Dapper;
using Microsoft.Data.SqlClient;
using PRV.Web.Models;
using System.Data;

namespace PRV.Web.Repositories
{
    public class DashboardRepository
    {
        private readonly string _connectionString;

        public DashboardRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("PRV_BD")!;
        }

        // =========================================
        // CONSULTAR VENTAS
        // =========================================
        public List<DashboardVenta> ListarVentas(
            long idEmpresa,
            DateTime fechaDesde,
            DateTime fechaHasta)
        {
            using var conexion = new SqlConnection(_connectionString);

            return conexion.Query<DashboardVenta>(
                "dbo.sp_Venta_Consultar",
                new
                {
                    IdEmpresa = idEmpresa,
                    FechaDesde = fechaDesde,
                    FechaHasta = fechaHasta
                },
                commandType: CommandType.StoredProcedure
            ).ToList();
        }

        // =========================================
        // CONTAR PRODUCTOS ACTIVOS
        // =========================================
        public int ContarProductosActivos(long idEmpresa)
        {
            using var conexion = new SqlConnection(_connectionString);

            return conexion.Query<Producto>(
                "dbo.sp_Producto_ListarMantenedor",
                new { IdEmpresa = idEmpresa },
                commandType: CommandType.StoredProcedure
            ).Count(p => p.Estado == "Activo");
        }

        // =========================================
        // CONTAR SOLICITUDES MAYORISTAS
        // =========================================
        public int ContarSolicitudesPendientes(long idEmpresa)
        {
            using var conexion = new SqlConnection(_connectionString);

            return conexion.Query<Usuario>(
                "dbo.sp_Usuario_ListarSolicitantes",
                new { IdEmpresa = idEmpresa },
                commandType: CommandType.StoredProcedure
            ).Count();
        }
    }
}
