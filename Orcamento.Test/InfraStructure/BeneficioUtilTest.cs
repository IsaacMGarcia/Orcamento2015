﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Orcamento.InfraStructure.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Orcamento.TestMethod.InfraStructure
{
    [TestClass]
    public class BeneficioUtilTestMethod
    {
        [TestMethod]
        public void data_de_admissao_de_2011_retorna_12_meses_em_relacao_a_data_atual_de_2012_com_sucesso()
        {
            DateTime dataDeAdmissao = new DateTime(2011, 01, 01);

            Assert.AreEqual(dataDeAdmissao.ObterOTotalDeMesesAReceberOBeneficio(new DateTime(2012, 1, 1)), 12);
        }

        [TestMethod]
        public void data_de_admissao_de_2012_janeiro_retorna_7_meses_em_relacao_a_data_atual_de_2012_agosto_com_sucesso()
        {
            DateTime dataDeAdmissao = new DateTime(2012, 01, 01);

            Assert.AreEqual(dataDeAdmissao.ObterOTotalDeMesesAReceberOBeneficio(new DateTime(2012, 8, 1)), 7);
        }

       

    }
}

