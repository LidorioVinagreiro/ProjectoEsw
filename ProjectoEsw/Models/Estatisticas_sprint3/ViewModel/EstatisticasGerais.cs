using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectoEsw.Models.Estatisticas_sprint3.ViewModel
{
    public class EstatisticasGerais
    {
        public List<TipoQuantidadeEstatistica> TipoQuantidadeEstatisticas { get; set; }
        public TotalBolsaEstatisticas TotalBolsaEstatisticas { get; set; }
        public List<ARCandidaturasEstatisticas> ARCandidaturaEstatisticas { get; set; }
        public List<InstituicaoEstatisticas> InstituicaoEstatistica { get; set; }
        public DestinosPreferenciasEstatisticas DestinosPreferenciasEstatisticas { get; set; }
    }
}
