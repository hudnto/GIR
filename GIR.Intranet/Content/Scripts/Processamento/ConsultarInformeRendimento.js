$(function () { });

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
        var url = urlRoot + "/Processamento/BuscarInformeRendimentoJSON";
        var mensagemSemRegistro = "Nenhum registro encontrado!";
        $("#tabelaConsulta").bootstrapTable("refresh");
        var options = {
            pagination: "true",
            pageSize: 10,
            cache: false,
            pageList: "[10, 25, 50]",
            sidePagination: "server",
            url: url,
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

        return '<a href="' + href + '">' + value + "</a>";
    }

    var operateFormatter = function (value, row) {
        var links = "";
        var visualizarLink = '<a href="{link}" class="margin-right-3 {desabilitar}"><span data-toggle="tooltip" title="Pesquisar os registros desta rotina." rel="tooltip" class="fa fa-search" aria-hidden="true"></span></a>';
        var conferirLink = '<a href="{link}" class="{desabilitar}"><span data-toggle="tooltip" title="Conferir os registros para disponibilização web." rel="tooltip" class="fa fa-check-square-o" aria-hidden="true"></span></a>';

        if (row.Situacao !== "Erro") {
            visualizarLink = visualizarLink.replace("{link}", window.urlRoot + "/ComprovanteRendimento/Index/" + row.Codigo);
            visualizarLink = visualizarLink.replace("{desabilitar}", "");

            conferirLink = conferirLink.replace("{link}", window.urlRoot + "/ComprovanteRendimento/Analisar/" + row.Codigo);
            conferirLink = conferirLink.replace("{desabilitar}", "");
        } else {
            visualizarLink = visualizarLink.replace("{link}", "#");
            visualizarLink = visualizarLink.replace("{desabilitar}", "desabilitar");
            conferirLink = conferirLink.replace("{link}", "#");
            conferirLink = conferirLink.replace("{desabilitar}", "desabilitar");
        }

        links = visualizarLink;
        links += conferirLink;

        return links;
    }

    informe.Consulta = {
        BuscaPaginada: buscar
    };

    return informe;

})(typeof Informe == "undefined" ? {} : Informe);