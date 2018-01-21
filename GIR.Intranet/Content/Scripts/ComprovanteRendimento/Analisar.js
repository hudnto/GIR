// Métodos para "Aprovar" o informe de rendimento
AnalisarComprovantesRendimento = (function(informe) {
    function clickAprovar() {
        chamarModal("/comprovanterendimento/ModalConfirmacao");
    }

    function chamarModal(url) {
        $.get(window.urlRoot + url, window.mostrarModalAprovar);
    }

    function mostrarModalConfirmar(modal) {
        $('#modalContainer>.modal').remove();
        $('#modalContainer').html(modal);
        $('#modalContainer>.modal').modal();
        $('#btnAprovarConfirmar').click(enviarAprovar);
    }

    function enviarAprovar(event) {

        //var observacao = window.$("#txtObservacao").val();
        var url = "/comprovanterendimento/ModalConfirmacao";
        var data = {
            //descricao: observacao,
            //proponenteId: window.$("#Identificacao_ProponenteId").val()
        };

        var sucesso = function(retorno) {
            if (retorno.sucesso) {
                //GerarBootstrapTable(retorno);
                RetornoMensagem(retorno);
            };
        };

        Dados.Post(url, data, sucesso);
    }

})(typeof AnalisarComprovantesRendimento == "undefined" ? {} : AnalisarComprovantesRendimento);