using System;
using System.Reflection;

//Classes Abstratas, Herança, Encapsulamento e Polimorfismo
namespace Exercícios_POO___Básico
{
    class Program
    {
        static void Main(string[] args)
        {
            Pessoa d = new Pessoa() { Nome = "Darwin", Endereco = "Av. Comendador Alberto Bonfiglioli, 618", Profissao = "Programador Jr", Sexo = Mamifero<Pessoa>.TipoSexo.Masc, Genero = Pessoa.TipoGenero.Hetero };
            Pessoa a = new Pessoa() { Nome = "Ana", Endereco = "R. Puta Que Pariu, 69", Profissao = "Designer", Sexo = Mamifero<Pessoa>.TipoSexo.Fem, Genero = Pessoa.TipoGenero.Bi };

            Cachorro s = new Cachorro() { Nome = "Spike", Sexo = Mamifero<Pet>.TipoSexo.Masc, Som = "Au" };
            Cachorro b = new Cachorro() { Nome = "Belinha", Sexo = Mamifero<Pet>.TipoSexo.Fem, Som = "Auu" };

            Gato f = new Gato() { Nome = "Fred", Sexo = Mamifero<Pet>.TipoSexo.Masc, Som = "Miau" };
            Gato p = new Gato() { Nome = "Pri", Sexo = Mamifero<Pet>.TipoSexo.Fem, Som = "Miaur" };

            Pessoa filho = a.ReproduzirCom(d) as Pessoa;
            filho.Nome = "João";

            Gato gatinho = f.ReproduzirCom(p) as Gato;
            gatinho.Nome = "Frajola";

            filho.AcariciarAnimal(s);
            filho.AcariciarAnimal(gatinho);

            Console.ReadKey();
        }
    }

    public abstract class Mamifero<T> where T : Mamifero<T> //Abstração (Descobrir propriedades e métodos em comum entre um grupo de classes e criar um Modelo)
    {
        protected string sNome;

        public enum TipoSexo { Masc, Fem}
        protected TipoSexo tsSexo;

        protected int iIdade;
        protected Mamifero<T> _Pai, _Mae;

        //Encapsulamento (Apenas os objetos podem mudar suas propriedades)
        public string Nome
        {
            get { return sNome; }
            set { sNome = value; }
        }
        public TipoSexo Sexo
        {
            get { return tsSexo; }
            set { tsSexo = value; }
        }
        public int Idade
        {
            get { return iIdade; }
        }
        public Mamifero<T> Pai
        {
            get { return _Pai; }
        }
        public Mamifero<T> Mae
        {
            get { return _Mae; }
        }

        public Mamifero<T> ReproduzirCom(Mamifero<T> parceiro)
        {

            Array tiposSexo = Enum.GetValues(typeof(TipoSexo));

            if (parceiro == this)
            {
                Console.WriteLine(this.Nome + " está carente!");
                return null;
            }

            if (Sexo != parceiro.Sexo)
            {

                Type tP = parceiro.GetType();

                if (tP == this.GetType())
                {
                    Mamifero<T> filhote = Activator.CreateInstance(tP) as Mamifero<T>;
                    
                    filhote.Sexo = (TipoSexo) tiposSexo.GetValue(new Random().Next(tiposSexo.Length));
                    filhote.iIdade = 0;

                    switch (Sexo)
                    {
                        case TipoSexo.Masc:

                            filhote._Pai = this;
                            filhote._Mae = parceiro;

                            break;
                        case TipoSexo.Fem:

                            filhote._Pai = parceiro;
                            filhote._Mae = this;

                            break;
                    }

                    filhote.Nome = parceiro.Nome + " " + Nome;

                    return filhote;
                }
                else
                {
                    Console.WriteLine("Não é possível cruzar duas espécies diferentes.");
                    return null;
                }
            }
            else
            {
                Console.WriteLine("Não foi possível efetuar a reprodução.");
                return null;
            }
        }
                
    }

    public abstract class Pet : Mamifero<Pet> //Herança
    {
        protected string sSom;

        public string Som
        {
            get { return sSom; }
            set { sSom = value; }
        }

        public virtual void EmitirSom()
        {
            Console.WriteLine(Nome + ": " + Som);
        }
    }

    public class Pessoa : Mamifero<Pessoa>
    {
        protected string _Endereco;
        protected string _Profissao;
        
        public enum TipoGenero { Hetero, Bi, Homo}
        public TipoGenero Genero;

        public string Endereco
        {
            get { return _Endereco; }
            set { _Endereco = value; }
        }
        public string Profissao
        {
            get { return _Profissao; }
            set { _Profissao = value; }
        }

        public Pessoa()
        {
            
        }

        public void AcariciarAnimal(Pet pet)
        {
            Console.WriteLine(Nome + ": " + "Quem é o bom menino? É você, " + pet.Nome + "!");
            pet.EmitirSom();
        }
    }

    public class Cachorro : Pet
    {
        public Cachorro()
        {
            Som = "Au";
        }
    }

    public class Gato : Pet
    {
        public Gato()
        {
            Som = "Miau";
        }

        public override void EmitirSom() //Polimorfismo (Alterar um método da superclasse)
        {
            base.EmitirSom();
            Ronronar();
        }

        private void Ronronar()
        {
            Console.WriteLine(Nome + ": " + "rrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
        }
    }
}
