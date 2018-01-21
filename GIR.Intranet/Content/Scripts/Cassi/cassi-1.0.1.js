$(document).ready(function () {
    //Configuração de menu com submenu.
    $(function () {
        $('.menu ul > li.itemSubmenu > a').click(function () {
            $('span', $(this)).toggleClass("caret-right");
            $('span', $(this)).toggleClass("caret-bottom");
        });
    });

    $(function () {
        $('li.itemSubmenu > a').click(function () {
            $(this).toggleClass("active");
        });
    });

    //Configura a máscara de telefone celular para permitir
    //a digitação do nono dígito.
    $(function () {
        $('[alt=celular]').on("keyup", function (event) {
            var target, phone, element;
            target = (event.currentTarget) ? event.currentTarget : event.srcElement;
            phone = target.value.replace(/\D/g, '');
            element = $(target);
            element.unsetMask();
            if (phone.length > 10) {
                element.setMask("(99) 99999-9999");
            } else {
                element.setMask("(99) 9999-99999");
            }
        });
    });

    //Verifica se o componente Datepicker está sendo utilizado pela página atual. 
    //Caso esteja, configura o mesmo conforme o campo.
    if ($().datepicker != undefined) {
        $(function () {
            $('.date').datepicker({
                language: "pt-BR",
                todayHighlight: true,
                format: 'dd/mm/yyyy',
                autoclose: true,
                orientation: "top auto"
            });
        });

        $(function () {
            $('.competencia').datepicker({
                language: "pt-BR",
                format: 'mm/yyyy',
                autoclose: true,
                orientation: "top auto",
                minViewMode: 1
            });
        });
    }

    //Faça $("#IdCampo").keydown(filtroSoNumeros); 
    //para permitir a entrada apenas de números no campo.
    var filtroSoNumeros = function (e) {
        // Allow: backspace, delete, tab, escape and enter
        if ($.inArray(e.keyCode, [46, 8, 9, 27, 13, 110]) !== -1 ||
            // Allow: Ctrl+A
            (e.keyCode == 65 && e.ctrlKey === true) ||
            // Allow: home, end, left, right
            (e.keyCode >= 35 && e.keyCode <= 39)) {
            // let it happen, don't do anything
            return;
        }
        // Ensure that it is a number and stop the keypress
        if ((e.shiftKey || (e.keyCode < 48 || e.keyCode > 57)) && (e.keyCode < 96 || e.keyCode > 105)) {
            e.preventDefault();
        }
    };
});