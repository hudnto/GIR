ComprovantesRendimento = (function (informe) {

    informe.Consulta = {
        BuscaPaginada: buscar,
        BuscaPaginadaView: buscarPaginado,
        Mascara: mascaraCpfCnpj
    };

    return informe;

    function buscar(input) {
        LimparValidacao();
        var retorno = input.responseJSON == undefined ? input : input.responseJSON;
        if (retorno.sucesso) {
            buscarPaginado();
            $("#botaolimpar").show();
        } else {
            MostrarErroModelState(retorno);
        }
    }

    function buscarPaginado() {
        var url = window.urlRoot + "/ComprovanteRendimento/BuscarJSON";
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
                        title: "Comprovantes DIRF",
                        field: "NomeArquivo",
                        align: "center",
                        valign: "middle",
                        formatter: operateFormatter
                    },
                    {
                        title: "CPF/CNPJ",
                        field: "CPFCNPJ",
                        align: "center",
                        valign: "middle"
                    },
                    {
                        title: "Data",
                        field: "DataUltimaAtualizacao",
                        align: "center",
                        valign: "middle"
                    },
                    {
                        title: "UF",
                        field: "Uf",
                        align: "center",
                        valign: "middle"
                    },
                    {
                        title: "Unid. Org.",
                        field: "UnidadeOrganizacional",
                        align: "center",
                        valign: "middle"
                    },
                    {
                        title: "Descr. Unid. Org.",
                        field: "UnidadeOrganizacionalDescr",
                        align: "center",
                        valign: "middle"
                    },
                    {
                        title: "Email",
                        field: "Email",
                        align: "center",
                        valign: "middle"
                    }
            ],
            formatNoMatches: function () {
                return mensagemSemRegistro;
            }
        };

        $("#tabelaConsulta").bootstrapTable(options);
    }

    //formata com a máscara de CPF ou CNPJ dependendo da quantidade de caracteres 
    function cpfCnpj(v) {

        //Remove tudo o que não é dígito
        v = v.replace(/\D/g, "");

        if (v.length <= 11) { //CPF

            //Coloca um ponto entre o terceiro e o quarto dígitos
            v = v.replace(/(\d{3})(\d)/, "$1.$2");

            //Coloca um ponto entre o terceiro e o quarto dígitos
            //de novo (para o segundo bloco de números)
            v = v.replace(/(\d{3})(\d)/, "$1.$2");

            //Coloca um hífen entre o terceiro e o quarto dígitos
            v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2");

        } else { //CNPJ

            //Coloca ponto entre o segundo e o terceiro dígitos
            v = v.replace(/^(\d{2})(\d)/, "$1.$2");

            //Coloca ponto entre o quinto e o sexto dígitos
            v = v.replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3");

            //Coloca uma barra entre o oitavo e o nono dígitos
            v = v.replace(/\.(\d{3})(\d)/, ".$1/$2");

            //Coloca um hífen depois do bloco de quatro dígitos
            v = v.replace(/(\d{4})(\d)/, "$1-$2");

        }

        return v;
    }

    function mascaraCpfCnpj(o) {
        v_obj = o;
        v_fun = cpfCnpj;
        setTimeout(execmascara, 1);
    }

    function execmascara() {
        v_obj.value = v_fun(v_obj.value);
    }

    function convertData(value, row, i) {
        return new Date(value).toString();
    }

    function operateFormatter(value, row) {
        var links = "";
        var visualizarLink = '<a href="' + window.urlRoot + "/Arquivo/BuscaPdf/" + row.NomeArquivo + '">' + row.NomeArquivo + ".pdf" + "</a>";

        if (row.Situacao !== "Erro")
            links = visualizarLink;

        return links;
    }

    function buscarDescricaoUnidade(codigoUnidade) {
        var dropDownUnidade = $("#unidade");

        dropDownUnidade.change(function () {

            var self = $(this);
            var unidade = self.val();
            var dropDownUnidadeDescr = $("#UnidadeDescr");

            if (unidade !== "") {

                var url = "/Ajax/BuscarUnidadeOrganizacionalPorCodigo/?unidade=" + unidade;

                Dados.Get(url, function (contratos) {
                    window.CriarDropDown("UnidadeDescr", contratos, "Id", "UnidadeDescr", true);

                    dropDownUnidadeDescr.removeAttr("disabled");
                    dropDownUnidadeDescr.trigger('chosen:updated');
                });
            } else {
                dropDownUnidadeDescr.empty();
                dropDownUnidadeDescr.trigger("chosen:updated");
            }
        });
    }

})(typeof ComprovantesRendimento == "undefined" ? {} : ComprovantesRendimento);

(function ($) {
    //preenche os dados dos comprovantes sem filto quando carrega a página
    $(document).ready(ComprovantesRendimento.Consulta.BuscaPaginadaView);

    //faz a consulta passando parâmetgros que filtram os registro
    $("#pesquisar").click(enviarDados);

    function enviarDados() {
        var request = {};
        request.Codigo = $("input#Codigo.form-control").val();
        request.CpfCnpj = $("#CpfCnpj_chosen").text();
        request.UnidadeOrganizacional = $("#UnidadeOrganizacional_chosen").text();
        request.UnidadeOrganizacionalDescr = $("#UnidadeOrganizacionalDescr_chosen").text();
        request.Uf = $("#Uf_chosen").text();

        $.ajax({
            url: "/comprovanterendimento/visualizar",
            type: "post",
            dataType: "json",
            success: ComprovantesRendimento.Consulta.BuscaPaginada,
            data: request
        });
    }

})(jQuery);


