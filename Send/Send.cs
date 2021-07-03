using System;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;

namespace Send
{
    class Program
    {
        static void Main()
        {
            Contato contato = new Contato();

            string mensagemRabbit = JsonConvert.SerializeObject(contato);

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(queue: "cadastro",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);

                   // string message = "Primeira requisição RabbitMQ";

                    var body = Encoding.UTF8.GetBytes(mensagemRabbit);

                    channel.BasicPublish(exchange: "",
                                         routingKey: "cadastro",
                                         basicProperties: null,
                                         body: body);
                    
                    Console.WriteLine(" [x] Message sent.");
                }
            }

            Console.WriteLine("Press [enter] to exit.");
            Console.ReadLine();
        }
    }

    public class Contato
    {
        public Contato()
        {
            Nome = "Flavio Cunha";
            CPF = "123.456.789-00";
            Endereco = "Rua dos Testes";
            Telefone = "(11) 9 9107-1256";
        }

        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
    }
}
