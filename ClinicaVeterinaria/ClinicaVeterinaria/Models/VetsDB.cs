﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace ClinicaVeterinaria.Models
{
    public class VetsDB : DbContext {
        //representar as tabelas a criar na Base de Dados
        public virtual DbSet<Donos> Donos { get; set; }
        public virtual DbSet<Animais> Animais { get; set; }

        //especificar onde será criada a Base de Dados
        public VetsDB() : base("LocalizacaoDaBD") { }
    }
}