﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ClinicaVeterinaria.Models;

namespace ClinicaVeterinaria.Controllers
{
    public class DonosController : Controller
    {
        private VetsDB db = new VetsDB();

        // GET: Donos
        public ActionResult Index()
        {
            return View(db.Donos.ToList().OrderBy(d=>d.Nome));
        }

        // GET: Donos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donos donos = db.Donos.Find(id);
            if (donos == null)
            {
                return HttpNotFound();
            }
            return View(donos);
        }

        // GET: Donos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Donos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Nome,NIF")] Donos dono)
        {
            //determinar o ID a atribuir ao novo 'dono'
            int novoID = 0;
            try
            {
                novoID = db.Donos.Max(d => d.DonoID) + 1;
            }
            catch (Exception)
            {
                //não existe dados na BD o MAX devolve Null
                novoID=1;
            }
            
            //outra forma
            // novoID = db.Donos.Last().DonoID + 1;
            //outra forma
            // novoID = (from d in db.Donos
            //           orderby d.DonoID descending
            //           select d.DonoID).FirstOrDefault() + 1;
            //outra forma
            // novoID = db.Donos.OrderByDescending(d => d.DonoID).FirstOrDefault().DonoID + 1;

            //atribuir o novoID ao 'dono'
            dono.DonoID = novoID;
            try
            {
            if (ModelState.IsValid) //confronta se os dados a ser introduzidos estão consisentes com o Model
            {
                //adicionar um novo 'dono'
                db.Donos.Add(dono);
                //guardar as alterações
                db.SaveChanges();
                //redireccionar para a página de início
                return RedirectToAction("Index");
            }
            }
            catch (Exception)
            {
                //cria uma mensagem de erro 
                //a ser apresentada ao utilizador
                ModelState.AddModelError("",
                    string.Format("Ocorreu um erro na operação de guardar um novo Dono")
                    );
              /*adicionar a uma classe ERRO 
               * -id
               * -timestamp
               * -operação que gerou o ERRO
               * -mensagem de erro gerada pelo sistema (ex.Message)
               * -qual o User que gerou o ERRO
               * -...
               * 
               * ENVIAR email ao utilizador 'Admin' a avisar da ocorrência do erro
               * 
               * outras coisas consideradas importantes.....
               * */
            }
            //se houver problemas, volta a VIEW do Create com os dados do 'dono'
            return View(dono);
        }

        // GET: Donos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Donos donos = db.Donos.Find(id);
            if (donos == null)
            {
                return HttpNotFound();
            }
            return View(donos);
        }

        // POST: Donos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DonoID,Nome,NIF")] Donos donos)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(donos);
        }

        // GET: Donos/Delete/5
        public ActionResult Delete(int? id)
        {
            //se não foi fornecido o ID do 'Dono'....
            if (id == null)
            {
                //redireciono o utilizador para a lista de Donos
                return RedirectToAction("Index");
            }
            //vai à procura do 'Dono', cujo o ID foi fornecido
            Donos dono = db.Donos.Find(id);

            //se o 'dono' associado ao ID fornecido Não existe....
            if (dono == null)
            {
                //redireciono o utilizador para a lista de Donos
                return RedirectToAction("Index");
            }
            //mostra os dados do 'Dono'
            return View(dono);
        }

        // POST: Donos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //procura o 'dono' na base de dados
            Donos dono = db.Donos.Find(id);
            try
            {
                //marcar o 'dono' para eliminação
                db.Donos.Remove(dono);

                //efetuar um 'commit' ao comando anterior
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                //cria uma mensagem de erro 
                //a ser apresentada ao utilizador
                ModelState.AddModelError("",
                    string.Format("Ocorreu um erro na eliminação do Dono com ID={0}-{1}", id, dono.Nome)
                    );
                //invoca a View, com os dados 'dono' atual
                return View(dono);          
            }
           
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
