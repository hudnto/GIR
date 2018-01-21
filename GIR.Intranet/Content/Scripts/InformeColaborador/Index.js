(function ($) {
    //inicia com o botão Limpar escondido e só mostra quando tiver feito uma consulta
    $('#botaolimpar').hide();       

})(jQuery);

InformeColaborador = (function (informe) {
    

    informe.Consulta = {
        BuscaPaginada: buscar,
        Mascara: mascaraCpfCnpj
    };

    return informe;

    function buscar(input) {
        LimparValidacao();
        var retorno = input.responseJSON == undefined ? input : input.responseJSON;        
        if (retorno.sucesso) {
            buscarPaginado();
            $('#botaolimpar').show();
        } else {
            MostrarErroModelState(retorno);
        }
    }

    function buscarPaginado() {
        var url = urlRoot + "/InformeColaborador/BuscarJSON";
        var mensagemSemRegistro = "Nenhum registro encontrado!";
        $('#tabelaConsulta').bootstrapTable('refresh');
        var options = {
            pagination: "true",
            pageSize: 10,
            cache: false,
            pageList: "[10, 25, 50]",
            sidePagination: "server",
            url : url,
            columns: [
                    {
                        title: 'CPF/CNPJ',
                        field: 'CPFCNPJ',
                        align: 'center',
                        valign: 'middle',
                        formatter: cpfCnpj
                    },
                    //{
                    //    title: 'Ano Base',
                    //    field: 'AnoExercicio',
                    //    align: 'center',
                    //    valign: 'middle'
                    //},
                    {

                        title: 'Ações',
                        align: 'center',
                        formatter: operateFormatter
                    }
            ],
            formatNoMatches: function () {
                return mensagemSemRegistro;
            }
        };

        $('#tabelaConsulta').bootstrapTable(options);
    }

    /*
    *   formata com a máscara de CPF ou CNPJ dependendo da quantidade de caracteres    
    */
    function cpfCnpj(v) {

        //Remove tudo o que não é dígito
        v = v.replace(/\D/g, "")

        if (v.length <= 11) { //CPF

            //Coloca um ponto entre o terceiro e o quarto dígitos
            v = v.replace(/(\d{3})(\d)/, "$1.$2")

            //Coloca um ponto entre o terceiro e o quarto dígitos
            //de novo (para o segundo bloco de números)
            v = v.replace(/(\d{3})(\d)/, "$1.$2")

            //Coloca um hífen entre o terceiro e o quarto dígitos
            v = v.replace(/(\d{3})(\d{1,2})$/, "$1-$2")

        } else { //CNPJ

            //Coloca ponto entre o segundo e o terceiro dígitos
            v = v.replace(/^(\d{2})(\d)/, "$1.$2")

            //Coloca ponto entre o quinto e o sexto dígitos
            v = v.replace(/^(\d{2})\.(\d{3})(\d)/, "$1.$2.$3")

            //Coloca uma barra entre o oitavo e o nono dígitos
            v = v.replace(/\.(\d{3})(\d)/, ".$1/$2")

            //Coloca um hífen depois do bloco de quatro dígitos
            v = v.replace(/(\d{4})(\d)/, "$1-$2")

        }

        return v;
    }

    function mascaraCpfCnpj(o) {
        v_obj = o;
        v_fun = cpfCnpj;
        setTimeout(execmascara, 1);
    }

    function execmascara() {
        v_obj.value = v_fun(v_obj.value)
    }

    function operateFormatter(value, row) {
        var links = '';
        var visualizarLink = '<a href="' + urlRoot + "/Arquivo/BuscaPdf/" + row.CPFCNPJ + '/' + row.AnoExercicio + '"><span class="fa fa-search" aria-hidden="true"></span></a>';
        
        if (row.Situacao !== "Erro")
            links = visualizarLink;

        return links;
    }

})(typeof InformeColaborador == "undefined" ? {} : InformeColaborador);
