﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeddingServices.Interface
{
    public interface IRelationship
    {
        IGuest getRelatedGuest();
    }
}
