using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;

namespace Receive
{
    class Program
    {
        static void Main()
        {
            Contato contato = new Contato();

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

                    var consumer = new EventingBasicConsumer(channel);

                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        contato = JsonConvert.DeserializeObject<Contato>(message);

                        Console.WriteLine($"Nome: {contato.Nome}, CPF: {contato.CPF}, Endereço: {contato.Endereco}, Telefone: {contato.Telefone}");
                    };

                    channel.BasicConsume(queue: "cadastro",
                                         autoAck: true,
                                         consumer: consumer);

                    Console.ReadLine();
                }
            }
        }
    }

    public class Contato
    {
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Endereco { get; set; }
        public string Telefone { get; set; }
    }
}
