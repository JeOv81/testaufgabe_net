﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces;
public interface ILocalizationService
{
    string GetString(string key, string resourceBaseName = default!);
}