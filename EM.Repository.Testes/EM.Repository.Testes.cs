using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using EM.Domain;
using System.Linq;

namespace EM.Repository.Testes
{
    [TestClass]
    public class RepositoryTestes
    {
        RepositorioAluno repositorioAluno = new RepositorioAluno();
        
        [TestMethod]
        public void TesteAdd()
        {
            Aluno alunoAdd = new Aluno();
            //Arrange
            alunoAdd.Matricula = 135790;
            alunoAdd.Nome = "TesteAdd";
            alunoAdd.Nascimento = Convert.ToDateTime("06/07/2001").Date;
            alunoAdd.Sexo = (EnumeradorSexo)1;
            alunoAdd.CPF = "";

            //Act
            repositorioAluno.Add(alunoAdd);
            //Assert
            Assert.IsNotNull(alunoAdd);

        }
        [TestMethod]
        public void TesteUpdate()
        {
            Aluno alunoUpdate = new Aluno();
            //Arrange
            alunoUpdate.Matricula = 135790;
            alunoUpdate.Nome = "TesteUpdate1";
            alunoUpdate.Nascimento = Convert.ToDateTime("09/06/1965").Date;
            alunoUpdate.Sexo = 0;
            alunoUpdate.CPF = "35128720187";
            //Act
            repositorioAluno.Update(alunoUpdate);
            //Assert
            Assert.IsNotNull(alunoUpdate);
        }
        [TestMethod]
        public void TesteRemove()
        {
            Aluno alunoRemove = new Aluno();
            //Arrange
            alunoRemove.Matricula = 135790;
            //Act
            repositorioAluno.Remove(alunoRemove);
            //Assert
            Assert.IsNotNull(alunoRemove);
        }
        [TestMethod]
        public void TesteGetAll()
        {
            //Arrange

            //Act
            
            //Assert
            Assert.IsNotNull(repositorioAluno.GetAll());
        }
        [TestMethod]
        public void TesteGetByMatricula()
        {
            Aluno aluno = new Aluno();
            //Arrange
            int matricula = 74631;
            
            //Act & Assert
            Assert.AreEqual(repositorioAluno.GetByMatricula(matricula).Nome, "Ana Luiza");
            Assert.AreEqual(repositorioAluno.GetByMatricula(matricula).Nascimento, Convert.ToDateTime("02/06/2002").Date);
            Assert.AreEqual(repositorioAluno.GetByMatricula(matricula).CPF, "");
            Assert.AreEqual(repositorioAluno.GetByMatricula(matricula).Sexo, (EnumeradorSexo)1);
        }
        [TestMethod]
        public void TesteGetByContendoNoNome()
        {
            Aluno aluno = new Aluno();
            //Arrange
            string parteDoNome = "Gu";
            //Act
            bool isEmpty = !repositorioAluno.GetByContendoNoNome(parteDoNome).Any();
            //Assert
            Assert.IsNotNull(aluno);
            Assert.IsNotNull(repositorioAluno.GetByContendoNoNome(parteDoNome));
            Assert.IsFalse(isEmpty);
        }
        [TestMethod]
        public void TesteGet()
        {
            //Arrange
            string parteDoNome = "a";
            //Act
            bool IsEmpty = repositorioAluno.Get(x => x.Nome.ToUpper().Contains(parteDoNome.ToUpper())).Any();
            //Assert
            Assert.IsNotNull(repositorioAluno.Get(x => x.Nome.ToUpper().Contains(parteDoNome.ToUpper())));
            Assert.IsTrue(IsEmpty);
            //^^^^ Se isEmpty == false, a lista está vazia, ou seja, não foi encontrado um match
        }

    }
}
