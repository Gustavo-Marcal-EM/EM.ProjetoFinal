using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using EM.Domain;

namespace EM.Domain.Testes
{
    [TestClass]
    public class DomainTestes
    {
        Aluno aluno = new Aluno();
        [TestMethod]
        public void TesteEquals()
        {
            //Arrange
            aluno.Matricula = 216;
            //Act
            aluno.Matricula.Equals(aluno.Matricula);
            //Assert
            Assert.AreEqual(aluno.Matricula, aluno.Matricula);
        }
        [TestMethod]
        public void TesteGetHashCode()
        {
            //Arrange
            int hashCode = -1073629611;
            //Act
            hashCode = (hashCode * -1521134295) + aluno.Matricula.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(aluno.Nome);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(aluno.CPF);
            hashCode = (hashCode * -1521134295) + aluno.Nascimento.GetHashCode();
            hashCode = (hashCode * -1521134295) + aluno.Sexo.GetHashCode();

            //Assert
            Assert.AreNotEqual(hashCode, 0);
            
        }
        [TestMethod]
        public void TesteToString()
        {
            //Arrange
            aluno.Matricula = 89123;
            aluno.Nome = "Carlito";
            aluno.CPF = "03979073114";
            aluno.Nascimento = Convert.ToDateTime("06/07/2002").Date;
            aluno.Sexo = (EnumeradorSexo)1;
            //Act
            string a = aluno.Matricula.ToString();
            string b = aluno.Nome.ToString();
            string c = aluno.CPF.ToString();
            string d = aluno.Nascimento.ToString();
            string e = aluno.Sexo.ToString();
            //Assert
            Assert.IsNotNull(aluno);
            Assert.AreEqual(a, "89123");
            Assert.AreEqual(b, "Carlito");
            Assert.AreEqual(c, "03979073114");
            Assert.AreEqual(d, "06/07/2002 00:00:00");
            Assert.AreEqual(e, "Feminino");
        }
    }
}

