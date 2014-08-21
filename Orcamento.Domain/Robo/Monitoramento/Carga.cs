﻿using Orcamento.Domain.Gerenciamento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Orcamento.Domain.Robo.Monitoramento;
using Orcamento.Domain.Robo.Monitoramento.EstrategiasDeCargas;
using Orcamento.Domain.Robo.Fabricas;
using Orcamento.Domain.DB.Repositorio.Robo;
using System.Net.Http;

namespace Orcamento.Domain.Entities.Monitoramento
{
    public class Carga : IAggregateRoot<Guid>
    {
        private IProcessaCarga _processaCarga;

        public virtual Guid Id { get; set; }

        public Carga()
        {

        }

        public Carga(IProcessaCarga processaCarga,TipoEstrategiaDeCargaEnum tipo, string nome, string diretorio)
        {
            _processaCarga = processaCarga;
            this.DataCriacao = DateTime.Now;
            AtribuirNome(nome);
            AtribuirTipo(tipo);
            AtribuirDiretorio(diretorio);
            CriarCarga();
        }

        private void AtribuirDiretorio(string diretorio)
        {
            this.Diretorio = diretorio;
        }

        private void AtribuirNome(string nome)
        {
            this.NomeArquivo = nome;
        }

        private void CriarCarga()
        {
            this.Status = "Criada";
        }

        public virtual void IniciarCarga()
        {
            this.DataInicio = DateTime.Now;
            this.Status = "Iniciada";
        }

        public virtual void FinalizarCarga()
        {
            this.DataFim = DateTime.Now;
            this.Status = "Finalizada";
        }

        public virtual void AtribuirTipo(TipoEstrategiaDeCargaEnum tipo)
        {
            Tipo = tipo;
        }

        public virtual DateTime DataCriacao { get; set; }
        public virtual DateTime? DataInicio { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual string Diretorio { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual string Status { get; set; } //Altera 
        public virtual string NomeArquivo { get; set; }
        public virtual IList<Detalhe> Detalhes { get; set; }
        public virtual TipoEstrategiaDeCargaEnum Tipo { get; set; }

        public virtual void AdicionarDetalhe(string nome, string descricao, int linha, TipoDetalheEnum detalheEnum)
        {

            if (Detalhes == null)
                Detalhes = new List<Detalhe>();

            var detalhe = new Detalhe() { Nome = nome, Descricao = descricao, Linha = linha, TipoDetalhe = detalheEnum };

            Detalhes.Add(detalhe);
        }

        public virtual void AdicionarDetalhe(string nome, string descricao, int linha, TipoDetalheEnum detalheEnum, string execao)
        {

            if (Detalhes == null)
                Detalhes = new List<Detalhe>();

            var detalhe = new Detalhe() { Nome = nome, Descricao = descricao, Linha = linha, TipoDetalhe = detalheEnum ,Excecao = execao};
            if (detalheEnum != TipoDetalheEnum.sucesso)
                this.RegistrarExecao();

            Detalhes.Add(detalhe);
        }

        private void RegistrarExecao()
        {
            this.Status = "Erro de processo.";
        }

        public virtual void Processar()
        {
            _processaCarga.Processar(this, false);

            if (this.Ok())
            {
                ChamarServico();
            }
        }

        private void ChamarServico()
        {
            //HttpClient client = new HttpClient();
            //HttpResponseMessage resp = client.GetAsync(string.Format("http://localhost:2884/api/cargas?idCarga={0}", Id)).Result;
            //resp.EnsureSuccessStatusCode();

            HttpClient client = new HttpClient();
            client.GetAsync(string.Format("http://localhost:2884/api/cargas?idCarga={0}", Id));

        }

        public virtual void Processa()
        {
            _processaCarga = FabricaDeImportacao.Criar(Tipo);
            _processaCarga.Processar(this, true);
        }

        public virtual bool Ok()
        {
            return this.Detalhes == null || this.Detalhes.Count == 0 || this.Detalhes.Where(d => d.TipoDetalhe == TipoDetalheEnum.erroDeProcesso || d.TipoDetalhe == TipoDetalheEnum.erroLeituraExcel) != null
                && this.Detalhes.Where(d => d.TipoDetalhe == TipoDetalheEnum.erroDeProcesso || d.TipoDetalhe == TipoDetalheEnum.erroLeituraExcel).Count() == 0;
        }
    }
}
