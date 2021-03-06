﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinqToObject
{
    public class Esercizio
    {
        //Creazione liste
        public static List<Product> CreateProductList()
        {
            var lista = new List<Product>
            {
                new Product { ID = 1, Name= "Telefono",UnitPrice=300.99},
                new Product { ID = 2, Name= "Computer",UnitPrice=800},
                new Product { ID = 3, Name= "Tablet",UnitPrice=550.99}

                
            };
            return lista;
        }
        public static List<Order> CreateOrderList()
        {
            var lista = new List<Order>();
            var order = new Order
            {
                ID = 1,
                ProductID =1,
                Quantity =4
            };
            lista.Add(order);
            var order1 = new Order
            {
                ID = 2,
                ProductID = 2,
                Quantity = 1
            };

            lista.Add(order1);

            var order2 = new Order
            {
                ID = 3,
                ProductID = 1,
                Quantity= 1
            };

            lista.Add(order2);
            return lista;

        }

        //esecuzione immediata e ritardata
        public static void DefferedExecution()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();

            foreach (var p in productList)
            {
                Console.WriteLine(p.ID + ") "+p.Name + " " + p.UnitPrice);
            }
            foreach (var o in orderList)
            {
                Console.WriteLine(o.ID + " " + o.ProductID + " " + o.Quantity);
            }
            //CREAZIONE QUERY
            var list = productList
                .Where(product => product.UnitPrice > 400)   //FILTRO
                .Select(p => new { Nome = p.Name, Prezzo =p.UnitPrice});    //Creo una nuova lista con campi prezzo e nome
                                                                            //Aggiungo prodotto
            productList.Add(new Product {ID =4, Name = "Bici",UnitPrice = 500.99 });

            //Risultati
            Console.WriteLine("Esecuzione differita: ");
            foreach (var item in list)
            {
                Console.WriteLine("{0} - {1}" ,item.Nome, item.Prezzo);
            }
            //VIENE ESEGUITA QUI NON VIENE ESEGUITA PRIMA

            //ESECUZIONE IMMEDIATA

            var list1 = productList.
                Where(p => p.UnitPrice > 400)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice }).ToList();
            //ToList mi forza ad eseguire la select

            productList.Add(new Product { ID =1, Name = "Divano", UnitPrice = 450.99 });

            Console.WriteLine("Esecuzione immediata: ");
            foreach (var item in list1)
            {
                Console.WriteLine("{0} - {1}", item.Nome, item.Prezzo);
            }
            //La select era già stata eseguita non vedo Divano
            //Se fosse in list vedrei divano
        }
        public static void Syntax()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();

            //Method Syntax
            var methodList = productList
                .Where(p => p.UnitPrice <= 600)
                .Select(p => new { Nome = p.Name, Prezzo = p.UnitPrice })
                .ToList();

            //Query Syntax
            var queryList =
                from p in productList
                where p.UnitPrice <= 600
                select new { Nome = p.Name, Prezzo = p.UnitPrice };
            queryList.ToList();


            //FANNO LA STESSA COSA MA SONO SCRITTE IN MANIERA DIVERSA
            foreach (var item in queryList)
            {
                Console.WriteLine("{0} - {1}", item.Nome, item.Prezzo);
            }
            foreach (var item in methodList)
            {
                Console.WriteLine("{0} - {1}", item.Nome, item.Prezzo);
            }
        }
        //OPERATORI
        public static void Operators()
        {
            var productList = CreateProductList();
            var orderList = CreateOrderList();

            Console.WriteLine("Lista Prodotti: ");
            foreach (var p in productList)
            {
                Console.WriteLine(p.ID + ") " + p.Name + " " + p.UnitPrice);
            }
            Console.WriteLine("Lista Ordini: ");
            foreach (var o in orderList)
            {
                Console.WriteLine(o.ID + " " + o.ProductID + " " + o.Quantity);
            }

            //Filtro OfType, mi serve un arraylist
            Console.WriteLine("------------------");
            var list = new ArrayList();
            list.Add(productList);
            list.Add("questa è una stringa");
            list.Add(1);

            var typeQuery =
                from item in list.OfType<int>()
                select item;

            Console.WriteLine("Gli elementi interi nella lista sono: ");
            foreach (var item in typeQuery)
            {
                Console.WriteLine(item);
            }
            //Element 
            Console.WriteLine("Elemento 1");
            int[] empty = { };
            var el1 = empty.FirstOrDefault();
            Console.WriteLine(el1);

            var p1 = productList.ElementAt(0).Name;
            Console.WriteLine(p1);

            //ordinamento
            Console.WriteLine("------------------");
            Console.WriteLine("Ordinamento: ");
            var orderedList =
                from p in productList
                orderby p.Name ascending, p.UnitPrice descending
                select new { Nome = p.Name, Prezzo = p.UnitPrice };
           
            var orderedList2 =
                productList.OrderBy(p => p.Name)
                .ThenByDescending(p => p.UnitPrice)
                .Select(p=> new { Nome=p.Name, Prezzo=p.UnitPrice})
                .Reverse();

            productList.Add(new Product { ID = 4, Name = "Telefono", UnitPrice = 1000 });

            Console.WriteLine("PRIMO ORDINAMENTO");
            foreach (var item in orderedList)
            {
                Console.WriteLine(item.Nome + "  " + item.Prezzo);
            }
            
            Console.WriteLine("SECONDO ORDINAMENTO");
            foreach (var item in orderedList2)
            {
                Console.WriteLine(item.Nome + "  " + item.Prezzo);
            }
            Console.WriteLine("------------------");
            //quantificatori
            var HasProductWithT = productList.Any(p => p.Name.StartsWith('T'));
            var AllProductWithT = productList.All(p => p.Name.StartsWith('T'));
            
            Console.WriteLine("Ci sono prodotti che iniziano con la T? {0}",HasProductWithT);
            Console.WriteLine("Tutti i prodotti iniziano con la T? {0}", AllProductWithT);

            //GROUP BY
            Console.WriteLine("------------------");
            Console.WriteLine("GROUP BY");

            //query
            //raggruppiamo order per id del prodotto
            var groupByList = from o in orderList
                              group o by (o.ProductID) into groupList
                              select groupList;


            foreach (var order in groupByList)
            {
                Console.WriteLine(order.Key);
                foreach (var item in order)
                {
                    Console.WriteLine($"\t {item.ProductID} {item.Quantity}");
                }
            }
            var groupByList2 = from o in productList
                              group o by (o.Name) into groupList
                              select groupList;
            //groupList e groupByList sono uguali, avrei potuto andare avanti
            //ritorna un IGrouping -> una chiave + una lista delle cose che hai raggruppato per quella chiave.
            //Lista di liste identificate da un chiave
            foreach (var o in groupByList2)
            {
                Console.WriteLine(o.Key);
                foreach (var item in o)
                {
                    Console.WriteLine($"\t {item.Name} - {item.UnitPrice}");
                }
            }

            var groupByList3 =
                orderList.GroupBy(o => o.ProductID);

            foreach (var order in groupByList3)
            {
                Console.WriteLine(order.Key);
                foreach (var item in order)
                {
                    Console.WriteLine($"\t {item.ProductID} {item.Quantity}");
                }
            }

            //
            Console.WriteLine("------------------");
            Console.WriteLine("Funzione di Aggregazione");
            //Ragruppo ordini per prodotto e ricavare la somma delle quantità per prodotto
            //select dice come voglio vedere i risultati
            var sumQuantityByProduct =
                orderList
                .GroupBy(p => p.ProductID)
                .Select(lista => new 
                { 
                    Id = lista.Key, 
                    Quantities = lista.Sum(p => p.Quantity)
                });

            foreach (var item in sumQuantityByProduct)
            {
                Console.WriteLine("{0} - {1}", item.Id,item.Quantities);
            }
            
            var sumQuantityByProduct1= 
                from o in orderList
                group o by o.ProductID into groupList
                select new { Id = groupList.Key, Quantities = groupList.Sum(x => x.Quantity) };

            foreach (var item in sumQuantityByProduct1)
            {
                Console.WriteLine("{0} - {1}", item.Id, item.Quantities);
            }

            //Join, sempre inner join
            Console.WriteLine("------------");
            Console.WriteLine("JOIN");
            //RECUPERIAMO I PRODOTTI CHE HANNO ORDINI
            //NOME - ID ORDINE - QUANTITY

            //METHOD SYNTAX
            var joinList = productList
                .Join(orderList,
                p => p.ID,
                o => o.ProductID,
                (p, o) => new { Nome = p.Name, IDOrder = o.ID, Quant = o.Quantity });

            foreach (var p in joinList)
            {
                Console.WriteLine("{0} -{1} - {2}",p.IDOrder, p.Nome, p.Quant);
            }

            //query syntax

            var joinList1 = from p in productList
                            join o in orderList
                            on p.ID equals o.ProductID
                            select new
                            {
                                Nome = p.Name,
                                OrderID = o.ID,
                                Quant = o.Quantity
                            };
            Console.WriteLine("         ");
            foreach (var item in joinList1)
            {
                Console.WriteLine("{0} - {1} - {2}",item.OrderID,item.Nome,item.Quant);
            }


            //GROUP JOIN
            Console.WriteLine("--------");
            Console.WriteLine("GROUP JOIN");
            //Recuperare ordini per prodotto e fare somma per quantità
            //nome prodotto - quantita totale

            var groupJoinList = productList
                .GroupJoin(orderList,
                p => p.ID,     ///questo è rispetto a cosa raggruppiamo
                o => o.ProductID,
                (p, o) => new
                {
                    Prodotto = p.Name,
                    ID = p.ID,
                    Quantita = o.Sum(o => o.Quantity)
                });
            foreach (var item in groupJoinList)
            {
                Console.WriteLine("{0} - {1} - {2}",item.ID, item.Prodotto, item.Quantita);
            }


            var groupJoinList1 = from p in productList
                                 join o in orderList
                                 on p.ID equals o.ProductID
                                 into gr
                                 select new { nome = p.Name, Quant = gr.Sum(x => x.Quantity) };
            
            Console.WriteLine("        ");
            foreach (var item in groupJoinList1)
            {
                Console.WriteLine("{0} - {1} ", item.nome,item.Quant);
            }
            var lista4 =
                from o in orderList
                group o by o.ProductID
                into gr
                select new { ProdottoID = gr.Key, Quantità = gr.Sum(o => o.Quantity)}
                into gr1
                join p in productList
                on gr1.ProdottoID equals p.ID
                select new { p.Name , gr1.Quantità };

            Console.WriteLine( "     ");
            foreach (var item in lista4)
            {
                Console.WriteLine(item.Name + " - " + item.Quantità);
            }
        }

    }


}
