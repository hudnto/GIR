var Informe;

$(function () {
    Eventos.ApresentarProcessandoAjaxRequest();
    Eventos.RemoverProcessandoFimAjaxRequest();
    Eventos.InicializarChosenJS();
    Eventos.InicializarDatePicker();
    Eventos.InicializarTooltip();
    Eventos.SetaMascaraNumero();
});

Eventos = {
    ApresentarProcessandoAjaxRequest: function () {
        $(document).on("ajaxStart", function (evt) {
            Eventos.MostrarLoading(true);
        });
    },

    RemoverProcessandoFimAjaxRequest: function () {
        $(document).on("ajaxStop", function (evt) { 
                Eventos.MostrarLoading(false);
        });
    },

    MostrarLoading: function (mostrar) {
        if (mostrar) {
            $("#dvOverlay").addClass('showOverlay');
            $('button[type=submit]', this).attr('disabled', 'disabled');
            $('input[type=submit]', this).attr('disabled', 'disabled');
        } else {
            $("#dvOverlay").removeClass('showOverlay');
            $('button[type=submit]').removeAttr('disabled');
            $('input[type=submit]').removeAttr('disabled');
        }
    },

    InicializarChosenJS: function () {
        $(".chosen-select").chosen({
            width: "100%",
            no_results_text: "Registro não encontrado!"
        });

        $(".chosen-select-m").chosen({
            no_results_text: "Registro não encontrado!"
        });
    },

    InicializarTooltip: function () {
        $('[data-toggle="tooltip"]').tooltip();
    },

    InicializarDatePicker: function () {
        $('.date').datepicker({
            language: "pt-BR",
            todayHighlight: true,
            format: "dd/mm/yyyy",
            autoclose: true,
            orientation: "top auto"
        });
    },

    SetaMascaraNumero: function() {
        $.mask.masks.numero = { mask: '9999' };
        $('input[type="text"]').setMask();
    }
}

function RetornoMensagem(input) {
    LimparValidacao();
    var retorno = input.responseJSON == undefined ? input : input.responseJSON;
    if (retorno.sucesso) {
        $('.modal').modal();
    } else {
        MostrarErroModelState(retorno);
    }
}

function LimparValidacao() {
    var divSummary = $('.validation-summary-errors');
    divSummary.removeClass('validation-summary-errors');
    divSummary.addClass('validation-summary-valid');
    var li = divSummary.find('ul li');
    li.remove();

    var divAlertaMensagem = $("#divAlertaMensagem");

    if (divAlertaMensagem.length !== 0) {
        divAlertaMensagem.remove();
    }

    $('.has-error').removeClass('has-error');
    $(".itens-pdf").css("color", "");
    $(".itens-txt").css("color", "");
}

function MostrarErroModelState(retorno) {
    var erros = retorno.listaErros;
    var chaves = retorno.chaves;

    $.each(chaves, function (index, elem) {
        if ($('[name=' + elem + ']').length === 0) {
            chaves[index] = elem + 'Id';
        }
    });

    var divSummary = $('.validation-summary-valid');
    divSummary.removeClass('validation-summary-valid');
    divSummary.addClass('validation-summary-errors');

    var ul = divSummary.find('ul');
    $.each(erros, function (index, elem) {
        $(ul).append('<li>' + elem + '</li>');
    });

    $.each(chaves, function (index, elem) {
        $('[name=' + elem + ']').closest('.form-group').addClass('has-error');
    });
    $('[type=submit]').prop('disabled', false);
}

function MostrarErroCustomInputFile(retorno, seletor) {
    $(seletor).css("color", "#a94442");
    MostrarErroModelState(retorno);
}

var HabilitarChoosen = function () {
    if ($(".chosen-select")) {
        $(".chosen-select").chosen({ width: "20%" });
    };
};