using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiBound
{
    public class Celestial
    {
        int orbitalLevels;
        int chunkSize;
        int XYmin;
        int XYmax;
        int Zmin;
        int Zmax;
        sector[] sectors;

        public Celestial(int _orbitalLevels, int _chunkSize, int _XYmin, int _XYmax, int _Zmin, int _Zmax, sector[] _sectors)
        {
            orbitalLevels = _orbitalLevels;
            chunkSize = _chunkSize;
            XYmin = _XYmin;
            XYmax = _XYmax;
            Zmin = _Zmin;
            Zmax = _Zmax;
            sectors = _sectors;
        }
    }
    public struct sector
    {
        string sectorID;
        string sectorName;
        ulong sectorSeed;
        string sectorPrefix;
        dynamic parameters;
        dynamic sectorConfig;

        public sector(string _sectorID, string _sectorName, ulong _sectorSeed, string _sectorPrefix, dynamic _parameters, dynamic _sectorConfig)
        {
            sectorID = _sectorID;
            sectorName = _sectorName;
            sectorSeed = _sectorSeed;
            sectorPrefix = _sectorPrefix;
            parameters = _parameters;
            sectorConfig = _sectorConfig;
        }
    }
}
