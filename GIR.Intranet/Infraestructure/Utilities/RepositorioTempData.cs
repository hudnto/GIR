using System.Web.Mvc;

namespace GIR.Intranet.Infraestructure.Utilities
{
    public class RepositorioTempData
    {
        private TempDataDictionary _tempData;
        public RepositorioTempData(TempDataDictionary tempData)
        {
            _tempData = tempData;
        }

        public void Adicionar(string key, object valor)
        {
            _tempData[key] = valor;
        }

        public void Excluir(string key)
        {
            _tempData.Remove(key);
        }

        public object Buscar(string key)
        {
            var valor = _tempData[key];
            _tempData.Keep(key);
            return valor;
        }

        public T Buscar<T>(string key) where T : class
        {
            var valor = Buscar(key);

            return valor as T;
        }
    }
}