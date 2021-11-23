using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using System.Web.Mvc;
using WebAppAspNetMvcDatabaseQuery.Models;

namespace WebAppAspNetMvcDatabaseQuery.Controllers
{
    public class ClientsController : Controller
    {
        public readonly string _connectionString = WebConfigurationManager.AppSettings["ConnectionString"];

        [HttpGet]
        public ActionResult Index()
        {
            var clients = SelectClients();
            return View(clients);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var client = new Client();

            return View(client);
        }

        [HttpPost]
        public ActionResult Create(Client model)
        {
            if (!ModelState.IsValid)
                return View(model);

            

            InsertClient(model);

            return RedirectPermanent("/Clients/Index");
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var client = SelectClients().FirstOrDefault(x => x.Id == id);
            if (client == null)
                return RedirectPermanent("/Clients/Index");

            DeleteClient(client);

            return RedirectPermanent("/Clients/Index");
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var client = SelectClients().FirstOrDefault(x => x.Id == id);
            if (client == null)
                return RedirectPermanent("/Clients/Index");

            return View(client);
        }

        [HttpPost]
        public ActionResult Edit(Client model)
        {
            var client = SelectClients().FirstOrDefault(x => x.Id == model.Id);
            if (client == null)
                ModelState.AddModelError("Id", "Книга не найдена");

            if (!ModelState.IsValid)
                return View(model);

            MappingClient(model, client);

            UpdateClient(client);

            return RedirectPermanent("/Clients/Index");
        }

        private void MappingClient(Client sourse, Client destination)
        {
            destination.Name = sourse.Name;
            destination.Surname = sourse.Surname;
            destination.Age = sourse.Age;
            destination.Birthday = sourse.Birthday;
        }



        public void InsertClient(Client client)
        {
            IDbConnection connection = new SqlConnection(_connectionString);

            string cmdStr = $@"INSERT INTO [{connection.Database}].[dbo].[Clients] ([Name], [Surname], [Age], [Birthday])  VALUES (@Name,@Surname, @Age, @Birthday)";
            IDbCommand cmd = new SqlCommand(cmdStr);
            cmd.Connection = connection;
            connection.Open();

            cmd.Parameters.Add(new SqlParameter("@Name", client.Name));
            cmd.Parameters.Add(new SqlParameter("@Surname", client.Surname));
            cmd.Parameters.Add(new SqlParameter("@Age", client.Age));
            cmd.Parameters.Add(new SqlParameter("@Birthday", client.Birthday));
            

            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void UpdateClient(Client client)
        {
            IDbConnection connection = new SqlConnection(_connectionString);

            string cmdStr = $@"UPDATE [{connection.Database}].[dbo].[Clients] SET [Name] = @Name, [Surname] = @Surname , [Age] = @Age, [Birthday] = @Birthday WHERE Id = {client.Id}";
            IDbCommand cmd = new SqlCommand(cmdStr);
            cmd.Connection = connection;
            connection.Open();

            cmd.Parameters.Add(new SqlParameter("@Name", client.Name));
            cmd.Parameters.Add(new SqlParameter("@Surname", client.Surname));
            cmd.Parameters.Add(new SqlParameter("@Age", client.Age));
            cmd.Parameters.Add(new SqlParameter("@Birthday", client.Birthday));

            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public void DeleteClient(Client client)
        {
            IDbConnection connection = new SqlConnection(_connectionString);

            string cmdStr = $@"DELETE [{connection.Database}].[dbo].[Clients] WHERE Id = {client.Id}";
            IDbCommand cmd = new SqlCommand(cmdStr);
            cmd.Connection = connection;
            connection.Open();
            cmd.ExecuteNonQuery();
            connection.Close();
        }
        public List<Client> SelectClients()
        {
            IDbConnection connection = new SqlConnection(_connectionString);

            string cmdStr = $"SELECT * FROM [{connection.Database}].[dbo].[Clients]";
            IDbCommand cmd = new SqlCommand(cmdStr);
            cmd.Connection = connection;
            connection.Open();

            IDataReader read = cmd.ExecuteReader();
            var clients = new List<Client>();
            while (read.Read())
            {
                var parser = read.GetRowParser<Client>(typeof(Client));
                var client = parser(read);
                clients.Add(client);
            }

            connection.Close();
            return clients;
        }
    }
}