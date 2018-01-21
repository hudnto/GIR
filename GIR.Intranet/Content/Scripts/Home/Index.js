$(function () {});

Informe = (function (informe) {
    
    var buscar = function (input) {
        LimparValidacao();
        var retorno = input.responseJSON == undefined ? input : input.responseJSON;
        if (retorno.sucesso) {
            buscarPaginado();
        } else {
            MostrarErroModelState(retorno);
        }
    }

    var buscarPaginado = function () {
        var url = urlRoot + "/Home/BuscarJson";
        var mensagemSemRegistro = "Nenhum registro encontrado.";
        $("#tabelaConsulta").bootstrapTable("refresh");
        var options = {
            pagination: "true",
            pageSize: 10,
            cache: false,
            pageList: "[10, 25, 50]",
            sidePagination: "server",
            url : url,
            columns: [
                    {
                        title: "Código",
                        field: "Codigo",
                        align: "center",
                        valign: "middle",
                        formatter: linkCodigo
                    },
                    {
                        title: "Ano Exercício",
                        field: "Ano",
                        align: "center",
                        valign: "middle"
                    },
                    {
                        title: "Data",
                        field: "Data",
                        align: "center",
                        valign: "middle"
                    },
                    {
                        title: "Situação",
                        field: "Situacao",
                        align: "center",
                        valign: "middle"
                    },
                    {
                        title: "Descrição",
                        field: "Descricao",
                        align: "center",
                        valign: "middle"
                    },
                    {
                        title: "Tipo",
                        field: "Tipo",
                        align: "center",
                        valign: "middle"
                    },
                    {

                        title: "Ações",
                        align: "center",
                        formatter: operateFormatter
                    }
            ],
            formatNoMatches: function () {
                return mensagemSemRegistro;
            }
        };

        $("#tabelaConsulta").bootstrapTable(options);
    }

    var linkCodigo = function (value, row, i) {
        var href = window.urlRoot + "/Processamento/Rotina/" + value;

        return '<a href="'+ href + '">'+ value + "</a>";
    }

    var operateFormatter = function (value, row) {
        var links = "";
        var visualizarLink = '<a href="' + window.urlRoot + "/ComprovanteRendimento/Index/" + row.Codigo + '" style="margin-right: 3px;"><span class="fa fa-search" aria-hidden="true"></span></a>';
        var conferirLink = '<a href="' + window.urlRoot + "/ComprovanteRendimento/Analisar/" + row.Codigo + '"><span class="fa fa-check-square-o" aria-hidden="true"></span></a>';

        if (row.Situacao !== "Erro")
            links = visualizarLink;

        if (row.Situacao === "Processado")
            links += conferirLink;

        return links;
    }

    informe.Consulta = {
        BuscaPaginada: buscar
    };

    return informe;

})(typeof Informe == "undefined" ? {} : Informe);
