using System;
using System.Collections.Generic;

namespace ClientLogic.Manager
{
	public class InternalClientManager
	{
        private List<InternalClient> clients;

        public InternalClientManager()
        {
            clients = new List<InternalClient>();
        }

        public List<InternalClient> getPatients()
        {
            return clients;
        }
        public InternalClient createPatient(string name, string lastname,string seclastname, int CI, string address, int phone, int ranking)
        {
            string codigocliente = name[0].ToString().ToUpper() + lastname[0].ToString().ToUpper() + seclastname[0].ToString().ToUpper() + "-" + CI.ToString();
            InternalClient client = new InternalClient() { Nombre = name, ApellidoPaterno = lastname, ApellidoMaterno= seclastname, CI = CI, Direccion=address, Telefono=phone,Ranking=ranking, CodigoCliente=codigocliente };
            clients.Add(client);
            return client;
        }
        public InternalClient updatePatient(string address, int phone, string codigo)
        {
            InternalClient client = null;
            clients.ForEach(c =>
            {
                if (c.CodigoCliente == codigo)
                    c.Direccion = address; c.Telefono = phone; client =c;
            });
            return client;
        }
        public InternalClient removePatient(int ci)
        {
            InternalClient client = clients.Find(p => p.CI == ci);
            clients.Remove(client);
            return client;
        }
    }

}
