$(function () {
    Anexo.Controle.HabilitarUploadViaAjax();
    Anexo.Controle.HabilitarEnvioForm();
    Anexo.Controle.HabilitarToggleCollapse();
    Anexo.Controle.HabilitarTooltipDinamicos();
    Anexo.Controle.HabilitaRemoverAnexos();
    Anexo.Controle.HabilitarValidacaoAnoCalendario();
    Anexo.Controle.HabilitarMascaraAnoExercicioCustom();
});

Anexo = (function (anexo) {
    var divAtual = $([]);
    var modalExcluir = $("#modalExcluir");
    var btnExcluir = $("#btn-excluir");
    var anexos = $("[data-documentos]");
    var body = $("body");

    // Polyfill pro ie para usar includes
    (function () {
        if (!Array.prototype.includes) {
            Object.defineProperty(Array.prototype, "includes", {
                enumerable: false,
                value: function (obj) {
                    var newArr = this.filter(function (el) {
                        return el == obj;
                    });
                    return newArr.length > 0;
                }
            });
        }
    })();

    var irParaTopo = function () {
        $("body, html").animate({ scrollTop: 0 }, "slow");
    };

    var mensagemErroPadrao = function () {
        var mensagem = {
            chaves: [],
            listaErros: ["Desculpe, erro não esperado na sua solicitação."]
        };
        MostrarErroModelState(mensagem);
    }

    var configurarModalExcluir = function () {
        modalExcluir.on('show.bs.modal', function (event) {
            var modal = $(this);
            var botaoOrigem = $(event.relatedTarget);
            var exclusaoDocumentoAnexo = {
                IdDocumentoAnexo: botaoOrigem.data("iddocumentoanexo"),
                IdItemVerificacaoProcedimento: botaoOrigem.closest("form").find("[name=IdItemVerificacaoProcedimento]").val(),
                IdDespesa: botaoOrigem.closest("form").find("[name=IdDespesaSolicitacaoReembolso]").val()
            };
            var modeloExclusaoDocumentoAnexo = JSON.stringify(exclusaoDocumentoAnexo);

            modal.find("#btn-excluir").attr("data-modeloExclusaoDocumentoAnexo", modeloExclusaoDocumentoAnexo);
        });
    };

    var onExcluirSucess = function (res) {
        Eventos.LimparValidacao();
        if (res.sucesso) {
            mostrarAnexos(res.arquivos);
        } else if (res.redirect) {
            window.location = res.url;
        } else {
            Eventos.MostrarErroModelState(res);
            irParaTopo();
        }

        HideLoadingMessage();
        modalExcluir.modal("hide");
    };

    var onExcluirFail = function (res) {
        mensagemErroPadrao();
        modalExcluir.modal("hide");
        irParaTopo();
    };

    var btnExcluirClick = function () {

        Dados.Post("/Reembolso/RemoverAnexo", JSON.parse(btnExcluir.attr("data-modeloexclusaodocumentoanexo")), onExcluirSucess, onExcluirFail);
    };

    var habilitarexclusao = function () {
        btnExcluir.click(btnExcluirClick);
    };

    var habilitarRemoverAnexo = function () {
        configurarModalExcluir();
        habilitarexclusao();

        $(document).on("click", ".btn-remover", function (evt) {
            divAtual = $(evt.currentTarget).closest("form");
            modalExcluir.modal("show", $(evt.currentTarget));
        });
    };

    var mostrarAnexos = function (res) {
        var files = res.arquivos;
        divAtual.find(".row.arquivos").empty();
        var tmplt = "<div class='btn-group anexo col-xs-2'><button class='btn btn-default btn-xs btn-sem-acao' data-toggle='tooltip' title='{FileName}' rel='tooltip'>{Nome}</button><button class='btn {TipoBotao} btn-xs btn-sem-acao'><span class='glyphicon {TipoIcone}'></span></button></div>";

        var i = files.length;
        $.each(files, function (indice, arquivoEnviado) {

            var fileName = "Arquivo " + (i);

            var arquivo = arquivoEnviado.Nome + (arquivoEnviado.FalhaAoImportar ? " - ERRO DE ENVIO" : "");
            var btn = tmplt.replace("{Nome}", fileName);
            btn = btn.replace("{TipoBotao}", (arquivoEnviado.FalhaAoImportar ? "btn-danger" : "btn-success"));
            btn = btn.replace("{TipoIcone}", (arquivoEnviado.FalhaAoImportar ? "glyphicon-exclamation-sign" : "glyphicon-paperclip"));
            btn = btn.replace("{FileName}", arquivo);

            divAtual.find(".row.arquivos").prepend(btn);
            i--;
        });

        if (files.length > 0) {
            divAtual.find(".itemenviado").removeClass("hidden");
        } else {
            divAtual.find(".itemenviado").addClass("hidden");
        }
    };

    var desabilitarBotoes = function () {
        $("#sim").prop("disabled", true);
        $("#executar").prop("disabled", true);
    }

    var desabilitarInputsUpload = function () {
        $('[name="txt"]').prop("disabled", true);
        $('[name="pdfs"]').prop("disabled", true);
    }

    var callbackVerificaSeExisteProcessamento = function (res) {
        var response = ($.isArray(res)) ? res[0] : res;
        if (response.sucesso) {
            $("#sim").val("reprocessar");
            $("[name=codigo]").val(response.id);
            $("[name=operacao]").val("ReprocessarRotina");
            $("#modal-reprocessamento").modal();
        } else {
            $("[name=codigo]").val(response.id);
            $("[name=operacao]").val("IncluirRotina");
            desabilitarBotoes();
            desabilitarInputsUpload();
            $("form").submit();
            //Tratamento de bug para não finalizar o loading antes do submit de form finalizar
            setTimeout(function () { ShowLoadingMessage(); }, 1500);
        }
    }

    var callbackFailRequest = function (res) {
        mensagemErroPadrao();
    };

    var onAdicionarAnexoSucess = function (res) {
        var response = ($.isArray(res)) ? res[0] : res;
        LimparValidacao();
        if (response.sucesso) {
            $("input[type=file]").val("");
            var tipo = response.contentType;
            var url = urlRoot + "/Processamento/ArquivosArmazenadosJSON";
            $.get(url, { tipo: response.contentType }).done(mostrarAnexos).fail(callbackFailRequest);
        } else if (response.redirect) window.location = res.url;
        else {
            var seletor = (response.chaves[0] === "PDF") ? ".itens-pdf" : ".itens-txt";
            MostrarErroModelState(response);
            irParaTopo();
        }

        HideLoadingMessage();
    };

    var verificaSeExisteProcessamento = function () {
        var ano = $("#AnoExercicio").val();
        var tipo = $("[type=radio]:checked").val();
        var data = { anoExercicio: ano, tipo: tipo };
        var url = urlRoot + "/Processamento/VerificaSeExisteProcessamentoJSON";

        $.get(url, data).done(callbackVerificaSeExisteProcessamento).fail(callbackFailRequest);
    }

    var enviarArquivos = function (arquivos, extensaoValida) {
        var range = 5;
        var formData = new FormData();

        if (arquivos.length <= range) {
            $.each(arquivos, function (i, value) {
                formData.append("arquivos", arquivos[i]);
            });

            formData.append("tipoArquivo", extensaoValida);
            Dados.PostFormData("/Processamento/UploadArquivosJSON", formData, onAdicionarAnexoSucess, callbackFailRequest, true);
        }
        else {
            var promises = [];
            var key = "arquivos";
            var optionsAjax = {
                type: "POST",
                url: urlRoot + "/Processamento/UploadArquivosJSON",
                processData: false,
                contentType: false,
                dataType: undefined,
                async: false,
                contentType: false,
                cache: false,
                header: { 'X-Requested-With': 'XMLHttpRequest' }
            };
            ShowLoadingMessage();

            $.each(arquivos, function (i, value) {
                if (i < range && arquivos[(i + 1)] != undefined) {
                    formData.append(key, value);
                }

                if (i < range && arquivos[(i + 1)] == undefined) {
                    formData.append(key, value);
                    formData.append("tipoArquivo", extensaoValida);
                    optionsAjax.data = formData;

                    var ajax = $.ajax(optionsAjax);
                    formData = new FormData();
                    promises.push(ajax);
                }

                if (i === range) {
                    range = range + 5;
                    formData.append(key, value);
                    formData.append("tipoArquivo", extensaoValida);
                    optionsAjax.data = formData;

                    var ajax = $.ajax(optionsAjax);
                    formData = new FormData();
                    promises.push(ajax);
                }
            });

            $.when.apply(null, promises).then(onAdicionarAnexoSucess, callbackFailRequest);
        }
    };

    var habilitarToggleCollapse = function () {
        $(".list-group-item").on("click", function () {
            $("[class*=glyphicon-chevron]", this)
                .toggleClass("glyphicon-chevron-right")
                .toggleClass("glyphicon-chevron-down");
        });
    };

    var habilitarEnvioForm = function () {

        $(document).on("click", "#executar", function (evt) {
            var isIndividual = $("#Individual")[0];
            if (isIndividual.checked) {
                $("#sim").val("individual");
                $("#modal-individual").modal();
                evt.preventDefault();
            } else {
                verificaSeExisteProcessamento();
            }
        });

        $(document).on("click", "#sim", function (evt) {
            var valor = $("#sim").val();

            if (valor === "individual") {
                verificaSeExisteProcessamento();
            }

            if (valor === "reprocessar") {
                desabilitarBotoes();
                desabilitarInputsUpload();
                $("form").submit();
            }
        });

        $(document).on("click", "#Individual", function (evt) {
            var individual = evt.currentTarget;
            if (individual.checked)
                $('[name="txt"]').prop("disabled", true);
            else
                $('[name="txt"]').prop("disabled", false);
        });
    }

    var habilitarUploadViaAjax = function () {

        $(document).on("click", ".item-verificacao", function (evt) {
            var originalSrcElement = $(evt.originalEvent.target);
            // Caso a pessoa tenha clicado no botão com o nome do anexo, não faz nada
            var isBotao = originalSrcElement.hasClass("btn") || originalSrcElement.hasClass("glyphicon-remove");
            if (isBotao) {
                return false;
            }
            var el = $(evt.currentTarget);
            el.closest(".item").find("input[type=file]").trigger("click");
        });

        $(document).on("change", "input[type=file]", function (evt) {
            var el = $(evt.currentTarget);
            // IE
            if (el.val() === "") return false;

            divAtual = el.closest("div");

            var arquivosNoInput = evt.currentTarget.files;

            var extensaoValida = (el[0].accept === ".pdf") ? "application/pdf" : "text/plain";

            if (el[0].accept === ".txt")
                $("#Individual").prop("disabled", true);

            var contemExtensaoNaoValida = false;

            for (var i = 0; i < arquivosNoInput.length; i++) {
                if (arquivosNoInput[i].type !== extensaoValida) {
                    contemExtensaoNaoValida = true;
                }
            }

            var vinteMegaByte = 20971520; // 20 mb em bytes

            var contemArquivoGrande = false;

            for (var i = 0; i < arquivosNoInput.length; i++) {
                if (arquivosNoInput[i].size > vinteMegaByte) {
                    contemArquivoGrande = true;
                }
            }

            var contemErro = contemArquivoGrande || contemExtensaoNaoValida;
            LimparValidacao();
            if (contemErro) {

                var mensagem = {
                    chaves: [],
                    listaErros: []
                };

                if (contemArquivoGrande || contemExtensaoNaoValida) {
                    mensagem.listaErros.push("Arquivo informado é inválido.");
                }

                var seletor = (el[0].accept === ".pdf") ? ".itens-pdf" : ".itens-txt";

                MostrarErroModelState(mensagem);
                irParaTopo();
                return false;

            }

            enviarArquivos(arquivosNoInput, extensaoValida);
        });
    };

    var habilitarToolTipDinamicos = function () {
        $(body).tooltip({
            selector: "[rel=tooltip]"
        });

    };

    var habilitarEdicaoAnexo = function () {

        anexos.each(function (indice, item) {
            divAtual = $(item).closest("div");
            mostrarAnexos($(item).data("documentos"));
        });
    };

    var habilitarValidacaoAnoCalendario = function() {
        $(document).on("change", "#AnoExercicio", function() {
            if (this.value.trim().length == 4) {
                var valor = this.value - 1;
                $("#AnoCalendario").val(valor);
            } else {
                $("#AnoCalendario").val("");
            }
        });
    };

    var habilitarMascaraAnoExercicioCustom = function () {
        $(".date").datepicker("remove");
        $(".date").datepicker({
            language: "pt-BR",
            todayHighlight: true,
                format: "dd/mm/yyyy",
            autoclose: true,
            orientation: "top auto",
            format: " yyyy",
            viewMode: "years",
            minViewMode: "years"
        });
    }

    anexo.Controle = {
        HabilitarUploadViaAjax: habilitarUploadViaAjax,
        HabilitarEnvioForm: habilitarEnvioForm,
        HabilitarToggleCollapse: habilitarToggleCollapse,
        HabilitarTooltipDinamicos: habilitarToolTipDinamicos,
        HabilitaRemoverAnexos: habilitarRemoverAnexo,
        HabilitarEdicaoAnexo: habilitarEdicaoAnexo,
        HabilitarValidacaoAnoCalendario: habilitarValidacaoAnoCalendario,
        HabilitarMascaraAnoExercicioCustom: habilitarMascaraAnoExercicioCustom
    };

    return anexo;
})(typeof (Anexo) === "undefined" ? {} : Anexo)