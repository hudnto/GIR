using GIR.Core.Infraestrutura.Extensoes;
using GIR.Core.Negocio.Servico;
using GIR.Core.Negocio.Enum;
using GIR.Integracao.ServicoUtilitarioConsulta;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using GASC.Seguranca;

namespace GIR.Intranet.Infraestructure.Helpers
{
    public static class MvcHtmlHelper
    {
        private static readonly GIRServico GirServico;
        private static readonly UtilitarioConsultaClient ServicoUtilitarioConsulta;
        private static readonly UnidadeOrganizacionalServico ServicoUnidadeOrganizacional;

        static MvcHtmlHelper()
        {
            GirServico = new GIRServico();
            ServicoUtilitarioConsulta = new UtilitarioConsultaClient();
            ServicoUnidadeOrganizacional = new UnidadeOrganizacionalServico();
        }

        public static MvcHtmlString DropDownListAnoExercicioFor<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes, bool preenchimentoOpcional)
        {
            var anos = GirServico.BuscarAnosExercicio();

            var selectListAnos = anos.Select(a => new SelectListItem
            {
                Text = a.ToString(),
                Value = a.ToString()
            }).ToList();

            if (preenchimentoOpcional)
            {
                selectListAnos.Insert(0, new SelectListItem
                {
                    Text = @"Selecione",
                    Value = "0"
                });
            }

            return helper.DropDownListFor(expression, selectListAnos, htmlAttributes);
        }

        public static MvcHtmlString DropDownListCpfcnpjFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {
            var resultado = GirServico.BuscarTodosRegistrosProcessados();

            var selectListCpfCnpj = resultado.Select(c => new SelectListItem
            {
                Text = c.CPFCNPJ,
                Value = c.Id.ToString()
            });

            return htmlHelper.DropDownListFor(expression, selectListCpfCnpj, null, htmlAttributes);
        }

        public static MvcHtmlString DropDownListUfsFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {
            var resultado = ServicoUtilitarioConsulta.listarEstados();
            var selectListEstado = resultado.Select(u => new SelectListItem
            {
                Text = u.sigla,
                Value = u.idEstado.ToString()
            });

            return htmlHelper.DropDownListFor(expression, selectListEstado, null, htmlAttributes);
        }

        public static MvcHtmlString DropDownListUnidadeOrganicionalFor<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            object htmlAttributes)
        {
            var resultado = ServicoUnidadeOrganizacional.ListarUnidades();
            var selectListunidades = resultado.Select(u => new SelectListItem()
            {
                Text = u.Codigo.ToString(),
                Value = u.Codigo.ToString()
            });

            return htmlHelper.DropDownListFor(expression, selectListunidades, null, htmlAttributes);
        }

        public static MvcHtmlString MenuGIR(this HtmlHelper html, CassiAuthorizePrincipal usuario)
        {
            StringBuilder tagMenuGIR = new StringBuilder();
            UrlHelper urlHelper = new UrlHelper(html.ViewContext.RequestContext);

            if (usuario.IsInFuncionalidade(GascFuncionalidade.GR01))
            {
                tagMenuGIR.Append("<li><span class='tituloMenu'>Gerenciador</span></li>");
                tagMenuGIR.Append(string.Format("<li><a href='{0}'>Consultar Informes de Rendimentos</a></li>", urlHelper.Action("ConsultarInformeRendimento", "Processamento")));
            }

            if (usuario.IsInFuncionalidade(GascFuncionalidade.GR08))
            {
                tagMenuGIR.Append("<li><span class='tituloMenu'>Funcionário CASSI</span></li>");
                tagMenuGIR.Append(string.Format("<li><a href='{0}'>Consultar Informes de Rendimentos</a></li>", urlHelper.Action("Index", "InformeColaborador")));
            }

            return new MvcHtmlString(tagMenuGIR.ToString());
        }

        public static MvcHtmlString RadioButtonsForEnums<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var stringBuilder = new StringBuilder();
            var tipos = Enum.GetValues(typeof(TipoContribuinte)).Cast<TipoContribuinte>().Where(t => t != TipoContribuinte.Nenhum);
            var propName = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).PropertyName;

            foreach (var tipoFuncionario in tipos)
            {
                var id = "tipo-contribuinte-" + ((int)tipoFuncionario);
                var input = new TagBuilder("input");
                input.Attributes.Add("type", "radio");
                input.Attributes.Add("value", ((int)tipoFuncionario).ToString());
                input.Attributes.Add("name", propName);
                input.Attributes.Add("id", id);
                if (tipoFuncionario == TipoContribuinte.Todos)
                    input.Attributes.Add("checked", "checked");
                stringBuilder.Append(input);

                var label = new TagBuilder("label");
                label.Attributes.Add("for", id);
                label.AddCssClass("control-label margin-1");
                label.SetInnerText(tipoFuncionario.GetDescription());
                stringBuilder.Append(label);
            }

            return MvcHtmlString.Create(stringBuilder.ToString());
        }

        public static MvcHtmlString RadioButtonsIncluirRotina<TModel, TProperty>(
            this HtmlHelper<TModel> helper,
            Expression<Func<TModel, TProperty>> expression,
            CassiAuthorizePrincipal usuario)
        {
            var stringBuilder = new StringBuilder();
            var tipos = Enum.GetValues(typeof(TipoContribuinte)).Cast<TipoContribuinte>()
                .Where(t => t != TipoContribuinte.Nenhum && t != TipoContribuinte.Todos).ToList();

            if (!usuario.IsInFuncionalidade(GascFuncionalidade.GR09))
                tipos.Remove(TipoContribuinte.FuncionarioCassi);

            if (!usuario.IsInFuncionalidade(GascFuncionalidade.GR02))
                tipos.Remove(TipoContribuinte.PrestadorFornecedor);

            var propName = ModelMetadata.FromLambdaExpression(expression, helper.ViewData).PropertyName;

            foreach (var tipoFuncionario in tipos)
            {
                var div = new TagBuilder("div");
                var divFormGroup = new TagBuilder("div");
                div.Attributes.Add("class", "col-xs-4");
                divFormGroup.Attributes.Add("class", "form-group");
                var id = "tipo-contribuinte-" + ((int)tipoFuncionario);
                var input = new TagBuilder("input");
                input.Attributes.Add("type", "radio");
                input.Attributes.Add("value", ((int)tipoFuncionario).ToString());
                input.Attributes.Add("checked", "checked");
                input.Attributes.Add("class", "margin-right-1");
                input.Attributes.Add("name", propName);
                input.Attributes.Add("id", id);

                var label = new TagBuilder("label");
                label.Attributes.Add("for", id);
                label.AddCssClass("control-label");
                label.SetInnerText(tipoFuncionario.GetDescription());

                divFormGroup.InnerHtml = input.ToString();
                divFormGroup.InnerHtml += label.ToString();
                div.InnerHtml = divFormGroup.ToString();
                stringBuilder.Append(div);
            }

            return MvcHtmlString.Create(stringBuilder.ToString());
        }

    }
}