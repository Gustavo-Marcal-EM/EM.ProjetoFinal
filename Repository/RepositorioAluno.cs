using EM.Domain;
using System;
using FirebirdSql.Data.FirebirdClient;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Collections.Generic;

namespace EM.Repository
{
    public class RepositorioAluno : RepositorioAbstrato<Aluno>
    {
        private static readonly RepositorioAluno instanciaFireBird = new RepositorioAluno();
        public static RepositorioAluno getInstancia()
        {
            return instanciaFireBird;
        }
        public FbConnection getConexao()
        {
            string conn = @"DataSource=localhost; Port=3053; Database=C:\TesteSQL\BANCODEDADOS_v3.fb3; username=sysdba; password=masterkey";
            return new FbConnection(conn);
        }
        public override void Add(Aluno aluno)
        {
            using (FbConnection conexaoFireBird = getInstancia().getConexao())
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = "INSERT into Aluno(Matricula, Nome, CPF, Nascimento, Sexo) " +
                        "Values(" + aluno.Matricula + ",'" 
                        + aluno.Nome + "','" +
                          aluno.CPF + "','" + 
                          aluno.Nascimento.ToString("MM/dd/yyyy") + "'," + 
                          (int)aluno.Sexo + ");";
                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.ExecuteNonQuery();
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
            base.Add(aluno);
        }
        public override IEnumerable<Aluno> Get(Expression<Func<Aluno, bool>> predicate)
        {
            return base.Get(predicate);
        }

        public override IEnumerable<Aluno> GetAll()
        {
            List<Aluno> alunos = new List<Aluno>();
            using (FbConnection conexaoFireBird = getInstancia().getConexao())
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = "Select * from ALUNO";
                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    FbDataReader dataReader = cmd.ExecuteReader();
                    
                    while (dataReader.Read())
                    {
                        alunos.Add(new Aluno()
                        {
                            Matricula = Convert.ToInt32(dataReader["MATRICULA"]),
                            Nome = dataReader["NOME"].ToString(),
                            CPF = dataReader["CPF"].ToString(),
                            Nascimento = Convert.ToDateTime(dataReader["NASCIMENTO"]),
                            Sexo = ((EnumeradorSexo)dataReader["SEXO"])
                        });
                    }
                    dataReader.Close();
                    base.GetAll();
                    return alunos;
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            } 
        }

        public override void Remove(Aluno aluno)
        {
            using (FbConnection conexaoFireBird = getInstancia().getConexao())
            {
                try
                { 
                     conexaoFireBird.Open();
                     string mSQL = "DELETE from Aluno Where matricula = " + aluno.Matricula;
                     FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                     cmd.ExecuteNonQuery();
                    
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
            base.Remove(aluno);
        }
        public override void Update(Aluno aluno)
        {
            using (FbConnection conexaoFireBird = getInstancia().getConexao())
            {
                try
                {
                    conexaoFireBird.Open();
                    string mSQL = "UPDATE ALUNO set NOME= '"
                                  + aluno.Nome
                                  + "', CPF= '"
                                  + aluno.CPF
                                  + "', NASCIMENTO= '"
                                  + aluno.Nascimento.ToString("MM/dd/yyyy")
                                  + "',SEXO= "
                                  + (int)aluno.Sexo
                                  + "WHERE MATRICULA= "
                                  + aluno.Matricula;
                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    cmd.ExecuteNonQuery();
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
            
            base.Update(aluno);
        }
        public Aluno GetByMatricula(int matricula)
        {
            Aluno aluno = new Aluno();
           
            using (FbConnection conexaoFireBird = getInstancia().getConexao())
            {
                try
                {   
                    conexaoFireBird.Open();
                    string mSQL = "SELECT * FROM ALUNO WHERE MATRICULA = " + matricula;
                    FbCommand cmd = new FbCommand(mSQL, conexaoFireBird);
                    FbDataReader dataReader = cmd.ExecuteReader();
                    while (dataReader.Read())
                    {
                        aluno.Matricula = Convert.ToInt32(dataReader[0]);
                        aluno.Nome = dataReader[1].ToString();
                        aluno.CPF = dataReader[2].ToString();
                        aluno.Nascimento = Convert.ToDateTime(dataReader[3]).Date;
                        aluno.Sexo = (EnumeradorSexo)dataReader[4];
                    }
                    return aluno;
                }
                catch (FbException fbex)
                {
                    throw fbex;
                }
                finally
                {
                    conexaoFireBird.Close();
                }
            }
            
        }
        public IEnumerable<Aluno> GetByContendoNoNome(string parteDoNome)
        {
            List<Aluno> alunos = new List<Aluno>();
            return null;
        }
    }
    
}
