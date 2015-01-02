﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movselex.Core
{
    public class MovselexClientFactory
    {
        public static IMovselexClient Create(string appConfigFilePath)
        {
            return new MovselexClient(appConfigFilePath);
        }
    }
}
