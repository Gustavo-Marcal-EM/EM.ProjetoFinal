using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EM.Domain
{
    public class Aluno : IEntidade
    {
        
        public int Matricula { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public DateTime Nascimento { get; set; }
        public EnumeradorSexo Sexo { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Aluno aluno &&
                   Matricula == aluno.Matricula &&
                   Nome == aluno.Nome &&
                   CPF == aluno.CPF &&
                   Nascimento == aluno.Nascimento &&
                   Sexo == aluno.Sexo;
        }
        public override int GetHashCode()
        {
            int hashCode = -1073629611;
            hashCode = (hashCode * -1521134295) + Matricula.GetHashCode();
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Nome);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(CPF);
            hashCode = (hashCode * -1521134295) + Nascimento.GetHashCode();
            hashCode = (hashCode * -1521134295) + Sexo.GetHashCode();
            return hashCode;
        }
        public override string ToString()
        {
            string matriculaConvertido = Convert.ToString(Matricula);
            string nomeConvertido = Nome.ToString();
            string cpfConvertido = CPF.ToString();
            string nascimentoConvertido = Convert.ToDateTime(Nascimento).ToString("dd/MM/yyyy");
            string sexoConvertido = Sexo.ToString();


            return $"{matriculaConvertido},{nomeConvertido},{cpfConvertido},{nascimentoConvertido},{sexoConvertido}";
        }
    }
}
