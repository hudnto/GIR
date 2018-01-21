$(function () {
    $(document).on("click", ".active-result", function (evt) {
        
        $(".chosen-single span").addClass(this.classList[1]);
    });

    $(".field-validation-error")
        .wrap("<label class='control-label'></label>")
        .closest(".form-group")
        .addClass("has-error");

   var queryStringSemRegistro = GetParametrosQueryString("semprocedimento");
   if (queryStringSemRegistro) {
        $(".chosen-select").chosen("destroy");
        $("select.chosen-select option").remove();
        $(".chosen-select").append("<option value>Todos</option>");
        HabilitarChoosen();
    }
});

// retorna parametros da QueryString
function GetParametrosQueryString(name, url) {
    if (!url) {
        url = window.location.href;
    }
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

$(function () { });

Comprovante = (function (comprovante) {

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
        var url = window.urlRoot + "/Processamento/BuscarJSON";
        var mensagemSemRegistro = "Nenhum registro encontrado.";
        $('#tabelaConsulta').bootstrapTable('refresh');
        var options = {
            pagination: "true",
            pageSize: 10,
            cache: false,
            pageList: "[10, 25, 50]",
            sidePagination: "server",
            url: url,
            columns: [
                    {
                        title: 'Comprovante DIRF',
                        field: 'Comprovante',
                        align: 'center',
                        valign: 'middle',
                        formatter: linkComprovante
                    },
                    {
                        title: 'CPF/CNPJ',
                        field: 'Ano',
                        align: 'center',
                        valign: 'middle'
                    },
                    {
                        title: 'Data',
                        field: 'Data',
                        align: 'center',
                        valign: 'middle'
                    },
                    {
                        title: 'UF',
                        field: 'Situacao',
                        align: 'center',
                        valign: 'middle'
                    },
                    {
                        title: 'Àrea RH',
                        field: 'Descricao',
                        align: 'center',
                        valign: 'middle'
                    },
                    {
                        title: 'Email',
                        field: 'Tipo',
                        align: 'center',
                        valign: 'middle'
                    }
                 
            ],
            formatNoMatches: function () {
                return mensagemSemRegistro;
            }
        };

        $('#tabelaConsulta').bootstrapTable(options);
    }

    var linkComprovante = function (value, row, i) {
        var href = window.urlRoot + '/Processamento/Rotina/' + value;

        return '<a href="' + href + '">' + value + '</a>';
    }

    var operateFormatter = function (value, row) {
        var links = '';
        var visualizarLink = '<a href="' + urlRoot + "/Processamento/Visualizar/" + row.Codigo + '"><span class="fa fa-search" aria-hidden="true"></span></a>';
        var conferirLink = '<a href="' + urlRoot + "/Processamento/Conferir/" + row.Codigo + '"><span class="fa fa-check-square-o" aria-hidden="true"></span></a>';

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