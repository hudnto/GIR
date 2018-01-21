function Dados() {

};


//Recupera dados em JSON da URL informada, invocando a função 'success' caso tudo ocorra bem
Dados.prototype.Get = function (url, success, error) {
    this.Ajax(url, 'GET', 'json', null, success, error);
},

//Envia dados JSON para uma URL informada, invodando a função 'success' em caso de sucesso
Dados.prototype.Post = function (url, dados, success, error) {
    this.Ajax(url, 'POST', 'json', dados, success, error);
},

//Envia dados do formulário e retorna objeto JSON com dados do resultado
Dados.prototype.PostForm = function (form, success, error) {
    var $form = $(form);

    this.post($form.attr('action'), $form.serialize(), success, error);
},

Dados.prototype.PostFormData = function (url, formData, success, error, isAsync) {

    this.Ajax(url, 'POST', undefined, undefined, success, error, undefined, {
        processData: false,
        contentType: false,
        dataType: undefined,
        data: formData,
        async: isAsync
    });
}

Dados.prototype.Ajax = function (url, method, type, dados, success, error, complete, options) {

    var defaultOptions = {
        url: window.urlRoot + url,
        type: method,
        data: JSON.stringify(dados),
        dataType: type,
        contentType: "application/json;charset=utf-8",
        cache: false,
        header: { 'X-Requested-With': 'XMLHttpRequest' },
        complete: complete
    };

    var opcoesEnvio = $.extend(defaultOptions, options);

    $.ajax(opcoesEnvio).done(success).fail(error);
}

Dados = new Dados();