using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AEC_Pos.Services
{
    public interface IRepository
    {
        string GeneratePONumber();
        string GenerateGRNumber();
        string GenerateSONumber();
        string GenerateInvenTranNumber();
    }
}
