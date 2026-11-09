
// =========================================
// BUSCAR INFORMACIÓN
// =========================================

document.getElementById("btnBuscar").addEventListener("click", cargarDashboard);

cargarDashboard();

function cargarDashboard() {

    const anio = document.getElementById("anio").value;
    const mes = document.getElementById("mes").value;

    let url = "/Dashboard/ObtenerResumen?anio=" + anio;

    if (mes) {
        url += "&mes=" + mes;
    }

    fetch(url)
        .then(response => {
            if (!response.ok) {
                throw new Error("Error al consultar el Dashboard.");
            }

            return response.json();
        })
        .then(data => {

            mostrarIndicadores(data);
            mostrarGrafico(data.grafico, anio, mes);
            mostrarVentas(data.ventas);

        })
        .catch(() => {
            alert("No se pudo cargar la información del Dashboard.");
        });
}


// =========================================
// INDICADORES
// =========================================

function mostrarIndicadores(data) {

    document.getElementById("totalConfirmadas").textContent =
        formatoPesos(data.totalConfirmadas);

    document.getElementById("cantidadConfirmadas").textContent =
        data.cantidadConfirmadas + " operaciones";

    document.getElementById("totalAnuladas").textContent =
        formatoPesos(data.totalAnuladas);

    document.getElementById("cantidadAnuladas").textContent =
        data.cantidadAnuladas + " operaciones";

    document.getElementById("productosActivos").textContent =
        data.productosActivos;

    document.getElementById("solicitudesPendientes").textContent =
        data.solicitudesPendientes;
}


// =========================================
// GRÁFICO DE VENTAS
// =========================================

function mostrarGrafico(datos, anio, mes) {

    const contenedor = document.getElementById("graficoVentas");
    const titulo = document.getElementById("tituloGrafico");

    contenedor.innerHTML = "";
    contenedor.classList.remove("grafico-vacio");

    const meses = [
        "Ene", "Feb", "Mar", "Abr", "May", "Jun",
        "Jul", "Ago", "Sep", "Oct", "Nov", "Dic"
    ];

    titulo.textContent = mes
        ? "Ventas diarias - " + anio
        : "Ventas mensuales - " + anio;

    const cantidadPeriodos = mes
        ? new Date(Number(anio), Number(mes), 0).getDate()
        : 12;

    const totales = [];

    for (let i = 1; i <= cantidadPeriodos; i++) {

        const registro = datos.find(x => x.periodo === i);

        totales.push(registro ? registro.total : 0);
    }

    // NO EXISTEN VENTAS EN EL PERÍODO
    if (totales.every(valor => valor === 0)) {

        contenedor.classList.add("grafico-vacio");
        contenedor.textContent =
            "No se registran ventas en el período seleccionado.";

        return;
    }

    const maximo = Math.max(...totales, 1);

    for (let i = 0; i < cantidadPeriodos; i++) {

        const valor = totales[i];

        const item = document.createElement("div");
        item.className = "grafico-item";

        const barra = document.createElement("div");
        barra.className = "grafico-barra";

        barra.style.height = valor > 0
            ? Math.max(4, valor / maximo * 170) + "px"
            : "0px";

        barra.title = formatoPesos(valor);

        const etiqueta = document.createElement("span");
        etiqueta.className = "grafico-etiqueta";

        etiqueta.textContent = mes
            ? i + 1
            : meses[i];

        item.appendChild(barra);
        item.appendChild(etiqueta);

        contenedor.appendChild(item);
    }
}


// =========================================
// DETALLE DE VENTAS
// =========================================

function mostrarVentas(ventas) {

    const tabla = document.getElementById("tablaVentas");

    tabla.innerHTML = "";

    if (ventas.length === 0) {

        const fila = tabla.insertRow();
        const celda = fila.insertCell();

        celda.colSpan = 5;
        celda.className = "text-center text-muted";
        celda.textContent = "No existen ventas para el período seleccionado.";

        return;
    }

    ventas.forEach(venta => {

        const fila = tabla.insertRow();

        fila.insertCell().textContent = venta.idVenta;

        const fecha = venta.fechaVenta.substring(0, 10).split("-");

        fila.insertCell().textContent =
            fecha[2] + "/" + fecha[1] + "/" + fecha[0];

        fila.insertCell().textContent = venta.cliente;

        const estado = fila.insertCell();
        estado.textContent = venta.estado;

        estado.className = venta.estado === "Confirmada"
            ? "text-success"
            : venta.estado === "Anulada"
                ? "text-warning"
                : "text-secondary";

        const total = fila.insertCell();
        total.textContent = formatoPesos(venta.total);
        total.className = "text-end";
    });
}


// =========================================
// FORMATO MONEDA CHILENA
// =========================================

function formatoPesos(valor) {

    return "$" + Number(valor).toLocaleString("es-CL");
}
