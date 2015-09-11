﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Typeyatsu.Server
{
    public class PlayerModel
    {
        public PlayerModel(string id)
        {
            this.Id = id;
        }

        public string Id { get; }
        public string Rival { get; set; }
    }
}