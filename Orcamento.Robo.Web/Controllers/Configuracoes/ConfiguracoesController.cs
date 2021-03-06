﻿using Orcamento.Domain.Robo.Monitoramento.EstruturaOrcamentaria;
using Orcamento.Robo.Web.Models.Configuracoes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Orcamento.Robo.Web.Controllers.Configuracoes
{
    public class ConfiguracoesController : ControllerBase
    {
        private Orcamento.Controller.Robo.ConfiguracoesController controller = new Controller.Robo.ConfiguracoesController();

        //
        // GET: /Configuracoes/

        public ActionResult Index()
        {
            var configuracao = ObterProcessos();

            return View(configuracao);
        }

        private ConfiguracaoModel ObterProcessos()
        {
            List<Orcamento.Domain.Robo.Monitoramento.EstruturaOrcamentaria.Processo> processos = controller.ObterProcessos();

            ConfiguracaoModel configuracao = new ConfiguracaoModel();
            foreach (var processo in processos)
            {
                configuracao.AdicionarProcesso(processo);
            }
            return configuracao;
        }

        public ActionResult Deletar()
        {
            try
            {
                controller.Deletar();
                this.ShowMessage(MessageTypeEnum.success, "Estrutura deletada com sucesso!", true);
                var configuracao = ObterProcessos();
                configuracao.Tipo = "success";
                configuracao.Mensagem = "Estrutura deletada com sucesso.";
                configuracao.Titulo = "Estrutura deletada.";
                return PartialView("_Processos", configuracao);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult Delete(int tipoProcesso)
        {
            try
            {
                controller.Deletar((TipoProcessoEnum)tipoProcesso);

                this.ShowMessage(MessageTypeEnum.success, "Estrutura deletada com sucesso!");
                var configuracao = ObterProcessos();

                CriarConfiguracao(tipoProcesso, configuracao, "success", "Processo " +
                                    configuracao.Processos.FirstOrDefault(p => p.TipoProcesso == tipoProcesso).Nome +
                                    " foi concluido.", "Processo: " + configuracao.Processos.FirstOrDefault(p => p.TipoProcesso == tipoProcesso).Nome);

                return PartialView("_Processos", configuracao);
            }
            catch (Exception)
            {

                throw;
            }

        }

        public ActionResult Processos()
        {
            try
            {
                var configuracao = ObterProcessos();
                return PartialView("_Processos", configuracao);
            }
            catch (Exception ex)
            {
                this.ShowMessage(MessageTypeEnum.danger, "Erro ao obter Processos.");
                return null;
            }
        }

    }
}
