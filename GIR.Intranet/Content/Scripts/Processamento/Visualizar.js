$(function () {
    Compravante.Buscar();
});

Compravante =  {
    Buscar: function () {
        var id = "#tabelaComprovante";
        $(id).bootstrapTable("destroy");
        var url = window.urlRoot + "/Processamento/VisualizarJson";
        var mensagemSemRegistro = "Nenhum registro encontrado.";
        var options = {
            pagination: "true",
            pageSize: 10,
            cache: false,
            classes: "table",
            pageList: "[10, 25, 50]",
            sidePagination: "server",
            url: url,
            //queryParams: function (params) {
            //    debugger;
            //    return { cpfCnpj: "12312312322" }    
            //},
            columns: [
                        {
                            title: 'Comprovante DIRF',
                            field: 'NomeArquivo',
                            align: 'center',
                            valign: 'middle'
                        },
                        {

                            title: 'CPF/CNPJ',
                            field: 'CpfCnpj',
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
                            field: 'Uf',
                            align: 'center',
                            valign: 'middle'
                        },
                        {
                            title: 'Área RH',
                            field: 'AreaRH',
                            align: 'center',
                            valign: 'middle'
                        },
                        {
                            title: 'Email',
                            field: 'Email',
                            align: 'center',
                            valign: 'middle'
                        }
            ],
            formatNoMatches: function () {
                return mensagemSemRegistro;
            }
        }
        $(id).bootstrapTable(options);
    }
}
